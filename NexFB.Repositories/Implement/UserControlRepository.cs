// ----------------------------------------------------------------------
// <copyright file="UserControlRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/28/2014 2:28:17 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Core.Enums;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Models.UserControl;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// The User Control repository.
    /// </summary>
    public class UserControlRepository : IUserControlRepository
    {
        /// <summary>
        /// Get all employee role.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Employee Model.</returns>
        public IEnumerable<EmployeeModel> GetAllEmployeeRole(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    IEnumerable<EmployeeModel> employees = multi.Read<EmployeeModel, Department, Team, EmployeeModel>(
                        (emp, dep, te) =>
                        {
                            emp.Department = dep;
                            emp.Team = te;
                            return emp;
                        },
                        "DepartmentID, TeamID");

                    IEnumerable<EmployeeRoleModel> roles = multi.Read<EmployeeRoleModel, Role, EmployeeRoleModel>(
                        (empRo, ro) =>
                        {
                            empRo.Role = ro;
                            return empRo;
                        },
                        "RoleId");

                    IEnumerable<RoleDepartmentModel> rolesDepartment = multi.Read<RoleDepartmentModel, Department, RoleDepartmentModel>(
                        (roDep, dep) =>
                        {
                            roDep.Department = dep;
                            return roDep;
                        },
                        "DepartmentId");

                    if (employees != null && roles != null)
                    {
                        if (rolesDepartment != null)
                        {
                            roles = GetAdminDepartment(roles, rolesDepartment);
                        }

                        employees = GetRolesOfEmployees(employees, roles);
                    }

                    return employees;
                }
            }
        }

        /// <summary>
        /// Get employee role by ID.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The employee model.</returns>
        public EmployeeModel GetEmployeeRoleById(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    IEnumerable<EmployeeModel> employees = multi.Read<EmployeeModel, Department, Team, EmployeeModel>(
                        (emp, dep, te) =>
                        {
                            emp.Department = dep;
                            emp.Team = te;
                            return emp;
                        },
                        "DepartmentID, TeamID");

                    IEnumerable<EmployeeRoleModel> roles = multi.Read<EmployeeRoleModel, Role, EmployeeRoleModel>(
                        (empRo, ro) =>
                        {
                            empRo.Role = ro;
                            return empRo;
                        },
                        "RoleId");

                    IEnumerable<RoleDepartmentModel> rolesDepartment = multi.Read<RoleDepartmentModel, Department, RoleDepartmentModel>(
                        (roDep, dep) =>
                        {
                            roDep.Department = dep;
                            return roDep;
                        },
                        "DepartmentId");

                    if (employees != null && roles != null)
                    {
                        if (rolesDepartment != null)
                        {
                            roles = GetAdminDepartment(roles, rolesDepartment);
                        }

                        employees = GetRolesOfEmployees(employees, roles);
                    }

                    return employees.FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Get departments that user has Admin permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Departments.</returns>
        public IEnumerable<Department> GetDepartmentUserHasPermission(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    multi.Read();
                    multi.Read();
                    IEnumerable<Department> rolesDepartment = multi.Read<Department>();

                    return rolesDepartment;
                }
            }
        }

        /// <summary>
        /// Get roles of employees.
        /// </summary>
        /// <param name="employees">The enumerator employees.</param>
        /// <param name="roles">The enumerator roles.</param>
        /// <returns>The enumerator employee with roles.</returns>
        private IEnumerable<EmployeeModel> GetRolesOfEmployees(
            IEnumerable<EmployeeModel> employees,
            IEnumerable<EmployeeRoleModel> roles)
        {
            foreach (EmployeeModel employee in employees)
            {
                IEnumerable<EmployeeRoleModel> rolesOfEmployee = roles.Where(item => item.UserId == employee.UserId);

                foreach (EmployeeRoleModel role in rolesOfEmployee) 
                {
                    employee.RoleItems.Add(role.Role);

                    string roleNameWithDepartment = GetRolesWithDepartment(role.Role);
                    employee.Roles = string.IsNullOrEmpty(employee.Roles)
                                        ? roleNameWithDepartment
                                        : employee.Roles + ", " + roleNameWithDepartment;
                }
            }

            return employees;
        }

        /// <summary>
        /// Get role name with department name.
        /// </summary>
        /// <param name="role">The role of employee.</param>
        /// <returns>The string contain role name and department name.</returns>
        private string GetRolesWithDepartment(Role role)
        {
            string roles = string.Empty;

            if (role != null)
            {
                roles = role.Name;

                if (role.Departments != null && role.Departments.Count > 0)
                {
                    roles += " (";
                    string departmentName = string.Empty;

                    foreach (Department department in role.Departments)
                    {
                        departmentName = string.IsNullOrEmpty(departmentName)
                                            ? department.Name
                                            : departmentName + ", " + department.Name;
                    }

                    roles += departmentName + ")";
                }
            }

            return roles;
        }

        /// <summary>
        /// Get department of Admin role.
        /// </summary>
        /// <param name="roles">The enumerator roles.</param>
        /// <param name="rolesDepartment">The enumerator role and department of role.</param>
        /// <returns>The enumerator roles with department.</returns>
        private IEnumerable<EmployeeRoleModel> GetAdminDepartment(
            IEnumerable<EmployeeRoleModel> roles,
            IEnumerable<RoleDepartmentModel> rolesDepartment)
        {
            foreach (EmployeeRoleModel role in roles)
            {
                if (role.Role != null && role.Role.RoleId == (int)RolesSystem.Finance)
                {
                    IEnumerable<RoleDepartmentModel> departments = rolesDepartment.Where(item => item.UserId == role.UserId);

                    foreach (RoleDepartmentModel department in departments)
                    {
                        role.Role.Departments.Add(department.Department);
                    }
                }
            }

            return roles;
        }
    }
}
