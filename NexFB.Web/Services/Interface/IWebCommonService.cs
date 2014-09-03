// ----------------------------------------------------------------------
// <copyright file="IWebCommonService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/3/2014 2:26:19 PM">Created.</change>
// </history>

namespace NexFB.Web.Services.Interface
{
    using NexFB.Core.Enums;

    /// <summary>
    /// The interface of authority service.
    /// </summary>
    public interface IWebCommonService
    {
        int GetPermission(string functionKey);

        bool HasPermissionOnFunction(string functionKey, Policy policy);
    }
}
