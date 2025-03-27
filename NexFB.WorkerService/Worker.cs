using NexFB.WorkerService.Services;

namespace NexFB.WorkerService
{
    public class Worker(ILogger<Worker> logger, IEmployeeService employeeService) : BackgroundService
    {
        private readonly ILogger<Worker> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await employeeService.StartEmployeeJobAsync();
                //if (_logger.IsEnabled(LogLevel.Information))
                //{
                //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //}
                //await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
