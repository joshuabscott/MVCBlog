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
        private ApplicationDbContext _context;

        public SeedHelper(ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedData()
        {
            await SeedRolesAsync();
            await SeedUsersAndAssignAsync(userManager);
        }
        public async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
            
          var roles = new List<string> { "Admin", "Moderator" };

          foreach (var role in roles)
          {
             if (!await.roleManager.RoleExistsAsync(role))
             {
                    await roleManager.CreateAsync(new IdentityRole { Name = role, NormalizedName = role.ToUpper()
    });
                }
            }
        }
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
