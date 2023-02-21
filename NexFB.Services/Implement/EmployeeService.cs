namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Core.Security;
    using NexFB.Core.Services;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Services.Interface;

    public class EmployeeService : BaseService, IEmployeeService
    {
        public UserSession GetUserById(GetEmployeeByIdCriteria criteria)
        {
            UserSession user = null;
            if (criteria.IsValid())
            {
                user = GetDataEntity<UserSession>(criteria);
            }

            return user;
        }

        public UserSession GetUserByDomain(GetEmployeeByDomainCriteria criteria)
        {
            UserSession user = null;
            if (criteria.IsValid())
            {
                user = GetDataEntity<UserSession>(criteria);
            }

            return user;
        }

        public UserSession GetUserByCode(GetEmployeeByCodeCriteria criteria)
        {
            UserSession user = null;
            if (criteria.IsValid())
            {
                user = GetDataEntity<UserSession>(criteria);
            }

            return user;
        }

        public bool IsValidateUser(UserSession user, string password)
        {
            bool isValid = user != null
                && !string.IsNullOrWhiteSpace(user.Password)
                && Encryptor.CompareStringToHash(HashProvider.Sha1, password, user.Password);

            return isValid;
        }

        public CommandResult UpdatePassword(ChangePasswordCriteria criteria)
        {
            var result = new CommandResult();
            if (criteria.IsValid())
            {
                string hashPassword = Encryptor.ComputeHash(HashProvider.Sha1, criteria.Password);
                criteria.Password = hashPassword;
                result = Repository.Update(criteria);
            }

            return result;
        }

        public IList<MenuGroup> GetEmployeeMenuById(GetEmployeeMenuBySystemIdCriteria criteria)
        {
            IList<MenuGroup> menuGroups = null;
            if (criteria.IsValid())
            {
                IEnumerable<MenuItem> menuItems = GetData<MenuItem>(criteria);
                menuGroups = GroupMenuItem(menuItems);
            }

            return menuGroups;
        }

        public IList<UserFunction> GetAllEmployeeFunction(GetAllEmployeeFunctionCriteria criteria)
        {
            IList<UserFunction> functions = null;
            if (criteria.IsValid())
            {
                functions = GetData<UserFunction>(criteria).ToList();
            }

            return functions;
        }

        public IList<EmployeeShift> GetEmployeeShiftInMonth(GetEmployeeShiftInMonthCriteria criteria)
        {
            IList<EmployeeShift> result = null;
            if (criteria.IsValid())
            {
                result = GetData<EmployeeShift>(criteria).ToList();
            }

            return result;
        }

        public IList<Employee> GetEmployeeByQuery(GetEmployeeCriteria criteria)
        {
            IList<Employee> employees = null;
            if (criteria.IsValid())
            {
                employees = GetData<Employee>(criteria).ToList();
            }

            return employees;
        }

        public CommandResult InsertSpecialUser(InsertSpecialUserCriteria criteria)
        {
            var result = new CommandResult();
            if (criteria.IsValid())
            {
                result = Repository.Insert(criteria, hasGetOutputParams: true);
            }

            return result;
        }

        public CommandResult UpdateSpecialUser(UpdateSpecialUserCriteria criteria)
        {
            var result = new CommandResult();
            if (criteria.IsValid())
            {
                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateStatusSpecialUser(ActivateSpecialUserCriteria criteria)
        {
            var result = new CommandResult();
            if (criteria.IsValid())
            {
                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult DeleteSpecialUser(DeleteSpecialUserCriteria criteria)
        {
            var result = new CommandResult();
            if (criteria.IsValid())
            {
                result = Repository.Delete(criteria);
            }

            return result;
        }

        public IList<SpecialUser> GetSpecialUsers(GetSpecialUserCriteria criteria)
        {
            IList<SpecialUser> employees = null;
            if (criteria.IsValid())
            {
                employees = GetData<SpecialUser>(criteria).ToList();
            }

            return employees;
        }

        public SpecialUser GetSpecialUserById(GetSpecialUserByIdCriteria criteria)
        {
            SpecialUser employee = null;

            if (criteria.IsValid())
            {
                employee = GetDataEntity<SpecialUser>(criteria);
            }

            return employee;
        }

        /// <summary>
        /// Function to get special employee by id.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Employee.</returns>
        public Employee GetEmployeeById(GetEmployeeByIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            Employee employee = null;

            if (criteria.IsValid())
            {
                employee = GetDataEntity<Employee>(criteria);
            }

            return employee;
        }

        /// <summary>
        /// Group menu item.
        /// </summary>
        /// <param name="menuItems">The enumerator menu item.</param>
        /// <returns>The list group menu.</returns>
        private IList<MenuGroup> GroupMenuItem(IEnumerable<MenuItem> menuItems)
        {
            var menuGroups = new List<MenuGroup>();
            IEnumerable<MenuItem> parentMenu = menuItems.Where(item => item.ParentId == null).ToList();

            foreach (MenuItem parent in parentMenu)
            {
                var menuGroupItem = new MenuGroup
                {
                    MenuId = parent.MenuId,
                    Name = parent.Name,
                    MenuLink = parent.MenuLink
                };

                menuGroupItem.MenuItems = menuItems.Where(menuItem => menuItem.ParentId == menuGroupItem.MenuId);
                menuGroups.Add(menuGroupItem);
            }

            return menuGroups;
        }
    }
}
