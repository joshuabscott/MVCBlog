using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
using MVCBlog.Utilities;

namespace MVCBlog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; /*set;*/ }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseNpgsql(GetConnectionString()));
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<BlogUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        ////Add theGetConnectionString method
        //private string GetConnectionString()
        //{
        //    var config = new PostgreSqlConnection();
        //    var dbUrl = Configuration["DATABASE_URL"];
        //    if(string.IsNullOrEmpty(dbUrl))
        //    {
        //        Configuration.Blind("PostgreSQL", config);
        //    }
        //    else
        //    {
        //        var dbUrlData = dbUrl.Split(":");
        //        config.Server = dbUrlData[2].Split("@")[1];
        //        config.Port = dbUrlData[3].Split("/")[0];
        //        config.Database = dbUrlData[3].Split("/")[1];
        //        config.UserId = dbUrlData[1].TrimStart('/');
        //        config.Password = dbUrlData[2].Split("@")[0];
        //    }
        //    string connString =
        //        $"Server={config.Server}; Port={config.Port}; Database={config.Database}; User Id={config.UserId}; Password={config.Password}";

        //    return connString;
        //}



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}