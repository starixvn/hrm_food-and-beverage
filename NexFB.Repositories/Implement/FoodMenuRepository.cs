// ----------------------------------------------------------------------
// <copyright file="FoodMenuRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 11:21:31 AM">Created.</change>
//   <change who="Quinn.Vo" date="2014.10.23">Change Food items to Set menu.</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.FoodMenuManagement;
    using NexFB.Models.SetMenuManagement;
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
                        foodMenu.SetMenus = GetSetMenuOfMenu(menuId, rawFoodItems);
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

        public IEnumerable<FoodMenuHistoryModel> GetFoodMenuHistory(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    return multi.Read<FoodMenuHistoryModel>();
                }
            }
        }

        /// <summary>
        /// Get set menu of menu.
        /// </summary>
        /// <param name="menuId">The menu ID.</param>
        /// <param name="rawSetMenus">The list set menu from database.</param>
        /// <returns>The list Set Menu belong to current menu.</returns>
        private IList<SetMenuModel> GetSetMenuOfMenu(int menuId, IList<dynamic> rawSetMenus)
        {
            IList<SetMenuModel> setMenus = new List<SetMenuModel>();
            IEnumerable<dynamic> rawSetMenuOfMenu = rawSetMenus.Where(x => x.MenuID == menuId);

            foreach (dynamic rawSetMenu in rawSetMenuOfMenu)
            {
                var setMenu = new SetMenuModel
                {
                    SetMenuId = rawSetMenu.FoodSetID,
                    SetMenuEngName = rawSetMenu.FoodSetENGName,
                    SetMenuVnmName = rawSetMenu.FoodSetVNMName
                };

                setMenus.Add(setMenu);
            }

            return setMenus;
        }
    }
}