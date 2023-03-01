using CI_Platform_MVC.Entity.Data;
using CI_Platform_MVC.Reposatory.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_MVC.Reposatory.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly CiPlatformContext _db;
        public ICityRepository City { get; private set; }
        public ICountryRepository Country { get; private set; }

        public IPasswordResetRepository PasswordReset { get; private set; } 

        public IUserRepository User { get; private set; }
        public UnitOfWork(CiPlatformContext db)
        {
            _db = db;
            City = new CityRepository(_db);
            Country = new CountryRepository(_db);
            User = new UserRepository(_db);
            PasswordReset = new PasswordResetRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
