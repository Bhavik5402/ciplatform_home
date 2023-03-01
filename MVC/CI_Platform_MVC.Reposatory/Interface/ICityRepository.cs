using CI_Platform_MVC.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_MVC.Reposatory.Interface
{
    public interface ICityRepository : IRepository<City>
    {
        public List<City> GetCityList();

        public List<City> GetCitiesByCountry(long country_id);
    }
}
