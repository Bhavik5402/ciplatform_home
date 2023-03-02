using CI_Platform_MVC.Entity.Data;
using CI_Platform_MVC.Entity.Models;
using CI_Platform_MVC.Reposatory.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_MVC.Reposatory.Repositories
{
    public class MissionThemeRepository : Repository<MissionTheme>, IMissionThemeRepository
    {
        public readonly CiPlatformContext _db;
        public MissionThemeRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public List<MissionTheme> GetThemeList()
        {
            List<MissionTheme> themeList = GetAll().ToList();
            //List<MissionTheme> themeList = _db.MissionThemes.ToList();
            return themeList;
        }
    }
}
