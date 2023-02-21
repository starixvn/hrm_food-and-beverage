namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Core.Enums;
    using NexFB.Models;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Models.UserControl;
    using NexFB.Repositories.Interface;

    public class UserControlRepository : IUserControlRepository
    {
        public IEnumerable<EmployeeModel> GetAllEmployeeRole(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    IEnumerable<EmployeeModel> employees = GetEmployee(multi);
                    IEnumerable<EmployeeRoleModel> roles = GetEmployeeRole(multi);

                    var roleCompanies = multi.Read<RoleCompany>();

                    if (employees != null && roles != null)
                    {
                        if (roleCompanies != null)
                        {
                            roles = GetFinanceRole(roles, roleCompanies);
                        }

                        employees = GetRolesOfEmployees(employees, roles);
                    }

                    return employees;
                }
            }
        }

        public EmployeeModel GetEmployeeRoleById(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    IEnumerable<EmployeeModel> employees = GetEmployee(multi);
                    IEnumerable<EmployeeRoleModel> roles = GetEmployeeRole(multi);

                    var roleCompanies = multi.Read<RoleCompany>();

                    if (employees != null && roles != null)
                    {
                        if (roleCompanies != null)
                        {
                            roles = GetFinanceRole(roles, roleCompanies);
                        }

                        employees = GetRolesOfEmployees(employees, roles);
                    }

                    return employees.FirstOrDefault();
                }
            }
        }

        public IEnumerable<Company> GetDepartmentUserHasPermission(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    multi.Read();
                    multi.Read();
                    var rolesDepartment = multi.Read<Company>();

                    return rolesDepartment;
                }
            }
        }

        private static IEnumerable<EmployeeModel> GetEmployee(GridReader multi)
        {
            return multi.Read<EmployeeModel, Team, EmployeeModel>(
                        (emp, team) =>
                        {
                            emp.Team = team;
                            return emp;
                        },
                        "TeamId");
        }

        private static IEnumerable<EmployeeRoleModel> GetEmployeeRole(GridReader multi)
        {
            return multi.Read<EmployeeRoleModel, Role, EmployeeRoleModel>(
                        (empRo, ro) =>
                        {
                            empRo.Role = ro;
                            return empRo;
                        },
                        "RoleId");
        }

        private static IEnumerable<EmployeeModel> GetRolesOfEmployees(
            IEnumerable<EmployeeModel> employees,
            IEnumerable<EmployeeRoleModel> roles)
        {
            foreach (EmployeeModel employee in employees)
            {
                IEnumerable<EmployeeRoleModel> rolesOfEmployee = roles.Where(item => item.UserId == employee.UserId).ToList();

                foreach (EmployeeRoleModel role in rolesOfEmployee)
                {
                    employee.RoleItems.Add(role.Role);

                    string roleName = GetRoleWithCompany(role.Role);
                    employee.Roles = string.IsNullOrWhiteSpace(employee.Roles) ? roleName : employee.Roles + ", " + roleName;
                }
            }

            return employees;
        }

        private static string GetRoleWithCompany(Role role)
        {
            var roles = string.Empty;

            if (role != null)
            {
                roles = role.Name;

                if (role.Companies != null && role.Companies.Any())
                {
                    roles += " (";
                    var companyName = string.Empty;

                    foreach (Company company in role.Companies)
                    {
                        companyName = string.IsNullOrEmpty(companyName) ? company.Name : string.Format("{0}, {1}", companyName, company.Name);
                    }

                    roles += companyName + ")";
                }
            }

            return roles;
        }

        private static IEnumerable<EmployeeRoleModel> GetFinanceRole(
            IEnumerable<EmployeeRoleModel> roles,
            IEnumerable<RoleCompany> roleCompanies)
        {
            foreach (EmployeeRoleModel role in roles)
            {
                if (role.Role != null && role.Role.RoleId == (byte)RolesSystem.Finance)
                {
                    role.Role.Companies = roleCompanies.Where(item => item.UserId == role.UserId)
                        .Select(item => new Company
                        {
                            CompanyId = item.CompanyId,
                            Name = item.CompanyName
                        }).ToList();
                }
            }

            return roles;
        }
    }
}