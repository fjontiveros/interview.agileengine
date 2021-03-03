using AgileEngine.ImageGallerySearch.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileEngine.ImageGallerySearch.Helpers.ImageSearch
{
    public class InMemoryImageCacheRepository : IImageCacheRepository
    {
        public IImageAgileEngineClient ImageAgileEngineClient { get; }

        private static IEnumerable<Image> Images { get; set; }

        public InMemoryImageCacheRepository(IImageAgileEngineClient imageAgileEngineClient)
        {
            ImageAgileEngineClient = imageAgileEngineClient;
        }

        public async Task LoadCache()
        {
            Images = await ImageAgileEngineClient.GetImagesAsync();
        }

        public IEnumerable<Image> Search(string term)
        {
            if (Images == null)
                return new List<Image>();

            return term == null ? Images 
                : Images.Where(x => x.Author.Contains(term)
                                    || x.Camera.Contains(term)
                                    || x.Tags.Contains(term)).ToList();
        }
    }
}
