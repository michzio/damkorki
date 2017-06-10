using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DamkorkiWebApi.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DamkorkiWebApi.Repositories;
using Microsoft.Extensions.Configuration;
using DamkorkiWebApi.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;

namespace DamkorkiWebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; } 

        public Startup(IHostingEnvironment env) { 

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();     
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFramework();

            // Add Identity Services & Stores
            services.AddIdentity<ApplicationUser, IdentityRole>(
                config =>
                {
                    config.User.RequireUniqueEmail = true;
                    config.Password.RequireNonAlphanumeric = true;
                    config.Cookies.ApplicationCookie.AutomaticChallenge = false;
                })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();


            // Add Database Context 
            services.AddDbContext<DatabaseContext>( options => 
                    options.UseSqlServer(Configuration.GetConnectionString("MacOsSQLServerDatabase")) );

            services.AddMvc()
                    .AddXmlDataContractSerializerFormatters()
                    .AddJsonOptions(options => {
                            options.SerializerSettings.ReferenceLoopHandling = 
                                                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });
            services.Configure<MvcOptions>(options =>
            {
                    options.Filters.Add(new CorsAuthorizationFilterFactory("AllowClientOrigin"));
            });
        
            
            // Add Cross Origin Resource Sharing support 
            services.AddCors(
                options => options.AddPolicy("AllowClientOrigin", 
                                   builder => { 
                                               builder.WithOrigins("http://localhost:5000", "http://0.0.0.0:5000")
                                                    //.AllowAnyOrigin()
                                                      .AllowAnyMethod()
                                                    // .WithHeaders("accept", "content-type", "origin", "x-custom-header");
                                                      .AllowAnyHeader();
                                                     //.WithExposedHeaders("x-custom-header");
                                                     //.AllowCredentials();
                                    }
            ));       
		
            // registration custom objects for dependency injection
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
	        
                loggerFactory.AddConsole();
		        loggerFactory.AddDebug();

		        if (env.IsDevelopment())
		        {
		          app.UseDeveloperExceptionPage();
		        }

                // configuring Cross Origin Resource Sharing policy 
                // app.UseCors( "AllowClientOrigin" ); 

	            app.UseIdentity();

                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
            
                // Seed the database if needed
                try { 

                    new UserRolesSeeder(app.ApplicationServices.GetService<RoleManager<IdentityRole>>()).SeedRolesAsync().Wait();
                    new SampleDataSeeder(app.ApplicationServices.GetService<IUnitOfWork>()).SeedSampleDataAsync().Wait(); 
                
                } catch(AggregateException e) { 
                    throw new Exception(e.ToString()); 
                }
        }
    }
}
