
using CI_Platform_MVC.Entity.Data;
//using CI_Platform_MVC.Entity.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SendGrid.Extensions.DependencyInjection;
using CI_Platform_MVC.Models;
using CI_Platform_MVC.Utility;
using CI_Platform_MVC.Reposatory.Interface;
using CI_Platform_MVC.Reposatory.Repositories;
//using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});

builder.Services.AddDbContext<CiPlatformContext>();
//builder.Services.AddScoped<ICityRepository , CityRepository>();
//builder.Services.AddScoped<ICountryRepository , CountryRepository>();
//builder.Services.AddScoped<ICountryRepository , CountryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILandingPage, LandingPage>();
//builder.Services.AddScoped<IPasswordResetRepository , PasswordResetRepository>();
//builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<Functions>();

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

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
