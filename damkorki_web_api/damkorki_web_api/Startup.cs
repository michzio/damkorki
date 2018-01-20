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

        public Startup(IConfiguration configuration) { 

            Configuration = configuration;     
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add EntityFramework support for SqlServer
            services.AddEntityFrameworkSqlServer();

            // Add Identity Services & Stores
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = true; 
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true; 
                    options.Password.RequiredLength = 7;
                })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            // Add Authentication
            services.AddAuthentication(options => 
                {  
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
                })
                .AddJwtBearer(options => 
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true; 
                    options.TokenValidationParameters = new TokenValidationParameters()
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

                        // validate the token expiration time  
                        ValidateLifetime = true,
                        RequireExpirationTime = true, 

                         // if you want to allow a certain amount of clock drift, set that here: 
                        ClockSkew = TimeSpan.Zero
                    };
                    options.IncludeErrorDetails = true;     
                })
                .AddFacebook(options => 
                {
                    options.AppId = Configuration["Auth:Facebook:AppId"];
                    options.AppSecret = Configuration["Auth:Facebook:AppSecret"];
                    options.Scope.Add("public_profile");
                    options.Scope.Add("email");
                    options.Fields.Add("email");
                    options.Fields.Add("id");
                    options.Fields.Add("name");
                    options.Fields.Add("first_name"); 
                    options.Fields.Add("last_name");  
                    options.Fields.Add("gender"); 
                })
                .AddGoogle(options => 
                {
                    options.ClientId = Configuration["Auth:Google:Clientid"]; 
                    options.ClientSecret = Configuration["Auth:Google:ClientSecret"]; 
                    options.CallbackPath = "/signin-google";
                    options.Scope.Add("profile"); 
                });

            // Add Database Context 
            services.AddDbContext<DatabaseContext>(options => 
                    //options.UseSqlServer(Configuration.GetConnectionString("WindowsLocalSQLServerDatabase")) );
                    //options.UseSqlServer(Configuration.GetConnectionString("MacOsSQLServerDatabase")) );
                    options.UseSqlServer(Configuration.GetConnectionString("AzureSQLServerDatabase")) );

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
                                               builder.WithOrigins("http://localhost:5000", 
                                                                   "http://0.0.0.0:5000",
                                                                   "https://damkorki.azurewebsites.net")
                                                    //.AllowAnyOrigin()
                                                      .AllowAnyMethod()
                                                    // .WithHeaders("accept", "content-type", "origin", "x-custom-header");
                                                      .AllowAnyHeader();
                                                     //.WithExposedHeaders("x-custom-header");
                                                     //.AllowCredentials();
                                    }
            ));       
		
            // registration of custom objects for dependency injection
            services.AddScoped<IUnitOfWork, UnitOfWork>();
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
                else 
                { 
                    app.UseExceptionHandler("/Home/Error"); 
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

                // add the Jwt Bearer Authentication to validate Tokens
                app.UseAuthentication(); 

                // add MVC to the pipeline
                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
