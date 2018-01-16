
using DamkorkiWebApi.Models;
using DamkorkiWebApi.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DamkorkiWebApi.Controllers
{
    [Route("")]
    public class TutorsSkillsController : BaseController
    {
        IUnitOfWork _unitOfWork; 
        
        public TutorsSkillsController(DatabaseContext databaseContext, 
                                      UserManager<ApplicationUser> userManager, 
                                      IUnitOfWork unitOfWork) 
                                      : base(databaseContext, userManager)
        {
        }
    }
}