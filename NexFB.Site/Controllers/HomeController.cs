// ----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/22/2014 9:03:31 AM">Created.</change>
// </history>

namespace NexFB.Site.Controllers
{
    using System.Web.Mvc;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web;
    using System.IO;
    using System.Drawing;

    /// <summary>
    /// The home controller.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        
        public HomeController()
            : this(null)
        {
        }

         public HomeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService ?? new EmployeeService();
        }

        /// <summary>
        /// Action Index.
        /// </summary>
         /// <returns>Action Result.</returns>
        public ActionResult Index()
        {
            if (SessionManager.CurrentUser != null)
            {
                UserSession user = SessionManager.CurrentUser;
                if (user != null)
                {
                    var getEmployeeMenuCriteria = new GetEmployeeMenuBySystemIdCriteria
                    {
                        SystemId = user.UserId
                    };
                    SessionManager.MenuGroups = _employeeService.GetEmployeeMenuById(getEmployeeMenuCriteria);

                    var getallFunctionCriteria = new GetAllEmployeeFunctionCriteria
                    {
                        SystemId = user.UserId
                    };
                    SessionManager.Functions = _employeeService.GetAllEmployeeFunction(getallFunctionCriteria);
                }

                byte[] bytes = System.IO.File.ReadAllBytes("D:\\Demo\\Avatars\\02303.jpg");

                MemoryStream ms = new MemoryStream(bytes);
                Image returnImage = Image.FromStream(ms);

                ViewBag.Image = returnImage;

                return View(user);
            }

            return RedirectToAction("Login", "auth");
        }
    }
}
