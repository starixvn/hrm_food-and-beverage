namespace NexFB.WCFService.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeControler : Controller
    {
        public IActionResult Index()
        {
            return Ok("I'm fine!");
        }
    }
}
