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
            var missionList = _db.Missions.Include(m => m.City).Include(m => m.Theme).Include(m=>m.MissionSkills).ToList();
            return missionList;
        }

        public Mission GetMission(long id)
        {
            var mission = _db.Missions.Include(m => m.City).Include(m => m.Theme).Where(u=>u.MissionId == id).FirstOrDefault();
            return mission;
        }

        public List<Mission> RelatedMissions(long theme_id)
        {
            var relatedMissionList = _db.Missions.Where(u => u.ThemeId == theme_id).ToList();
            return relatedMissionList;
        }
    }
}
