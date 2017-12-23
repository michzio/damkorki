
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DamkorkiWebApi.Controllers { 

    public class BaseController : Controller { 
        
        protected DatabaseContext _databaseContext; 
        protected UserManager<ApplicationUser> _userManager; 

        public BaseController(DatabaseContext databaseContext,
                              UserManager<ApplicationUser> userManager) 
        { 
            _databaseContext = databaseContext; 
            _userManager = userManager; 
        }

        protected string GetCurrentUserId() { 
            
            if(!User.Identity.IsAuthenticated) { 
                throw new System.Exception("User identity is not authenticated.");
            }

            return User.FindFirst("uid").Value;
        }
    }
}