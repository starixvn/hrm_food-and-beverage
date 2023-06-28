namespace NexFB.Web.Attributes
{
    using System;
    using System.Web.Http.Filters;
    using Fanex.Logging;

    [AttributeUsage(AttributeTargets.All)]
    public sealed class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var localFlag = actionExecutedContext.Request.Properties["MS_IsLocal"] as Lazy<bool>;
            if (localFlag == null || !localFlag.Value)
            {
                var exception = actionExecutedContext.Exception;

                LogService.Error(exception.Message, exception);
            }
        }
    }
}