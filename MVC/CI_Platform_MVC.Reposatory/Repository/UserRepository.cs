using CI_Platform_MVC.Entity.Data;
using CI_Platform_MVC.Entity.Models;
using CI_Platform_MVC.Reposatory.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_MVC.Reposatory.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly CiPlatformContext _db;

        public UserRepository(CiPlatformContext db)
        {
            _db = db;
        }   

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public string EncryptPass(string str)
        {
            byte[] encData_byte = new byte[str.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(str);
            string strpass = Convert.ToBase64String(encData_byte);
            return strpass;
        }
    } 
}
