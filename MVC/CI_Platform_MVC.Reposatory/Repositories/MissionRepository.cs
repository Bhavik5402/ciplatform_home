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
            var missionList = _db.Missions.Include(m => m.City).Include(m => m.Theme).ToList();
            return missionList;
        }
    }
}
