

using CI_Platform_MVC.Entity.Data;
using CI_Platform_MVC.Entity.Models;
using CI_Platform_MVC.Entity.Models.ViewModel;
using CI_Platform_MVC.Models;
using CI_Platform_MVC.Reposatory.Interface;
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
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CI_Platform_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly IUserRepository _userRepository;
        private readonly ISendGridClient _sendGridClient;
        private readonly CiPlatformContext _db;
        private IConfiguration _configuration;
        private readonly SMTPConfigModel _smtpConfig;


        public HomeController(CiPlatformContext db , IConfiguration iconfig , ILogger<HomeController> logger , ISendGridClient sendGridClient, 
            IOptions<SMTPConfigModel> smtpConfig , IUserRepository userRepository)
        {;
            this._db = db;
            this._configuration = iconfig;
            this._logger = logger;
            _sendGridClient = sendGridClient;
            _smtpConfig = smtpConfig.Value;
            _userRepository = userRepository;
        }

        
        //login page controller

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Index(Entity.Models.User obj)
        {
            var encodedPassword = _userRepository.EncryptPass(obj.Password);

            var user = _db.Users.FirstOrDefault(u => u.Email == obj.Email);



            if (user == null)
            {
                TempData["error"] = "User is not Registered";
                return RedirectToAction("Index");
            }
            if (user.Password == encodedPassword)
            {
                TempData["success"] = "Login Successful";
                return RedirectToAction("Home");
            }
            TempData["error"] = "Invalid Password";
            return RedirectToAction("Index");
        }



        //forgot password controller

        public IActionResult Forgotpassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]


        public async Task<IActionResult> Forgotpassword(string email)
        {
            if(ModelState.IsValid)
            {
                var user = _db.Users.FirstOrDefault(u => u.Email == email);
                if(user != null)
                {
                    var token = GenerateToken(user);
                    var token2 = new JwtSecurityTokenHandler().WriteToken(token);

                    var obj = new Entity.Models.PasswordReset()
                    {
                        Email = email,
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        CreateAt = DateTime.Now
                    };

                    _db.PasswordResets.Add(obj);
                    _db.SaveChanges();

                    var passwordresetlink = Url.Action("Resetpassword" , "Home" , new {token = token2 } , Request.Scheme);
                    TempData["link"]= passwordresetlink;

                    UserEmailOptions userEmailOptions = new UserEmailOptions()
                    {
                        Subject = "Reset Password Link",
                        Body = "<a href="+passwordresetlink+">"+passwordresetlink+"</a>"
                    };
                    SendEmail(email, userEmailOptions);

                    


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


        //Send Email Method


        public void SendEmail(string toEmail, UserEmailOptions userEmailOptions)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_smtpConfig.SenderDisplayName,_smtpConfig.SenderAddress));
            email.To.Add(new MailboxAddress("Bhavik", toEmail ));

            email.Subject = userEmailOptions.Subject;
            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = userEmailOptions.Body;
            email.Body = bodyBuilder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 465, true);

                smtp.Authenticate("bjparmar.5402@gmail.com", "fcizgxoijgbcypox");

                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
       


        //generate token method

        private JwtSecurityToken GenerateToken(Entity.Models.User user)
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
                expires: DateTime.Now.AddMinutes(240),
                signingCredentials: credentials);

            return token;

        }

        //reset password controller
        public IActionResult Resetpassword(string token)
        {
            var find_token = _db.PasswordResets.FirstOrDefault(u => u.Token == token);
            if(find_token == null)
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
            if(tokenobj.Payload.Exp < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                return BadRequest("Reset Password Link has been expired try again");
            }
            return View();
        }
        
        [HttpPost]
        public IActionResult Resetpassword(ResetPassVM obj)
        {
            var encodedPassword = _userRepository.EncryptPass(obj.password);

            if (obj.password == obj.confirmpassword)
            {
                var user = _db.Users.FirstOrDefault(x => x.Email == obj.email);
                user.Password = encodedPassword;
                user.UpdatedAt = DateTime.Now;
                _db.Users.Update(user);
                var token_remove = _db.PasswordResets.FirstOrDefault(x => x.Token == obj.token);
                _db.PasswordResets.Remove(token_remove);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        //registration controller

        public IActionResult Registration()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Registration(RegisterVM obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.User.Password != obj.ConfirmPassword)
                {
                    TempData["error"] = "Passwords are not matched";
                    return View();
                }

                obj.User.Password = _userRepository.EncryptPass(obj.User.Password);
                _db.Users.Add(obj.User);
                _db.SaveChanges();
                TempData["success"] = "Registration Successful";
                return RedirectToAction("Index");

            }


            return View();
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
