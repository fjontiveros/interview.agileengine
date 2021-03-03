using System.Net.Http.Headers;

namespace AgileEngine.ImageGallerySearch.Helpers
{
    public class RequestConfiguration
    {
        public AuthenticationType AuthenticationType { get; set; }
        public AuthenticationHeaderValue AuthenticationValue { get; set; }
    }
}
