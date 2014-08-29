// ----------------------------------------------------------------------
// <copyright file="ApiAuthorizeAttribute.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/26/2014 10:09:46 AM">Created.</change>
// </history>

namespace NexFB.Web.Attributes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using NexFB.Core.Enums;
    using NexFB.Models.Authority;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// API authorize attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Reviewed. Suppression is OK here.")]
    public sealed class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        private string _functionKey;
        private Policy _policy;
        private ICommonService _commonService;

        public ApiAuthorizeAttribute()
            : this(null)
        {
        }

        public ApiAuthorizeAttribute(ICommonService commonService)
        {
            _commonService = commonService ?? new CommonService();
        }

        public string FunctionKey
        {
            get
            {
                return _functionKey != null ? _functionKey : string.Empty;
            }

            set
            {
                _functionKey = value;
            }
        }

        public Policy Policy
        {
            get
            {
                return _policy;
            }

            set
            {
                _policy = value;
            }
        }

        /// <summary>
        /// Authorization event handler.
        /// </summary>
        /// <param name="actionContext">HttpAction Context.</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            UserSession currentUser = SessionManager.CurrentUser;

            if (currentUser != null)
            {
                CheckUserPermission(actionContext);
                return;
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

        /// <summary>
        /// Check user permission.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        private void CheckUserPermission(HttpActionContext actionContext)
        {
            Collection<ApiAuthorizeAttribute> attributes = actionContext.ActionDescriptor.GetCustomAttributes<ApiAuthorizeAttribute>();

            foreach (ApiAuthorizeAttribute item in attributes)
            {
                if (!_commonService.HasPermissionOnFunction(item.FunctionKey, item.Policy))
                {
                    HandlePermissionRequest(actionContext);
                }
            }
        }
    }
}
