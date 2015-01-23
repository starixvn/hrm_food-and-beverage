// ----------------------------------------------------------------------
// <copyright file="UserControlController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/26/2014 2:04:39 PM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.UserControl;
    using NexFB.Models.Employee;
    using NexFB.Models.UserControl;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// The user control controller.
    /// </summary>
    public class UserControlController : BaseController
    {
        private IUserControlService _userControlService;

        private ICommonService _commonService;

        private IWebCommonService _webCommonService;

        private IEmployeeService _employeeService;

        public UserControlController()
            : this(null, null, null, null)
        {
        }

        public UserControlController(
            IUserControlService userControlService,
            ICommonService commonService,
            IWebCommonService webCommonService,
            IEmployeeService employeeService)
        {
            _userControlService = userControlService ?? new UserControlService();
            _commonService = commonService ?? new CommonService();
            _webCommonService = webCommonService ?? new WebCommonService();
            _employeeService = employeeService ?? new EmployeeService();
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageRole, Policy = Policy.Read)]
        public IEnumerable<FunctionModel> GetFunctionByRole(int roleId)
        {
            var criteria = new GetFunctionByRoleCriteria()
            {
                RoleId = roleId
            };

            return _userControlService.GetFunctionByRole(criteria);
        }

        [HttpGet]
        public IEnumerable<RoleSystemModel> GetAllSystemRole(bool exceptSystemAdmin = false)
        {
            var criteria = new GetAllSystemRoleCriteria();

            return _commonService.GetAllSystemRole(criteria, exceptSystemAdmin);
        }

        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageRole, Policy = Policy.Host)]
        public CommandResult AssignPermissionToRole(AssignPermissionToRoleCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _userControlService.AssignPermissionToRole(criteria);
        }

        [HttpGet]
        public int GetManageRolePermission()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.ManageRole);
            return policy;
        }

        /// <summary>
        /// Get all employee role.
        /// </summary>
        /// <param name="keyword">The keyword to search.</param>
        /// <param name="departmentId">The department ID.</param>
        /// <param name="teamId">The team ID.</param>
        /// <param name="roleId">The role ID.</param>
        /// <returns>The enumerator Employee model.</returns>
        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageUser, Policy = Policy.Read)]
        public IEnumerable<EmployeeModel> GetAllEmployeeRole(
            string keyword = null,
            string departmentId = null,
            string teamId = null,
            string roleId = null)
        {
            var criteria = new GetAllEmployeeRoleCriteria
            {
                Keyword = keyword,
                DepartmentId = departmentId,
                TeamId = teamId,
                RoleId = roleId
            };

            return _userControlService.GetAllEmployeeRole(criteria);
        }

        /// <summary>
        /// Get employee role by Id.
        /// </summary>
        /// <param name="systemId">The system ID.</param>
        /// <returns>The employee model.</returns>
        [HttpGet]
        public EmployeeModel GetEmployeeRoleById(int systemId)
        {
            var criteria = new GetEmployeeRoleByIdCriteria
            {
                SystemId = systemId
            };

            return _userControlService.GetEmployeeRoleById(criteria);
        }

        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageUser, Policy = Policy.Host)]
        public CommandResult AssignRoleToUser(AssignRoleToUserCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _userControlService.AssignRoleToUser(criteria);
        }

        [HttpGet]
        public int GetManageUserPermission()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.ManageUser);
            return policy;
        }

        /// <summary>
        /// Reset Password.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <returns>Command Result.</returns>
        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageUser, Policy = Policy.Host)]
        public CommandResult ResetPassword(int userId)
        {
            var criteria = new ChangePasswordCriteria()
            {
                UserId = userId,
                Password = Constants.DefaultPassword,
                ModifiedBy = this.CurrentUser.UserId
            };

            return _employeeService.UpdatePassword(criteria);
        }
    }
}