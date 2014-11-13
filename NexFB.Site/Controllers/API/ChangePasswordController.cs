// --------------------------------------------------------------------------
// <copyright file="ChangePasswordController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.11.12">Created</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Models;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Resources;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// ChangePassword Controller.
    /// </summary>
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

        /// <summary>
        /// Function change password.
        /// </summary>
        /// <param name="model">ChangePassword Model.</param>
        /// <returns>Command Result.</returns>
        [HttpPut]
        public CommandResult UpdatePassword(ChangePasswordModel model)
        {
            var result = new CommandResult()
            {
                Code = 1,
                Message = Lang.InvalidInputData
            };

            if (this.CurrentUser.DepartmentId == Constants.NexcelDepartment)
            {
                if (!string.IsNullOrWhiteSpace(model.OldPassword)
                && !string.IsNullOrWhiteSpace(model.NewPassword)
                && !string.IsNullOrWhiteSpace(model.ConfirmPassword)
                && string.Compare(model.NewPassword, model.ConfirmPassword, System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    var getUserCriteria = new GetEmployeeByIdCriteria()
                    {
                        UserId = this.CurrentUser.UserId
                    };

                    UserSession user = _employeeService.GetUserById(getUserCriteria);
                    int checkCode = _employeeService.ValidateUser(user, model.OldPassword);
                    if (checkCode == 0)
                    {
                        var criteria = new ChangePasswordCriteria()
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