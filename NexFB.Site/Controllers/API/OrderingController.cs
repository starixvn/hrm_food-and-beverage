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
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.Ordering;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;

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
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
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
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
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
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
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
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public MealOrderingModel GetAddNew()
        {
            var criteria = new GetMemberNewOrderCriteria()
            {
                UserId = this.CurrentUser.UserId
            };

            MealOrderingModel order = _orderService.GetMemberNewOrder(criteria);

            if (order == null)
            {
                order = new MealOrderingModel();
            }

            return order;
        }

        /// <summary>
        /// Get order by user id.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>The model meal ordering.</returns>
        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public MealOrderingModel GetOrderByUserId(int userId)
        {
            var criteria = new GetMemberNewOrderCriteria()
            {
                UserId = userId
            };

            MealOrderingModel order = _orderService.GetMemberNewOrder(criteria);
            return order;
        }

        /// <summary>
        /// Function to get saved order.
        /// </summary>
        /// <returns>MealOrdering Model.</returns>
        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public MealOrderingModel GetSavedOrder()
        {
            var criteria = new GetMemberSavedOrderCriteria()
            {
                UserId = this.CurrentUser.UserId
            };

            MealOrderingModel order = _orderService.GetMemberSavedOrder(criteria);

            if (order == null)
            {
                order = new MealOrderingModel();
            }

            return order;
        }

        /// <summary>
        /// Function to get saved order.
        /// </summary>
        /// <param name="orderId">The Order Id.</param>
        /// <returns>MealOrdering Model.</returns>
        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public MealOrderingModel GetOrderById(int orderId)
        {
            var criteria = new GetOrderByIdCriteria()
            {
                OrderId = orderId
            };

            MealOrderingModel order = _orderService.GetOrderById(criteria);
            order.CurrentUser = new Employee() 
            {
                UserId = this.CurrentUser.UserId,
                NickName = this.CurrentUser.NickName,
                FullName = this.CurrentUser.FullName
            };

            return order;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public IList<Employee> GetEmployees(int departmentId, string keyWord)
        {
            var criteria = new GetEmployeeCriteria()
            {
                DepartmentId = departmentId == 0 ? string.Empty : departmentId.ToString(),
                Keyword = keyWord
            };

            IList<Employee> employees = _employeeService.GetEmployeeByQuery(criteria);
            return employees;
        }

        /// <summary>
        /// Function to insert a order.
        /// </summary>
        /// <param name="model">MealOrdering Model.</param>
        /// <returns>Command Result.</returns>
        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Host)]
        public object Post(MealOrderingModel model)
        {
            var criteria = new InsertOrderCriteria() 
            { 
                CreatedBy = this.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                IsSavedOrder = model.IsSavedOrder,
                LocationId = model.Location != null ? (int?)model.Location.LocationId : null,
                Phone = model.Phone,
                Remark = model.Remark,
                Members = model.Members
            };

            CommandResult result = _orderService.InserOrder(criteria);
            return new { result = result, orderId = criteria.OrderId };
        }

        /// <summary>
        /// Function to update a order.
        /// </summary>
        /// <param name="model">MealOrdering Model.</param>
        /// <returns>Command Result.</returns>
        [HttpPut]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Host)]
        public CommandResult Put(MealOrderingModel model)
        {
            var criteria = new UpdateOrderCriteria()
            {
                OrderId = model.OrderId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now,
                IsSavedOrder = false,
                LocationId = model.Location.LocationId,
                Phone = model.Phone,
                Remark = model.Remark,
                Members = model.Members
            };

            CommandResult result = _orderService.UpdateOrder(criteria);
            return result;
        }

        /// <summary>
        /// Function to delete a order.
        /// </summary>
        /// <param name="orderId">The order id.</param>
        /// <returns>Command Result.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Host)]
        public CommandResult Delete(int orderId)
        {
            var criteria = new DeleteOrderCriteria()
            {
                OrderId = orderId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            CommandResult result = _orderService.DeleteOrder(criteria);
            return result;
        }

        public Employee GetCurrentUser()
        {
            var user = new Employee()
            {
                UserId = this.CurrentUser.UserId,
                NickName = this.CurrentUser.NickName,
                FullName = this.CurrentUser.FullName
            };

            return user;
        }
    }
}