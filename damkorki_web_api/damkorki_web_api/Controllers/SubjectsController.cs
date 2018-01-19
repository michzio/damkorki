
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using DamkorkiWebApi.Repositories;
using DamkorkiWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DamkorkiWebApi.Controllers {

    [Route("subjects")]
    public class SubjectsController : BaseController
    {
        private IUnitOfWork _unitOfWork; 

        public SubjectsController(DatabaseContext databaseContext, 
                                  UserManager<ApplicationUser> userManager, 
                                  IUnitOfWork unitOfWork) : base(databaseContext, userManager)
        {
            _unitOfWork = unitOfWork; 
        }

        // GET: /subjects
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllSubjects() 
        {
            if(Request.Query.ContainsKey("nested")) { 
                return await GetAllSubjectsNested(); 
            }

            List<SubjectViewModel> vmSubjects = (await _unitOfWork.Subjects.GetAllAsync()).Select(s => new SubjectViewModel(s, false)).ToList(); 

            return Ok(vmSubjects); 
        }
        
        // GET: /subjects?nested
        private async Task<IActionResult> GetAllSubjectsNested() { 

             List<SubjectViewModel> vmSubjects = (await _unitOfWork.Subjects.GetAllAsync()).Select(s => new SubjectViewModel(s, false)).ToList(); 

             var nestedVmSubjects = new List<SubjectViewModel>(); 

            foreach(var s in vmSubjects) {    
                if(s.SuperSubjectId == null || s.SuperSubjectId < 1) { 
                    // it is root subject
                    nestedVmSubjects.Add(s);
                    AddChildSubjects(s, vmSubjects); 
                }
            }

            return Ok(nestedVmSubjects); 
        }

        private void AddChildSubjects(SubjectViewModel vmSubject, List<SubjectViewModel> vmSubjects) { 

            vmSubject.SubSubjects = new List<SubjectViewModel>();

            foreach(var s in vmSubjects) { 
                if(s.SuperSubjectId == vmSubject.SubjectId) {
                    vmSubject.SubSubjects.Add(s);
                    AddChildSubjects(s, vmSubjects);
                }
            }
        }

        // GET: /subjects/{subjectId}
        [Authorize]
        [HttpGet("{subjectId}")]
        public async Task<IActionResult> GetSubject(int subjectId) { 

            Subject subject = await _unitOfWork.Subjects.GetAsync(subjectId); 
            if(subject == null) { 
                return NotFound(new { error = String.Format("Subject with id {0} has not been found.", subjectId) });
            }

            SubjectViewModel vmSubject = new SubjectViewModel(subject, false);
            return Ok(vmSubject);  
        }
    }
}