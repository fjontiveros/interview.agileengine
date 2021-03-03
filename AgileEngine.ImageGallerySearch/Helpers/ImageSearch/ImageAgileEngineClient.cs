using AgileEngine.ImageGallerySearch.Exceptions;
using AgileEngine.ImageGallerySearch.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AgileEngine.ImageGallerySearch.Helpers.ImageSearch
{
    public class ImageAgileEngineClient : IImageAgileEngineClient
    {
        public ImageAgileEngineClient(IRestHttpProxy restHttpProxy,
           IAuthenticationHeaderFactory authenticationHeaderFactory,
           IOptions<ImageSearchConfig> imageSearchConfig,
           ILogger<InMemoryImageCacheRepository> logger)
        {
            RestHttpProxy = restHttpProxy;
            AuthenticationHeaderFactory = authenticationHeaderFactory;
            Logger = logger;
            ImageSearchConfig = imageSearchConfig.Value;
        }

        public IRestHttpProxy RestHttpProxy { get; }
        public IAuthenticationHeaderFactory AuthenticationHeaderFactory { get; }
        public ILogger<InMemoryImageCacheRepository> Logger { get; }
        public ImageSearchConfig ImageSearchConfig { get; }
        private static AuthenticationHeaderValue Auth { get; set; }

        public async Task<IEnumerable<Image>> GetImagesAsync()
        {
            var newImageBag = new ConcurrentBag<Image>();

            var imageIds = new ConcurrentBag<string>();

            Logger.LogDebug("Start loading images cache");

            if (Auth == null)
            {
                Logger.LogDebug("Loading auth");

                Auth = AuthenticationHeaderFactory.GetAuthenticationHeaderAsync().Result;
            }

            ImageSearchResult result = null;
            var url = $"{ImageSearchConfig.BaseUrl}/{ImageSearchConfig.ImageSearchPath}";

            try
            {
                result = await RestHttpProxy.GetWebRequestAsync<ImageSearchResult>(new Uri(url), new RequestConfiguration { AuthenticationValue = Auth });
            }
            catch(UnauthorizedException)
            {
                Auth = AuthenticationHeaderFactory.GetAuthenticationHeaderAsync().Result;
                result = await RestHttpProxy.GetWebRequestAsync<ImageSearchResult>(new Uri(url), new RequestConfiguration { AuthenticationValue = Auth });
            }


            Logger.LogDebug($"Getting image ids. Total pages: {result.PageCount}");

            this.AddImageIds(imageIds, result);

            var getImageIdsContainerTasks = new List<Task>();

            for (int i = 1; i < result.PageCount; i++)
            {
                getImageIdsContainerTasks.Add(Task.Run(async () =>
                {
                    var currentUrl = $"{url}?page={i}";
                    result = await RestHttpProxy.GetWebRequestAsync<ImageSearchResult>(new Uri(url), new RequestConfiguration { AuthenticationValue = Auth });
                    AddImageIds(imageIds, result);
                }));
            }

            await Task.WhenAll(getImageIdsContainerTasks);

            Logger.LogDebug($"Getting image details");

            var getImageDetailContainerTasks = new List<Task>();

            foreach (var imageId in imageIds)
            {
                getImageDetailContainerTasks.Add(Task.Run(async () =>
                {
                    var image = await RestHttpProxy.GetWebRequestAsync<Image>(new Uri($"{ImageSearchConfig.BaseUrl}/{ImageSearchConfig.ImageSearchPath}/{imageId}"), new RequestConfiguration { AuthenticationValue = Auth });
                    newImageBag.Add(image);
                }));
            }

            await Task.WhenAll(getImageDetailContainerTasks);

            Logger.LogDebug($"Images cache loaded");

            return newImageBag;
        }

        private void AddImageIds(ConcurrentBag<string> imageIds, ImageSearchResult result)
        {
            foreach (var picture in result.Pictures)
            {
                imageIds.Add(picture.Id);
            }
        }
    }
}
