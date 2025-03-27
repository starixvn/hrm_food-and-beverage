using Fanex.Data.Repository;
using NexFB.WorkerService.Models;
using NexFB.WorkerService.Services;

namespace NexFB.WorkerService
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var apiSettings = configuration.GetSection("FnBSyncApiSettings").Get<FnBSyncApiSettings>();
            services.AddSingleton(apiSettings);

            services.AddSingleton<IBaseService, BaseService>();
            services.AddSingleton<IDynamicRepository, DynamicRepository>();
            services.AddSingleton<IBaseRepository, BaseRepository>();

            services.AddSingleton<IEmployeeService, EmployeeService>();
        }
    }
}
