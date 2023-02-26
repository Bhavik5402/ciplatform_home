

using CI_Platform_MVC.Data;
//using CI_Platform_MVC.Entity.Data;
//using CI_Platform_MVC.Entity.Data;
//using CI_Platform_MVC.Entity.Models;
using CI_Platform_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CI_Platform_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly CiPlatformContext _db;
        private IConfiguration _configuration;


        public HomeController(CiPlatformContext db , IConfiguration iconfig , ILogger<HomeController> logger)
        {;
            this._db = db;
            this._configuration = iconfig;
            this._logger = logger;
        }

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Index(User obj)
        {

            var user = _db.Users.FirstOrDefault(u => u.Email == obj.Email);



            if (user == null)
            {
                TempData["error"] = "User is not Registered";
                return RedirectToAction("Index");
            }
            if (user.Password == obj.Password)
            {
                TempData["success"] = "Login Successful";
                return RedirectToAction("Home");
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
        [ValidateAntiForgeryToken]

        public IActionResult Forgotpassword(string email)
        {
            if(ModelState.IsValid)
            {
                var user = _db.Users.FirstOrDefault(u => u.Email == email);
                if(user != null)
                {
                    //var token = GenerateToken(user);
                    var token = GenerateToken(user);

                    

                    var obj = new PasswordReset()
                    {
                        Email = email,
                        Token = token.ToString(),
                        CreateAt = DateTime.Now
                    };
                    
                    _db.PasswordResets.Add(obj);
                    _db.SaveChanges();
                    var passwordresetlink = Url.Action("Resetpassword" , "Home" , new {email = email , token = token } , Request.Scheme);
                    TempData["link"]= passwordresetlink;

                    //token.Payload.Exp > DateTime.UnixEpoch

                    return RedirectToAction("Index");

                    
                }
            }


            return View();
        }


       

        //generate token method

        //private string GenerateToken(User user)
        private JwtSecurityToken GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email , user.Email)
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            //return new JwtSecurityTokenHandler().WriteToken(token);
            return token;

        }



        public IActionResult Resetpassword()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(User user)
        {
            
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Index");
            
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult temp()
        {
            return View();
        }


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }


}
