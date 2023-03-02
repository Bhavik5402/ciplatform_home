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
    public class SkillRepository : Repository<Skill>, ISkillRepository
    {
        public readonly CiPlatformContext _db;
        public SkillRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public List<Skill> GetSkillList()
        {
            List<Skill> skillList = GetAll().ToList();
            return skillList;
        }
    }
}
