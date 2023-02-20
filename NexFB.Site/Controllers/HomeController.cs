namespace NexFB.Site.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeeService employeeService;

        public HomeController()
            : this(null)
        {
        }

        public HomeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService ?? new EmployeeService();
        }

        public ActionResult Index()
        {
            if (SessionManager.CurrentUser != null)
            {
                UserSession user = SessionManager.CurrentUser;
                if (user != null)
                {
                    var criteria = new GetEmployeeMenuBySystemIdCriteria
                    {
                        SystemId = user.UserId
                    };
                    IList<MenuGroup> menuGroup = employeeService.GetEmployeeMenuById(criteria);
                    ViewBag.MenuGroup = menuGroup;
                }

                return View(user);
            }

            return RedirectToAction("Login", "auth");
        }
    }
}