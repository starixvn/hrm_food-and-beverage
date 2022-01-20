namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.AlternativeMenuManagement;
    using NexFB.Models.Employee;
    using NexFB.Repositories.Interface;

    public class AlternativeMenuRepository : IAlternativeMenuRepository
    {
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

        private IList<EmployeeTypeModel> GetEmployeeTypeOfAlternativeMenu(
            IEnumerable<dynamic> employeeeTypesOfMenu)
        {
            IList<EmployeeTypeModel> employeeTypes = employeeeTypesOfMenu.Select(x => new EmployeeTypeModel
            {
                EmployeeTypeId = x.EmployeeTypeID,
                EmployeeTypeName = x.EmployeeTypeName
            }).ToList();

            return employeeTypes;
        }
    }
}