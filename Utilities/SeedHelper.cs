using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MVCBlog.Data;
using MVCBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Utilities
{
    public class SeedHelper
    {
        public static async Task GetDataAsync(UserManager<BlogUser>userManager,RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedAdmin(userManager);
            await SeedAdmin(userManager);
        }
        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
        }

        private static async Task SeedAdmin(UserManager<BlogUser> userManager)
        {
            if(await userManager.FindByEmailAsync("JoshuaBScott@gmail.com") == null)
            {
                var admin = new BlogUser()
                {
                    Email = "JoshuaBScott@gmail.com",
                    UserName = "JoshScott",
                    FirstName = "Josh",
                    LastName = "Scott",
                    EmailConfirmed = true
                };
            }
        }
            }

  
  /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  
        public async Task SeedData(UserManager<BlogUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
            AssignRoles(userManager);
        }

        private async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<string> { "Admin", "Moderator" };

            foreach (var role in roles)
            {
                if (!await.roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role, NormalizedName = role.ToUpper() });
                }
            }
        }

        public async Task SeedUsers(UserManager<BlogUser> userManager)
        {
            var userManager = UserStore<BlogUser>(db);
            //if (db.Users.Any(userManager => u.Email == "Joshuabscott@gmail.com"))

            if (db.Users.Any(userManager => u.Email == "Joshuabscott@gmail.com"))
            {
               var adminUser = new BlogUser()
               {
                 EmailTokenProvider = "Joshuabscott@gmail.com",
                 UserName = "JoshScott",
                 FirstName = "Josh",
                 LastName = "Scott"
               };
            }
        await userManager.CreateAsync(adminUser);
        await userManager.AddToRoleAsync(adminUser, "Admin");
    

    var roles = new List<string> { "Admin", "Moderator" };
    foreach(var role in roles)
    {
    if(!db.Roles.Any(RoleManager => ref.Name == role))
    {
    await roleManager.CreateAsync(new IdentityRole { Name = role, NormalizedName = role.ToUpper()});
    }
}
}

        private async Task SeedUsers(UserManager<BlogUser> userManager)
        {
            //var userStore = new UserStore<BlogUser>(db);
            
            if(!db.Users.Any(u => u.Email == "Joshuabscott@gmail.com"))
            {
                await userManager.CreateAsync()
                    new BlogUser(
                    {

                    });
            }
        }
    }
}
