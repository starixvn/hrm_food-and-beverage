// --------------------------------------------------------------------------
// <copyright file="EmployeeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.12.5">Created</change>
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
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// Employee controller.
    /// </summary>
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IWebCommonService _webCommonService;

        public EmployeeController() : this(null, null)
        {
        }

        public EmployeeController(IEmployeeService employeeService, IWebCommonService webCommonService)
        {
            _employeeService = employeeService ?? new EmployeeService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        [HttpGet]
        public IList<Employee> GetEmployees(string keyWord)
        {
            var criteria = new GetEmployeeCriteria()
            {
                DepartmentId = this.CurrentUser.DepartmentId.ToString(),
                Keyword = keyWord
            };

            IList<Employee> employees = _employeeService.GetEmployeeByQuery(criteria);
            return employees;
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SpecialEmployee, Policy = Policy.Read)]
        [HttpGet]
        public SpecialUser GetSpecialUserById(int userId)
        {
            var criteria = new GetSpecialUserByIdCriteria() 
            { 
                UserId = userId
            };
            SpecialUser user = _employeeService.GetSpecialUserById(criteria);

            return user;
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SpecialEmployee, Policy = Policy.Read)]
        [HttpGet]
        public IList<SpecialUser> GetSpecialUsers(string departmentId, string employeeTypeId, bool? isActive = null)
        {
            var criteria = new GetSpecialUserCriteria() 
            { 
                DepartmentId = departmentId,
                EmployeeTypeId = employeeTypeId,
                IsActive = isActive
            };
            IList<SpecialUser> users = _employeeService.GetSpecialUsers(criteria);
            return users;
        }

        /// <summary>
        /// Function to insert special employee.
        /// </summary>
        /// <param name="model">The Special User.</param>
        /// <returns>Command Result.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.SpecialEmployee, Policy = Policy.Host)]
        [HttpPost]
        public CommandResult InsertSpecialUser(SpecialUser model)
        {
            var criteria = new InsertSpecialUserCriteria()
            {
                CreatedBy = this.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                Code = model.Code,
                NickName = model.NickName,
                EmployeeCard = model.EmployeeCard,
                DepartmentId = model.DepartmentId,
                EmployeeTypeId = model.EmployeeType,
                IsActive = model.IsActive
            };

            return _employeeService.InsertSpecialUser(criteria);
        }

        /// <summary>
        /// Function to update special employee.
        /// </summary>
        /// <param name="model">The Special User.</param>
        /// <returns>Command Result.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.SpecialEmployee, Policy = Policy.Host)]
        [HttpPut]
        public CommandResult UpdateSpecialUser(SpecialUser model)
        {
            var criteria = new UpdateSpecialUserCriteria()
            {
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now,
                UserId = model.UserId,
                Code = model.Code,
                NickName = model.NickName,
                EmployeeCard = model.EmployeeCard == null ? string.Empty : model.EmployeeCard,
                DepartmentId = model.DepartmentId,
                EmployeeTypeId = model.EmployeeType,
                IsActive = model.IsActive
            };

            return _employeeService.UpdateSpecialUser(criteria);
        }

        /// <summary>
        /// Function to update status of special employee.
        /// </summary>
        /// <param name="model">The Special User.</param>
        /// <returns>Command Result.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.SpecialEmployee, Policy = Policy.Host)]
        [HttpPut]
        public CommandResult UpdateStatusSpecialUser(SpecialUser model)
        {
            var criteria = new ActivateSpecialUserCriteria()
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