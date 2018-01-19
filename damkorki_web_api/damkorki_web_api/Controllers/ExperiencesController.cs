
using System;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using DamkorkiWebApi.Repositories;
using DamkorkiWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DamkorkiWebApi.Controllers {

    [Route("experiences")]
    public class ExperiencesController : BaseController
    {
        private IUnitOfWork _unitOfWork; 

        public ExperiencesController(DatabaseContext databaseContext, 
                                     UserManager<ApplicationUser> userManager, 
                                     IUnitOfWork unitOfWork) 
                                     : base(databaseContext, userManager)
        {
            _unitOfWork = unitOfWork; 
        }

        // GET: /experiences/{experienceId}
        [Authorize]
        [HttpGet("{experienceId}")]
        public async Task<IActionResult> GetExperience(int experienceId) { 

            Experience experience = await _unitOfWork.Experiences.GetAsync(experienceId); 
            if(experience == null) { 
                return NotFound(new { error = String.Format("Experience with id {0} has not been found.", experienceId) }); 
            }

            ExperienceViewModel vmExperience = new ExperienceViewModel(experience, false); 
            return Ok(vmExperience); 
        }

        // POST: /experiences
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateExperience([FromBody] ExperienceViewModel vmExperience) { 

            if(vmExperience == null) 
                return BadRequest(new { error = "No Experience object in request body." }); 
            if(!ModelState.IsValid)
                return BadRequest(ModelState); 

            try 
            {
                // Create new Experience entity for given Tutor entity 
                var newExperience = new Experience 
                {
                    StartYear = vmExperience.StartYear,
                    EndYear = vmExperience.EndYear,
                    Description = vmExperience.Description, 
                    TutorId = vmExperience.TutorId // if Tutor doesn't exists it throws error
                };

                // Add new Experience to DbContext and save changes
                await _unitOfWork.Experiences.AddAsync(newExperience); 
                _unitOfWork.Complete(); 

                vmExperience = new ExperienceViewModel(newExperience); 
                return CreatedAtAction(nameof(GetExperience), new { experienceId = vmExperience.ExperienceId }, vmExperience);  
            } catch(Exception e)
            {
                return BadRequest(new { error = e.Message }); 
            }
        }
        
        // PUT: /experiences/{experienceId}
        [Authorize]
        [HttpPut("{experienceId}")]
        public async Task<IActionResult> UpdateExperience(int experienceId, [FromBody] ExperienceViewModel vmExperience) { 

            if(experienceId < 1)
                return BadRequest(new { error = "Incorrect experience id."}); 
            if(vmExperience == null) 
                return BadRequest(new { error = "No Experience object in request body."});
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try { 
                // check if Experience entity exists for given Experience Id
                Experience experience = await _unitOfWork.Experiences.GetAsync(experienceId);
                if(experience == null) { 
                    return NotFound(new { error = "Could not find Experience entity for given experience id to update."}); 
                }

                // update Experience object with new values 
                experience.StartYear = vmExperience.StartYear; 
                experience.EndYear = vmExperience.EndYear; 
                experience.Description = vmExperience.Description; 
                experience.TutorId = vmExperience.TutorId; 
                
                // and save changes with DBContext 
                _unitOfWork.Complete(); 

                vmExperience = new ExperienceViewModel(experience); 
                return Ok(vmExperience); 
            } catch(Exception e) 
            {
                return BadRequest(new { error = e.Message });
            }
        }

        // DELETE: /experiences/{experienceId}
        [Authorize]
        [HttpDelete("{experienceId}")]
        public async Task<IActionResult> DeleteExperience(int experienceId) { 

            Experience toDeleteExperience = await _unitOfWork.Experiences.GetAsync(experienceId); 
            if(toDeleteExperience == null) { 
                return NotFound("Could not find Experience for given id."); 
            }

            _unitOfWork.Experiences.Remove(toDeleteExperience); 
            _unitOfWork.Complete(); 

            return NoContent(); 
        }
    }
}