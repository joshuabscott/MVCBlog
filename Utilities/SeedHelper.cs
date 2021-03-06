﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using MVCBlog.Data;
using MVCBlog.Models;
using MVCBlog.Enums;

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
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
        }

        public static async Task SeedAdmin(UserManager<BlogUser> userManager)
        {
            if (await userManager.FindByEmailAsync("J@mailinator.com") == null)
            {
                var admin = new BlogUser()
                {
                    UserName = "J@mailinator.com",
                    Email = "J@mailinator.com",
                    FirstName = "Josh",
                    LastName = "Scott",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "!1Qwerty");
                await userManager.AddToRoleAsync(admin, Roles.Administrator.ToString());
            }
        }

        public static async Task SeedModerator(UserManager<BlogUser> userManager)
        {
            if (await userManager.FindByEmailAsync("W@mailinator.com") == null)
            {
                var moderator = new BlogUser()
                {
                    UserName = "W@mailinator.com",
                    Email = "W@mailinator.comJ",
                    FirstName = "Adam",
                    LastName = "West",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(moderator, "!1Qwerty");
                await userManager.AddToRoleAsync(moderator, Roles.Moderator.ToString());
            }
        }
    }
}