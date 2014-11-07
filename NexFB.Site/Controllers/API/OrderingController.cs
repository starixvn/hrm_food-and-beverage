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
    using NexFB.Criteria.SideDishManagement;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;
    using NexFB.Models.SideDishManagement;
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
        private readonly ISideDishService _sideDishService;

        public OrderingController()
            : this(null, null, null)
        {
        }

        public OrderingController(
            IOrderingService orderService,
            IEmployeeService employeeService,
            ISideDishService sideDishService)
        {
            _orderService = orderService ?? new OrderingService();
            _employeeService = employeeService ?? new EmployeeService();
            _sideDishService = sideDishService ?? new SideDishService();
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
            return order;
        }

        /// <summary>
        /// Get order to new order.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>The model meal ordering.</returns>
        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public MealOrderingModel GetOrderNewOrder(int userId)
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

        /// <summary>
        /// Function to insert a order.
        /// </summary>
        /// <param name="model">MealOrdering Model.</param>
        /// <returns>Command Result.</returns>
        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Host)]
        public object InsertOrder(MealOrderingModel model)
        {
            var criteria = new InsertOrderCriteria() 
            { 
                CreatedBy = this.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                IsSavedOrder = model.IsSavedOrder,
                LocationId = (model.Location != null && model.Location.LocationId != 0) ? (int?)model.Location.LocationId : null,
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
        public CommandResult UpdateOrder(MealOrderingModel model)
        {
            var criteria = new UpdateOrderCriteria()
            {
                OrderId = model.OrderId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now,
                IsSavedOrder = false,
                LocationId = (model.Location != null && model.Location.LocationId != 0) ? (int?)model.Location.LocationId : null,
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

        public IEnumerable<Menu> GetAlternativeMenusByUserId(int userId)
        {
            var criteria = new GetAlternativeMenuByUserIdCriteria
            {
                UserId = userId
            };

            return _orderService.GetAlternativeMenuByUserId(criteria);
        }

        public IEnumerable<SideDishModel> GetSideDishByUserId(int userId)
        {
            var criteria = new GetSideDishByUserIdCriteria
            {
                UserId = userId
            };

            return _sideDishService.GetSideDishByUserId(criteria);
        }

        /// <summary>
        /// Function to insert a order side dish.
        /// </summary>
        /// <param name="model">MealOrdering Model.</param>
        /// <returns>Command Result.</returns>
        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Host)]
        public object InsertOrderSideDish(MealOrderingModel model)
        {
            var criteria = new InsertOrderSideDishCriteria()
            {
                CreatedBy = this.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                LocationId = (model.Location != null && model.Location.LocationId != 0) ? (int?)model.Location.LocationId : null,
                Phone = model.Phone,
                Remark = model.Remark,
                Members = model.Members
            };

            CommandResult result = _orderService.InsertOrderSideDish(criteria);
            return new { result = result, orderId = criteria.OrderId };
        }

        /// <summary>
        /// Update order side dish.
        /// </summary>
        /// <param name="model">The order.</param>
        /// <returns>The command result.</returns>
        [HttpPut]
        public CommandResult UpdateOrderSideDish(MealOrderingModel model)
        {
            var criteria = new UpdateOrderSideDishCriteria()
            {
                OrderId = model.OrderId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now,
                LocationId = (model.Location != null && model.Location.LocationId != 0) ? (int?)model.Location.LocationId : null,
                Phone = model.Phone,
                Remark = model.Remark,
                Members = model.Members
            };

            CommandResult result = _orderService.UpdateOrderSideDish(criteria);
            return result;
        }

        /// <summary>
        /// Check start time (the time when user open popup edit) must be greater than the last modified time of this order.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="orderId">The order Id.</param>
        /// <returns>The boolean to know that user can update order or not.</returns>
        [HttpGet]
        public bool CheckValidModifiedTime(string startTime, int orderId)
        {
            var criteria = new GetLastMofifiedDateByOrderIdCriteria
            {
                OrderId = orderId
            };

            MealOrderingModel order = _orderService.GetLastModifiedDateByOrderId(criteria);
            DateTime startDateTime = DateTime.Parse(startTime);

            if (startDateTime > order.ModifiedDate)
            {
                return true;
            }

            return false;
        }
    }
}