using AgileEngine.ImageGallerySearch.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgileEngine.ImageGallerySearch.Helpers.ImageSearch
{
    public interface IImageAgileEngineClient
    {
        Task<IEnumerable<Image>> GetImagesAsync();
    }
}
