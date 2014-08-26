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
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Auth Controller.
    /// </summary>
    public class AuthController : Controller
    {
        [DllImport("ADVAPI32.dll", EntryPoint = "LogonUserW", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        /// <summary>
        /// Parses the string to pull the domain name out.
        /// </summary>
        /// <param name="usernameDomain">The string to parse that must contain the domain in either the domain\username or UPN format username@domain</param>
        /// <returns>The domain name or "" if not domain is found.</returns>
        public static string GetDomainName(string usernameDomain)
        {
            if (string.IsNullOrEmpty(usernameDomain))
            {
                throw (new ArgumentException("Argument can't be null.", "usernameDomain"));
            }
            if (usernameDomain.Contains("\\"))
            {
                int index = usernameDomain.IndexOf("\\");
                return usernameDomain.Substring(0, index);
            }
            else if (usernameDomain.Contains("@"))
            {
                int index = usernameDomain.IndexOf("@");
                return usernameDomain.Substring(index + 1);
            }
            else
            {
                return "";
            }
        }

        ////private readonly IEmployeeService _employeeService;
        ////public AuthController()
        ////{
        ////    _employeeService = new EmployeeService();
        ////}

        ////public AuthController(IEmployeeService employeeService)
        ////{
        ////    _employeeService = employeeService;
        ////}

        ////[HttpGet]
        ////public ActionResult Login()
        ////{
        ////    return View();
        ////}

        /////// <summary>
        /////// Login Action.
        /////// </summary>
        /////// <param name="model">LoginModel Object.</param>
        /////// <returns>Action Result Login.</returns>
        ////[HttpPost]
        ////public ActionResult Login(LoginModel model)
        ////{
        ////    string domainName = GetDomainName(model.UserName);
        ////    IntPtr token = IntPtr.Zero;
        ////    bool result = LogonUser(model.UserName, domainName, model.Password, 2, 0, ref token);
        ////    if (result)
        ////    {
        ////        //If Sucessfully authenticated
        ////        var getEmployeeCriteria = new GetEmployeeCriteria() { DomainUserName = model.UserName };
        ////        var user = new EmployeeService().GetEmployeeById(getEmployeeCriteria);
        ////        HttpContext.Session["CurrentUser"] = user;
        ////        FormsAuthentication.SetAuthCookie(model.UserName, false);

        ////        //When an unatuthenticated user try to visit any page of your application that is only allowed to view by authenticated users then,ASP.NET automatically redirect the user to login form and add ReturnUrl query string parameter that contain the url of a page that user want to visit, So that we can redirect the user to that page after authenticated. FormsAuthentication.RedirectFromLoginPage() method not only redirect the user to that page but also genrate an authentication token for that user.
        ////        if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
        ////        {
        ////            FormsAuthentication.RedirectFromLoginPage(model.UserName, false);
        ////        }
        ////        //If ReturnUrl query string parameter is not present , then we need to genrate authentication token and redirect the user to any page ( acording to your application need). FormsAuthentication.SetAuthCookie() method will genrate Authentication token 
        ////        else
        ////        {
        ////            FormsAuthentication.SetAuthCookie(model.UserName, false);
        ////            Response.Redirect("~/employee/myspace");
        ////        }
        ////    }
        ////    else
        ////    {
        ////        //If not authenticated then display an error message
        ////        Response.Write("Invalid username or password.");
        ////    }

        ////    return View(model);
        ////}

        ////[HttpGet]
        ////public ActionResult Logout()
        ////{
        ////    if (HttpContext.Session != null)
        ////    {
        ////        HttpContext.Session.Abandon();
        ////        HttpContext.Session.RemoveAll();
        ////    }

        ////    FormsAuthentication.SignOut();
        ////    return RedirectToAction("Login");
        ////}
    }
}
