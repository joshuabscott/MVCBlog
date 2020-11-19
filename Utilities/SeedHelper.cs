using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCBlog.Models;


namespace MVCBlog.Utilities
{
    public enum Roles
    {
        Administrator,
        Moderator,
        Demo
    }

    public class SeedHelper
    {
        //Seed Roles
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Demo.ToString()));
        }

        public static async Task SeedDefaultUsersAsync(UserManager<BlogUser> userManager)
        {
            //SeedDefault Administrator
            #region SeedAdministrator
            var defaultAdmin = new BlogUser
            {
                UserName = "J@mailinator.com",
                Email = "J@mailinator.comJ",
                FirstName = "Josh",
                LastName = "Scott",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultAdmin.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultAdmin, "!1Qqazwsxedc");
                    await userManager.AddToRoleAsync(defaultAdmin, Roles.Administrator.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************ ERROR  ************");
                Debug.WriteLine("Error Seeding Default Administrator.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("********************************");
                throw;
            }
            #endregion

            //SeedDefault Moderator
            #region SeedModerator
            var defaultModerator = new BlogUser
            {
                UserName = "W@mailinator.com",
                Email = "W@mailinator.comJ",
                FirstName = "Adam",
                LastName = "West",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultModerator.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultModerator, "!1Qqazwsxedc");
                    await userManager.AddToRoleAsync(defaultModerator, Roles.Moderator.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************ ERROR  ************");
                Debug.WriteLine("Error Seeding Default Administrator.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("********************************");
                throw;
            }
            #endregion



            //SeedDemo Administrator
            #region Demo Administrator
            var demoAdministrator = new BlogUser
            {
                UserName = "demoW@mailinator.com",
                Email = "demoW@mailinator.comJ",
                FirstName = "Adam",
                LastName = "West",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultModerator.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(demoAdministrator, "g9N88.se!");
                    await userManager.AddToRoleAsync(demoAdministrator, Roles.Administrator.ToString());
                    await userManager.AddToRoleAsync(demoAdministrator, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************ ERROR  ************");
                Debug.WriteLine("Error Seeding Default Administrator.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("********************************");
                throw;
            }
            #endregion

            //SeedDemo Moderator
            #region Demo Moderator
            var demoModerator = new BlogUser
            {
                UserName = "demoW@mailinator.com",
                Email = "demoW@mailinator.comJ",
                FirstName = "B",
                LastName = "West",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(demoModerator.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(demoModerator, "g9N88.se!");
                    await userManager.AddToRoleAsync(demoModerator, Roles.Moderator.ToString());
                    await userManager.AddToRoleAsync(demoModerator, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************ ERROR  ************");
                Debug.WriteLine("Error Seeding Default Administrator.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("********************************");
                throw;
            }
            #endregion

        }
    }
}