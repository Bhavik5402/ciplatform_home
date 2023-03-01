using CI_Platform_MVC.Entity.Models;
using CI_Platform_MVC.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CI_Platform_MVC.Utility
{
    public class Functions
    {
        private IConfiguration _configuration;
        private readonly SMTPConfigModel _smtpConfig;

        public Functions (IConfiguration configuration , IOptions<SMTPConfigModel> smtpConfig )
        {
            _configuration = configuration;
            _smtpConfig = smtpConfig.Value;
        }



        //encode password
        public string encodepass(string str)
        {
            byte[] encData_byte = new byte[str.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(str);
            string encodedPassword = Convert.ToBase64String(encData_byte);
            return encodedPassword;
        }


        //generate token method

        public JwtSecurityToken GenerateToken(User user)
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

        //send email method

        public void SendEmail(string toEmail, UserEmailOptions userEmailOptions)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_smtpConfig.SenderDisplayName, _smtpConfig.SenderAddress));
            email.To.Add(new MailboxAddress("Bhavik", toEmail));

            email.Subject = userEmailOptions.Subject;
            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = userEmailOptions.Body;
            email.Body = bodyBuilder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 465, true);

                smtp.Authenticate(_smtpConfig.UserName,_smtpConfig.Password);

                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }


    }
}
