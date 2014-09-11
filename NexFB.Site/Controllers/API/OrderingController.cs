// --------------------------------------------------------------------------
// <copyright file="OrderingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.Ordering;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// Ordering Controller.
    /// </summary>
    public class OrderingController : BaseController
    {
        private readonly IOrderingService _orderService;
        private readonly IEmployeeService _employeeService;

        public OrderingController()
            : this(null, null)
        {
        }

        public OrderingController(IOrderingService orderService, IEmployeeService employeeService)
        {
            _orderService = orderService ?? new OrderingService();
            _employeeService = employeeService ?? new EmployeeService();
        }

        /// <summary>
        /// Function to get my orders.
        /// </summary>
        /// <returns>MealOrdering Model.</returns>
        [HttpGet]
        public IList<MealOrderingModel> GetMyOrder()
        {
            var criteria = new GetMyOrderInCurrentDayCriteria()
            {
                UserId = this.CurrentUser.UserId
            };

            IList<MealOrderingModel> orders = _orderService.GetMyOrderByCurrentDay(criteria);
            return orders;
        }

         /// <summary>
        /// Function to get menu.
        /// </summary>
        /// <returns>MealOrdering Model.</returns>
        [HttpGet]
        public Menu GetMyMenu()
        {
            var criteria = new GetMenuByUserIdCriteria()
            {
                UserId = this.CurrentUser.UserId
            };

            Menu menu = _orderService.GetMenuByUserId(criteria);
            return menu;
        }

        [HttpGet]
        public Menu GetMenuByUserId(int userId)
        {
            var criteria = new GetMenuByUserIdCriteria()
            {
                UserId = userId
            };

            Menu menu = _orderService.GetMenuByUserId(criteria);
            return menu;
        }

        /// <summary>
        /// Add new order.
        /// </summary>
        /// <returns>The model meal ordering.</returns>
        [HttpGet]
        public MealOrderingModel GetAddNew()
        {
            var criteria = new GetMemberNewOrderCriteria()
            {
                UserId = this.CurrentUser.UserId
            };

            MealOrderingModel order = _orderService.GetMemberNewOrder(criteria);
            return order;
        }

        /// <summary>
        /// Function to get saved order.
        /// </summary>
        /// <returns>MealOrdering Model.</returns>
        [HttpGet]
        public MealOrderingModel GetSavedOrder()
        {
            var criteria = new GetMemberSavedOrderCriteria()
            {
                UserId = this.CurrentUser.UserId
            };

            MealOrderingModel order = _orderService.GetMemberSavedOrder(criteria);
            return order;
        }

        [HttpGet]
        public IList<Employee> GetEmployees(int departmentId, string keyWord)
        {
            var criteria = new GetEmployeeCriteria()
            {
                DepartmentId = departmentId.ToString(),
                Keyword = keyWord
            };

            IList<Employee> employees = _employeeService.GetEmployeeByQuery(criteria);
            return employees;
        }
    }
}