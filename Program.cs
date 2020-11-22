using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVCBlog.Models;
using MVCBlog.Services;
using MVCBlog.Utilities;

namespace MVCBlog
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();
            await DataHelper.ManageDataAsync(host);
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

        //public async static Task SeedDataAsync(IHost host)
        //{
        //    using (var scope = host.Services.CreateScope())
        //    {
        //        var services = scope.ServiceProvider;
        //        try
        //        {
        //            var userManager = services.GetRequiredService<UserManager<BlogUser>>();
        //            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        //            await SeedHelper.SeedDataAsync(userManager, roleManager);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex);
        //        }
        //    }
        //}
    }
}