using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using DamkorkiWebApi.ViewModels;
using DamkorkiWebApi.Helpers; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DamkorkiWebApi.Controllers { 

    [Route("users")]
    public class ApplicationUsersController : BaseController {  

        public ApplicationUsersController(DatabaseContext databaseContext,
                                          UserManager<ApplicationUser> userManager) 
                                          : base(databaseContext, userManager) 
        {

        }

        // GET: /users 
        [Authorize]
        [HttpGet()]
        public IActionResult GetAllApplicationUsers() { 

            List<ApplicationUserViewModel> vmApplicationUsers = _userManager.Users.AsEnumerable().Select(au => new ApplicationUserViewModel(au)).ToList(); 

            return Ok(vmApplicationUsers); 
        }

        // GET: /users/{userId}
        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetApplicationUser(string userId) { 

            if(userId.Equals("me")) { 
                userId = GetCurrentUserId();
            }
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if(user == null) { 
                return NotFound(new { error = String.Format("Application user with id {0} has not been found.", userId) }); 
            } 

            ApplicationUserViewModel vmApplicationUser = new ApplicationUserViewModel(user); 
            return Ok(vmApplicationUser); 
        }

        // POST: /users 
        [HttpPost()]
        public async Task<IActionResult> CreateApplicationUser([FromBody] ApplicationUserViewModel vmApplicationUser) { 

            if(vmApplicationUser == null) 
                return BadRequest(new { error = "No user object in request body."}); 
            if(!ModelState.IsValid)
                return BadRequest(ModelState); 

            try {
                // check if the username already exists 
                ApplicationUser user = await _userManager.FindByNameAsync(vmApplicationUser.UserName);
                if(user != null) 
                    return BadRequest("Username already exists");  

                // check if the email already exists 
                user = await _userManager.FindByEmailAsync(vmApplicationUser.Email); 
                if(user != null) 
                    return BadRequest("Email already exists.");

                var now = DateTime.Now; 

                // create a new Application User entity with data given in view model 
                user = new ApplicationUser { 
                    SecurityStamp = Guid.NewGuid().ToString(), 
                    UserName = vmApplicationUser.UserName, 
                    Email = vmApplicationUser.Email,
                    RegistrationDate = now,  
                    LastModifiedDate = now, 
                };

                // Add the Application User to the DB with the choosen password 
                var identityResult = await _userManager.CreateAsync(user, vmApplicationUser.Password); 
                 
                // Assign the Application User to the 'User' role. 
                await _userManager.AddToRoleAsync(user, "User"); 

                // Remove Lockout and Email confirmation 
                user.EmailConfirmed = false; 
                user.LockoutEnabled = false; 

                // persist the changes into the Database
                _databaseContext.SaveChanges();

                // send confirmation email 
                string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                
                string confirmationUrl = Url.Action("", "confirm-email", new { 
                    userId = user.Id, 
                    confirmationToken = confirmationToken
                }, protocol: HttpContext.Request.Scheme, host: "localhost:5000" );

                var emailResult = EmailHelper.Send(user.Email, "Damkorki.pl - Confirm Email Address", 
                    "Click the link below to confirm your email address: \n" + confirmationUrl); 

                // return the newly-created Application User to the client
                vmApplicationUser = new ApplicationUserViewModel(user); 
                return CreatedAtAction(nameof(GetApplicationUser), new { userId = vmApplicationUser.UserId }, vmApplicationUser);

            } catch(Exception e) 
            { 
                return new ObjectResult(new { error = e.Message });
            } 
        }

        // GET: /users/{userId}/confirm-email
        [AllowAnonymous]
        [HttpGet("{userId}/confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, [FromQuery] string confirmationToken) { 
            
            if(string.IsNullOrEmpty(confirmationToken)) { 
                return BadRequest(new { error = "No confirmation token" }); 
            }

            // get application user by id 
            ApplicationUser user = await _userManager.FindByIdAsync(userId); 
            if(user == null) { 
                return NotFound(new { error = String.Format("Application user with id {0} has not been found.", userId) }); 
            }

            var identityResult = await _userManager.ConfirmEmailAsync(user, confirmationToken);
            if(!identityResult.Succeeded) { 
                return Unauthorized(); 
            }

            return Ok( new { 
                            message = String.Format("Application user with id {0} confirmed.", userId),
                            email = user.Email
                    });
        }

        // POST: /users/reset-password
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> CreatePasswordResetToken([FromForm] string username) { 

            if(string.IsNullOrEmpty(username)) { 
                return BadRequest("There is no username/email address provided in the request."); 
            }

            // check if there's an user with the given username 
            ApplicationUser user = await _userManager.FindByNameAsync(username);
            // fallback to support email address instead of username 
            if(user == null && username.Contains("@")) 
            { 
                user = await _userManager.FindByEmailAsync(username);
            }

            if(user == null) { 
                return NotFound("Could not find user for given username."); 
            }

            try { 
                // send password reset email 
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user); 

                string resetUrl = Url.Action("", "reset-password", new { 
                        userId = user.Id, 
                        resetToken = resetToken
                    }, protocol: HttpContext.Request.Scheme, host: "localhost:5000" );

                var emailResult = EmailHelper.Send(user.Email, "Damkorki.pl - Reset Password", 
                        "Click the link below to reset your password: \n" + resetUrl);

                return Ok(); 

            } catch(Exception e) 
            { 
                return new ObjectResult(new { error = e.Message });
            }
        }

        // PUT: /users/{userId}/reset-password
        [AllowAnonymous]
        [HttpPut("{userId}/reset-password")]
        public async Task<IActionResult> ResetPassword(string userId, [FromBody] ResetPasswordViewModel vmResetPassword) { 

            if(vmResetPassword == null) { 
                return BadRequest(new { error = "No reset password object in request body."}); 
            }
            if(!ModelState.IsValid) { 
                return BadRequest(ModelState); 
            }

            // get application user by id 
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if(user == null) { 
                return NotFound(new { error = String.Format("Application user with id {0} has not been found.", userId) }); 
            }

            var identityResult = await _userManager.ResetPasswordAsync(user, vmResetPassword.ResetToken, vmResetPassword.PasswordNew); 
            if(!identityResult.Succeeded) { 
                return Unauthorized(); 
            }

            return Ok( new {
                        message = String.Format("Password reset for application user with id {0}.", userId),
                        email = user.Email
                    });
        }

        // PUT: /users/{userId}
        [Authorize]
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateApplicationUser(string userId, [FromBody] ApplicationUserViewModel vmApplicationUser) { 

            if(vmApplicationUser == null) 
                return BadRequest(new { error = "No user object in request body."}); 
            if(!ModelState.IsValid)
                return BadRequest(ModelState); 

            try { 
                // retrieve user to update 
                if(userId.Equals("me")) { 
                    userId = GetCurrentUserId();
                }
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                if(user == null) { 
                    return NotFound(new { error = String.Format("Application user with id {0} has not been found.", userId) }); 
                } 

                // validate user password 
                if( ! await _userManager.CheckPasswordAsync(user, vmApplicationUser.Password) ) { 
                    return Unauthorized();
                }

                bool hasChanges = false; 

                if(user.Email != vmApplicationUser.Email) { 
                    // is email address free? 
                    ApplicationUser emailUser = await _userManager.FindByEmailAsync(vmApplicationUser.Email);
                    if(emailUser != null) { 
                        return BadRequest(new { error = "New email address is not free. It is used by another user."}); 
                    }
                    await _userManager.SetEmailAsync(user, vmApplicationUser.Email);
                    hasChanges = true;  
                }

                if(!string.IsNullOrEmpty(vmApplicationUser.PasswordNew)) { 
                    // change existing password to new one 
                    await _userManager.ChangePasswordAsync(user, vmApplicationUser.Password, vmApplicationUser.PasswordNew);
                    hasChanges = true; 
                }

                if(user.UserName != vmApplicationUser.UserName) { 
                    user.UserName = vmApplicationUser.UserName; 
                    hasChanges = true; 
                }

                if(hasChanges) { 
                    // if there was some changes then update LastModifiedDate 
                    user.LastModifiedDate = DateTime.Now; 
                    // persist changes in database 
                    _databaseContext.SaveChanges(); 
                }

                // return the newly-updated Application User to the client
                vmApplicationUser = new ApplicationUserViewModel(user); 
                return Ok(vmApplicationUser);
            
            } catch(Exception e) 
            { 
                 return new ObjectResult(new { error = e.Message });
            }
        }
    }

}