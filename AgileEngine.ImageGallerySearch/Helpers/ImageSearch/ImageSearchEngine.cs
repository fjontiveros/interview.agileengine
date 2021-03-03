using AgileEngine.ImageGallerySearch.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace AgileEngine.ImageGallerySearch.Helpers.ImageSearch
{
    public class ImageSearchEngine : IImageSearchEngine
    {
        public IRestHttpProxy RestHttpProxy { get; }
        public IAuthenticationHeaderFactory AuthenticationHeaderFactory { get; }
        public ImageSearchConfig ImageSearchConfig { get; }
        public IImageCacheRepository ImageCacheRepository { get; }

        public ImageSearchEngine(IImageCacheRepository imageCacheRepository)
        {
            ImageCacheRepository = imageCacheRepository;
        }

        public IEnumerable<Image> Search(string term)
        {
            return ImageCacheRepository.Search(term);
        }
    }
}
