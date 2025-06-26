namespace NexFB.WCFService
{
    using System;
    using Fanex.Data.Repository;
    using Fanex.Data.SqlServer;
    using Fanex.Logging;
    using Fanex.Logging.Sentry;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NexFB.WCFService.Middlewares;
    using NexFB.WCFService.Services;

    public class Startup(IConfiguration configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddFanexLogSentry(configuration);
            services.AddFanexSql(configuration);
            services.AddServiceModelServices();
            services.AddServiceModelMetadata();
            services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

            services.AddSingleton<IDynamicRepository, DynamicRepository>();
            services.AddSingleton<IBaseService, BaseService>();
            services.AddSingleton<ILogServiceProvider, LogServiceProvider>();
            services.AddSingleton<IFBTranslatorService, FBTranslatorService>();
            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<SynchronizeDataService>();

            services.AddControllers();
        }

        public static void Configure(IApplicationBuilder app, IServiceProvider serviceProvider, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ErrorHandlerMiddleware>();
                app.UseHsts();
            }

            app.UseServiceModel(serviceBuilder =>
            {
                serviceBuilder.AddService<SynchronizeDataService>();
                serviceBuilder.AddServiceEndpoint<SynchronizeDataService, ISynchronizeDataService>(new BasicHttpBinding(BasicHttpSecurityMode.None), "/Service.svc");
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}