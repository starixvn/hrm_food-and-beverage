// ----------------------------------------------------------------------
// <copyright file="ICommonService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/27/2014 10:13:08 AM">Created.</change>
// </history>

namespace NexFB.Services.Interface
{
    using NexFB.Core.Enums;

    /// <summary>
    /// The common service.
    /// </summary>
    public interface ICommonService
    {
        int GetPermission(string functionKey);

        bool HasPermissionOnFunction(string functionKey, Policy policy);
    }
}
