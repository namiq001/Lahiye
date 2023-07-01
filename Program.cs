using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.Services;
using NOOKX_Project.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<NookxDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 8;
    opt.Password.RequireUppercase = true;

    opt.SignIn.RequireConfirmedEmail = true;
    opt.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<NookxDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<NookxDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
