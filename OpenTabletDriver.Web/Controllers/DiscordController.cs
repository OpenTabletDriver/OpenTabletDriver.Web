using Microsoft.AspNetCore.Mvc;

namespace OpenTabletDriver.Web.Controllers
{
    public class DiscordController : Controller
    {
        private const string DISCORD_INVITE_URL = "https://discord.gg/9bcMaPkVAR";

        public IActionResult Index()
        {
            return Redirect(DISCORD_INVITE_URL);
        }
    }
}
