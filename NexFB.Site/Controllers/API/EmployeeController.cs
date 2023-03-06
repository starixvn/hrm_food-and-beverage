namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using HRM.UserControl.Client.Core.Models.User;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IWebCommonService _webCommonService;

        public EmployeeController()
            : this(null, null)
        {
        }

        public EmployeeController(IEmployeeService employeeService, IWebCommonService webCommonService)
        {
            _employeeService = employeeService ?? new EmployeeService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        [HttpGet]
        public IList<Employee> GetEmployees(string keyWord = "", bool? isIncludeMember = null, string companyId = "", string teamId = null, bool isShowDisable = false)
        {
            var criteria = new GetEmployeeCriteria
            {
                UserRequestId = this.CurrentUser.UserId,
                CompanyId = string.IsNullOrWhiteSpace(companyId) ? this.CurrentUser.CompanyId.ToString() : companyId,
                IsIncludeMemberOtherDepartment = isIncludeMember,
                Keyword = keyWord,
                TeamId = teamId,
                IsShowDisable = isShowDisable
            };

            IList<Employee> employees = _employeeService.GetEmployeeByQuery(criteria);
            return employees;
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SpecialEmployee, Policy = Policy.Read)]
        [HttpGet]
        public SpecialUser GetSpecialUserById(int userId)
        {
            var criteria = new GetSpecialUserByIdCriteria
            {
                UserId = userId
            };
            SpecialUser user = _employeeService.GetSpecialUserById(criteria);

            return user;
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SpecialEmployee, Policy = Policy.Read)]
        [HttpGet]
        public IList<SpecialUser> GetSpecialUsers(string companyId, string employeeTypeId, bool? isActive = null)
        {
            var criteria = new GetSpecialUserCriteria
            {
                CompanyId = companyId,
                EmployeeTypeId = employeeTypeId,
                IsActive = isActive
            };
            IList<SpecialUser> users = _employeeService.GetSpecialUsers(criteria);
            return users;
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SpecialEmployee, Policy = Policy.Host)]
        [HttpPost]
        public CommandResult InsertSpecialUser(SpecialUser model)
        {
            var criteria = new InsertSpecialUserCriteria
            {
                CreatedBy = this.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                Code = model.Code,
                NickName = model.NickName,
                EmployeeCard = model.EmployeeCard,
                CompanyId = model.CompanyId,
                EmployeeTypeId = model.EmployeeType,
                IsActive = model.IsActive
            };

            return _employeeService.InsertSpecialUser(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SpecialEmployee, Policy = Policy.Host)]
        [HttpPut]
        public CommandResult UpdateSpecialUser(SpecialUser model)
        {
            var criteria = new UpdateSpecialUserCriteria
            {
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now,
                UserId = model.UserId,
                Code = model.Code,
                NickName = model.NickName,
                EmployeeCard = model.EmployeeCard ?? string.Empty,
                CompanyId = model.CompanyId,
                EmployeeTypeId = model.EmployeeType,
                IsActive = model.IsActive
            };

            return _employeeService.UpdateSpecialUser(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SpecialEmployee, Policy = Policy.Host)]
        [HttpPut]
        public CommandResult UpdateStatusSpecialUser(SpecialUser model)
        {
            var criteria = new ActivateSpecialUserCriteria
            {
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now,
                UserId = model.UserId,
                IsActive = model.IsActive
            };

            return _employeeService.UpdateStatusSpecialUser(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SpecialEmployee, Policy = Policy.Host)]
        [HttpDelete]
        public CommandResult DeleteSpecialUser(int userId)
        {
            var criteria = new DeleteSpecialUserCriteria
            {
                UserId = userId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            return _employeeService.DeleteSpecialUser(criteria);
        }

        public int GetUserPermissionOnSpecialEmployee()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.SpecialEmployee);
            return policy;
        }
    }
}