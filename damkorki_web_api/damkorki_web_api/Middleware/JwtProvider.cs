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
using Microsoft.Extensions.Options;
using System.Security.Principal;
using System.Linq;

namespace DamkorkiWebApi.Middleware
{
   public class JwtProvider
   {

       #region private members 
       private readonly RequestDelegate _next;
       // JWT-related options 
       private readonly JwtProviderOptions _options;  
       // EF and Identity members, available through DI 
       private DatabaseContext _dbContext; 
       private UserManager<ApplicationUser> _userManager; 
       private SignInManager<ApplicationUser> _signInManager;  
       #endregion

       #region constructor 
       public JwtProvider(RequestDelegate next,
                          IOptions<JwtProviderOptions> options,
                          DatabaseContext dbContext,
                          UserManager<ApplicationUser> userManager, 
                          SignInManager<ApplicationUser> signInManager) 
       {
           _next = next;
           _options = options.Value; 

           // Instantiate through Dependency Injection 
           _dbContext = dbContext; 
           _userManager = userManager; 
           _signInManager = signInManager; 
       }
       #endregion

       #region public methods  
       public Task Invoke(HttpContext context)
       {
            // Check if the request path matches our options path
            if(!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
                // Call the next delegate/middleware in the pipeline 
                return this._next(context);

            // Enable CORS authentication requests 
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Access-Control-Allow-Credentials", "true"); 
            context.Response.Headers.Add("Access-Control-Allow-Methods", "POST");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Lenght, Content-MD5, Date, X-API-Version, X-File-Name, Authorization"); 
            context.Response.Headers.Add("Cache-Control", "no-cache");
            context.Response.Headers.Add("Access-Control-Max-Age", "1728000"); 

             if(context.Request.Method.Equals("OPTIONS")) { 
                return context.Response.WriteAsync(""); 
            }   

            // Check if the current request is a valid POST with the appropriate 
            // content type (application/x-www-form-urlencoded)
            if(!context.Request.Method.Equals("POST") || !context.Request.HasFormContentType) {
                
                // Not OK: 400 - HTTP Bad Request error 
                context.Response.StatusCode = 400; 
                return context.Response.WriteAsync("Bad request.");
            } 

            // OK: generate JWT token and send it via json-formatted string 
            return HandleTokenRequest(context);
       }
       #endregion

       #region private methods
       private async Task HandleTokenRequest(HttpContext context) { 

           var grantType = context.Request.Form["grant_type"]; 
           switch(grantType) { 
                case "password": 
                    // validate credentials and return access/refresh tokens
                    await GetToken(context);
                    return; 
                case "refresh_token":
                    await RefreshToken(context); 
                    return; 
                default: 
                    // Not OK: 400 - HTTP Bad Request error 
                    context.Response.StatusCode = 400; 
                    await context.Response.WriteAsync("Bad request."); 
                    return; 
           }
       } 

       private async Task GetToken(HttpContext context) { 

            // retrieve the relevent FORM data 
            string username = context.Request.Form["username"]; 
            string password = context.Request.Form["password"];
            string clientId = context.Request.Form["client_id"]; 
            string clientSecret = context.Request.Form["client_secret"]; 

            var identity = await GetIdentityAsync(username, password); 
            if(identity == null)
            {
                context.Response.StatusCode = 401; 
                await context.Response.WriteAsync("Invalid username and password.");
                return; 
            }

            // create access token
            var accessToken = CreateAccessToken(identity); 
            
            // create refresh token
            var refreshToken = CreateRefreshToken(clientId, identity.FindFirst("uid").Value); 

            // add the refresh token to DB
            _dbContext.RefreshTokens.Add(refreshToken); 

            // persist changes
            _dbContext.SaveChanges(); 

            await TokenResponse(context, accessToken, refreshToken.TokenValue);
       }

       private string CreateAccessToken(ClaimsIdentity identity) { 

            DateTime now = DateTime.UtcNow; 

            // add the registered claims for JWT (RFC7519)
            // https://tools.ietf.org/html/rfc7519#section-4.1
            var claims = new Claim[]
            { 
                new Claim(JwtRegisteredClaimNames.Iss, _options.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, _options.Audience), 
                new Claim(JwtRegisteredClaimNames.Sub, identity.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                new Claim(JwtRegisteredClaimNames.Iat, 
                                new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                // TODO: add additional claims here 
            };

            identity.AddClaims(claims);

            // Create the JWT and write it to a string 
            var token = new JwtSecurityToken(
                    claims: identity.Claims, 
                    notBefore: now, 
                    expires: now.Add(_options.TokenExpiration), 
                    signingCredentials: _options.SigningCredentials
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken; 
       }

       private async Task<ClaimsIdentity> GetIdentityAsync(string userId) { 

            // check if there's an user with the given userId 
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null) { 
                return await GetIdentityAsync(user); 
            }
            return null; 
       }    

       private async Task<ClaimsIdentity> GetIdentityAsync(string username, string password) { 

            // check if there's an user with the given username 
            var user = await _userManager.FindByNameAsync(username); 
            // fallback to support email address instead of username 
            if(user == null && username.Contains("@")) 
            { 
                   user = await _userManager.FindByEmailAsync(username);
            }

            var success = user != null && await _userManager.CheckPasswordAsync(user, password);
            if(success) { 
                return await GetIdentityAsync(user); 
            }
            return null; 
       }

       private async Task<ClaimsIdentity> GetIdentityAsync(ApplicationUser user) { 
            
            // get user roles
            var roles = String.Join(",", await _userManager.GetRolesAsync(user));
            
            return new ClaimsIdentity(new GenericIdentity(user.UserName, "Token"), 
                                       new Claim[] { 
                                           new Claim(JwtRegisteredClaimNames.Email, user.Email), 
                                           new Claim("uid", user.Id), 
                                           new Claim("roles", roles)
                                        });
       }

       private async Task RefreshToken(HttpContext context) { 

           // check if the received refresh_token exists for the given client_id
           string refreshTokenValue = context.Request.Form["refresh_token"];
           string clientId = context.Request.Form["client_id"];

           var refreshToken = _dbContext.RefreshTokens
                    .FirstOrDefault(rt => rt.ClientId == clientId && rt.TokenValue == refreshTokenValue);  

            if( refreshToken == null) { 
                // refresh token not found or invalid 
                context.Response.StatusCode = 401; 
                await context.Response.WriteAsync("Invalid refresh token.");
            }

            // check if there is an User with the RefreshToken's userId 
            var identity = await GetIdentityAsync(refreshToken.UserId); 
            if(identity == null) { 
                // user not found 
                context.Response.StatusCode = 401; 
                await context.Response.WriteAsync("User not found for refresh token."); 
            }

            // create access token
            var accessToken = CreateAccessToken(identity); 

            // generate a new refresh token 
            var newRefreshToken = CreateRefreshToken(refreshToken.ClientId, refreshToken.UserId); 

            // invalidate the old refresh token 
            _dbContext.RefreshTokens.Remove(refreshToken); 

            // add the new refresh token to DB
            _dbContext.RefreshTokens.Add(newRefreshToken); 

            // persist changes
            _dbContext.SaveChanges(); 

            // create token response and send it to the client 
            await TokenResponse(context, accessToken, newRefreshToken.TokenValue);
       }

       private RefreshToken CreateRefreshToken(string clientId, string userId) 
       { 
           return new RefreshToken() { 
               ClientId = clientId, 
               TokenValue = Guid.NewGuid().ToString("N"), 
               CreatedDate = DateTime.UtcNow, 
               UserId = userId
           };
       }

       private async Task TokenResponse(HttpContext context, string accessToken, string refreshToken) { 

           // build the json response 
            var response = new { 
                access_token = accessToken, 
                expiration = (int) _options.TokenExpiration.TotalSeconds, 
                refresh_token = refreshToken, 
            };

            // return serialized json response  
            context.Response.ContentType = "application/json"; 
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));

       }
       #endregion 
   }
    
    #region extension methods 
    // Extension method used to add the middleware to the HTTP request pipeline
    public static class JwtProviderExtensions
    {
        public static IApplicationBuilder UseJwtProvider(this IApplicationBuilder builder, JwtProviderOptions options) 
        { 
            return builder.UseMiddleware<JwtProvider>(Options.Create(options)); 
        }
    }
    #endregion

    public class JwtProviderOptions
    {
        public string Path { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan TokenExpiration { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}