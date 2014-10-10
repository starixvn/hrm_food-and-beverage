// ----------------------------------------------------------------------
// <copyright file="AlternativeMenuRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/10/2014 2:34:55 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.AlternativeMenuManagement;
    using NexFB.Models.Employee;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// The alternative menu repository.
    /// </summary>
    public class AlternativeMenuRepository : IAlternativeMenuRepository
    {
        /// <summary>
        /// Get all alternative menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Alternative menu model.</returns>
        public IEnumerable<AlternativeMenuModel> GetAllAlternativeMenu(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    IEnumerable<AlternativeMenuModel> alternativeMenus = multi.Read<AlternativeMenuModel>();
                    IEnumerable<dynamic> allEmployeeTypes = multi.Read<dynamic>();

                    foreach (AlternativeMenuModel alternativeMenu in alternativeMenus)
                    {
                        int menuId = alternativeMenu.AlternativeMenuId;
                        IEnumerable<dynamic> employeeeTypesOfMenu = allEmployeeTypes.Where(x => x.AltMenuID == menuId);
                        alternativeMenu.EmployeeTypes = GetEmployeeTypeOfAlternativeMenu(employeeeTypesOfMenu);
                    }

                    return alternativeMenus;
                }
            }
        }

        /// <summary>
        /// Get employee types of alternative menu.
        /// </summary>
        /// <param name="employeeeTypesOfMenu">All dynamic employee types of alternative menu.</param>
        /// <returns>The list Employee Type model.</returns>
        private IList<EmployeeTypeModel> GetEmployeeTypeOfAlternativeMenu(
            IEnumerable<dynamic> employeeeTypesOfMenu)
        {
            IList<EmployeeTypeModel> employeeTypes = new List<EmployeeTypeModel>();

            foreach (dynamic rawEmployeeType in employeeeTypesOfMenu)
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
