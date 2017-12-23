using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DamkorkiWebApi.Middleware;
using DamkorkiWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using static DamkorkiWebApi.Models.Person;

namespace DamkorkiWebApi.Controllers { 

    [Route("auth")]
    public class AuthController : Controller { 

        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private DatabaseContext _dbContext; 

        private IConfiguration _configuration; 

        public AuthController(SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager,
                              DatabaseContext dbContext, 
                              IConfiguration configuration)
        { 
            _signInManager = signInManager;
            _userManager = userManager; 
            _dbContext = dbContext;
            _configuration = configuration; 
        }

       [HttpPost("logout")]
       public IActionResult ExternalLogout() { 
           
           if(HttpContext.User.Identity.IsAuthenticated)
           {
               _signInManager.SignOutAsync().Wait(); 
           }

           return Ok(new { logout = "Success" }); 
       }

        [HttpGet("{provider}")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null) 
        { 
                switch(provider) { 
                    case "facebook":
                    case "google":
                        // capitalize provider name e.g. facebook => Facebook
                        provider = provider.First().ToString().ToUpper() + provider.Substring(1);
                        // redirect the request to the external provider
                        var redirectUrl = Url.Action(
                            nameof(ExternalLoginCallback),
                            "auth", 
                            new { returnUrl }
                        );
                        var properties = 
                            _signInManager.ConfigureExternalAuthenticationProperties(
                                provider,
                                redirectUrl
                            );
                        return Challenge(properties, provider); 
                    default: 
                        // provider not supported 
                        return BadRequest(new { error = String.Format("Provider '{0}' is not supported.", provider) });
                }
        }

        [HttpGet("callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, 
                                                               string remoteError = null)
        {
            if(!String.IsNullOrEmpty(remoteError)) 
            { 
                // TODO: handle external login errors 
                throw new Exception(String.Format("External login error: {0}", remoteError));
            }

            // extract external login info 
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if(loginInfo == null) 
            { 
                // if there's no login info then emit an error 
                throw new Exception("External login error: no login info.");
            } 

            // check if user has already registered using this external provider
            var user = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, 
                                                           loginInfo.ProviderKey);
            if(user == null) 
            {
                IdentityResult identityResult; 

                // user has never tried to log in using this external provider
                // it could have used other providers or local account 
                // check this by looking for email address
                var emailKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
                var email = loginInfo.Principal.FindFirstValue(emailKey); 
                if(email == null) 
                { 
                    // if there's no email then emit an error 
                    throw new Exception("External login error: no email in login info.");
                } 

                // look up user in database by this email address 
                user = await _userManager.FindByEmailAsync(email); 
                if(user == null)
                {
                    // no user has been found 
                    // register a new user 
                    DateTime now = DateTime.Now; 

                    // create unique username 
                    var identifierKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
                    var identifier = loginInfo.Principal.FindFirstValue(identifierKey);
                    if(identifier == null) 
                    { 
                        // if there's no identifier then emit an error 
                        throw new Exception("External login error: no identifier in login info.");
                    } 
                    
                    var username = String.Format("{0}{1}{2}", loginInfo.LoginProvider, 
                                                 identifier, 
                                                 Guid.NewGuid().ToString("N")); 

                    user = new ApplicationUser()
                    {
                        SecurityStamp = Guid.NewGuid().ToString(), 
                        UserName = username, 
                        Email = email, 
                        RegistrationDate = now, 
                        LastModifiedDate = now
                    };

                    // add the user to database with random password 
                    var password = RandomPassword(); 
                    identityResult = await _userManager.CreateAsync(user, password);

                    // assign the user to 'User' role
                    await _userManager.AddToRoleAsync(user, "User"); 

                    // remove lockout and email confirmation 
                    user.EmailConfirmed = true;
                    user.LockoutEnabled = false; 

                    var firstNameKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
                    var firstName = loginInfo.Principal.FindFirstValue(firstNameKey); 
                    var lastNameKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
                    var lastName = loginInfo.Principal.FindFirstValue(lastNameKey);
                    var genderKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender";
                    var gender = loginInfo.Principal.FindFirstValue(genderKey);
                    var genderType =  (gender != null && gender.ToLower() == "female") ? GenderType.Female : GenderType.Male;

                    // add Person profile entity 
                     var newPerson = new Person { 
                        FirstName = firstName, 
                        LastName = lastName, 
                        Gender = genderType,
                        UserId = user.Id 
                    }; 
                    await _dbContext.Set<Person>().AddAsync(newPerson); 

                    // persist in database 
                    await _dbContext.SaveChangesAsync(); 
                }
                // connect this user with external provider login info
                identityResult = await _userManager.AddLoginAsync(user, loginInfo);
                if(identityResult.Succeeded)
                {
                    // persist in database 
                    _dbContext.SaveChanges(); 
                } else { 
                    throw new Exception("Error connecting user with external provider login info");
                }
            } 

            // create refresh token 
            var refreshToken = CreateRefreshToken("DamkorkiApp", user.Id);  

            // add the refresh token to DB
            _dbContext.RefreshTokens.Add(refreshToken); 

            // persist changes
            _dbContext.SaveChanges(); 

            // create access token
            var identity = await GetIdentityAsync(user); 
            var accessToken = CreateAccessToken(identity);

            // build the json response 
            var tokenExpiration = TimeSpan.FromMinutes(int.Parse(_configuration["Auth:Jwt:TokenExpiration"]));
            var response = new { 
                access_token = accessToken, 
                expiration = (int) tokenExpiration.TotalSeconds, 
                refresh_token = refreshToken.TokenValue, 
            }; 

            var serializedResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }); 

