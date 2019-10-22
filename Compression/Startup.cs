using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

namespace Compression
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });
            // добавляем сервис компрессии
            services.AddResponseCompression(options =>
            {
                options.Providers.Add(new DeflateCompressionProvider());
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
            {
                "image/svg+xml",
                "application/atom+xml"
            });
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // подключаем компрессию
            app.UseResponseCompression();
            app.Run(async context =>
            {
                // отправляемый текст
                string loremIpsum = "Lorem Ipsum is simply dummy text ... including versions of Lorem Ipsum.";
                // установка mime-типа отправляемых данных
                context.Response.ContentType = "text/plain";
                // отправка ответа
                await context.Response.WriteAsync(loremIpsum);
            });
        }
    }
}
