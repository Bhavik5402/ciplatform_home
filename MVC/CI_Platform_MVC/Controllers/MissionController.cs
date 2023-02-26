using CI_Platform_MVC.Data;
//using CI_Platform_MVC.Entity.Data;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_MVC.Controllers
{
    public class MissionController : Controller
    {

        private readonly CiPlatformContext _db;


        public MissionController(CiPlatformContext db)
        {
            ;
            this._db = db;
        }

        public IActionResult Volunteering_Page()
        {
            return View();
        }

    }
}
