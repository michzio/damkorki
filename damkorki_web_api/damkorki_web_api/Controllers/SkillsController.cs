
using System;
using System.Linq;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using DamkorkiWebApi.Repositories;
using DamkorkiWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DamkorkiWebApi.Controllers {

    [Route("skills")]
    public class SkillsControler : BaseController
    {
        private IUnitOfWork _unitOfWork; 

        public SkillsControler(DatabaseContext databaseContext, 
                               UserManager<ApplicationUser> userManager,
                               IUnitOfWork unitOfWork) 
                               : base(databaseContext, userManager)
        {
            _unitOfWork = unitOfWork; 
        }

        // GET: /skills/{skillId}
        [Authorize]
        [HttpGet("{skillId}")]
        public async Task<IActionResult> GetSkill(int skillId) { 

            Skill skill = await _unitOfWork.Skills.GetAsync(skillId);
            if(skill == null) { 
                return NotFound(new { error = String.Format("Skill with id {0} has not been found.", skillId) }); 
            } 

            SkillViewModel vmSkill = new SkillViewModel(skill, false); 
            return Ok(vmSkill); 
        }

        // POST: /skills 
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSkill([FromBody] SkillViewModel vmSkill) { 
            
            if(vmSkill == null)
                return BadRequest(new { error = "No Skill object in request body." }); 
            if(!ModelState.IsValid) 
                return BadRequest(ModelState); 

            try { 

                Skill skill = _unitOfWork.Skills.Find(s => s.Name == vmSkill.Name).SingleOrDefault();
                if(skill == null) { 
                    // Create new Skill entity 
                    skill = new Skill 
                    {
                        Name = vmSkill.Name
                    };

                    // Add new Skill to DbContext and save changes
                    await _unitOfWork.Skills.AddAsync(skill); 
                    _unitOfWork.Complete();   
                }

                // return created or found Skill entity
                vmSkill = new SkillViewModel(skill); 
                return CreatedAtAction(nameof(GetSkill), new { skillId = vmSkill.SkillId }, vmSkill); 

            } catch(Exception e) {

                return new ObjectResult(new { error = e.Message }); 
            }
        }
    }
}