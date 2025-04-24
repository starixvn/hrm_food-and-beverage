using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using NexFB.WorkerService.Models;
using NexFB.WorkerService.Services;

namespace NexFB.WorkerService;

public class Worker(
    ILogger<Worker> logfile,
    Fanex.Logging.ILogger logger,
    IEmployeeService employeeService,
    IDepartmentService departmentService,
    ISectionService sectionService,
    IShiftService shiftService,
    IUnitService unitService,
    AppSettings appSettings) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int millisecond = 1000;

        while (!stoppingToken.IsCancellationRequested)
        {
            logfile.LogInformation("Service started at " + DateTime.Now);

            try
            {
                await employeeService.StartEmployeeJobAsync();
                await departmentService.StartDepartmentJobAsync();
                await sectionService.StartSectionJobAsync();
                await shiftService.StartShiftJobAsync();
                await unitService.StartUnitJobAsync();
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(e);
            }

            logfile.LogInformation("Service stopped at " + DateTime.Now);

            var stopTime = appSettings.TimeInterval * millisecond;
            await Task.Delay(stopTime, stoppingToken);
        }
    }

    private async Task HandleExceptionAsync(Exception exception)
    {
        var logMessage = new LogMessage(exception.Message)
            .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
            .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
            .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
            .ToString();
        await logger.ErrorAsync(logMessage);
    }
}