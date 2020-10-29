using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MVCBlog.Data;
using MVCBlog.Enums;
using MVCBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Utilities
{
    public class SeedHelper
    {
        public static async Task GetDataAsync(UserManager<BlogUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedAdmin(userManager);
            await SeedModerator(userManager);
        }
        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
        }

        private static async Task SeedAdmin(UserManager<BlogUser> userManager)
        {
            if (await userManager.FindByEmailAsync("JoshuaBScott@gmail.com") == null)
            {
                var admin = new BlogUser()
                {
                    Email = "JoshuaBScott@gmail.com",
                    UserName = "JoshScott",
                    FirstName = "Josh",
                    LastName = "Scott",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "!1Qazwsx");
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }

        internal static Task SeedDataAsync(UserManager<BlogUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            throw new NotImplementedException();
        }

        private static async Task SeedModerator(UserManager<BlogUser> userManager)
        {
            if (await userManager.FindByEmailAsync("JoshuaBScott@outlook.com") == null)
            {
                var moderator = new BlogUser()
                {
                    Email = "JoshuaBScott@outlook.com",
                    UserName = "JoshBScott",
                    FirstName = "Josh",
                    LastName = "Scott",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(moderator, "!1Qazwsx");
                await userManager.AddToRoleAsync(moderator, Roles.Moderator.ToString());
            }
        }

    }
}
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
