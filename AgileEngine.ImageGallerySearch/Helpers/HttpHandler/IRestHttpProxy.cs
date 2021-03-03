using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileEngine.ImageGallerySearch.Helpers
{
    public interface IRestHttpProxy
    {
        Task<T> GetWebRequestAsync<T>(Uri uri, RequestConfiguration requestConfiguration = null) where T : class;
        Task<T> PostWebRequestAsync<T, U>(Uri uri, U body, RequestConfiguration requestConfiguration = null) where T : class;
    }
}
