namespace NexFB.Web.Services.Implement
{
    using System.Linq;
    using NexFB.Core.Enums;
    using NexFB.Models.Authority;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// The authority service.
    /// </summary>
    public class WebCommonService : IWebCommonService
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
            UserFunction currentFunction = SessionManager.CurrentUser.Functions
                        .Where(func => func.FunctionKey.Equals(functionKey)).FirstOrDefault();

            if (currentFunction != null)
            {
                switch (policy)
                {
                    case Policy.None:
                        return false;

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
