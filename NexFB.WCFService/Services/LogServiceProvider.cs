namespace NexFB.WCFService.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Microsoft.Extensions.Logging;

    public class LogServiceProvider : ILogServiceProvider
    {
        private readonly Fanex.Logging.ILogger logger;
        private readonly ILogger logfile;

        public LogServiceProvider(
            Fanex.Logging.ILogger logger,
            ILogger<LogServiceProvider> logfile)
        {
            this.logger = logger;
            this.logfile = logfile;
        }

        public void LogFile(string message)
        {
            logfile.LogError(message);
        }

        public async Task LogExceptionAsync(Exception exception)
        {
            var logMessage = new LogMessage(exception.Message)
                .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                .ToString();
            await logger.ErrorAsync(logMessage);
        }
    }
}
