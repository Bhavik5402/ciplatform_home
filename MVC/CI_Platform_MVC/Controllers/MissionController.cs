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

        public IActionResult Home(string filter , long id = 0, string sort = "", int page = 0 )
        {
            var sessionValue = HttpContext.Session.GetString("UserEmail");

            if (String.IsNullOrEmpty(sessionValue))
            {
                TempData["error"] = "Session Expired! Please Login Again!";
                return RedirectToAction("Index", "Home");
            }

            

            MissionVM missionVM = _landingPage.GetMissionVM (sessionValue , id , sort);
            ViewBag.TotalMissions = missionVM.Missions.Count();
            



            if (filter != null || sort != "" || page>0)
            {
                return RedirectToAction("GetAllMissions" , new {filter , id , sort , page});
            }
            //TempData["TotalMissions"] = missionVM.Missions.Count();
            HttpContext.Session.SetString("CountryId", id.ToString());
            missionVM.Missions = missionVM.Missions.Skip(page*9).Take(9);
           
            return View(missionVM);
        }

        public JsonResult[] GetAllMissions(string filter , long id = 0, string sort = "" , int page = 0)
        {
            var sessionValue = HttpContext.Session.GetString("UserEmail");
            var countryId = HttpContext.Session.GetString("CountryId");
            var Id = int.Parse(countryId);
            IEnumerable<Mission> allmissions = _landingPage.ApplyFilter(filter ,sessionValue, Id , sort , page);
            JsonResult[] missions = new JsonResult[allmissions.ToList().Count];
            //TempData["TotalMissions"] = allmissions.ToList().Count(); 

            int i = 0;
            foreach(Mission mission in allmissions)
            {
                JsonResult eachmission = new JsonResult(
                    new {
                            mission.Title,
                            mission.City.Name,
                            startDate = mission.StartDate.Value.ToShortDateString(),
                            endDate = mission.EndDate.Value.ToShortDateString(),
                            theme = mission.Theme.Title,
                            mission.ShortDescription,
                            mission.OrganizationName,
                            deadLine = ((mission.StartDate - TimeSpan.FromDays(1)).Value.ToShortDateString())
                            //count = 
                        }
                    
                );
                missions[i] = eachmission;
                i++;
            }
            return missions;
        }


        public IActionResult Volunteering_Page( long id = 0 )
        {
            var sessionValue = HttpContext.Session.GetString("UserEmail");
            if (String.IsNullOrEmpty(sessionValue))
            {
                TempData["error"] = "Session Expired! Please Login Again!";
                return RedirectToAction("Index", "Home");
            }
            MissionVM missionlandingpageVM = _landingPage.GetMissionPage( id , sessionValue );
            return View(missionlandingpageVM);
        }

        public void AddRating(string rating)
        {
            var robj = JObject.Parse(rating);
            var ratingVal = robj.Value<int>("ratingval");
            var missionId = robj.Value<long>("missionId");
            var userId = robj.Value<long>("userId");

            var ratingObj = _UnitOfWork.MissionRating.GetFirstOrDefault(u => u.UserId == userId && u.MissionId == missionId);
            ViewBag.Rating = ratingVal; 
            var obj = new MissionRating()
            {
                MissionId = missionId,
                UserId = userId,
                Rating = ratingVal
            };
            if (ratingObj != null)
            {
                _UnitOfWork.MissionRating.Update(obj);
                _UnitOfWork.Save();
            }
            else
            {
                _UnitOfWork.MissionRating.Add(obj);
                _UnitOfWork.Save();
            }
        }

        public void AddToFav(string favObj)
        {
            //MissionVM mv = new(); 
            var fObj = JObject.Parse(favObj);
            var missionId = fObj.Value<long>("missionId");
            var userId = fObj.Value<long>("userId");
            var obj = new FavoriteMission()
            {
                UserId = userId,
                MissionId = missionId
            };
            var favmission = _UnitOfWork.FavoriteMission.GetFirstOrDefault(u=>u.MissionId == missionId && u.UserId == userId);
            if(favmission == null)
            {
                _UnitOfWork.FavoriteMission.Add(obj);
                
                _UnitOfWork.Save();
            }
            else
            {
                _UnitOfWork.FavoriteMission.Remove(favmission);
                
                _UnitOfWork.Save();
            }
            
            
        }

        public void AddToRec(string RecObj)
        {
            var recObj = JObject.Parse(RecObj); 
            var FromUserId = recObj.Value<long>("FromUserId");
            var ToUserId = recObj.Value<long>("ToUserId");
            var MissionId = recObj.Value<long>("MissionId");
            var obj = new MissionInvite()
            {
                FromUserId = FromUserId,
                MissionId = MissionId,
                ToUserId = ToUserId,
            };
            var recmission = _UnitOfWork.MissionInvite.GetFirstOrDefault(m => m.MissionId == MissionId && m.FromUserId == FromUserId && m.ToUserId == ToUserId);
            if(recmission == null)
            {
                _UnitOfWork.MissionInvite.Add(obj);
                _UnitOfWork.Save();
            }
            else
            {
                _UnitOfWork.MissionInvite.Remove(recmission);
                _UnitOfWork.Save();
            }
        }

    }
}
