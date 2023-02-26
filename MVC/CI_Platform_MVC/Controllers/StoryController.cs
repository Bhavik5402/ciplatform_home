using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_MVC.Controllers
{
    public class StoryController : Controller
    {
        public IActionResult StoryListingPage()
        {
            return View();
        }
    }
}
