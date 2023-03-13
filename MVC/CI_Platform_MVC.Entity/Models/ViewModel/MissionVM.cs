using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_MVC.Entity.Models.ViewModel
{
    public class MissionVM
    {
        public IEnumerable<Mission> Missions { get; set; }
        public IEnumerable<User> users { get; set; }
        public IEnumerable<City> cities { get; set; }
        public IEnumerable<Country> country { get; set; }   

        public IEnumerable<MissionTheme> missionThemes { get; set; }

        public IEnumerable<Skill> skills { get; set; }

        public User User { get; set; }

        public Mission Mission { get; set; }

        public IEnumerable<Mission> RelatedMissions { get; set; }

        public IEnumerable<MissionRating> MissionRating { get; set; }

        public int AvgRating { get; set; }

        public bool IsFav { get; set; }
    }
}
