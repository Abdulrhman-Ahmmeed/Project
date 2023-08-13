using Bookify.Web2.Core.Consts;
using Bookify.Web2.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Bookify.Web2.Seeds
{
    public class DefaultUsers
    {
        public static async Task SeedAdminUser(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser admin = new()
            {
                
                UserName = "Admin",
                Email = "Admin@Bookify.com",
                FullName = "Admin",
                EmailConfirmed = true
            };
          
            var user = await userManager.FindByEmailAsync(admin.Email);
            if (user is null)
            {
                await userManager.CreateAsync(admin, "Password1453!");
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);
            }
        }
    }
}
