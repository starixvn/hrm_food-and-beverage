// ----------------------------------------------------------------------
// <copyright file="ICommonService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/26/2014 10:18:56 AM">Created.</change>
// </history>

namespace NexFB.Web.Service.Interface
{
    using NexFB.Core.Enums;

    /// <summary>
    /// The common service.
    /// </summary>
    public interface ICommonService
    {
        bool HasPermissionOnFunction(string functionKey, Policy policy);
    }
}
