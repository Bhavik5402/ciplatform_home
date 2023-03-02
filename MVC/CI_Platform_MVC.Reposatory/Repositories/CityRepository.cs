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
    public class CityRepository : Repository<City>, ICityRepository
    {
        public readonly CiPlatformContext _db;
        public CityRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public List<City> GetCityList()
        {
            List<City> citylist = _db.Cities.ToList();
            return citylist;
        }

        public List<City> GetCitiesByCountry(long country_id)
        {
            List<City> citylistbycountry = GetAll().Where(c => c.CountryId == country_id).ToList();
            return citylistbycountry;
        }
    }
}
