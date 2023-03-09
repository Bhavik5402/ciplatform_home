using CI_Platform_MVC.Entity.Data;
using CI_Platform_MVC.Entity.Models;
using CI_Platform_MVC.Entity.Models.ViewModel;
using CI_Platform_MVC.Reposatory.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace CI_Platform_MVC.Controllers
{
    public class MissionController : Controller
    {
        public readonly IUnitOfWork _UnitOfWork;
        public readonly ILandingPage _landingPage;

        public MissionController(IUnitOfWork unitOfWork , ILandingPage landingPage)
        {
            _UnitOfWork = unitOfWork;
            _landingPage = landingPage; 
        }

        public IActionResult Home(string filter , long id = 0, string sort = "", string page = "1")
        {
            var sessionValue = HttpContext.Session.GetString("UserEmail");

            if (String.IsNullOrEmpty(sessionValue))
            {
                TempData["error"] = "Session Expired! Please Login Again!";
                return RedirectToAction("Index", "Home");
            }
            int pageNumber = int.Parse(page);
            
            MissionVM missionVM = _landingPage.GetMissionVM (sessionValue , id , sort , pageNumber);
            missionVM.Missions = missionVM.Missions.Skip((pageNumber - 1)*9).Take(9);

            if (filter != null || sort != "")
            {

                return RedirectToAction("GetAllMissions" , new {filter , id , sort});


            }


           
            return View(missionVM);
        }

        public JsonResult[] GetAllMissions(string filter , long id = 0, string sort = "" , string page = "1")
        {
            int pageNumber = int.Parse(page);
            var sessionValue = HttpContext.Session.GetString("UserEmail");
           
            IEnumerable<Mission> allmissions = _landingPage.ApplyFilter(filter ,sessionValue, id , sort );
            JsonResult[] missions = new JsonResult[allmissions.ToList().Count];
            
            int i = 0;
            foreach(Mission mission in allmissions)
            {
                JsonResult eachmission = new JsonResult(
                    new {
                        mission.Title,
                        mission.City.Name,
                        //mission.Country.Name,
                        //mission.CityId,
                        startDate = mission.StartDate.Value.ToShortDateString(),
                        endDate = mission.EndDate.Value.ToShortDateString(),
                        theme = mission.Theme.Title,
                        mission.ShortDescription,
                        mission.OrganizationName,
                        deadLine = ((mission.StartDate - TimeSpan.FromDays(1)).Value.ToShortDateString())
                    }
                    
                    );
                missions[i] = eachmission;
                i++;
            }
            //missions = missions.Skip((pageNumber - 1)).Take(9);
            return missions;


        }


        public IActionResult Volunteering_Page(long themeid , long id = 0 )
        {
            var sessionValue = HttpContext.Session.GetString("UserEmail");
            if (String.IsNullOrEmpty(sessionValue))
            {
                TempData["error"] = "Session Expired! Please Login Again!";
                return RedirectToAction("Index", "Home");
            }
            //var user = _UnitOfWork.User.GetFirstOrDefault(u => u.Email == sessionValue);
            //List<Mission> missionlist = _UnitOfWork.Mission.GetAllMissions();
            //List<User> allUsers = _UnitOfWork.User.GetAll().Where(u => u.Email != sessionValue).ToList();
            //ViewBag.MissionList = missionlist;
            //ViewBag.AllUsers = allUsers;
            //ViewBag.User = user;
            //MissionVM missionVM = _landingPage.GetMissionVM(sessionValue, id, sort);
            MissionVM missionlandingpageVM = _landingPage.GetMissionPage(id , sessionValue , themeid);
            return View(missionlandingpageVM);
        }

    }
}
