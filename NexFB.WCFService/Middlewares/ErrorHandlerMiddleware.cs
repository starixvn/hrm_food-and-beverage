namespace NexFB.WCFService.Middlewares
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using NexFB.WCFService.Services;
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogServiceProvider logServiceProvider;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogServiceProvider logServiceProvider)
        {
            this.next = next;
            this.logServiceProvider = logServiceProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var message = string.Format(
                    "{0}\n\rSource: {1}\n\rDomain: {2}\n\rTargetSite: {3}\n\rStackTrace: {4}",
                    ex.Message,
                    ex.Source,
                    context.Request.Headers["Host"],
                    ex.TargetSite,
                    ex.StackTrace);

                logServiceProvider.LogException(ex);
                logServiceProvider.LogFile(message);
            }
        }
    }
}
