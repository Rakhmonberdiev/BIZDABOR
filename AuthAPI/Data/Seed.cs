using AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await userManager.Users.AnyAsync())
            {
                return;
            }

            var roles = new List<IdentityRole>
            {
                new IdentityRole{Name="Admin"},
                new IdentityRole{Name="Client"}
            };
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
            var admin = new AppUser
            {
                UserName = "admin",

            };
            await userManager.CreateAsync(admin, "Admin123*");
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
