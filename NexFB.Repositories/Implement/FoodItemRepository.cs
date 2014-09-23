// ----------------------------------------------------------------------
// <copyright file="FoodItemRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 9:28:44 AM">Created.</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.FoodItemManagement;
    using NexFB.Models.FoodMenuManagement;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// The food item management repository.
    /// </summary>
    public class FoodItemRepository : IFoodItemRepository
    {
        /// <summary>
        /// Get all food item.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list Food Item Model.</returns>
        public IList<FoodItemModel> GetAllFoodItem(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    IList<FoodItemModel> foodItems = multi.Read<FoodItemModel>().ToList();
                    IEnumerable<RawFoodMenuModel> foodMenus = multi.Read<RawFoodMenuModel>();

                    foodItems = GetFoodMenuOfItem(foodItems, foodMenus);
                    return foodItems;
                }
            }
        }

        /// <summary>
        /// Get food menu of food item.
        /// </summary>
        /// <param name="foodItems">The list food items.</param>
        /// <param name="foodMenus">The list food menus.</param>
        /// <returns>The list food items that has food menus.</returns>
        private IList<FoodItemModel> GetFoodMenuOfItem(
            IList<FoodItemModel> foodItems, 
            IEnumerable<RawFoodMenuModel> foodMenus)
        {
            foreach (FoodItemModel foodItem in foodItems)
            {
                IEnumerable<RawFoodMenuModel> menuOfItem = foodMenus.Where(x => x.FoodId == foodItem.FoodId);

                foreach (RawFoodMenuModel rawMenu in menuOfItem)
                {
                    var menu = new FoodMenuModel
                    {
                        MenuId = rawMenu.MenuId,
                        MenuEngName = rawMenu.MenuEngName,
                        MenuVnmName = rawMenu.MenuVnmName
                    };

                    foodItem.FoodMenus.Add(menu);
                }
            }

            return foodItems;
        }
    }
}
