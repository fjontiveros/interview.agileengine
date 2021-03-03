using System.Collections.Generic;

namespace AgileEngine.ImageGallerySearch.Helpers.ImageSearch
{
    public class ImageSearchResult
    {
        public List<ImageSearchResultPicture> Pictures { get; set; }
        public int Page { get; set; }
        public int PageCount { get; set; }
        public bool HasMore { get; set; }

        public class ImageSearchResultPicture
        {
            public string Id { get; set; }
            public string CroppedPicture { get; set; }
        }
    }
}
