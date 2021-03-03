using AgileEngine.ImageGallerySearch.Helpers;
using AgileEngine.ImageGallerySearch.Helpers.ImageSearch;
using AgileEngine.ImageGallerySearch.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileEngine.ImageGallerySearch
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AgileEngine.ImageGallerySearch", Version = "v1" });
            });

            services.Configure<ImageSearchConfig>(x => Configuration.GetSection("ImageSearchConfiguration").Bind(x));

            services.AddSingleton<IImageSearchEngine, ImageSearchEngine>();
            services.AddSingleton<IRestHttpProxy, RestHttpProxy>();
            services.AddSingleton<IAuthenticationHeaderFactory, AuthenticationHeaderFactory>();
            services.AddSingleton<IImageCacheRepository, InMemoryImageCacheRepository>();
            services.AddSingleton<IImageAgileEngineClient, ImageAgileEngineClient>();
            services.AddHostedService<LoadCacheServiceHost>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgileEngine.ImageGallerySearch v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
