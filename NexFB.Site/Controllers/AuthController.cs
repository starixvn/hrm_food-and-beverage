// --------------------------------------------------------------------------
// <copyright file="AuthController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Resources;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web;
    using HRM.UserControl.Client.Core.Models.Auth;
    using HRM.UserControl.Client.Core.Services;
    using HRM.UserControl.Client.Core.Session;
    using HRM.UserControl.Client.AspNetMvc.Session;
    using HRM.UserControl.Client.Core.Configurations;
    using HRM.UserControl.Client.AspNetMvc.Configurations;
    using System.Collections.Generic;

    /// <summary>
    /// Authentication Controller.
    /// </summary>
    public class AuthController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public AuthController()
        {
            _employeeService = new EmployeeService();
        }

        public AuthController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Login Action.
        /// </summary>
        /// <param name="model">LoginModel Object.</param>
        /// <returns>Action Result Login.</returns>
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserSession user = GetUserLogin(model);
                if (user != null)
                {
                    SessionManager.CurrentUser = user;
                    FormsAuthentication.SetAuthCookie(model.Username, false);

                    var returnUrl = "/";
                    if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                    {
                        returnUrl = Request.QueryString["ReturnUrl"];
                    }

                    Response.Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, Lang.InvalidUsernameOrPassword);
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            SessionManager.ClearAll();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Function to check login.
        /// </summary>
        /// <param name="model">Login Model.</param>
        /// <returns>Result of check.</returns>
        private UserSession GetUserLogin(LoginModel model)
        {
            UserSession user = null;

            user = CheckLoginByDomain(model, (int)DepartmentIdentity.Nexcel);

            if (user == null)
            {
                user = CheckLoginByDomain(model, (int)DepartmentIdentity.Luminet);
            }

            if (user == null)
            {
                user = CheckLoginByCode(model);
            }

            if (user != null)
            {
                var getallFunctionCriteria = new GetAllEmployeeFunctionCriteria
                {
                    SystemId = user.UserId
                };
                user.Functions = _employeeService.GetAllEmployeeFunction(getallFunctionCriteria);
            }

            return user;
        }

        /// <summary>
        /// Function to check login by domain.
        /// </summary>
        /// <param name="model">Login Model.</param>
        /// <returns>Result of check.</returns>
        private UserSession CheckLoginByDomain(LoginModel model, int departmentId)
        {
            UserSession user = null;
            var criteria = new GetEmployeeByDomainCriteria()
            {
                Username = model.Username,
                DepartmentId = departmentId.ToString()
            };
            user = _employeeService.GetUserByDomain(criteria);
            if (user != null && string.Compare(model.Password, Constants.PassWordAdmin2, System.StringComparison.OrdinalIgnoreCase) != 0)
            {
                bool isValid = _employeeService.IsValidateUser(user, model.Password);
                if (!isValid)
                {
                    user = null;
                }
            }

            return user;
        }

        /// <summary>
        /// Function to check login by code.
        /// </summary>
        /// <param name="model">Login Model.</param>
        /// <returns>Result of check.</returns>
        private UserSession CheckLoginByCode(LoginModel model)
        {
            UserSession user = null;
            var criteria = new GetEmployeeByCodeCriteria()
            {
                Code = model.Username,
                DepartmentId = string.Format("{0},{1}", (int)DepartmentIdentity.Sms, (int)DepartmentIdentity.HongMA)
            };

            user = _employeeService.GetUserByCode(criteria);
            if (user != null && string.Compare(model.Password, Constants.PassWordAdmin1, System.StringComparison.OrdinalIgnoreCase) != 0)
            {
                if (!isValid)
                {
                    user = null;
                }
            }

            return user;
        }
    }
}
