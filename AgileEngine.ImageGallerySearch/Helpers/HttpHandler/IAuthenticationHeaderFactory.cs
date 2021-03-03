using AgileEngine.ImageGallerySearch.Model;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AgileEngine.ImageGallerySearch.Helpers
{
    public interface IAuthenticationHeaderFactory
    {
        Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync();
    }

    public class AuthenticationHeaderFactory : IAuthenticationHeaderFactory
    {
        public IRestHttpProxy RestHttpProxy { get; }
        public ImageSearchConfig ImageSearchConfig { get; }

        public AuthenticationHeaderFactory(IRestHttpProxy restHttpProxy,
            IOptions<ImageSearchConfig> imageSearchConfig)
        {
            RestHttpProxy = restHttpProxy;
            ImageSearchConfig = imageSearchConfig.Value;
        }


        public async Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync()
        {
            var authRequestSend = new AuthenticationRequestSend { ApiKey = ImageSearchConfig.ApiKey };
            var response = await RestHttpProxy.PostWebRequestAsync<AuthenticationRequestResponse, AuthenticationRequestSend>(new System.Uri($"{ImageSearchConfig.BaseUrl}/{ImageSearchConfig.AuthPath}"), authRequestSend);
            return new AuthenticationHeaderValue("Bearer", response.Token);
        }
    }

    public class AuthenticationRequestSend
    {
        public string ApiKey { get; set; }
    }

    public class AuthenticationRequestResponse
    {
        public bool Auth { get; set; }
        public string Token { get; set; }
    }
}
