using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using DamkorkiWebApi.Repositories;
using DamkorkiWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DamkorkiWebApi.Controllers { 

    [Route("lesson-offers")]
    public class LessonOffersController : Controller { 

        private IUnitOfWork _unitOfWork; 

        public LessonOffersController(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork; 
        }

        // GET: /lesson-offers 
        [HttpGet()]
        public async Task<IActionResult> GetAllLessonOffers() { 

            List<LessonOfferViewModel> vmLessonOffers = (await _unitOfWork.LessonOffers.GetAllAsync()).Select(lo => new LessonOfferViewModel(lo)).ToList(); 
            
            return Ok(vmLessonOffers); 
        }

        // GET: /lesson-offers/{lessonOfferId}
        [HttpGet("{lessonOfferId}")]
        public async Task<IActionResult> GetLessonOffer(int lessonOfferId) { 

            LessonOffer lessonOffer = await _unitOfWork.LessonOffers.GetAsync(lessonOfferId); 
            if(lessonOffer == null) { 
                return NotFound(new { error = String.Format("Lesson offer with id {0} has not been found.", lessonOfferId) }); 
            }

            LessonOfferViewModel vmLessonOffer = new LessonOfferViewModel(lessonOffer); 
            return Ok(vmLessonOffer); 
        }

        // POST: /lesson-offers
        [Authorize] 
        [HttpPost]
        public async Task<IActionResult> CreateLessonOffer([FromBody] LessonOfferViewModel vmLessonOffer) { 

           if(vmLessonOffer == null)
                return BadRequest(new { error = "No Lesson Offer object in request body."});
           if(!ModelState.IsValid) 
                return BadRequest(ModelState); 
                
            try { 
                // create new Lesson Offer entity with data given in view model
                var newLessonOffer = new LessonOffer 
                { 
                    Title = vmLessonOffer.Title, 
                    Description = vmLessonOffer.Description, 
                    Cost = vmLessonOffer.Cost, 
                    Type = vmLessonOffer.Type, 
                    Location = vmLessonOffer.Location, 
                    Level = vmLessonOffer.Level, 
                    SubjectId = vmLessonOffer.SubjectId, 
                    TutorId = vmLessonOffer.TutorId
                };

                // add new Lesson Offer to DbContext and save changes
                await _unitOfWork.LessonOffers.AddAsync(newLessonOffer); 
                _unitOfWork.Complete(); 

                vmLessonOffer.LessonOfferId = newLessonOffer.LessonOfferId; 

                return CreatedAtAction(nameof(GetLessonOffer), new { lessonOfferId = vmLessonOffer.LessonOfferId }, vmLessonOffer); 

            } catch(Exception e) { 

                return BadRequest(new { error = e.Message }); 
            }
        }

        // PUT: /lesson-offers/{lessonOfferId}
        [Authorize]
        [HttpPut("{lessonOfferId}")]
        public async Task<IActionResult> UpdateLessonOffer(int lessonOfferId, [FromBody] LessonOfferViewModel vmLessonOffer) { 

            throw new NotImplementedException(); 

            return null; 
        }

        // DELETE: /lesson-offers/{lessonOfferId}
        [HttpDelete("{lessonOfferId}")]
        public async Task<IActionResult> DeleteLessonOffer(int lessonOfferId) { 

            throw new NotImplementedException(); 

            return null; 
        }

        // GET: /lesson-offers/count
        [HttpGet("count")]
        public async Task<IActionResult> countAllLessonOffers() { 

            throw new NotImplementedException(); 

            return null; 
        }


         // GET: /lesson-offers/eagerly 
        [HttpGet("eagerly")]
        public async Task<IActionResult> GetAllLessonOffersEagerly() {

             List<LessonOfferViewModel> vmLessonOffers = (await _unitOfWork.LessonOffers.GetAllEagerlyAsync()).Select(lo => new LessonOfferViewModel(lo)).ToList(); 
            
            return Ok(vmLessonOffers); 
        } 

    }


}

