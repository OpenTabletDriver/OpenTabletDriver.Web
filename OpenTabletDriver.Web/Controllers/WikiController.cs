using Microsoft.AspNetCore.Mvc;

namespace OpenTabletDriver.Web.Controllers
{
    public class WikiController : Controller
    {
        [ResponseCache(Duration = 300)]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 300)]
        [Route("Wiki/{category}/{page}")]
        public IActionResult Wiki(string category, string page)
        {
            return View($"{category}/{page}");
        }
    }
}