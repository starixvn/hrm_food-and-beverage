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
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.FoodMenuManagement;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// The food menu repository.
    /// </summary>
    public class FoodMenuRepository : IFoodMenuRepository
    {
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
    }
}
