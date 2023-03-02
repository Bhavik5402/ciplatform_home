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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public readonly CiPlatformContext _db;
        public UserRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }

        public void UpdatePassword(User user, string newPassword)
        {
            user.Password = newPassword;
            user.UpdatedAt = DateTime.Now;
            _db.Users.Update(user);
        }
    }
}
