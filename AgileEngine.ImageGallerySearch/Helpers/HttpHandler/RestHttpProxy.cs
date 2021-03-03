using AgileEngine.ImageGallerySearch.Exceptions;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgileEngine.ImageGallerySearch.Helpers
{
    public class RestHttpProxy : IRestHttpProxy
    {
        public async Task<T> GetWebRequestAsync<T>(Uri uri, RequestConfiguration requestConfiguration = null) where T : class
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            return await CreateRequest<T>(requestConfiguration, httpRequestMessage).ConfigureAwait(false);
        }

        public async Task<T> PostWebRequestAsync<T, U>(Uri uri, U body, RequestConfiguration requestConfiguration = null) where T : class
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(CustomJsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            return await CreateRequest<T>(requestConfiguration, httpRequestMessage).ConfigureAwait(false);
        }

        private async Task<T> CreateRequest<T>(RequestConfiguration requestConfiguration, HttpRequestMessage httpRequestMessage) where T : class
        {
            if (requestConfiguration != null &&
                            requestConfiguration.AuthenticationValue != null)
            {
                httpRequestMessage.Headers.Authorization = requestConfiguration.AuthenticationValue;
            }

            using (var httpClient = new HttpClient())
            {
                using (var httpResponse = httpClient.SendAsync(httpRequestMessage))
                {
                    var responseMessage = await httpResponse.ConfigureAwait(false);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseBody = await responseMessage.Content.ReadAsStringAsync();
                        return CustomJsonSerializer.Deserialize<T>(responseBody);
                    }

                    if(responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedException();
                    }
                    else
                    {
                        throw new Exception($"Error requesting {httpRequestMessage.RequestUri}. Status code: {responseMessage.StatusCode}");
                    }
                }
            }
        }

    }
}
