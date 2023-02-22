namespace NexFB.Site.Controllers.API
{
    using System.Web.Http;
    using System.Web.Security;
    using NexFB.Models.Authority;
    using NexFB.Web;
    using NexFB.Web.Attributes;

    [ApiAuthorize]
    public class BaseController : ApiController
    {
        protected UserSession CurrentUser
        {
            get
            {
                var currentUser = SessionManager.CurrentUser;
                if (currentUser == null)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }

                return currentUser;
            }
        }
    }
}