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
    using NexFB.Core.Models;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Entities;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// Employee controller.
    /// </summary>
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController() : this(null)
        {
        }

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService ?? new EmployeeService();
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

       // [ApiAuthorize(FunctionKey = FBFunctionKey.FoodItemManagement, Policy = Policy.Read)]
        public Employee GetSpecialUserById(int userId)
        {
            var criteria = new GetSpecialUserByIdCriteria() 
            { 
                UserId = userId
            };
            Employee employee = _employeeService.GetSpecialUserById(criteria);

            return employee;
        }

        public IList<Employee> GetSpecialUsers()
        {
            var criteria = new GetSpecialEmployeeCriteria();
            IList<Employee> employees = _employeeService.GetSpecialUsers(criteria);

            return employees;
        }

        /// <summary>
        /// Function to insert special employee.
        /// </summary>
        /// <param name="model">The employee model.</param>
        /// <returns>Command Result.</returns>
        [HttpPost]
        public CommandResult InsertSpecialUser(Employee model)
        {
            var criteria = new InsertSpecialUserCriteria()
            {
                CreatedBy = this.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                Code = model.Code,
                Name = model.NickName,
                EmployeeCard = model.EmployeeCard,
                DepartmentId = model.DepartmentId,
                EmployeeTypeId = model.EmployeeType
            };

            return _employeeService.InsertSpecialUser(criteria);
        }

        /// <summary>
        /// Function to update special employee.
        /// </summary>
        /// <param name="model">The employee model.</param>
        /// <returns>Command Result.</returns>
        [HttpPut]
        public CommandResult UpdateSpecialUser(Employee model)
        {
            var criteria = new UpdateSpecialUserCriteria()
            {
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now,
                Code = model.Code,
                Name = model.NickName,
                EmployeeCard = model.EmployeeCard,
                DepartmentId = model.DepartmentId,
                EmployeeTypeId = model.EmployeeType
            };

            return _employeeService.UpdateSpecialUser(criteria);
        }

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
    }
}