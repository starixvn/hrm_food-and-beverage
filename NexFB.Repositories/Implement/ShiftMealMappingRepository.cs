// ----------------------------------------------------------------------
// <copyright file="ShiftMealMappingRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/14/2014 12:37:53 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.Entities;
    using NexFB.Models.ShiftMealMapping;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// Shift-Meal mapping repository.
    /// </summary>
    public class ShiftMealMappingRepository : IShiftMealMappingRepository
    {
        /// <summary>
        /// Get all shift meal to mapping.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Shift Meal Mapping model.</returns>
        public IEnumerable<ShiftMealMappingModel> GetAllShiftMeal(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<ShiftMealMappingModel, Department, ShiftMealMappingModel>(
                    (shMe, de) =>
                    {
                        shMe.Department = de;
                        return shMe;
                    },
                    "DepartmentId",
                    criteria.GetSpParams());
            }
        }
    }
}
