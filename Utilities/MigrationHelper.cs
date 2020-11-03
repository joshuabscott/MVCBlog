using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVCBlog.Data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MVCBlog.Utilities
{
    public static class MigrationHelper
    {
        public static IHost MigrateDataBase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var pendingMigragtions = context.Database.GetPendingMigrations().ToList();
            if (pendingMigragtions.Count > 0)
            {
                var migrator = context.Database.GetService<IMigratior>();
                foreach (var targetMigration in pendingMigragtions)
                {
                    migrator.Migrate(targetMigration);
                }
            }
        }
        CatchBlock (PostgresException ex)
        {
            Console.WriteLine(ex);
        }
        return host;
    }
}
