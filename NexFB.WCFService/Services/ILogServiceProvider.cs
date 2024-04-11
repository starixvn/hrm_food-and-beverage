namespace NexFB.WCFService.Services
{
    using System;
    public interface ILogServiceProvider
    {
        void LogFile(string message);

        void LogException(Exception exception);
    }
}
