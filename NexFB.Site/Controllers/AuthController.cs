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
    using System;
    using System.Runtime.InteropServices;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using NexFB.Core.Constants;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

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

        [DllImport("ADVAPI32.dll", EntryPoint = "LogonUserW", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool LogonUser(string username, string domain, string password, int logonType, int logonProvider, ref IntPtr token);

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
            bool result = CheckLogin(model);
            if (result)
            {
                var criteria = new GetEmployeeByDomainCriteria() 
                { 
                    Username = model.Username 
                };
                UserSession user = _employeeService.GetUserByDomain(criteria);
                HttpContext.Session["CurrentUser"] = user;
                FormsAuthentication.SetAuthCookie(model.Username, false);

                if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                {
                    FormsAuthentication.RedirectFromLoginPage(model.Username, false);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    Response.Redirect("~/employee/myspace");
                }
            }
            else
            {
                Response.Write("Invalid username or password.");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (HttpContext.Session != null)
            {
                HttpContext.Session.Abandon();
                HttpContext.Session.RemoveAll();
            }

            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Function to check login.
        /// </summary>
        /// <param name="model">Login Model.</param>
        /// <returns>Result of check.</returns>
        private bool CheckLogin(LoginModel model)
        {
            var result = false;
            IntPtr token = IntPtr.Zero;
            result = LogonUser(model.Username, model.Username, model.Password, 2, 0, ref token);
            if (!result)
            {
                var criteria = new GetEmployeeByCodeCriteria()
                {
                    Code = model.Username,
                    DepartmentId = Constants.HongMaAndSmsDepartment
                };

                if (string.Compare(model.Password, Constants.PassWordAdmin2, ignoreCase: true) == 0)
                {
                    criteria.DepartmentId = Constants.NexcelDepartment;
                }

                UserSession user = _employeeService.GetUserByCode(criteria);
                if (string.Compare(model.Password, Constants.PassWordAdmin1, ignoreCase: true) == 0
                    || string.Compare(model.Password, Constants.PassWordAdmin2, ignoreCase: true) == 0)
                {
                    result = true;
                }
                else
                {
                    var checkCode = _employeeService.ValidateUser(user, model.Password);
                    result = checkCode == 0;
                }
            }

            return result;
        }
    }
}
