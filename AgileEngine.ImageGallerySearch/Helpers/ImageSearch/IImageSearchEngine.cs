using AgileEngine.ImageGallerySearch.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileEngine.ImageGallerySearch.Helpers.ImageSearch
{
    public interface IImageSearchEngine
    {
        IEnumerable<Image> Search(string term);
    }
}
