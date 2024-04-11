namespace NexFB.WCFService.Services
{
    using System;
    using System.Threading.Tasks;

    public interface ILogServiceProvider
    {
        void LogFile(string message);

        Task LogExceptionAsync(Exception exception);
    }
}
