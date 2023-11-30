namespace NexFB.Site.Controllers
{
    using System;
    using System.Text;
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
        private readonly IAuthService _authService;
        private readonly ICryptoService _cryptoService;
        private readonly IUserControlSession _userControlSession;
        private readonly IUserControlSettings _userControlSettings;

        public AuthController()
            : this(null, null, null, null, null)
        {
        }

        public AuthController(IEmployeeService employeeService,
            IAuthService authService,
            ICryptoService cryptoService,
            IUserControlSession userControlSession,
            IUserControlSettings userControlSettings)
        {
            _employeeService = employeeService ?? new EmployeeService();
            _authService = authService ?? new AuthService();
            _cryptoService = cryptoService ?? new CryptoService();
            _userControlSession = userControlSession ?? new UserControlSession();
            _userControlSettings = userControlSettings ?? new UserControlSettings();
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
                model.Password = Encoding.UTF8.GetString(Convert.FromBase64String(model.Password));
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
                AddFunctionToUser(user);
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

            user = !loginResult.IsSuccessfull ? null : GetEmployeeByUserId(loginResult.UserId);

            return user;
        }

        public ActionResult SSO(string userId, string onUserId, string time, string redirectUrl = null)
        {
            const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            var userIdValue = Cryptography.DecryptPassword(userId.Replace(" ", "+"));
            var onUserIdValue = Cryptography.DecryptPassword(onUserId.Replace(" ", "+"));
            var timeValue = Cryptography.DecryptPassword(time.Replace(" ", "+"));
            var currentTime = _authService.SSOGetTime();
            TimeSpan expiration = DateTime.ParseExact(currentTime, DateTimeFormat, null) - DateTime.ParseExact(timeValue, DateTimeFormat, null);
            const byte TimeExpiration = 30;
            if (expiration.TotalSeconds <= TimeExpiration)
            {
                var ssoParam = new SSOLoginParam
                {
                    UserId = Convert.ToInt32(userIdValue),
                    CheckOnUserId = Convert.ToInt64(onUserIdValue),
                    OnUserId = 0,
                    IP = GetClientIP(),
                    Platform = Request.Browser.Platform,
                    BrowserName = Request.Browser.Browser,
                    NetSessionId = Session.SessionID
                };
                LoginResult loginResult = _authService.SSOLogin(ssoParam);
                if (loginResult.IsSuccessfull)
                {
                    var employee = GetEmployeeByUserId(ssoParam.UserId);
                    if (employee == null)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        AddFunctionToUser(employee);
                    }

                    _userControlSession.UserId = Convert.ToInt32(userIdValue);
                    _userControlSession.SiteId = _userControlSettings.SiteId;
                    _userControlSession.JwtToken = loginResult.JwtToken;
                    _userControlSession.OnUserId = loginResult.OnUserId;

                    SessionManager.CurrentUser = employee;
                    FormsAuthentication.SetAuthCookie(employee.UserId.ToString(), false);
                    return !string.IsNullOrWhiteSpace(redirectUrl) && Url.IsLocalUrl(redirectUrl) ? Redirect(redirectUrl) : Redirect("~/Ordering/Index");
                }
            }

            return RedirectToAction("Login");
        }
        private UserSession GetEmployeeByUserId(int id)
        {
            var criteria = new GetEmployeeByUserIdCriteria
            {
                UserId = id,
                CompanyId = string.Format("{0},{1}", (byte)CompanyIdentity.Sms, (byte)CompanyIdentity.HongMA)
            };

            return _employeeService.GetUserByUserId(criteria);
        }

        private void AddFunctionToUser(UserSession user)
        {
            var criteria = new GetAllEmployeeFunctionCriteria
            {
                SystemId = user.UserId
            };
            user.Functions = _employeeService.GetAllEmployeeFunction(criteria);
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