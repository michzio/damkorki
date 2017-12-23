using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DamkorkiWebApi.Configuration;
using DamkorkiWebApi.Models;
using DamkorkiWebApi.Repositories;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DamkorkiWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args); 

            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                // Seed the database if needed
                try { 

                    new SampleDataSeeder(
                            services.GetService<IUnitOfWork>(),
                            services.GetService<RoleManager<IdentityRole>>(),
                            services.GetService<UserManager<ApplicationUser>>()
                        ).SeedSampleDataAsync().Wait(); 
                
                } catch(Exception ex) { 

                    var logger = services.GetRequiredService<ILogger<Program>>(); 
                    logger.LogError(ex, "An error occurred seeding the DB.");  
                } 
            }

            host.Run(); 
        }

        public static IWebHost BuildWebHost(string[] args) {
            return WebHost.CreateDefaultBuilder(args)
                        .UseStartup<Startup>()
                        .UseUrls("http://0.0.0.0:5050")
                        .ConfigureAppConfiguration((hostContext, config) => 
                        { 
                            // delete all default configuration providers 
                            config.Sources.Clear(); 
                            config.AddJsonFile("appsettings.json", optional: true); 
                        })
                        .Build();
        }
    }
}
