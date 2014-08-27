// ----------------------------------------------------------------------
// <copyright file="CommonService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/27/2014 10:13:58 AM">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using NexFB.Core.Enums;
    using NexFB.Models.UserControl;
    using NexFB.Services.Interface;

    /// <summary>
    /// The common service.
    /// </summary>
    public class CommonService : ICommonService
    {
        /// <summary>
        /// Get permission on function.
        /// </summary>
        /// <param name="functionKey">The function key.</param>
        /// <returns>The permission.</returns>
        public int GetPermission(string functionKey)
        {
            int policy = (int)Policy.None;

            if (HasPermissionOnFunction(functionKey, Policy.Host))
            {
                policy = (int)Policy.Host;
            }
            else if (HasPermissionOnFunction(functionKey, Policy.Read))
            {
                policy = (int)Policy.Read;
            }

            return policy;
        }

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
