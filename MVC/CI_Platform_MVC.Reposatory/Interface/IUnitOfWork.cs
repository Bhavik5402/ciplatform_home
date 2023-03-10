using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_MVC.Reposatory.Interface
{
    public interface IUnitOfWork
    {
        public IUserRepository User { get; }
        public ICityRepository City { get; }
        public ICountryRepository Country { get; }
        public IPasswordResetRepository PasswordReset { get; }
        public IMissionThemeRepository MissionTheme { get; }

        public IMissionRepository Mission { get; }

        public ISkillRepository Skill { get; }
        public IMissionRatingRepository MissionRating{ get; }

        public IFavoriteMissionRepository FavoriteMission { get; }  

        public IMissionInviteRepository MissionInvite { get; }
        public void Save();
    }
}
