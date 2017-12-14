using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using DamkorkiWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DamkorkiWebApi.Controllers { 

    [Route("users")]
    public class ApplicationUsersController : Controller { 

        private UserManager<ApplicationUser> _userManager;
        private DatabaseContext _databaseContext;  

        public ApplicationUsersController(UserManager<ApplicationUser> userManager, 
                                          DatabaseContext databaseContext) 
        { 
            _userManager = userManager; 
            _databaseContext = databaseContext; 
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
                user.EmailConfirmed = true; 
                user.LockoutEnabled = false; 

                // persist the changes into the Database
                _databaseContext.SaveChanges(); 

                // return the newly-created Application User to the client
                vmApplicationUser = new ApplicationUserViewModel(user); 
                return CreatedAtAction(nameof(GetApplicationUser), new { userId = vmApplicationUser.UserId }, vmApplicationUser);

            } catch(Exception e) { 

                return new ObjectResult(new { error = e.Message });
            } 
        }
    }

}