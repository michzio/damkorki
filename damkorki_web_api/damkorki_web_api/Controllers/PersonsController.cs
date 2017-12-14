using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using DamkorkiWebApi.Repositories;
using DamkorkiWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DamkorkiWebApi.Controllers { 

    [Route("persons")]
    public class PersonsController : Controller { 

        private IUnitOfWork _unitOfWork; 

        public PersonsController(IUnitOfWork unitOfWork) { 
            _unitOfWork = unitOfWork; 
        }

        // GET: /persons
        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetAllPersons() { 
            
            List<PersonViewModel> vmPersons = (await _unitOfWork.People.GetAllAsync()).Select(p => new PersonViewModel(p)).ToList(); 

            return Ok(vmPersons); 
        }

        // GET: /persons/{personId}
        [Authorize]
        [HttpGet("{personId}")]
        public async Task<IActionResult> GetPerson(int personId) {

            Person person = await _unitOfWork.People.GetAsync(personId); 
            if(person == null) { 
                return NotFound(new { error = String.Format("Person with id {0} has not been found.", personId) }); 
            }

            PersonViewModel vmPerson = new PersonViewModel(person);
            return Ok(vmPerson);  
        }

        // POST: /persons
        [HttpPost()]
        public async Task<IActionResult> CreatePerson([FromBody] PersonViewModel vmPerson) { 

            if(vmPerson == null) 
                return BadRequest(new { error = "No Person object in request body."});
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            try { 

                // create new Person entity with data given in view model
                var newPerson = new Person { 
                    FirstName = vmPerson.FirstName, 
                    LastName = vmPerson.LastName, 
                    Gender = vmPerson.Gender, 
                    Birthdate = vmPerson.Birthdate, 
                    Image = vmPerson.Image, 
                    Skype = vmPerson.Skype, 
                    PhoneNumber = vmPerson.PhoneNumber, 
                    AddressId = vmPerson.AddressId, 
                    UserId = vmPerson.UserId 
                }; 

                // add new Person to DbContext and save changes 
                await _unitOfWork.People.AddAsync(newPerson); 
                _unitOfWork.Complete();  

                vmPerson.PersonId = newPerson.PersonId; 

                return CreatedAtAction(nameof(GetPerson), new { personId = vmPerson.PersonId }, vmPerson); 

            } catch(Exception e) { 

                return new ObjectResult(new { error = e.Message }); 
            }
        }

        // GET: /persons/eagerly
        [Authorize]
        [HttpGet("eagerly")]
        public async Task<IActionResult> GetAllPersonsEagerly() { 

            List<PersonViewModel> vmPersons = (await _unitOfWork.People.GetAllEagerlyAsync()).Select(p => new PersonViewModel(p)).ToList(); 

            return Ok(vmPersons); 
        }
    } 
}