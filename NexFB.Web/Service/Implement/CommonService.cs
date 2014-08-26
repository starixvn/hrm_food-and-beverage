// ----------------------------------------------------------------------
// <copyright file="CommonService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/26/2014 10:19:43 AM">Created.</change>
// </history>

namespace NexFB.Web.Service.Implement
{
    using NexFB.Core.Enums;
    using NexFB.Models.UserControl;
    using NexFB.Web.Service.Interface;

    /// <summary>
    /// The common service.
    /// </summary>
    public class CommonService : ICommonService
    {
        /// <summary>
        /// Check current user has permission on current function or not.
        /// </summary>
        /// <param name="functionKey">The function key.</param>
        /// <param name="policy">THe policy.</param>
        /// <returns>True if has permission, false: otherwise.</returns>
        public bool HasPermissionOnFunction(string functionKey, Policy policy)
        {
            FunctionModel currentFunction = new FunctionModel();

            if (currentFunction != null)
            {
                switch (policy)
                {
                    case Policy.None:
                        return true;

                    case Policy.Read:
                        return currentFunction.IsRead || currentFunction.IsHost;

                    case Policy.Host:
                        return currentFunction.IsHost;
                }
            }

            return false;
        }
    }
}
