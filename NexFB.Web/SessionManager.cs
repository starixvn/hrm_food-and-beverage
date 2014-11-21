// --------------------------------------------------------------------------
// <copyright file="SessionManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Web
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using NexFB.Core.Constants;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.UserControl;
    using NexFB.Models.Authority;
    using NexFB.Models.UserControl;
    using NexFB.Services.Implement;
    using Fanex.Common;

    /// <summary>
    /// Session Manager.
    /// </summary>
    public static class SessionManager
    {
        private static IList<RoleFunctionModel> _allRoleFunctions = null;
        private static IDictionary<int, IList<int>> _allUserRoles = new Dictionary<int, IList<int>>();

        public static UserSession CurrentUser
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    return (UserSession)HttpContext.Current.Session[Constants.CurrentSessionKey];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                HttpContext.Current.Session[Constants.CurrentSessionKey] = value;
                UpdateUserRoles(value);
            }
        }

        public static IList<RoleFunctionModel> Functions
        {
            get
            {
                IEnumerable<RoleFunctionModel> functions =
                    AllRoleFunctions.Where(t => UserRoles.Contains(t.RoleId)).GroupBy(t => t.FunctionId).Select(grp => grp.First());

                return functions.ToList();
            }
        }

        public static IList<MenuGroup> MenuGroups
        {
            get
            {
                var getEmployeeMenuCriteria = new GetEmployeeMenuBySystemIdCriteria
                {
                    SystemId = CurrentUser.UserId
                };

                return new EmployeeService().GetEmployeeMenuById(getEmployeeMenuCriteria);
            }
        }

        private static IList<RoleFunctionModel> AllRoleFunctions
        {
            get
            {
                if (_allRoleFunctions == null)
                {
                    GetFunctionByRoleCriteria criteria = new GetFunctionByRoleCriteria()
                    {
                        RoleId = null
                    };
                    _allRoleFunctions = new UserControlService().GetAllRoleFunction(criteria);
                }

                return _allRoleFunctions;
            }
        }

        private static IList<int> UserRoles
        {
            get
            {
                IList<int> userRoles = new List<int>();
                if (CurrentUser != null)
                {
                    if (_allUserRoles.ContainsKey(CurrentUser.UserId))
                    {
                        userRoles = _allUserRoles[CurrentUser.UserId];
                    }
                    else
                    {
                        userRoles = GetUserRoles(CurrentUser);
                    }
                }

                return userRoles;
            }
        }

        /// <summary>
        /// Function to clear session manager.
        /// </summary>
        public static void ClearAll()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (CurrentUser != null)
                {
                    _allUserRoles.Remove(CurrentUser.UserId);
                }
                
                CurrentUser = null;
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Cookies.Clear();
            }
        }

        private static IList<int> GetUserRoles(UserSession user)
        {
            IList<int> userRoles = new List<int>();

            if (!string.IsNullOrWhiteSpace(user.Roles))
            {
                string[] roleArr = user.Roles.Split(',');
                int result;
                foreach (string role in roleArr)
                {
                    result = Converter.ToInt(role);
                    if (result != 0)
                    {
                        userRoles.Add(result);
                    }
                }

                SetUserRoles(user.UserId, userRoles);
            }

            return userRoles;
        }

        private static void SetUserRoles(int userId, IList<int> userRoles)
        {
            if (_allUserRoles.ContainsKey(userId))
            {
                _allUserRoles[userId] = userRoles;
            }
            else
            {
                _allUserRoles.Add(userId, userRoles);
            }
        }

        private static void UpdateUserRoles(UserSession user)
        {
            if (user != null)
            {
                IList<int> userRoles = GetUserRoles(user);
                SetUserRoles(user.UserId, userRoles);
            }
        }
    }
}
