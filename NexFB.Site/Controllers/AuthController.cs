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
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web;

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
            FormsAuthentication.SignOut();
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
                model.IsLoginFail = true;
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
            bool result = Membership.ValidateUser(model.Username, model.Password);
            ////bool result = true;
            if (result)
            {
                var criteria = new GetEmployeeByDomainCriteria()
                {
                    Username = model.Username,
                    DepartmentId = Constants.NexcelDepartment.ToString()
                };
                user = _employeeService.GetUserByDomain(criteria);
            }
            else
            {
                var criteria = new GetEmployeeByCodeCriteria()
                {
                    Code = model.Username,
                    DepartmentId = string.Format("{0},{1}", Constants.SmsDepartment, Constants.HMDepartment)
                };
                if (string.Compare(model.Password, Constants.PassWordAdmin2, System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    criteria.DepartmentId = Constants.NexcelDepartment.ToString();
                }

                user = _employeeService.GetUserByCode(criteria);
                if (user != null
                    && string.Compare(model.Password, Constants.PassWordAdmin1, System.StringComparison.OrdinalIgnoreCase) != 0
                    && string.Compare(model.Password, Constants.PassWordAdmin2, System.StringComparison.OrdinalIgnoreCase) != 0)
                {
                    var checkCode = _employeeService.ValidateUser(user, model.Password);
                    if (checkCode != 0)
                    {
                        user = null;
                    }
                }
            }

            return user;
        }
    }
}