            // output <script> tag to call JS function 
            return Content(
                "<script type=\"text/javascript\">" + 
                //"window.opener.externalLoginCallback(" + 
                "window.opener.postMessage(" +
                    serializedResponse + 
                ", 'http://localhost:5000/');" + 
                "window.close();" + 
                "</script>", 
                "text/html"
            );
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

        private string CreateAccessToken(ClaimsIdentity identity) { 

            DateTime now = DateTime.UtcNow; 

            // add the registered claims for JWT (RFC7519)
            // https://tools.ietf.org/html/rfc7519#section-4.1
            var claims = new Claim[]
            { 
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["Auth:Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["Auth:Jwt:Audience"]), 
                new Claim(JwtRegisteredClaimNames.Sub, identity.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                new Claim(JwtRegisteredClaimNames.Iat, 
                                new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                // TODO: add additional claims here 
            };

            identity.AddClaims(claims);

            var tokenExpiration = TimeSpan.FromMinutes(int.Parse(_configuration["Auth:Jwt:TokenExpiration"]));
            var signingCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:Jwt:SecretKey"])), 
                            SecurityAlgorithms.HmacSha256);

            // Create the JWT and write it to a string 
            var token = new JwtSecurityToken(
                    claims: identity.Claims, 
                    notBefore: now, 
                    expires: now.Add(tokenExpiration), 
                    signingCredentials:signingCredentials
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken; 
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


        private string RandomPassword(PasswordOptions options = null) { 

            if(options == null) {
                options = new PasswordOptions { 
                    RequireNonAlphanumeric = true,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true, 
                    RequiredLength = 7,
                    RequiredUniqueChars = 4,
                };
            }

            string uppercases = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lowercases = "abcdefghijklmnopqrstuvwxyz";
            string digits = "0123456789";
            string specialChars = "!@#$%^&*?_-";

            // create random generator 
            Random random = new Random(Environment.TickCount); 
            List<char> chars = new List<char>(); 

            if(options.RequireUppercase) { 
                // generate uppercase 
                chars.Insert(random.Next(0, chars.Count), // random position
                             uppercases[random.Next(0, uppercases.Length)]); // random uppercase
            }

            if(options.RequireLowercase) { 
                // generate lowercase
                chars.Insert(random.Next(0, chars.Count), // random position 
                             lowercases[random.Next(0, lowercases.Length)]); // random lowercase  
            }

            if(options.RequireDigit) { 
                // generate digit 
                chars.Insert(random.Next(0, chars.Count), // random position
                            digits[random.Next(0, digits.Length)]); // random digit  
            }

           if(options.RequireNonAlphanumeric) { 
               // generate special char (non-alphanumeric)
               chars.Insert(random.Next(0, chars.Count), // random position
                            specialChars[random.Next(0, specialChars.Length)]); // random special char
            }

            string[] charSources = { uppercases, lowercases, digits, specialChars };

            // generate minimum length and unique chars count 
            for(int i = chars.Count;    
                i < options.RequiredLength || chars.Distinct().Count() < options.RequiredUniqueChars; 
                i++) { 
                    
                    // select char source i.e. upper, lower, digit, special 
                    string charSource = charSources[random.Next(0, charSources.Length)];

                    chars.Insert(random.Next(0, chars.Count),  // random position
                                 charSource[random.Next(0, charSource.Length)]); // random char from selected source     
            }

            return new string(chars.ToArray());
        }
    }
}