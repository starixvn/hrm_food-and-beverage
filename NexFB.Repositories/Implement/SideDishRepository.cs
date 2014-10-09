// ----------------------------------------------------------------------
// <copyright file="SideDishRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/9/2014 10:10:56 AM">Created.</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.Employee;
    using NexFB.Models.SideDishManagement;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// The side dish repository.
    /// </summary>
    public class SideDishRepository : ISideDishRepository
    {
        /// <summary>
        /// Get all side dishes.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Side Dish model.</returns>
        public IEnumerable<SideDishModel> GetAllSideDish(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    IEnumerable<SideDishModel> sideDishes = multi.Read<SideDishModel, UnitModel, SideDishModel>(
                        (si, unit) => 
                        {
                            si.Unit = unit;
                            return si;
                        },
                        "UnitId");

                    IEnumerable<dynamic> rawEmployeeTypes = multi.Read<dynamic>();

                    foreach (SideDishModel sideDish in sideDishes)
                    {
                        int foodId = sideDish.FoodId;
                        sideDish.EmployeeTypes = GetEmployeeTypesOfSideDish(foodId, rawEmployeeTypes);
                    }

                    return sideDishes;
                }
            }
        }

        /// <summary>
        /// Get employee types of side dish.
        /// </summary>
        /// <param name="foodId">The food Id.</param>
        /// <param name="rawEmployeeTypes">All employee types.</param>
        /// <returns>The employee types of side dish.</returns>
        private IList<EmployeeTypeModel> GetEmployeeTypesOfSideDish(
            int foodId,
            IEnumerable<dynamic> rawEmployeeTypes)
        {
            IList<EmployeeTypeModel> employeeTypes = new List<EmployeeTypeModel>();
            IEnumerable<dynamic> employeeTypesOfSideDish = rawEmployeeTypes.Where(x => x.FoodID == foodId);

            foreach (dynamic rawEmployeeType in employeeTypesOfSideDish)
            {
                var employeeType = new EmployeeTypeModel
                {
                    EmployeeTypeId = rawEmployeeType.EmployeeTypeID,
                    EmployeeTypeName = rawEmployeeType.EmployeeTypeName
                };

                employeeTypes.Add(employeeType);
            }

            return employeeTypes;
        }
    }
}
