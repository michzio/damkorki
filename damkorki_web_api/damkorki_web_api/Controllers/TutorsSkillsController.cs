
using System;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using DamkorkiWebApi.Repositories;
using DamkorkiWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DamkorkiWebApi.Controllers
{
    [Route("/tutors-have-skills/{tutorId}has{skillId}")]
    public class TutorsSkillsController : BaseController
    {
        IUnitOfWork _unitOfWork; 

        public TutorsSkillsController(DatabaseContext databaseContext, 
                                      UserManager<ApplicationUser> userManager, 
                                      IUnitOfWork unitOfWork) 
                                      : base(databaseContext, userManager)
        {
            _unitOfWork = unitOfWork;
        }
        
        // POST: /tutors-have-skills/{tutorId}has{skillId}
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddSkillToTutor(int tutorId, int skillId) { 

            try {
                // Create relationship between Tutor and Skill
                TutorSkill tutorSkill = new TutorSkill { 
                    TutorId = tutorId,
                    SkillId = skillId 
                };
                // Add TutorSkill to DbContext and save changes
                await _unitOfWork.TutorsSkills.AddAsync(tutorSkill);
                _unitOfWork.Complete(); 

                TutorSkillViewModel vmTutorSkill = new TutorSkillViewModel(tutorSkill); 
                return Ok(vmTutorSkill);
            } catch(Exception e) { 
                return BadRequest(new { error = e.Message });
            }
        }

        // DELETE: /tutors-have-skills/{tutorId}has{skillId}
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> RemoveSkillFromTutor(int tutorId, int skillId) { 
            
            try { 
                TutorSkill toDeleteTutorSkill = await _unitOfWork.TutorsSkills.GetAsync( (tutorId, skillId) ); 
                _unitOfWork.TutorsSkills.Remove(toDeleteTutorSkill); 
                _unitOfWork.Complete();

                return NoContent(); 
            } catch(Exception e) { 
                return BadRequest(new { error = e.Message });
            }
        }
    }
}