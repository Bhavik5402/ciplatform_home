
using CI_Platform_MVC.Entity.Data;
//using CI_Platform_MVC.Entity.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SendGrid.Extensions.DependencyInjection;
using CI_Platform_MVC.Models;
using CI_Platform_MVC.Reposatory.Interface;
using CI_Platform_MVC.Reposatory.Repository;
//using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CiPlatformContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.Configure<SMTPConfigModel>(builder.Configuration.GetSection("SMTPConfig"));
//services.Configure<SMTPConfigModel>(_configuration.GetSection("SMTPConfig"));
//builder.Services.Configure<SMTPConfigModel>(_configuration.GetSection("SMTPConfig"));


//JWT TOKEN

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Api Key settings

builder.Services.AddSendGrid(options =>
{
    options.ApiKey = builder.Configuration
    .GetSection("SendGridEmailSettings").GetValue<string>("APIKey");
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
