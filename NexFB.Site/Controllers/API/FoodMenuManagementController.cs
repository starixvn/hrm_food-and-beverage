// ----------------------------------------------------------------------
// <copyright file="FoodMenuManagementController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/18/2014 10:36:15 AM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Core.Models;
    using NexFB.Criteria.FoodMenuManagement;
    using NexFB.Models.FoodMenuManagement;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// Food menu management.
    /// </summary>
    public class FoodMenuManagementController : BaseController
    {
        private readonly IFoodMenuService _foodMenuService;

        public FoodMenuManagementController() 
            : this(null)
        {
        }

        public FoodMenuManagementController(IFoodMenuService foodMenuService)
        {
            _foodMenuService = foodMenuService ?? new FoodMenuService();
        }

        public IList<FoodMenuModel> GetAllFoodMenu(
            string mealId = "",
            string employeeTypeId = "")
        {
            var criteria = new GetAllFoodMenuCriteria
            {
                EmployeeTypeId = employeeTypeId,
                MealId = mealId
            };

            return _foodMenuService.GetAllFoodMenu(criteria);
        }

        public IEnumerable<FoodMenuModel> GetAllFoodMenuToFilter()
        {
            var criteria = new GetAllFoodMenuCriteria();
            return _foodMenuService.GetAllFoodMenuToFilter(criteria);
        }

        [HttpPost]
        public CommandResult InsertFoodMenu(InsertFoodMenuCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.CreatedBy = this.CurrentUser.UserId;
                criteria.CreatedDate = DateTime.Now;
            }

            return _foodMenuService.InsertFoodMenu(criteria);
        }
    }
}