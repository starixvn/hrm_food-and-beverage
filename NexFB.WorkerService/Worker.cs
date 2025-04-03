using NexFB.WorkerService.Models;
using NexFB.WorkerService.Services;

namespace NexFB.WorkerService;

public class Worker(ILogger<Worker> logger, 
    IEmployeeService employeeService,
    IDepartmentService departmentService,
    IShiftService shiftService,
    IUnitService unitService,
    AppSettings appSettings) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int millisecond = 1000;

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Service started at " + DateTime.Now);
            await employeeService.StartEmployeeJobAsync();
            await departmentService.StartDepartmentJobAsync();
            await shiftService.StartShiftJobAsync();
            await unitService.StartUnitJobAsync();
            logger.LogInformation("Service stopped at " + DateTime.Now);

            var stopTime = appSettings.TimeInterval * millisecond;
            await Task.Delay(stopTime, stoppingToken);
        }
    }
}