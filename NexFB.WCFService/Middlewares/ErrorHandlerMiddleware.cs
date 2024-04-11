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
                await logServiceProvider.LogExceptionAsync(ex);
            }
        }
    }
}
