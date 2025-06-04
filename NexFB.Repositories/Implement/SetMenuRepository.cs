namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.SetMenuManagement;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// Order Repository.
    /// </summary>
    public class SetMenuRepository : ISetMenuRepository
    {
        /// <summary>
        /// Function to get set menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Set Menu Models.</returns>
        public IList<SetMenuModel> GetSetMenu(ICriteria criteria)
        {
            IList<SetMenuModel> setMenus = null;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var dataSetMenus = multi.Read<SetMenuModel>();
                    if (dataSetMenus != null)
                    {
                        setMenus = new List<SetMenuModel>();
                        var foods = multi.Read<FoodSetMenuModel>();
                        var menus = multi.Read<MenuSetMenuModel>();

                        foreach (SetMenuModel setMenu in dataSetMenus)
                        {
                            setMenu.FoodItems = GetFoodBySetMenuId(setMenu.SetMenuId, foods);
                            setMenu.Menus = GetMenuBySetMenuId(setMenu.SetMenuId, menus);

                            setMenus.Add(setMenu);
                        }
                    }
                }
            }

            return setMenus;
        }

        /// <summary>
        /// Function to get foods of a set menu.
        /// </summary>
        /// <param name="setMenuId">The set menu id.</param>
        /// <param name="foods">The foods list.</param>
        /// <returns>Foods list.</returns>
        private IList<FoodSetMenuModel> GetFoodBySetMenuId(int setMenuId, IEnumerable<FoodSetMenuModel> foods)
        {
            var foodItems = new List<FoodSetMenuModel>();
            if (foods != null)
            {
                foreach (FoodSetMenuModel food in foods)
                {
                    if (food.SetMenuId == setMenuId)
                    {
                        foodItems.Add(food);
                    }
                }
            }

            return foodItems;
        }

        /// <summary>
        /// Function to get menus of a set menu.
        /// </summary>
        /// <param name="setMenuId">The set menu id.</param>
        /// <param name="menus">The menus list.</param>
        /// <returns>Menus list.</returns>
        private IList<MenuSetMenuModel> GetMenuBySetMenuId(int setMenuId, IEnumerable<MenuSetMenuModel> menus)
        {
            var menuItems = new List<MenuSetMenuModel>();
            if (menus != null)
            {
                foreach (MenuSetMenuModel menu in menus)
                {
                    if (menu.SetMenuId == setMenuId)
                    {
                        menuItems.Add(menu);
                    }
                }
            }

            return menuItems;
        }
    }
}
