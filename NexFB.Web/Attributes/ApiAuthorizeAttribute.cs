namespace NexFB.Web.Attributes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using NexFB.Core.Enums;
    using NexFB.Models.Authority;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    [AttributeUsage(AttributeTargets.All)]
    public sealed class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        private string _functionKey;
        private readonly IWebCommonService _webCommonService;

        public ApiAuthorizeAttribute()
            : this(null)
        {
        }

        public ApiAuthorizeAttribute(IWebCommonService webCommonService)
        {
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        public string FunctionKey
        {
            get
            {
                return _functionKey ?? string.Empty;
            }

            set
            {
                _functionKey = value;
            }
        }

        public Policy Policy { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            UserSession currentUser = SessionManager.CurrentUser;

            if (currentUser != null)
            {
                CheckUserPermission(actionContext);
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            actionContext.Response = responseMessage;
        }

        private void HandlePermissionRequest(HttpActionContext actionContext)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
            actionContext.Response = responseMessage;
        }

        private void CheckUserPermission(HttpActionContext actionContext)
        {
            Collection<ApiAuthorizeAttribute> attributes = actionContext.ActionDescriptor.GetCustomAttributes<ApiAuthorizeAttribute>();

            foreach (ApiAuthorizeAttribute item in attributes)
            {
                if (!_webCommonService.HasPermissionOnFunction(item.FunctionKey, item.Policy))
                {
                    HandlePermissionRequest(actionContext);
                }
            }
        }
    }
}