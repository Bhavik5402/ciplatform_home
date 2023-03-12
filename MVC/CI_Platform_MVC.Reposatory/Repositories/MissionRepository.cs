using CI_Platform_MVC.Entity.Data;
using CI_Platform_MVC.Entity.Models;
using CI_Platform_MVC.Reposatory.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_MVC.Reposatory.Repositories
{
    public class MissionRepository : Repository<Mission> , IMissionRepository
    {
        public readonly CiPlatformContext _db;
        public MissionRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public List<Mission> GetAllMissions()
        {
            //List<Mission> missionList = GetAll().ToList();
            var missionList = _db.Missions.Include(m => m.City).Include(m => m.Theme).Include(m=>m.MissionSkills).Include(m=>m.MissionMedia).ToList();
            return missionList;
        }

        public Mission GetMission(long id)
        {
            var mission = _db.Missions.Include(m => m.City).Include(m => m.Theme).Include(m=>m.MissionSkills).Include(m=>m.MissionRatings).Include(m=>m.MissionMedia).Where(u=>u.MissionId == id).FirstOrDefault();
            return mission;
        }

        public List<Mission> RelatedMissions(long missionId)
        {
            Mission mission = _db.Missions.FirstOrDefault(u => u.MissionId == missionId);

            long themeId = mission.ThemeId;
            //long skillId = _db.Missions.Select(m=>m.s).FirstOrDefault();
            long cityId = mission.CityId;
            long countryId = mission.CountryId;
            var relatedMissionList = GetAllMissions().Where(u => u.CityId == cityId).ToList();
            if(relatedMissionList.Count() < 3)
            {
                relatedMissionList = GetAllMissions().Where(m => m.CountryId == countryId).ToList();
                if(relatedMissionList.Count() < 3)
                {
                    relatedMissionList = GetAllMissions().Where(m=>m.ThemeId==themeId).ToList();
                }
              
            }
            relatedMissionList = relatedMissionList.Take(3).ToList();

            
            return relatedMissionList;
        }
    }
}
