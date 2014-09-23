// ----------------------------------------------------------------------
// <copyright file="FoodItemManagementController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/16/2014 2:09:44 PM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System.Collections.Generic;
    using NexFB.Criteria.FoodItemManagement;
    using NexFB.Models.FoodItemManagement;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// Food item management controller.
    /// </summary>
    public class FoodItemManagementController : BaseController
    {
        private readonly IFoodItemService _foodItemService;

        public FoodItemManagementController()
            : this(null)
        {
        }

        public FoodItemManagementController(IFoodItemService foodItemService)
        {
            _foodItemService = foodItemService ?? new FoodItemService();
        }

        public IList<FoodItemModel> GetAllFoodItem(string menuGroupId = "", bool? isActive = null)
        {
            var criteria = new GetAllFoodItemCriteria 
            {
                MenuGroupId = menuGroupId,
                IsActive = isActive
            };

            return _foodItemService.GetAllFoodItem(criteria);
        }
    }
}