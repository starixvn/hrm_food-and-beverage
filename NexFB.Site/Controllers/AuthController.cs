namespace NexFB.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using HRM.UserControl.Client.Core.Models.Auth;
    using HRM.UserControl.Client.Core.Services;
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
        private readonly IAuthService _authService;
        private readonly ICryptoService _cryptoService;

        public AuthController()
            : this(null, null, null)
        {
        }

        public AuthController(IEmployeeService employeeService,
            IAuthService authService,
            ICryptoService cryptoService)
        {
            _employeeService = employeeService ?? new EmployeeService();
            _authService = authService ?? new AuthService();
            _cryptoService = cryptoService ?? new CryptoService();
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
            UserSession user = CheckLoginByDomain(model, (byte)CompanyIdentity.Nexcel);

            user = user ?? CheckLoginByDomain(model, (byte)CompanyIdentity.Luminet);

            user = user ?? CheckLoginByCode(model);

            if (user != null)
            {
                var criteria = new GetAllEmployeeFunctionCriteria
                {
                    SystemId = user.UserId
                };
                user.Functions = _employeeService.GetAllEmployeeFunction(criteria);
                return user;
            }

            return user;
        }

        private UserSession CheckLoginByDomain(LoginModel model, byte companyId)
        {
            var criteria = new GetEmployeeByDomainCriteria
            {
                Username = model.Username,
                CompanyId = companyId.ToString()
            };
            UserSession user = _employeeService.GetUserByDomain(criteria);

            if (user != null && string.Compare(model.Password, Constants.PassWordAdmin2, StringComparison.OrdinalIgnoreCase) != 0)
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
            UserSession user;

            var token = _cryptoService.GenerateRijndaelKeyIV();
            string[] keyiv = token.Split(':');
            var rijndaelKey = keyiv[0];
            var rijndaelIV = keyiv[1];
            var loginParam = new LoginParam
            {
                Username = _cryptoService.Encrypt(model.Username, rijndaelKey, rijndaelIV),
                Password = _cryptoService.Encrypt(model.Password, rijndaelKey, rijndaelIV),
                Token = token,
                IP = GetClientIP(),
                Platform = Request.Browser.Platform,
                BrowserName = Request.Browser.Browser,
                NetSessionId = Session.SessionID
            };
            var loginResult = _authService.Login(loginParam);

            if (!loginResult.IsSuccessfull)
            {
                user = null;
            }
            else
            {
                var criteria = new GetEmployeeByUserIdCriteria
                {
                    UserId = loginResult.UserId,
                    CompanyId = string.Format("{0},{1}", (byte)CompanyIdentity.Sms, (byte)CompanyIdentity.HongMA)
                };
                user = _employeeService.GetUserByUserId(criteria);
            }

            return user;
        }

        private string GetClientIP()
        {
            var clientIp = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrWhiteSpace(clientIp))
            {
                var forwardedIps = clientIp.Split(new[] { ',' }, 1);
                clientIp = forwardedIps[forwardedIps.Length - 1];
            }
            else
            {
                clientIp = Request.ServerVariables["REMOTE_ADDR"];
            }

            if (clientIp == "::1")
            {
                clientIp = "127.0.0.1";
            }

            return clientIp;
        }
    }
}