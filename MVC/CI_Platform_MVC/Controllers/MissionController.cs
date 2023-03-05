using CI_Platform_MVC.Entity.Data;
using CI_Platform_MVC.Entity.Models;
using CI_Platform_MVC.Reposatory.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_MVC.Controllers
{
    public class MissionController : Controller
    {
        public readonly IUnitOfWork _UnitOfWork;

        public MissionController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Home(long id = 0, string sort = "")
        {
            var sessionValue = HttpContext.Session.GetString("UserEmail");

            if (String.IsNullOrEmpty(sessionValue))
            {
                TempData["error"] = "Session Expired! Please Login Again!";
                return RedirectToAction("Index", "Home");
            }


            List<City> citylist = _UnitOfWork.City.GetCityList().Where(x => x.Name != "undefined").ToList();
            List<MissionTheme> themelist = _UnitOfWork.MissionTheme.GetThemeList();
            List<Mission> missionlist = _UnitOfWork.Mission.GetAllMissions();
            List<Skill> skillList = _UnitOfWork.Skill.GetSkillList();
            List<Country> countrylist = _UnitOfWork.Country.GetCountryList().Where(x => x.Name != "undefined").ToList();
            var user = _UnitOfWork.User.GetFirstOrDefault(u => u.Email == sessionValue);
            List<City> citybycountry = _UnitOfWork.City.GetCitiesByCountry(id);
            List<User> allUsers = _UnitOfWork.User.GetAll().Where(u => u.Email != sessionValue).ToList(); 

            


            if (id == 0)
            {
                ViewBag.City = citylist;
                if (sort == "" || sort == "Name")
                {
                    ViewBag.MissionList = missionlist.OrderBy(m => m.Title).ToList();
                }
                if (sort == "Newest")
                {
                    ViewBag.MissionList = missionlist.OrderBy(m => m.CreateAt).ToList();
                }

                if (sort == "Oldest")
                {
                    ViewBag.MissionList = missionlist.OrderByDescending(m => m.CreateAt).ToList();
                }

                if (sort == "SortByDeadline")
                {
                    ViewBag.MissionList = missionlist.OrderBy(m => m.StartDate).ToList();
                }
                //ViewBag.MissionList = missionlist;
            }
            else
            {
                ViewBag.City = citybycountry;
                List<Mission> missionlistcountry = _UnitOfWork.Mission.GetAllMissions().Where(u => u.CountryId == id).ToList();
                if (sort == "" || sort == "Name")
                {
                    ViewBag.MissionList = missionlistcountry.OrderBy(m => m.Title).ToList();
                }
                if (sort == "Newest")
                {
                    ViewBag.MissionList = missionlistcountry.OrderBy(m => m.CreateAt).ToList();
                }

                if (sort == "Oldest")
                {
                    ViewBag.MissionList = missionlistcountry.OrderByDescending(m => m.CreateAt).ToList();
                }

                if (sort == "SortByDeadline")
                {
                    ViewBag.MissionList = missionlistcountry.OrderBy(m => m.StartDate).ToList();
                }
            }

            

            ViewBag.AllUsers = allUsers;

            ViewBag.Country = countrylist;

            ViewBag.ThemeList = themelist;

            ViewBag.SkillList = skillList;

            //ViewBag.MissionList = missionlist;
            //missionlist[1].StartDate - TimeSpan.FromDays(1);
            ViewBag.User = user;

            return View();
        }


        public IActionResult Volunteering_Page()
        {
            var sessionValue = HttpContext.Session.GetString("UserEmail");
            if (String.IsNullOrEmpty(sessionValue))
            {
                TempData["error"] = "Session Expired! Please Login Again!";
                return RedirectToAction("Index", "Home");
            }
            var user = _UnitOfWork.User.GetFirstOrDefault(u => u.Email == sessionValue);
            List<Mission> missionlist = _UnitOfWork.Mission.GetAllMissions();
            List<User> allUsers = _UnitOfWork.User.GetAll().Where(u => u.Email != sessionValue).ToList();
            ViewBag.MissionList = missionlist;
            ViewBag.AllUsers = allUsers;
            ViewBag.User = user;
            return View();
        }

    }
}
