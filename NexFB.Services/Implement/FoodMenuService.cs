// ----------------------------------------------------------------------
// <copyright file="FoodMenuService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 11:05:16 AM">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Services;
    using NexFB.Criteria.FoodMenuManagement;
    using NexFB.Models.FoodMenuManagement;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// The food menu service.
    /// </summary>
    public class FoodMenuService : BaseService, IFoodMenuService
    {
        private readonly IFoodMenuRepository _foodMenuRepository;

        public FoodMenuService()
            : this(null)
        {
        }

        public FoodMenuService(IFoodMenuRepository foodMenuRepository)
        {
            _foodMenuRepository = foodMenuRepository ?? new FoodMenuRepository();
        }

        /// <summary>
        /// Get all food menu to filter.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator food menu model.</returns>
        public IEnumerable<FoodMenuModel> GetAllFoodMenuToFilter(GetAllFoodMenuCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<FoodMenuModel> foodMenus = null;

            if (criteria.IsValid())
            {
                foodMenus = _foodMenuRepository.GetAllFoodMenuToFilter(criteria);
                foodMenus = CreateMenuNameMaker(foodMenus);
            }

            return foodMenus;
        }

        private IEnumerable<FoodMenuModel> CreateMenuNameMaker(IEnumerable<FoodMenuModel> foodMenus)
        {
            foreach (FoodMenuModel foodMenu in foodMenus)
            {
                foodMenu.MenuEngMaker = " (" + foodMenu.MenuEngName + ")";
            }

            return foodMenus;
        }
    }
}
