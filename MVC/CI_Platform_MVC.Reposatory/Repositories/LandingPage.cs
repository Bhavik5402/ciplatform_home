using CI_Platform_MVC.Entity.Models;
using CI_Platform_MVC.Entity.Models.ViewModel;
using CI_Platform_MVC.Reposatory.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_MVC.Reposatory.Repositories
{
    public class LandingPage : ILandingPage
    {
        public readonly IUnitOfWork _UnitOfWork;
        public LandingPage(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        public MissionVM GetMissionVM(string sessionValue , long id = 0, string sort = "" )
        {
            MissionVM missionVM = new();

            missionVM.missionThemes = _UnitOfWork.MissionTheme.GetThemeList();
            IEnumerable<Mission> missionlist = _UnitOfWork.Mission.GetAllMissions();
            missionVM.skills = _UnitOfWork.Skill.GetSkillList();
            missionVM.country = _UnitOfWork.Country.GetAll().Where(x => x.Name != "undefined");
            missionVM.User = _UnitOfWork.User.GetFirstOrDefault(u => u.Email == sessionValue);
            missionVM.users = _UnitOfWork.User.GetAll().Where(u => u.Email != sessionValue);
            




            if (id == 0)
            {
                missionVM.cities = _UnitOfWork.City.GetCityList().Where(x => x.Name != "undefined");


                if (sort == "" || sort == "Name")
                {

                    missionVM.Missions = missionlist.OrderBy(m => m.Title).ToList();
                }
                if (sort == "Newest")
                {
                    missionVM.Missions = missionlist.OrderBy(m => m.CreateAt).ToList();
                }

                if (sort == "Oldest")
                {
                    missionVM.Missions = missionlist.OrderByDescending(m => m.CreateAt).ToList();
                }

                if (sort == "SortByDeadline")
                {
                    missionVM.Missions = missionlist.OrderBy(m => m.StartDate).ToList();
                }
                
            }
            else
            {
                missionVM.cities = _UnitOfWork.City.GetCitiesByCountry(id);

                List<Mission> missionlistcountry = _UnitOfWork.Mission.GetAllMissions().Where(u => u.CountryId == id).ToList();
                if (sort == "" || sort == "Name")
                {
                    missionVM.Missions = missionlistcountry.OrderBy(m => m.Title).ToList();
                }
                if (sort == "Newest")
                {
                    missionVM.Missions = missionlistcountry.OrderByDescending(m => m.CreateAt).ToList();
                }

                if (sort == "Oldest")
                {
                    missionVM.Missions = missionlistcountry.OrderBy(m => m.CreateAt).ToList();
                }

                if (sort == "SortByDeadline")
                {
                    missionVM.Missions = missionlistcountry.OrderBy(m => m.StartDate).ToList();
                }
            }
           return missionVM;

        }

        public IEnumerable<Mission> ApplyFilter(string filter, string sessionValue, long id = 0, string sort = "" , int page =1)
        {
            MissionVM missionObj = GetMissionVM(sessionValue, id, sort);
            IEnumerable<Mission> missions = missionObj.Missions;
            IEnumerable<Mission> filterMissions;

            if(filter != null)
            {
                var obj = JObject.Parse(filter);
                var cityName = obj.Value<string>("city");
                var themeName = obj.Value<string>("theme");
                var skillName = obj.Value<string>("skill");
                var searchData = obj.Value<string>("searchItem");
                var filterCity = cityName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var filterTheme = themeName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var filterSkill = skillName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (filterCity.Length != 0 && filterTheme.Length != 0 && filterSkill.Length != 0)
                {
                    IEnumerable<Mission> missionsList = new List<Mission>();
                    filterMissions = missions.Where(m => m.City.Name == filterCity[0] && m.Theme.Title == filterTheme[0] && m.MissionSkills.Any(s => s.Skill.SkillName == filterSkill[0]));
                    foreach (string city in filterCity)
                    {
                        foreach (string theme in filterTheme)
                        {
                            foreach (string skill in filterSkill)
                            {
                                missionsList = missionsList.Where(m => m.Theme.Title == theme && m.City.Name == city && m.MissionSkills.Any(s => s.Skill.SkillName == skill));
                            }
                        }
                        filterMissions = filterMissions.Concat(missionsList);
                    }
                }

                else if (filterCity.Length != 0 && filterTheme.Length != 0)
                {
                    IEnumerable<Mission> missionsList = new List<Mission>();
                    filterMissions = missions.Where(m => m.City.Name == filterCity[0] && m.Theme.Title == filterTheme[0]);
                    foreach (string city in filterCity)
                    {
                        foreach (string theme in filterTheme)
                        {
                            missionsList = missionsList.Where(m => m.Theme.Title == theme && m.City.Name == city);
                        }
                        filterMissions = filterMissions.Concat(missionsList);
                    }
                }

                else if (filterTheme.Length != 0 && filterSkill.Length != 0)
                {
                    IEnumerable<Mission> missionsList = new List<Mission>();
                    filterMissions = missions.Where(m => m.MissionSkills.Any(s => s.Skill.SkillName == filterSkill[0]) && m.Theme.Title == filterTheme[0]);
                    foreach (string theme in filterTheme)
                    {
                        foreach (string skill in filterSkill)
                        {
                            missionsList = missionsList.Where(m => m.Theme.Title == theme && m.MissionSkills.Any(s => s.Skill.SkillName == skill));
                        }
                        filterMissions = filterMissions.Concat(missionsList);
                    }
                }

                else if (filterCity.Length != 0 && filterSkill.Length != 0)
                {
                    IEnumerable<Mission> missionsList = new List<Mission>();
                    filterMissions = missions.Where(m => m.MissionSkills.Any(s => s.Skill.SkillName == filterSkill[0]) && m.City.Name == filterCity[0]);
                    foreach (string city in filterCity)
                    {
                        foreach (string skill in filterSkill)
                        {
                            missionsList = missionsList.Where(m => m.City.Name == city && m.MissionSkills.Any(s => s.Skill.SkillName == skill));
                        }
                        filterMissions = filterMissions.Concat(missionsList);
                    }
                }



                else if (filterCity.Length != 0)
                {
                    filterMissions = missions.Where(u => u.City.Name == filterCity[0]);
                    foreach (string item in filterCity)
                    {
                        filterMissions = filterMissions.Concat(missions.Where(u => u.City.Name == item));
                    }
                }
                else if (filterTheme.Length != 0)
                {
                    filterMissions = missions.Where(m => m.Theme.Title == filterTheme[0]);
                    foreach (string item in filterTheme)
                    {
                        filterMissions = filterMissions.Concat(missions.Where(u => u.Theme.Title == item));
                    }
                }
                else if (filterSkill.Length != 0)
                {
                    filterMissions = missions.Where(m => m.MissionSkills.Any(s => s.Skill.SkillName == filterSkill[0]));
                    foreach (string item in filterSkill)
                    {
                        filterMissions = filterMissions.Concat(missions.Where(u => u.MissionSkills.Any(s => s.Skill.SkillName == item)));
                    }
                }
                else
                {
                    filterMissions = missions;
                }

                filterMissions = filterMissions.Distinct();

                if (sort == "" || sort == "Name")
                {
                    filterMissions = filterMissions.OrderBy(m => m.Title).ToList();
                }
                else if (sort == "Newest")
                {
                    filterMissions = filterMissions.OrderByDescending(m => m.CreateAt).ToList();
                }

                else if (sort == "Oldest")
                {
                    filterMissions = filterMissions.OrderBy(m => m.CreateAt).ToList();
                }

                else
                {
                    filterMissions = filterMissions.OrderBy(m => m.StartDate).ToList();
                }


                if(searchData != "")
                {
                    filterMissions = filterMissions.Where(u => u.Title.ToLower().Contains(searchData));
                }

                filterMissions = filterMissions.Skip((page-1) * 9).Take(9);
                return filterMissions;
            }
            missions = missions.Skip((page-1) * 9).Take(9);
            return missions;

            
        }



        
        public MissionVM GetMissionPage(long id , string sessionValue)
        {
            MissionVM missionVM = new();
            missionVM.skills = _UnitOfWork.Skill.GetAll();
            IEnumerable<MissionRating> missionRating = _UnitOfWork.MissionRating.GetAll().Where(m=>m.MissionId == id);
            int sum = 0;
            foreach (MissionRating rating in missionRating)
            {
                sum += rating.Rating;
            }
            if(sum > 0)
            {
                missionVM.AvgRating = sum/missionRating.Count();
            }
            else
            {
                missionVM.AvgRating = 0;
            }
            missionVM.Mission = _UnitOfWork.Mission.GetMission(id);
            missionVM.User = _UnitOfWork.User.GetFirstOrDefault(u => u.Email == sessionValue);
            missionVM.users = _UnitOfWork.User.GetAll().Where(u => u.Email != sessionValue);
            missionVM.RelatedMissions = _UnitOfWork.Mission.RelatedMissions(id);
            return missionVM;
        }


    }
}
