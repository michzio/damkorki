using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using DamkorkiWebApi.Repositories;
using DamkorkiWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DamkorkiWebApi.Controllers
{
	[EnableCors("AllowClientOrigin")]
	[Route("[controller]")]
	public class TutorsController : BaseController
	{
		#region Private Fields
		private IUnitOfWork _unitOfWork;
		#endregion

		#region Constructor
		public TutorsController(DatabaseContext databaseContext, 
							    UserManager<ApplicationUser> userManager, 
								IUnitOfWork unitOfWork) 
								: base(databaseContext, userManager) 
		{
			_unitOfWork = unitOfWork;
		}
		#endregion

		// GET: /tutors 
		[EnableCors("AllowClientOrigin")]
		[Authorize]
		[HttpGet]
		// [Produces("application/json")]
		public async Task<IActionResult> GetAllTutors()
		{
			List<TutorViewModel> vmTutors = (await _unitOfWork.Tutors.GetAllAsync()).Select(t => new TutorViewModel(t, false)).ToList();

			return Ok(vmTutors); 
		}

		// GET: tutors/{tutorId}
		[Authorize]
		[HttpGet("{tutorId}")]
		public async Task<IActionResult> GetTutor(int tutorId) { 

			Tutor tutor = await _unitOfWork.Tutors.GetAsync(tutorId);
			if(tutor == null) { 
				return NotFound(new { error = String.Format("Tutor with id {0} has not been found.", tutorId) }); 
			}

			TutorViewModel vmTutor = new TutorViewModel(tutor, false);
			return Ok(vmTutor); 
		}

		// POST: /tutors 
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> CreateTutor([FromBody] TutorViewModel vmTutor) {

			if (vmTutor == null) 
				return BadRequest(new { error = "No Tutor object in request body."});
			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				// check if the Person with given personId exists 
				// Person person = await _unitOfWork.People.GetAsync(vmTutor.PersonId);
				// if (person == null) throw new Exception("Given Person doesn't exists.");

				// create new Tutor entity for given Person entity 
				var newTutor = new Tutor
				{
					Description = vmTutor.Description,
					Qualifications = vmTutor.Qualifications,
					IsSuperTutor = vmTutor.IsSuperTutor,
					PersonId = vmTutor.PersonId // if Person doesn't exists it throws error
					//Person = person
				};

				// Add new Tutor to DBContext and save changes 
				await _unitOfWork.Tutors.AddAsync(newTutor);
				_unitOfWork.Complete();

				vmTutor.TutorId = newTutor.TutorId;

				return CreatedAtAction(nameof(GetTutor), new { tutorId = vmTutor.TutorId }, vmTutor);

			} catch(Exception e) 
			{
				return new ObjectResult(new { error = e.Message }); 
			}
		}

		// PUT: /tutors/{tutorId}
		[Authorize]
		[HttpPut("{tutorId}")]
		public async Task<IActionResult> UpdateTutor(int tutorId, [FromBody] TutorViewModel vmTutor)
		{
			if(tutorId < 1) 
				return BadRequest(new { error = "Incorrect tutor id."}); 
			if(vmTutor == null) 
				return BadRequest(new { error = "No Tutor object in request body."});
			if(!ModelState.IsValid)
				return BadRequest(ModelState);	

			try { 
				// check if Tutor entity exists for given Tutor Id or create new one
				Tutor tutor = await _unitOfWork.Tutors.GetAsync(tutorId);	
				if(tutor == null) {
					return NotFound(new { error = "Could not find Tutor entity for given tutor id to update."}); 
				}
				
				// update Tutor object with new values 
				tutor.Description = vmTutor.Description; 
				tutor.Qualifications = vmTutor.Qualifications; 
				tutor.IsSuperTutor = vmTutor.IsSuperTutor; 
				tutor.PersonId = vmTutor.PersonId; 

				// and save changes with DBContext
				_unitOfWork.Complete(); 

				vmTutor = new TutorViewModel(tutor);
				return Ok(vmTutor);

			} catch(Exception e) 
			{ 
				return new ObjectResult(new { error = e.Message }); 
			}
		}

		// DELETE: /tutors/{tutorId}
		[DisableCors]
		[Authorize]
		[HttpDelete("{tutorId}")]
		public async Task<IActionResult> DeleteTutor(int tutorId) { 
	
			Tutor toDeleteTutor = await _unitOfWork.Tutors.GetAsync(tutorId); 
			_unitOfWork.Tutors.Remove(toDeleteTutor);
			_unitOfWork.Complete();

			/*
			Tutor toDeleteTutor = await _unitOfWork.Tutors.GetAsync(tutorId);
			toDeleteTutor.IsDeleted = true; 
			_unitOfWork.Complete();
			*/ 

			return NoContent(); 
		}

		// GET: /tutors/count
		[Authorize]
		[HttpGet("count")]
		public async Task<IActionResult> CountAllTutors() { 

			int tutorCount = await _unitOfWork.Tutors.CountAsync(); 

			return Ok(new {count = tutorCount }); 
		}

		// GET: /persons/me/tutor
		[Authorize]
		[Route("~/persons/me/tutor")]
		[HttpGet]
		public async Task<IActionResult> GetTutorByCurrentPerson() { 

			string userId = GetCurrentUserId(); 

			ApplicationUser user = await _unitOfWork.Users.GetWithTutorProfileAsync(userId);
			if(user == null) { 
				return NotFound(new { error = "Application user has not been found."});
			}
			if(user.Person == null) { 
				return NotFound(new { error = String.Format("Person has not been found for user with id {0}.", user.Id) });
			}
			if(user.Person.Tutor == null) { 
				return NotFound(new { error = String.Format("Tutor has not been found for person with id {0}.", user.Person.PersonId) }); 
			}

			// map repository entity into view model object 
			TutorViewModel vmTutor = new TutorViewModel(user.Person.Tutor);

			return Ok(vmTutor);  
		}

		// GET: /persons/me/tutor/eagerly
		[Authorize]
		[Route("~/persons/me/tutor/eagerly")]
		[HttpGet]
			public async Task<IActionResult> GetTutorEagerlyByCurrentPerson() { 

			string userId = GetCurrentUserId(); 

			ApplicationUser user = await _unitOfWork.Users.GetWithTutorProfileAsync(userId);
			if(user == null) { 
				return NotFound(new { error = "Application user has not been found."});
			}
			if(user.Person == null) { 
				return NotFound(new { error = String.Format("Person has not been found for user with id {0}.", user.Id) });
			}
			if(user.Person.Tutor == null) { 
				return NotFound(new { error = String.Format("Tutor has not been found for person with id {0}.", user.Person.PersonId) }); 
			}

			Tutor tutor = await _unitOfWork.Tutors.GetEagerlyAsync(user.Person.Tutor.TutorId); 

			// map repository entity into view model object 
			TutorViewModel vmTutor = new TutorViewModel(tutor);

			return Ok(vmTutor);  
		}

		// GET: /persons/{personId}/tutor
		[Authorize]
		[Route("~/persons/{personId}/tutor")]
		[HttpGet]
		public IActionResult GetTutorByPerson(int personId) { 

			if(personId < 1) { 
				return BadRequest(new { error = "Incorrect person id." }); 
			}
			
			try { 
				Tutor tutor = _unitOfWork.Tutors.Find(t => t.PersonId == personId).SingleOrDefault();
				if(tutor == null) { 
					return NotFound(new { error = String.Format("Tutor has not been found for person with id {0}", personId) }); 
				}

				TutorViewModel vmTutor = new TutorViewModel(tutor); 
				return Ok(vmTutor); 
			} catch(Exception e) { 
				return new ObjectResult(new { error = e.Message });
			}
		}

		// GET: /persons/{personId}/tutor/eagerly
		[Authorize]
		[Route("~/persons/{personId}/tutor/eagerly")]
		[HttpGet]
		public IActionResult GetTutorEagerlyByPerson(int personId) { 

			if(personId < 1) { 
				return BadRequest(new { error = "Incorrect person id."});
			}

			try {
				Tutor tutor = _unitOfWork.Tutors.FindEagerly(t => t.PersonId == personId).SingleOrDefault();
				if(tutor == null) { 
					return NotFound(new { error = String.Format("Tutor has not been found for person with id {0}.", personId) }); 
				}

				TutorViewModel vmTutor = new TutorViewModel(tutor); 
				return Ok(vmTutor); 
			} catch(Exception e) { 
				return new ObjectResult(new { error = e.Message }); 
			}
		}
	}
}
