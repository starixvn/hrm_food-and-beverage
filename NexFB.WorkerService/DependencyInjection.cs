using Fanex.Data.Repository;
using NexFB.WorkerService.Models;
using NexFB.WorkerService.Services;

namespace NexFB.WorkerService;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var apiSettings = configuration.GetSection("FnBSyncApiSettings").Get<List<FnBSyncApiSettings>>();
        services.AddSingleton(apiSettings);

        var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
        services.AddSingleton(appSettings);

        services.AddSingleton<IBaseService, BaseService>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<IBaseRepository, BaseRepository>();

        services.AddSingleton<IEmployeeService, EmployeeService>();
        services.AddSingleton<IDepartmentService, DepartmentService>();
        services.AddSingleton<ISectionService, SectionService>();
        services.AddSingleton<IShiftService, ShiftService>();
        services.AddSingleton<IUnitService, UnitService>();
    }
}