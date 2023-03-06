namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using NexFB.Core.Enums;
    using NexFB.Core.Services;
    using NexFB.Criteria.Company;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Criteria.Team;
    using NexFB.Criteria.UserControl;
    using NexFB.Models;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Models.UserControl;
    using NexFB.Services.Interface;

    public class CommonService : BaseService, ICommonService
    {
        public IList<Company> GetAllCompany()
        {
            var criteria = new GetAllCompanyCriteria();
            return GetData<Company>(criteria).ToList();
        }

        public IEnumerable<RoleSystemModel> GetAllSystemRole(GetAllSystemRoleCriteria criteria, bool exceptSystemAdmin = false)
        {
            List<RoleSystemModel> roles = null;
            if (criteria.IsValid())
            {
                roles = GetData<RoleSystemModel>(criteria).ToList();

                if (exceptSystemAdmin)
                {
                    RoleSystemModel systemAdminRole = roles.First(x => x.RoleId == (int)RolesSystem.SA);
                    roles.Remove(systemAdminRole);
                }
            }

            return roles;
        }

        public IEnumerable<Unit> GetAllTeamByCompany(GetAllTeamByCompanyCriteria criteria)
        {
            List<Unit> units = new List<Unit>();
            IEnumerable<dynamic> rawReport = null;
            if (criteria.IsValid())
            {
                rawReport = GetData<dynamic>(criteria);
            }

            IEnumerable<dynamic> rawUnits = rawReport.Select(x => x.UnitId).Distinct();

            foreach (dynamic unitId in rawUnits)
            {
                units.Add(GroupUnits(units, unitId, rawReport));
            }

            return units;
        }

        public Unit GroupUnits(List<Unit> units, int unitId, IEnumerable<dynamic> rawReport)
        {
            Unit unit = new Unit();

            foreach (dynamic item in rawReport)
            {
                if (item.UnitId == unitId && !IsUnit(units, unitId))
                {
                    unit.UnitId = item.UnitId;
                    unit.Name = item.UnitName;
                }
            }

            unit.Departments = GroupDepartment(unit, rawReport);

            return unit;
        }

        public List<Department> GroupDepartment(Unit unit, IEnumerable<dynamic> rawReport)
        {
            List<Department> departments = new List<Department>();

            foreach (dynamic item in rawReport)
            {
                if (item.UnitId == unit.UnitId 
                    && !IsDepartment(departments, item.DepartmentId))
                {
                    departments.Add(new Department
                    {
                        DepartmentId = item.DepartmentId,
                        Name = item.DepartmentName
                    });
                }
            }

            foreach(var item in departments)
            {
                item.Sections = GroupSections(unit, item, rawReport);
            }
            return departments;
        }

        public List<Section> GroupSections(Unit unit, Department department, IEnumerable<dynamic> rawReport)
        {
            List<Section> sections = new List<Section>();

            foreach (dynamic item in rawReport)
            {
                if (item.UnitId == unit.UnitId 
                    && item.DepartmentId == department.DepartmentId 
                    && !IsSection(sections, item.SectionId))
                {
                    sections.Add(new Section
                    {
                        SectionId = item.SectionId,
                        Name = item.SectionName
                    });
                }
            }

            foreach (var item in sections)
            {
                item.Teams = GroupTeams(unit, department, item, rawReport);
            }

            return sections;
        }

        public List<Team> GroupTeams(Unit unit, Department department, Section section, IEnumerable<dynamic> rawReport)
        {
            List<Team> teams = new List<Team>();

            foreach (dynamic item in rawReport)
            {
                if (item.UnitId == unit.UnitId
                    && item.DepartmentId == department.DepartmentId
                    && item.SectionId == section.SectionId
                    && !IsTeam(teams, item.EmpTeamId))
                {
                    teams.Add(new Team
                    {
                        TeamId = item.EmpTeamId,
                        Name = item.EmpTeamName,
                        Code = item.EmpTeamCode
                    });
                }
            }
            return teams;
        }

        public bool IsUnit(List<Unit> units, int unitId)
        {
            foreach(var item in units)
            {
                if(item.UnitId == unitId)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsDepartment(List<Department> departments, int departmentId)
        {
            foreach (var item in departments)
            {
                if (item.DepartmentId == departmentId)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsSection(List<Section> sections, int sectionId)
        {
            foreach (var item in sections)
            {
                if (item.SectionId == sectionId)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsTeam(List<Team> teams, int teamId)
        {
            foreach (var item in teams)
            {
                if (item.TeamId == teamId)
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<MealTimesheet> GetAllMealTimesheet(GetMealTimesheetCriteria criteria)
        {
            IEnumerable<MealTimesheet> mealTimesheet = null;
            if (criteria.IsValid())
            {
                mealTimesheet = GetData<MealTimesheet>(criteria);
            }

            return mealTimesheet;
        }

        public IEnumerable<EmployeeTypeModel> GetAllEmployeeType(GetAllEmployeeTypeCriteria criteria)
        {
            IEnumerable<EmployeeTypeModel> employeeTypes = null;
            if (criteria.IsValid())
            {
                employeeTypes = GetData<EmployeeTypeModel>(criteria);
            }

            return employeeTypes;
        }

        public IList<MultiSelectItem> GetOrgChart(GetAllTeamByCompanyCriteria criteria)
        {
            var units = GetAllTeamByCompany(criteria);
            IList<MultiSelectItem> items = new List<MultiSelectItem>();
            if (units.Any())
            {
                items = units.Select(unit => new MultiSelectItem
                {
                    Id = unit.UnitId,
                    Text = unit.Name,
                    Items = unit.Departments.Select(dept => new MultiSelectItem {
                        Id = dept.DepartmentId,
                        Text = dept.Name,
                        ParentId = unit.UnitId,
                        Items = dept.Sections.Select(sect => new MultiSelectItem { 
                            Id = sect.SectionId,
                            Text = sect.Name,
                            ParentId = dept.DepartmentId,
                            Items = sect.Teams.Select(team => new MultiSelectItem{
                                Id = team.TeamId,
                                Text = team.Name,
                                IsLeaf = true,
                                ParentId = sect.SectionId
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList();
            }
            return items;
        }
    }
}