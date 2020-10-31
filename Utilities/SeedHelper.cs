using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MVCBlog.ViewModels;
using MVCBlog.Models;
using MVCBlog.Enums;
using MVCBlog.Data;

namespace MVCBlog.Utilities
{
    public class SeedHelper
    {
        public static async Task SeedDataAsync(UserManager<BlogUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedAdmin(userManager);
            await SeedModerator(userManager);
        }

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
        }

        public static async Task SeedAdmin(UserManager<BlogUser> userManager)
        {
            if (await userManager.FindByEmailAsync("JoshuaBScott@gmail.com") == null)
            {
                var admin = new BlogUser()
                {
                    Email = "JoshuaBScott@gmail.com",
                    UserName = "JoshScotty",
                    FirstName = "Joshua",
                    LastName = "Scotty",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "!1Qazwsx");
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }

        public static async Task SeedModerator(UserManager<BlogUser> userManager)
        {
            if (await userManager.FindByEmailAsync("J@joshscott.xyz") == null)
            {
                var moderator = new BlogUser()
                {
                    Email = "J@joshscott.xyz",
                    UserName = "JScott",
                    FirstName = "Josh",
                    LastName = "Scott",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(moderator, "!1Qazwsxedc");
                await userManager.AddToRoleAsync(moderator, Roles.Moderator.ToString());
            }
        }
    }
}