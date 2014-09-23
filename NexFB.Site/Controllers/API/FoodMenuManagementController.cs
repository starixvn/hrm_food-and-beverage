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
    using System.Collections.Generic;
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

        public IEnumerable<FoodMenuModel> GetAllFoodMenuToFilter()
        {
            var criteria = new GetAllFoodMenuCriteria();
            return _foodMenuService.GetAllFoodMenuToFilter(criteria);
        }
    }
}