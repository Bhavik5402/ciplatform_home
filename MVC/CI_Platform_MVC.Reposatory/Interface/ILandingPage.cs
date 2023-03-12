using CI_Platform_MVC.Entity.Models;
using CI_Platform_MVC.Entity.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_MVC.Reposatory.Interface
{
    public interface ILandingPage
    {
        public MissionVM GetMissionVM(string sessionValue, long id = 0, string sort = "");

        public IEnumerable<Mission> ApplyFilter(string filter, string sessionValue, long id=0 , string sort = "" , int page =  1);
        public MissionVM GetMissionPage(long id, string sessionValue);
    }
}
