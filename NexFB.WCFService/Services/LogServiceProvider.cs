namespace NexFB.WCFService.Services
{
    using System;
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

        public void LogException(Exception exception)
        {
            logger.Error(exception.ToString());
        }
    }
}
