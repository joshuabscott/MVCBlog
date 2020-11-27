using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using MVCBlog.Data;
using MVCBlog.Models;
using MVCBlog.ViewModels;
using MVCBlog.Utilities;
using MVCBlog.Enums;

namespace MVCBlog.Utilities
{
    public static class DataHelper
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            // the default connection string will come from appsettings.json like usual
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            // It will be automatically overwritten if we are running on Heroku
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        public static string BuildConnectionString(string databaseUrl)
        {
            // Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            //Provides a simple way to create and manage the contents of connection strings used by the NpgsqlConnection class.
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/')
            };
            return builder.ToString();
        }

        public static async Task ManageData(IHost host)
        {
            try
            {
                //This technique is used to obtain references to services
                // normally I would just inject these services but you cant use a constructor in a static class
                using var svcScope = host.Services.CreateScope();
                var svcProvider = svcScope.ServiceProvider;

                //The service will run your migrations
                var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
                await dbContextSvc.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while running Manage Data => {ex}");
            }
        }
    }
}


//using Npgsql;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.EntityFrameworkCore;
//using MVCBlog.Data;
//using MVCBlog.Models;
//using MVCBlog.ViewModels;
//using MVCBlog.Utilities;
//using MVCBlog.Services;
//using MVCBlog.Enums;

//namespace MVCBlog.Services
//{
//    public class DataHelper
//    {
//        //The default connection string will come from app settings like usual
//        public static string GetConnectionString(IConfiguration configuration)
//        {
//            //It will be automatically overwritten if we are running on Heroku
//            var connnectionString = configuration.GetConnectionString("DefaultConnection");

//            var herokuDatabaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
//            return string.IsNullOrEmpty(herokuDatabaseUrl) ? connnectionString : BuildConnectionString(herokuDatabaseUrl);
//        }

//        public static string BuildConnectionString(string herokuDataBaseUrl)
//        {
//            //Provide an object representation of a uniform resource identifier URI
//            var herokuDatabaseUri = new Uri(herokuDataBaseUrl);
//            var userInfo = herokuDatabaseUri.UserInfo.Split(":");
//            //Provides a simple way to create and manage the contents of connection strings used by the NpgsqlConnection class.
//            var builder = new NpgsqlConnectionStringBuilder
//            {
//                Host = herokuDatabaseUri.Host,
//                Port = herokuDatabaseUri.Port,
//                Username = userInfo[0],
//                Password = userInfo[1],
//                Database = herokuDatabaseUri.LocalPath.TrimStart('/')
//            };
//            return builder.ToString();
//        }

//        public static async Task ManageDataAsync(IHost host)
//        {
//            try
//            {
//                using var svcScope = host.Services.CreateScope();
//                var svcProvider = svcScope.ServiceProvider;

//                var context = svcProvider.GetRequiredService<ApplicationDbContext>();
//                await context.Database.MigrateAsync();
//            }
//            catch (PostgresException ex)
//            {
//                Console.WriteLine($"Something went wrong: {ex}");
//            }
//        }
//    }
//}