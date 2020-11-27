using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;
using MVCBlog.Data;
using MVCBlog.Models;
using MVCBlog.ViewModels;
using MVCBlog.Utilities;
using MVCBlog.Enums;

namespace MVCBlog
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await DataHelper.ManageData(host);
            await SeedDataAsync(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
                    webBuilder.UseStartup<Startup>();
                });

        public async static Task SeedDataAsync(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    
                    var userManager = services.GetRequiredService<UserManager<BlogUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await SeedHelper.SeedDataAsync(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}









//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Migrations;
//using MVCBlog.Data;
//using MVCBlog.Models;
//using MVCBlog.Services;
//using MVCBlog.Utilities;
//using Npgsql;

//namespace MVCBlog
//{
//    public class Program
//    {
//        public async static Task Main(string[] args)
//        //{
//        //    //CreateHostBuilder(args).Build().Run();
//        //    var host = CreateHostBuilder(args).Build();
//        //    await DataHelper.ManageDataAsync(host);
//        //    await SeedDataAsync(host);
//        //    host.Run();
//        //}

//        {
//            var host = CreateHostBuilder(args).Build();
//            await DataHelper.ManageDataAsync(host);
//            using (var scope = host.Services.CreateScope())
//            {
//                var services = scope.ServiceProvider;
//                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
//                try
//                {
//                    var context = services.GetRequiredService<ApplicationDbContext>();
//                    var userManager = services.GetRequiredService<UserManager<BlogUser>>();
//                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

//                    //This is where we seed Roles, Users, and Ticket items
//                    //await ContextSeed.SeedRolesAsync(roleManager);
//                    //await ContextSeed.SeedDefaultUsersAsync(userManager);
//                    //await ContextSeed.SeedDefaultTicketPrioritiesAsync(context);
//                    //await ContextSeed.SeedDefaultTicketStatusesAsync(context);
//                    //await ContextSeed.SeedDefaultTicketTypesAsync(context);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Exception while running Manage Data => {ex}");
//                    var logger = loggerFactory.CreateLogger<Program>();
//                    logger.LogError(ex, "An error occurred seeding the Database.");
//                }
//            }
//            host.Run();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.CaptureStartupErrors(true);
//                    webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
//                    webBuilder.UseStartup<Startup>();
//                });

//        //public async static Task SeedDataAsync(IHost host)
//        //{
//        //    using (var scope = host.Services.CreateScope())
//        //    {
//        //        var services = scope.ServiceProvider;
//        //        try
//        //        {
//        //            var userManager = services.GetRequiredService<UserManager<BlogUser>>();
//        //            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//        //            await SeedHelper.SeedDataAsync(userManager, roleManager);
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            Console.WriteLine(ex);
//        //        }
//        //    }
//        //}
//    }
//}