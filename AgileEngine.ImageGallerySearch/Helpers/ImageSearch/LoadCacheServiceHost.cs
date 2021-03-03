using AgileEngine.ImageGallerySearch.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        public ILogger<LoadCacheServiceHost> Logger { get; }

        public LoadCacheServiceHost(IOptions<ImageSearchConfig> imageSearchConfig,
            IImageCacheRepository imageCacheRepository,
            ILogger<LoadCacheServiceHost> logger)
        {
            ImageSearchConfig = imageSearchConfig.Value;
            ImageCacheRepository = imageCacheRepository;
            Logger = logger;
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
            try
            {
                ImageCacheRepository.LoadCache().Wait();
            }
            catch(Exception e)
            {
                Logger.LogError(e, "Error loading cache");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
