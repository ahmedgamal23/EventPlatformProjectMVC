using EventPlatformProjectMVC.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatformProjectMVC.Application.Services
{
    public static class SeedRoleService
    {
        public static async Task InitializeRoles(this IServiceProvider serviceProvider)
        {
            // Add default roles
            var adminId = Guid.NewGuid().ToString();
            var attendeeId = Guid.NewGuid().ToString();
            var organizerId = Guid.NewGuid().ToString();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            List<string> roles = new List<string> { UserRole.Admin.ToString(), UserRole.Attendee.ToString(), UserRole.Organizer.ToString() };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole
                    {
                        Id = roleName switch
                        {
                            "Admin" => adminId,
                            "Attendee" => attendeeId,
                            "Organizer" => organizerId,
                            _ => throw new Exception("Invalid Role")
                        },
                        Name = roleName,
                        NormalizedName = roleName.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    };
                    await roleManager.CreateAsync(role);
                }
            }

            // create default admin user
            var adminUserId = Guid.NewGuid().ToString();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var hasher = new PasswordHasher<ApplicationUser>();

            var user = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                CreateAt = DateTime.Now,
                Name = "Admin",
                PhoneNumber = "+201147893607",
                PhoneNumberConfirmed = true,
                Role = UserRole.Admin.ToString(),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            user.PasswordHash = hasher.HashPassword(user, "admin");
            await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
        }
    }
}
