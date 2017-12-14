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
using DamkorkiWebApi.Middleware;
using Microsoft.IdentityModel.Tokens; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

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
                    config.Password.RequireDigit = true;
                    config.Password.RequireLowercase = true;
                    config.Password.RequireUppercase = true; 
                    config.Password.RequiredLength = 7;
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

                // configure a rewrite rule to auto-lookup 
                // for standard default files such as index.html 
                app.UseDefaultFiles(); 

                // serve static files (html, css, js, images). 
                // see also the following URL for further reference:
                // https://docs.asp.net/en/latest/fundamentals/static-files.html 
                app.UseStaticFiles(new StaticFileOptions() { 
                    OnPrepareResponse = (context) => { 
                        // disable caching for all static files 
                        context.Context.Response.Headers["Cache-Control"] = 
                            Configuration["StaticFiles:Headers:Cache-Control"];
                        context.Context.Response.Headers["Pragma"] = 
                            Configuration["StaticFiles:Headers:Pragma"];
                        context.Context.Response.Headers["Expires"] = 
                            Configuration["StaticFiles:Headers:Expires"];
                    } 
                });

	            // purposely avoided cookies authentication
                // app.UseIdentity();

                // add a custom Jwt Provider to generate Tokens
                app.UseJwtProvider(new JwtProviderOptions() { 
                    Path = Configuration["Auth:Jwt:TokenEndPoint"], 
                    Issuer = Configuration["Auth:Jwt:Issuer"], 
                    Audience = Configuration["Auth:Jwt:Audience"], 
                    TokenExpiration = TimeSpan.FromMinutes(int.Parse(Configuration["Auth:Jwt:TokenExpiration"])),
                    SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:SecretKey"])), 
                            SecurityAlgorithms.HmacSha256)
                });

                // add the Jwt Bearer Header Authentication to validate Tokens
                app.UseJwtBearerAuthentication(new JwtBearerOptions() { 
                    AutomaticAuthenticate = true, 
                    AutomaticChallenge = true, 
                    RequireHttpsMetadata = false, 
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        // the signing key must match
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:SecretKey"])),
                        ValidateIssuerSigningKey = true,

                        // validate the JWT Issuer (iss) claim
                        ValidIssuer = Configuration["Auth:Jwt:Issuer"], 
                        ValidateIssuer = true, 
                        
                        // validate the JWT Audience (aud) claim
                        ValidAudience = Configuration["Auth:Jwt:Audience"],
                        ValidateAudience = true, 

                        // validate the token expiry 
                        ValidateLifetime = true,
                        RequireExpirationTime = true,  

                        // if you want to allow a certain amount of clock drift, set that here: 
                        ClockSkew = TimeSpan.Zero
                    }
                });

                // add MVC to the pipeline
                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
            
                // Seed the database if needed
                try { 

                    new SampleDataSeeder(
                            app.ApplicationServices.GetService<IUnitOfWork>(),
                            app.ApplicationServices.GetService<RoleManager<IdentityRole>>(),
                            app.ApplicationServices.GetService<UserManager<ApplicationUser>>()
                        ).SeedSampleDataAsync().Wait(); 
                
                } catch(AggregateException e) { 
                    throw new Exception(e.ToString()); 
                }
        }
    }
}
