
using CI_Platform_MVC.Entity.Data;
using CI_Platform_MVC.Entity.Models;
using CI_Platform_MVC.Entity.Models.ViewModel;
using CI_Platform_MVC.Models;
using CI_Platform_MVC.Reposatory.Interface;
using CI_Platform_MVC.Utility;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
//using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CI_Platform_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //public readonly ICityRepository _cityRepository;
        //public readonly ICountryRepository _countryRepository;
        public readonly IUnitOfWork _UnitOfWork;
        //public readonly IPasswordResetRepository _passwordresetRepository;
        //private readonly CiPlatformContext _db;

        private readonly Functions _f;


        public HomeController(ILogger<HomeController> logger, Functions f, IUnitOfWork unitOfWork)
        {
            //this._db = db;
            this._logger = logger;
            _f = f;
            //_cityRepository = cityRepository;
            //_countryRepository = countryRepository;
            //_passwordresetRepository = passwordResetRepository;
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var sessionValue = HttpContext.Session.GetString("UserEmail");
            if (!String.IsNullOrEmpty(sessionValue))
            {
                return RedirectToAction("Home" , "Mission");
            }

            return View();
        }

        [HttpPost]

        public IActionResult Index(User obj)
        {
            var encodedPassword = _f.encodepass(obj.Password);

            //var user = _db.Users.FirstOrDefault(u => u.Email == obj.Email);
            var user = _UnitOfWork.User.GetFirstOrDefault(u => u.Email == obj.Email);



            if (user == null)
            {
                TempData["error"] = "User is not Registered";
                return RedirectToAction("Index");
            }
            if (user.Password == encodedPassword)
            {
                TempData["success"] = "Login Successful";

                HttpContext.Session.SetString("UserEmail", user.Email);
                //var Username = user.FirstName;
                //ViewData["username"] = user.FirstName;
                return RedirectToAction("Home" , "Mission");
            }
            TempData["error"] = "Invalid Password";
            return RedirectToAction("Index");
        }



        public IActionResult Forgotpassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]


        public async Task<IActionResult> Forgotpassword(string email)
        {
            if (ModelState.IsValid)
            {
                //var user = _db.Users.FirstOrDefault(u => u.Email == email);
                var user = _UnitOfWork.User.GetFirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    var token = _f.GenerateToken(user);
                    var token2 = new JwtSecurityTokenHandler().WriteToken(token);

                    var obj = new PasswordReset()
                    {
                        Email = email,
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        CreateAt = DateTime.Now
                    };

                    //_db.PasswordResets.Add(obj);
                    _UnitOfWork.PasswordReset.Add(obj);
                    //_db.SaveChanges();
                    _UnitOfWork.Save();

                    var passwordresetlink = Url.Action("Resetpassword", "Home", new { token = token2 }, Request.Scheme);
                    TempData["link"] = passwordresetlink;

                    UserEmailOptions userEmailOptions = new UserEmailOptions()
                    {
                        Subject = "Reset Password Link",
                        Body = "<a href=" + passwordresetlink + ">" + passwordresetlink + "</a>"
                    };
                    _f.SendEmail(email, userEmailOptions);




                    TempData["success"] = "Email has been sent to your email account";
                    return RedirectToAction("Index");


                }
                else
                {
                    TempData["error"] = "Email has not been registered";
                }
            }


            return View();
        }



        public IActionResult Resetpassword(string token)
        {
            //var find_token = _db.PasswordResets.FirstOrDefault(u => u.Token == token);
            var find_token = _UnitOfWork.PasswordReset.GetFirstOrDefault(u => u.Token == token);
            if (find_token == null)
            {
                return BadRequest("token has been expired");
            }

            var tokenobj = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var email = tokenobj.Payload.Claims.ToList()[0].Value;
            ViewBag.Email = new
            {
                email = email,
                token = token
            };
            if (tokenobj.Payload.Exp < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                return BadRequest("Reset Password Link has been expired try again");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Resetpassword(ResetPassVM obj)
        {
            var encodedPassword = _f.encodepass(obj.password);
            if (obj.password == obj.confirmpassword)
            {
                //var user = _db.Users.FirstOrDefault(x => x.Email == obj.email);
                var user = _UnitOfWork.User.GetFirstOrDefault(x => x.Email == obj.email);
                //var removelist = _passwordresetRepository.GetAll().Where(x => x.Email == obj.email);
                var removelist = _UnitOfWork.PasswordReset.GetAll().Where(x => x.Email == obj.email);
                //user.Password = encodedPassword;
                user.UpdatedAt = DateTime.Now;
                //_db.Users.Update(user);
                _UnitOfWork.User.UpdatePassword(user , encodedPassword);
                //var token_remove = _db.PasswordResets.FirstOrDefault(x => x.Token == obj.token);
                //_db.PasswordResets.Remove(token_remove);
                //_passwordresetRepository.RemoveRange(removelist);
                _UnitOfWork.PasswordReset.RemoveRange(removelist);
                //_db.SaveChanges();
                _UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegisterVM obj)
        {
            if (ModelState.IsValid)
            {
                //var user = _db.Users.FirstOrDefault(x => x.Email == obj.User.Email);
                var user = _UnitOfWork.User.GetFirstOrDefault(x => x.Email == obj.User.Email);
                if (user == null)
                {
                    if (obj.User.Password != obj.ConfirmPassword)
                    {
                        TempData["error"] = "Passwords are not matched";
                        return View();
                    }
                    obj.User.Password = _f.encodepass(obj.User.Password);
                    //_db.Users.Add(obj.User);
                    _UnitOfWork.User.Add(obj.User);
                    //_db.SaveChanges();
                    _UnitOfWork.Save();
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Email Address is already registered";
                }


            }


            return View();
        }

        //public IActionResult Home(long id=0)
        //{
        //    var sessionValue = HttpContext.Session.GetString("UserEmail");

        //    List<City> citylist = _UnitOfWork.City.GetCityList().Where(x => x.Name != "undefined").ToList();
        //    List<MissionTheme> themelist = _UnitOfWork.MissionTheme.GetThemeList();
        //    List<Mission> missionlist = _UnitOfWork.Mission.GetAllMissions();
        //    List<Skill> skillList = _UnitOfWork.Skill.GetSkillList();
        //    List<Country> countrylist = _UnitOfWork.Country.GetCountryList().Where(x => x.Name != "undefined").ToList();
        //    var user = _UnitOfWork.User.GetFirstOrDefault(u => u.Email == sessionValue);
        //    List<City> citybycountry = _UnitOfWork.City.GetCitiesByCountry(id);
        //    if(id == 0)
        //    {
        //        ViewBag.City = citylist;
        //        ViewBag.MissionList = missionlist;
        //    }
        //    else
        //    {
        //        ViewBag.City = citybycountry;
        //        ViewBag.MissionList = _UnitOfWork.Mission.GetAllMissions().Where(u => u.CountryId == id);
        //    }
        //    ViewBag.Country = countrylist;

        //    ViewBag.ThemeList = themelist;

        //    ViewBag.SkillList = skillList;

        //    //ViewBag.MissionList = missionlist;
        //    //missionlist[1].StartDate - TimeSpan.FromDays(1);
        //    ViewBag.User = user;
            
        //    return View();
        //}

        public IActionResult temp()
        {
            return View();
        }

        public IActionResult logout()
        {
            HttpContext.Session.SetString("UserEmail", "");
            return RedirectToAction("Index");
        }


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }


}
