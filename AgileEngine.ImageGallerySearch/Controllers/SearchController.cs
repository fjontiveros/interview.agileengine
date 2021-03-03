using AgileEngine.ImageGallerySearch.Filters;
using AgileEngine.ImageGallerySearch.Helpers.ImageSearch;
using AgileEngine.ImageGallerySearch.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileEngine.ImageGallerySearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [LogFilter]
    [ExceptionFilter]
    public class SearchController : ControllerBase
    {
        public IImageSearchEngine ImageSearchEngine { get; }
        public SearchController(IImageSearchEngine imageSearchEngine)
        {
            ImageSearchEngine = imageSearchEngine;
        }

        [HttpGet("{searchTerm?}")]
        public IEnumerable<Image> Index(string searchTerm = null)
        {
            return ImageSearchEngine.Search(searchTerm);
        }
    }
}
