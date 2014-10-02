// ----------------------------------------------------------------------
// <copyright file="FoodMenuRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 11:21:31 AM">Created.</change>
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
    /// The food menu repository.
    /// </summary>
    public class FoodMenuRepository : IFoodMenuRepository
    {
        private readonly ICommonRepository _commonRepository;

        public FoodMenuRepository()
            : this(null)
        {
        }

        public FoodMenuRepository(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository ?? new CommonRepository();
        }

        /// <summary>
        /// Get all food menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list Food Menu model.</returns>
        public IList<FoodMenuModel> GetAllFoodMenu(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    IList<FoodMenuModel> foodMenus = multi.Read<FoodMenuModel>().ToList();
                    IList<dynamic> rawMeals = multi.Read<dynamic>().ToList();
                    IList<dynamic> rawEmployeeTypes = multi.Read<dynamic>().ToList();
                    IList<dynamic> rawFoodItems = multi.Read<dynamic>().ToList();

                    foreach (FoodMenuModel foodMenu in foodMenus)
                    {
                        int menuId = foodMenu.MenuId;

                        foodMenu.Meals = _commonRepository.GetMealsOfMenu(menuId, rawMeals);
                        foodMenu.EmployeeTypes = _commonRepository.GetEmployeeTypeOfMenu(menuId, rawEmployeeTypes);
                        foodMenu.FoodItems = GetFoodItemsOfMenu(menuId, rawFoodItems);
                    }

                    return foodMenus;
                }
            }
        }

        public IEnumerable<FoodMenuModel> GetAllFoodMenuToFilter(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    return multi.Read<FoodMenuModel>();
                }
            }
        }

        /// <summary>
        /// Get food items of menu.
        /// </summary>
        /// <param name="menuId">The menu ID.</param>
        /// <param name="rawFoodItems">The list food items from database.</param>
        /// <returns>The list Food Item belong to current menu.</returns>
        private IList<FoodItemModel> GetFoodItemsOfMenu(int menuId, IList<dynamic> rawFoodItems)
        {
            IList<FoodItemModel> foodItems = new List<FoodItemModel>();
            IEnumerable<dynamic> rawFoodItemsOfMenu = rawFoodItems.Where(x => x.MenuID == menuId);

            foreach (dynamic rawFoodItem in rawFoodItemsOfMenu)
            {
                var foodItem = new FoodItemModel
                {
                    FoodId = rawFoodItem.FoodID,
                    FoodEngName = rawFoodItem.FoodEngName,
                    FoodVnmName = rawFoodItem.FoodVNMName
                };

                foodItems.Add(foodItem);
            }

            return foodItems;
        }
    }
}
