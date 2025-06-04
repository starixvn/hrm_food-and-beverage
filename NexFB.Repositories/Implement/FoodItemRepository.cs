namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.FoodItemManagement;
    using NexFB.Models.SetMenuManagement;
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
                    IEnumerable<RawFoodSetMenuModel> foodMenus = multi.Read<RawFoodSetMenuModel>();

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
            IEnumerable<RawFoodSetMenuModel> foodMenus)
        {
            foreach (FoodItemModel foodItem in foodItems)
            {
                IEnumerable<RawFoodSetMenuModel> setMenuOfItem = foodMenus.Where(x => x.FoodId == foodItem.FoodId);

                foodItem.SetMenus = setMenuOfItem.Select(x => new SetMenuModel
                {
                    SetMenuId = x.SetMenuId,
                    SetMenuVnmName = x.SetMenuVnmName,
                    SetMenuEngName = x.SetMenuEngName
                }).ToList();
            }

            return foodItems;
        }
    }
}
