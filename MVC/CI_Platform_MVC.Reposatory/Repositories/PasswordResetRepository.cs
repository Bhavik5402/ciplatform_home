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
    public class PasswordResetRepository : Repository<PasswordReset> ,IPasswordResetRepository
    {
        public readonly CiPlatformContext _db;
        public PasswordResetRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public void Update(PasswordReset passwordReset)
        {
            throw new NotImplementedException();
        }
    


}
}
