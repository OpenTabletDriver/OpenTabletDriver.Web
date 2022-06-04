using Microsoft.AspNetCore.Mvc;

namespace OpenTabletDriver.Web.Controllers;

public class GitHubController : Controller
{
    private const string REPOSITORY_URL = "https://www.github.com/OpenTabletDriver/OpenTabletDriver";

    public IActionResult Index()
    {
        return Redirect(REPOSITORY_URL);
    }
}
