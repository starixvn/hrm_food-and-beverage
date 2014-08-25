// ----------------------------------------------------------------------
// <copyright file="ExceptionHandlingAttribute.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/25/2014 1:17:20 PM">Created.</change>
// </history>

namespace NexFB.Site.App_Start
{
    using System;
    using System.Web;
    using System.Web.Http.Filters;
    using Fanex.Logging;

    /// <summary>
    /// Exception Handling attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Function to write log for API controller exception.
        /// </summary>
        /// <param name="actionExecutedContext">HttpActionExecuted Context.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var localFlag = actionExecutedContext.Request.Properties["MS_IsLocal"] as Lazy<bool>;
            if (localFlag == null || !localFlag.Value)
            {
                var exception = actionExecutedContext.Exception;
                string message = exception.Message +
                        "\r\nSource: " + exception.Source +
                        "\r\nDomain: " + HttpContext.Current.Request.Headers["Host"] +
                        "\r\nTargetSite: " + exception.TargetSite +
                        "\r\nStackTrace: " + exception.StackTrace;

                LogService.Error(message);
            }
        }
    }
}