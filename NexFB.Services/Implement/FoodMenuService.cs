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
    using NexFB.Core.Models;
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
        /// Get all food menus.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list Food Menu model.</returns>
        public IList<FoodMenuModel> GetAllFoodMenu(GetAllFoodMenuCriteria criteria) 
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IList<FoodMenuModel> foodMenus = null;

            if (criteria.IsValid())
            {
                foodMenus = _foodMenuRepository.GetAllFoodMenu(criteria);
            }

            return foodMenus;
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

        /// <summary>
        /// Insert food menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Command Result.</returns>
        public CommandResult InsertFoodMenu(InsertFoodMenuCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Insert(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Delete food menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult DeleteFoodMenu(DeleteFoodMenuByMenuIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Delete(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Update food menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateFoodMenu(UpdateFoodMenuCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Activate or deactivate food menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult ActivateFoodMenu(ActivateFoodMenuCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
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
