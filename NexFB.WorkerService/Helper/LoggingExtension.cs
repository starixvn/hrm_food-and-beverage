using NexFB.WorkerService.Common;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Helper;

public static class LoggingExtension
{
    private static ILogger<Worker> _logger;
    public static void Config(ILogger<Worker> logger)
    {
        _logger = logger;
    }
    public static void LogUpdateNotify(int companyId, string function)
    {
        var identity = (CompanyIdentity)companyId;
        _logger.LogInformation($"Update {function} {identity.GetDescription()} started at {DateTime.Now}");
    }
    public static void LogResultNotify(int companyId, CommandResult result, string function)
    {
        var identity = (CompanyIdentity)companyId;
        if (result.IsSuccessful) result.Message = Constants.Successful;
        _logger.LogInformation($"Result of update {function} {identity.GetDescription()}: {result.Message}");
    }

    public static void LogExceptionNotify(int companyId, string function, Exception ex)
    {
        var identity = (CompanyIdentity)companyId;
        _logger.LogError(ex, $"Error in update {function} {identity.GetDescription()}");
    }
}