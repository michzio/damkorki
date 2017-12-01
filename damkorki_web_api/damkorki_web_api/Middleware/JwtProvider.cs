using Microsoft.IdentityModel.Tokens; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Text;
using System;
using DamkorkiWebApi.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Builder;

namespace DamkorkiWebApi.Middleware
{
   public class JwtProvider
   {
       #region static members
       private static readonly string PrivateKey = "private_key_1234567890";
       public static readonly SymmetricSecurityKey SecurityKey = 
                                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PrivateKey));
       public static readonly string Issuer = "DamkorkiWebApi";
       public static string TokenEndPoint = "/jwt/token";
       #endregion

       #region private members 
       private readonly RequestDelegate _next;
       // JWT-related members 
       private TimeSpan TokenExpiration; 
       private SigningCredentials SigningCredentials; 
       // EF and Identity members, available through DI 
       private DatabaseContext DbContext; 
       private UserManager<ApplicationUser> UserManager; 
       private SignInManager<ApplicationUser> SignInManager;  
       #endregion

       #region constructor 
       public JwtProvider(RequestDelegate next, 
                          DatabaseContext dbContext,
                          UserManager<ApplicationUser> userManager, 
                          SignInManager<ApplicationUser> signInManager) 
       {
           _next = next;

           // Instantiate JWT-related members 
           TokenExpiration = TimeSpan.FromMinutes(10);
           SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

           // Instantiate through Dependency Injection 
           DbContext = dbContext; 
           UserManager = userManager; 
           SignInManager = signInManager; 
       }
       #endregion

       #region public methods  
       public Task Invoke(HttpContext context)
       {
            // Check if the request path matches our TokenEndPoint
            if(!context.Request.Path.Equals(TokenEndPoint, StringComparison.Ordinal))
                // Call the next delegate/middleware in the pipeline 
                return this._next(context);

            // Check if the current request is a valid POST with the appropriate 
            // content type (application/x-www-form-urlencoded)
            if(context.Request.Method.Equals("POST") && context.Request.HasFormContentType) {
                
                // OK: generate JWT token and send it via json-formatted string 
                return CreateToken(context);
            } else { 
                // Not OK: output a 400 - Bad request HTTP error 
                context.Response.StatusCode = 400; 
                return context.Response.WriteAsync("Bad request.");
            }
       }
       #endregion

       #region private methods
       private async Task CreateToken(HttpContext context) { 

           try 
           { 
               // retrieve the relevent FORM data 
               string username = context.Request.Form["username"]; 
               string password = context.Request.Form["password"];

               var user = await UserManager.FindByNameAsync(username); 
               // fallback to support email address instead of username 
               if(user == null && username.Contains("@")) 
               { 
                   user = await UserManager.FindByEmailAsync(username);
               }

               var success = user != null && await UserManager.CheckPasswordAsync(user, password);
               if(success)
               {
                   DateTime now = DateTime.UtcNow; 

                   // add the registered calims for JWT (RFC7519)
                   // https://tools.ietf.org/html/rfc7519#section-4.1
                   var claims = new[] { 
                        new Claim(JwtRegisteredClaimNames.Iss, Issuer), 
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                        new Claim(JwtRegisteredClaimNames.Iat, 
                                new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                        // TODO: add additional claims here  
                   };

                   // Create the JWT and write it to a string 
                   var token = new JwtSecurityToken(
                       claims: claims, 
                       notBefore: now, 
                       expires: now.Add(TokenExpiration), 
                       signingCredentials: SigningCredentials
                   );
                   var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

                   // build the json response 
                   var jwt = new { 
                        access_token = encodedToken , 
                        expiration = (int) TokenExpiration.TotalSeconds

                   };

                   // return token 
                   context.Response.ContentType = "application/json"; 
                   await context.Response.WriteAsync(JsonConvert.SerializeObject(jwt));

                   return; 
               }
           } catch(Exception ex) 
           {
               // TODO: handle errors 
               throw ex; 
           }

           context.Response.StatusCode = 400; 
           await context.Response.WriteAsync("Invalid username and password.");
       }
       #endregion 
   }
    
    #region extension methods 
    // Extension method used to add the middleware to the HTTP request pipeline
    public static class JwtProviderExtensions
    {
        public static IApplicationBuilder UseJwtProvider(this IApplicationBuilder builder) 
        { 
            return builder.UseMiddleware<JwtProvider>(); 
        }
    }
    #endregion
}