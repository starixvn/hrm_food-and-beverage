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

    public class UserControlController : BaseController
    {
        private readonly IUserControlService _userControlService;
        private readonly ICommonService _commonService;
        private readonly IWebCommonService _webCommonService;
        private readonly IEmployeeService _employeeService;

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
            var criteria = new GetFunctionByRoleCriteria
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

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageUser, Policy = Policy.Read)]
        public IEnumerable<EmployeeModel> GetAllEmployeeRole(
            string keyword = null,
            string companyId = null,
            string teamId = null,
            string roleId = null)
        {
            var criteria = new GetAllEmployeeRoleCriteria
            {
                Keyword = keyword,
                CompanyId = companyId,
                TeamId = teamId,
                RoleId = roleId
            };

            return _userControlService.GetAllEmployeeRole(criteria);
        }

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

        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageUser, Policy = Policy.Host)]
        public CommandResult AssignFreeOrderToUser(AssignFreeOrderToUserCriteria criteria)
        {
            return _userControlService.AssignFreeOrderToUser(criteria);
        }

        [HttpGet]
        public int GetManageUserPermission()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.ManageUser);
            return policy;
        }

        [HttpGet]
        public string GetDefaultPassword()
        {
            return Constants.DefaultPassword;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageUser, Policy = Policy.Host)]
        public CommandResult ResetPassword(int userId)
        {
            var criteria = new ChangePasswordCriteria
            {
                UserId = userId,
                Password = Constants.DefaultPassword,
                ModifiedBy = this.CurrentUser.UserId
            };

            return _employeeService.UpdatePassword(criteria);
        }
    }
}