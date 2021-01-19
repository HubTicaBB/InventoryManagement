using InventoryAPI.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            SetupDb(host);
            host.Run();
        }

        private static void SetupDb(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            try
            {
                var db = serviceProvider.GetRequiredService<InventoryDbContext>();
                if (db.Database.IsSqlServer())
                {
                    db.Database.Migrate();
                    DbInitializer.Seed(db);
                }
            }
            catch (Exception exception)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError("An error occured while migrating or seeding the database: ", exception);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
