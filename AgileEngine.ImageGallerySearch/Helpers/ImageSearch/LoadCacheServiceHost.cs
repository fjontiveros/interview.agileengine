using AgileEngine.ImageGallerySearch.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AgileEngine.ImageGallerySearch.Helpers.ImageSearch
{
    public class LoadCacheServiceHost : IHostedService, IDisposable
    {
        private Timer _timer;

        public ImageSearchConfig ImageSearchConfig { get; }
        public IImageCacheRepository ImageCacheRepository { get; }

        public LoadCacheServiceHost(IOptions<ImageSearchConfig> imageSearchConfig,
            IImageCacheRepository imageCacheRepository)
        {
            ImageSearchConfig = imageSearchConfig.Value;
            ImageCacheRepository = imageCacheRepository;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(ImageSearchConfig.RefreshTime));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            ImageCacheRepository.LoadCache();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //_logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
