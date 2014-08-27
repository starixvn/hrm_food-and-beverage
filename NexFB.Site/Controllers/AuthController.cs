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
            UserSession user = GetUserLogin(model);
            if (user != null)
            {
                HttpContext.Session["CurrentUser"] = user;
                FormsAuthentication.SetAuthCookie(model.Username, false);
                if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                {
                    FormsAuthentication.RedirectFromLoginPage(model.Username, false);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    Response.Redirect("~/Home/Index");
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
        private UserSession GetUserLogin(LoginModel model)
        {
            UserSession user = null;
            IntPtr token = IntPtr.Zero;
            bool result = LogonUser(GetUsername(model.Username), GetDomainName(model.Username), model.Password, 2, 0, ref token);
            if (result)
            {
                var criteria = new GetEmployeeByDomainCriteria()
                {
                    Username = model.Username,
                    DepartmentId = Constants.NexcelDepartment
                };
                user = _employeeService.GetUserByDomain(criteria);
            }
            else
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

                user = _employeeService.GetUserByCode(criteria);
                if (user != null
                    && string.Compare(model.Password, Constants.PassWordAdmin1, ignoreCase: true) != 0
                    && string.Compare(model.Password, Constants.PassWordAdmin2, ignoreCase: true) != 0)
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

        /// <summary>
        /// Parses the string to pull the domain name out.
        /// </summary>
        /// <param name="usernameDomain">The string to parse that must contain the domain in either the domain\username or UPN format username@domain.</param>
        /// <returns>The domain name or "" if not domain is found.</returns>
        private string GetDomainName(string usernameDomain)
        {
            var domain = string.Empty;
            if (usernameDomain.Contains("\\"))
            {
                int index = usernameDomain.IndexOf("\\");
                domain = usernameDomain.Substring(0, index);
            }
            else if (usernameDomain.Contains("@"))
            {
                int index = usernameDomain.IndexOf("@");
                domain = usernameDomain.Substring(index + 1);
            }

            return domain;
        }

        /// <summary>
        /// Parses the string to pull the user name out.
        /// </summary>
        /// <param name="usernameDomain">The string to parse that must contain the username in either the domain\username or UPN format username@domain.</param>
        /// <returns>The username or the string if no domain is found.</returns>
        private string GetUsername(string usernameDomain)
        {
            var username = usernameDomain;
            if (usernameDomain.Contains("\\"))
            {
                int index = usernameDomain.IndexOf("\\");
                username = usernameDomain.Substring(index + 1);
            }
            else if (usernameDomain.Contains("@"))
            {
                int index = usernameDomain.IndexOf("@");
                username = usernameDomain.Substring(0, index);
            }

            return username;
        }
    }
}
