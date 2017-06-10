using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using DamkorkiWebApi.Repositories;
using DamkorkiWebApi.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DamkorkiWebApi.Controllers
{
	[EnableCors("AllowClientOrigin")]
	[Route("[controller]")]
	public class TutorsController : Controller
	{
		#region Private Fields
		private IUnitOfWork _unitOfWork;
		#endregion

		#region Constructor
		public TutorsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		#endregion

		// GET: /tutors 
		[EnableCors("AllowClientOrigin")]
		[HttpGet]
		// [Produces("application/json")]
		public async Task<IActionResult> GetAllTutors()
		{
			List<TutorViewModel> vmTutors = (await _unitOfWork.Tutors.GetAllAsync()).Select(t => new TutorViewModel(t)).ToList();

			return Ok(vmTutors); 
		}

		// GET: tutors/{tutorId}
		[HttpGet("{tutorId}")]
		public async Task<IActionResult> GetTutor(int tutorId) { 

			Tutor tutor = await _unitOfWork.Tutors.GetAsync(tutorId);
			if(tutor == null) { 
				return NotFound(new { error = String.Format("Tutor with id {0} has not been found.", tutorId) }); 
			}

			TutorViewModel vmTutor = new TutorViewModel(tutor);
			return Ok(vmTutor); 
		}

		// POST: /tutors 
		[HttpPost]
		public async Task<IActionResult> CreateTutor([FromBody] TutorViewModel vmTutor) {

			if (vmTutor == null) 
				return BadRequest(new { error = "No Tutor object in request body."});
			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				// check if the Person with given personId exists 
				Person person = await _unitOfWork.People.GetAsync(vmTutor.PersonId);
				if (person == null) throw new Exception("Given Person doesn't exists.");

				// create new Tutor entity for given Person entity 
				var newTutor = new Tutor
				{
					Description = vmTutor.Description,
					Qualifications = vmTutor.Qualifications,
					IsSuperTutor = vmTutor.IsSuperTutor,
					//PersonId = vmTutor.PersonId
					Person = person
				};

				// Add new Tutor to DBContext and save changes 
				await _unitOfWork.Tutors.AddAsync(newTutor);
				_unitOfWork.Complete();

				vmTutor.TutorId = newTutor.TutorId;

				return CreatedAtAction(nameof(GetTutor), new { tutorId = vmTutor.TutorId }, vmTutor);

			} catch(Exception e) {
				return new ObjectResult(new { error = e.Message }); 
			}
		}

		// PUT: /tutors/{tutorId}
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
				Tutor oldTutor = await _unitOfWork.Tutors.GetAsync(tutorId);	
				if(oldTutor == null) 
					return NotFound(new { error = "Could not find Tutor entity for given tutor id to update."}); 
				
				// update Tutor object with new values 
				oldTutor.Description = vmTutor.Description; 
				oldTutor.Qualifications = vmTutor.Qualifications; 
				oldTutor.IsSuperTutor = vmTutor.IsSuperTutor; 
				oldTutor.PersonId = vmTutor.PersonId; 

				// and save changes with DBContext
				_unitOfWork.Complete(); 

				return Ok(vmTutor);

			} catch(Exception e) { 
				return new ObjectResult(new { error = e.Message }); 
			}
		}

		// DELETE: /tutors/{tutorId}
		[DisableCors]
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
		[HttpGet("count")]
		public async Task<IActionResult> CountAllTutors() { 

			int tutorCount = await _unitOfWork.Tutors.CountAsync(); 

			return Ok(new {count = tutorCount }); 
		}
	}
}
