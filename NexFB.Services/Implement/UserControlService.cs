namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.UserControl;
    using NexFB.Models;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Models.UserControl;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    public class UserControlService : BaseService, IUserControlService
    {
        private readonly IUserControlRepository _userControlRepository;

        public UserControlService()
            : this(null)
        {
        }

        public UserControlService(IUserControlRepository userControlRepository)
        {
            _userControlRepository = userControlRepository ?? new UserControlRepository();
        }

        public IEnumerable<FunctionModel> GetFunctionByRole(GetFunctionByRoleCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<FunctionModel> functionGroup = null;

            if (criteria.IsValid())
            {
                IEnumerable<FunctionModel> rawFunctions = GetData<FunctionModel>(criteria);
                functionGroup = rawFunctions.Where<FunctionModel>(func => func.ParentFunctionId == null);

                foreach (FunctionModel rawParentFunctionItem in functionGroup)
                {
                    rawParentFunctionItem.ChildFunctions = GetFunctionByRoleRecursion(rawFunctions, rawParentFunctionItem);
                }
            }

            return functionGroup;
        }

        public CommandResult AssignPermissionToRole(AssignPermissionToRoleCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Insert(criteria);
            }

            return commandResult;
        }

        public IEnumerable<EmployeeModel> GetAllEmployeeRole(GetAllEmployeeRoleCriteria criteria)
        {
            IEnumerable<EmployeeModel> employees = null;
            if (criteria.IsValid())
            {
                employees = _userControlRepository.GetAllEmployeeRole(criteria);
            }

            return employees;
        }

        public EmployeeModel GetEmployeeRoleById(GetEmployeeRoleByIdCriteria criteria)
        {
            EmployeeModel employee = null;
            if (criteria.IsValid())
            {
                employee = _userControlRepository.GetEmployeeRoleById(criteria);
            }

            return employee;
        }

        public CommandResult AssignRoleToUser(AssignRoleToUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        public IEnumerable<Company> GetDepartmentUserHasPermission(GetEmployeeRoleByIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<Company> departments = null;

            if (criteria.IsValid())
            {
                departments = _userControlRepository.GetDepartmentUserHasPermission(criteria);
            }

            return departments;
        }

        private List<FunctionModel> GetFunctionByRoleRecursion(
            IEnumerable<FunctionModel> rawFunctions,
            FunctionModel rawParentFunctionItem)
        {
            List<FunctionModel> rawChildFunctions = rawFunctions.Where(func => func.ParentFunctionId == rawParentFunctionItem.FunctionId).ToList();

            if (rawChildFunctions != null && rawChildFunctions.Any())
            {
                foreach (var rawChildFunctionItem in rawChildFunctions)
                {
                    rawChildFunctionItem.ChildFunctions = GetFunctionByRoleRecursion(rawFunctions, rawChildFunctionItem);
                }
            }

            return rawChildFunctions;
        }
    }
}
