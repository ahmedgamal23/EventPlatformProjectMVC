using EventPlatformProjectMVC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EventPlatformProjectMVC.Domain;
using Microsoft.AspNetCore.Identity.UI.Services;
using EventPlatformProjectMVC.Infrastructure.Repositories;
using EventPlatformProjectMVC.Application.Services;
using EventPlatformProjectMVC.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Stripe;

namespace EventPlatformProjectMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // register connection string
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("EventPlatformConnectionString"));
            });

            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // register UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            // register IBaseRepository , BaseRepository
            builder.Services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));

            // register services
            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.LogoutPath = "/Identity/Account/Logout";
            });

            // register session service
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // initialize stripe configuration
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Seed roles
            using (var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                SeedRoleService.InitializeRoles(service).Wait();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "area",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapRazorPages();

            app.Run();
        }
    }
}
