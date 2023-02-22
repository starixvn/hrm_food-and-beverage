namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Web.Http;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Resources;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    public class ChangePasswordController : BaseController
    {
        private readonly IEmployeeService _employeeService;

        public ChangePasswordController()
            : this(null)
        {
        }

        public ChangePasswordController(IEmployeeService employeeService)
        {
            _employeeService = employeeService ?? new EmployeeService();
        }

        [HttpPut]
        public CommandResult UpdatePassword(ChangePasswordModel model)
        {
            var result = new CommandResult();
            if (this.CurrentUser.CompanyId == (byte)CompanyIdentity.Nexcel)
            {
                if (!string.IsNullOrWhiteSpace(model.OldPassword)
                && !string.IsNullOrWhiteSpace(model.NewPassword)
                && !string.IsNullOrWhiteSpace(model.ConfirmPassword)
                && string.Compare(model.NewPassword, model.ConfirmPassword, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    var getUserCriteria = new GetEmployeeByIdCriteria
                    {
                        UserId = this.CurrentUser.UserId
                    };

                    UserSession user = _employeeService.GetUserById(getUserCriteria);
                    bool isValid = _employeeService.IsValidateUser(user, model.OldPassword);
                    if (isValid)
                    {
                        var criteria = new ChangePasswordCriteria
                        {
                            UserId = this.CurrentUser.UserId,
                            Password = model.NewPassword,
                            ModifiedBy = this.CurrentUser.UserId
                        };

                        result = _employeeService.UpdatePassword(criteria);
                    }
                    else
                    {
                        result.Message = Lang.ValidateWrongPassword;
                    }
                }
            }
            else
            {
                result.Message = Lang.NoPermissionChangePassword;
            }

            return result;
        }
    }
}