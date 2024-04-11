namespace NexFB.WCFService
{
    using System;
    using System.Collections.Generic;
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
    using Sentry;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            LogManager
               .SetDefaultLogCategory(Configuration.GetSection("Fanex.Logging:LogCategory").Value)
               .Use(new List<ILogEngine>
               {
                    new SentryLogging(new SentryEngineOptions
                    {
                        Dsn = new Dsn(Configuration.GetSection("Fanex.Logging:SentryEngineOptions:Dsn").Value)
                    })
               });

            services.AddFanexSql(Configuration);
            services.AddServiceModelServices();
            services.AddServiceModelMetadata();
            services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<IDynamicRepository, DynamicRepository>();
            services.AddSingleton<IBaseService, BaseService>();
            services.AddSingleton<ILogServiceProvider, LogServiceProvider>();
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
                serviceBuilder.AddServiceEndpoint<SynchronizeDataService, ISynchronizeDataService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/Service.svc");
                var serviceMetadataBehavior = serviceProvider.GetRequiredService<ServiceMetadataBehavior>();
                serviceMetadataBehavior.HttpsGetEnabled = true;
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
