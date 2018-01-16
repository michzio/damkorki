
using System;
using System.Threading.Tasks; 
using DamkorkiWebApi.Models;
using DamkorkiWebApi.Repositories;
using DamkorkiWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq; 

namespace DamkorkiWebApi.Controllers {

    [Route("addresses")]
    public class AddressesController : BaseController { 

        private IUnitOfWork _unitOfWork; 

        public AddressesController(DatabaseContext databaseContext,
                                   UserManager<ApplicationUser> userManager,
                                   IUnitOfWork unitOfWork) 
                                   : base(databaseContext, userManager)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /addresses/{addressId}
        [Authorize]
        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddress(int addressId) { 

            Address address = await _unitOfWork.Addresses.GetAsync(addressId);
            if(address == null) { 
                return NotFound(new { error = String.Format("Address with id {0} has not been found.", addressId)}); 
            } 

            AddressViewModel vmAddress = new AddressViewModel(address); 
            return Ok(vmAddress); 
        }

        // PUT: /addresses/{addressId}
        [Authorize]
        [HttpPut("{addressId}")]
        public async Task<IActionResult> UpdateAddress(int addressId, [FromBody] AddressViewModel vmAddress) { 

            if(vmAddress == null)
                return BadRequest(new { error = "No Address object in request body." }); 
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try { 
                // get existing Address entity for given identifier 
                Address address = await _unitOfWork.Addresses.GetAsync(addressId);
                if(address == null) { 
                    return NotFound(new { error = String.Format("Address with id {0} has not been found.", addressId) }); 
                }

                // update existing Address entity with data in view model 
                address.AddressLine1 = vmAddress.AddressLine1; 
                address.AddressLine2 = vmAddress.AddressLine2;
                address.City = vmAddress.City;
                address.ZipCode = vmAddress.ZipCode; 
                address.Region = vmAddress.Region; 
                address.Country = vmAddress.Country;

                _unitOfWork.Complete(); 

                vmAddress = new AddressViewModel(address);
                return Ok(vmAddress);  

            } catch(Exception e) 
            { 
                return new ObjectResult(new { error = e.Message }); 
            }
        }

        // GET: /persons/me/address
        [Authorize]
        [Route("~/persons/me/address")]
        [HttpGet]
        public async Task<IActionResult> GetAddressByCurrentPerson() { 

            string userId = GetCurrentUserId(); 

            ApplicationUser user = await _unitOfWork.Users.GetWithPersonProfileAsync(userId); 
            if(user == null) { 
                return NotFound(new { error = "Application user has not been found." }); 
            }
            if(user.Person == null) { 
                return NotFound(new { error = String.Format("Person has not been found for user with id {0}.", user.Id) }); 
            }
            if(user.Person.Address == null) { 
                return NotFound(new { error = String.Format("Address has not been found for person with id {0}.", user.Person.PersonId) });
            }

            AddressViewModel vmAddress = new AddressViewModel(user.Person.Address);
            return Ok(vmAddress); 
        }

        // GET: /persons/{personId}/address
        [Authorize]
        [Route("~/persons/{personId}/address")]
        [HttpGet]
        public async Task<IActionResult> GetAddressByPerson(int personId) { 

            Person person = await _unitOfWork.People.GetEagerlyAsync(personId); 
            if(person == null) { 
                return NotFound(new { error = String.Format("Person with id {0} has not been found.", personId) }); 
            }
            if(person.Address == null) { 
                return NotFound(new { error = String.Format("Address has not been found for person with id {0}.", personId) }); 
            }

            AddressViewModel vmAddress = new AddressViewModel(person.Address); 
            return Ok(vmAddress); 
        }

        // POST: //persons/{personId}/address
        [Authorize]
        [Route("~/persons/{personId}/address")]
        [HttpPost]
        public async Task<IActionResult> CreateAddressForPerson(int personId, [FromBody] AddressViewModel vmAddress) {

            if(vmAddress == null) 
                return BadRequest(new { error = "No Address object in request body." });
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            Person person = await _unitOfWork.People.GetEagerlyAsync(personId); 
            if(person == null) { 
                return NotFound(new { error = String.Format("Person with id {0} has not been found.", personId) });
            }
            if(person.Address != null) { 
                return BadRequest(new { error = string.Format("Address currently exists for Person with id {0}. " +
                                                              "Could not create new Address entity. " +
                                                              "Please update or delete existing Address to proceed!", personId) }); 
            }

            try { 
                // try to find Address with the same data 
                var foundAddress = FindAddressMatching(vmAddress);
                if(foundAddress != null) {
                    // reuse existing Address
                    person.Address = foundAddress;
                } else { 
                    // create new Address entity with data given in view model 
                    var newAddress = new Address { 
                        AddressLine1 = vmAddress.AddressLine1,
                        AddressLine2 = vmAddress.AddressLine2, 
                        City = vmAddress.City, 
                        ZipCode = vmAddress.ZipCode, 
                        Region = vmAddress.Region, 
                        Country = vmAddress.Country
                    };

                    // add new Address to DbContext 
                    await _unitOfWork.Addresses.AddAsync(newAddress); 
                    // assign it to Person entity 
                    person.Address = newAddress; 
                }

                // save changes 
                _unitOfWork.Complete(); 

                vmAddress = new AddressViewModel(person.Address);
                return CreatedAtAction(nameof(GetAddress), new { addressId = vmAddress.AddressId }, vmAddress); 

            } catch(Exception e) 
            { 
                return new ObjectResult(new { error = e.Message }); 
            }
        }

        // PUT: //persons/{personId}/addresses
        [Authorize]
        [Route("~/persons/{personId}/addresses/{addressId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateAddressForPerson(int personId, int addressId, [FromBody] AddressViewModel vmAddress) { 

            if(vmAddress == null)
                return BadRequest(new { error = "No Address object in request body." }); 
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            Person person = await _unitOfWork.People.GetEagerlyAsync(personId); 
            if(person == null) { 
                return NotFound(new { error = String.Format("Person with id {0} has not been found.", personId) });
            }
            if(person.Address == null) { 
                return NotFound(new { error = String.Format("Address has not been found for Person with id {0}.", personId) }); 
            }
            if(person.Address.AddressId != addressId) { 
                return BadRequest(new { error = "Address identifier doesn't match."}); 
            }

            var address = await _unitOfWork.Addresses.GetEagerlyAsync(person.Address.AddressId); 
            
            if(address.People.Count > 1) { 
                // address is assigned to many profiles 
                // and cannot be safely updated, CREATE new address

                // try to find Address with the same data 
                var foundAddress = FindAddressMatching(vmAddress);
                if(foundAddress != null) { 
                    // reuse existing Address 
                    person.Address = foundAddress; 
                } else { 
                    // create new Address entity with data given in view model
                    var newAddress = new Address { 
                        AddressLine1 = vmAddress.AddressLine1,
                        AddressLine2 = vmAddress.AddressLine2, 
                        City = vmAddress.City, 
                        ZipCode = vmAddress.ZipCode, 
                        Region = vmAddress.Region, 
                        Country = vmAddress.Country
                    };
                    // add new Address to DbContext 
                    await _unitOfWork.Addresses.AddAsync(newAddress);
                    // assign it to Person entity 
                    person.Address = newAddress; 
                }

                // save changes 
                _unitOfWork.Complete(); 

                vmAddress = new AddressViewModel(person.Address); 
                return CreatedAtAction(nameof(GetAddress), new { addressId = vmAddress.AddressId }, vmAddress); 

            } else { 
                // address is assigned to only one profile 
                // and can be safely UPDATEed 

                // try to find Address with the same data 
                var foundAddress = FindAddressMatching(vmAddress);
                if(foundAddress != null) { 
                    // reuse existing Address & remove old one 
                    _unitOfWork.Addresses.Remove(address); 
                    person.Address = foundAddress; 
                } else { 
                    // update Address entity with data given in view model 
                    person.Address.AddressLine1 = vmAddress.AddressLine1;
                    person.Address.AddressLine2 = vmAddress.AddressLine2;
                    person.Address.City = vmAddress.City;
                    person.Address.ZipCode = vmAddress.ZipCode; 
                    person.Address.Region = vmAddress.Region;
                    person.Address.Country = vmAddress.Country; 
                }

                // save changes 
                _unitOfWork.Complete(); 

                vmAddress = new AddressViewModel(person.Address); 
                return Ok(vmAddress);
            }
        }

        private Address FindAddressMatching(AddressViewModel vmAddress) { 

            // try to find Address with the same data 
            var foundAddresses = _unitOfWork.Addresses.Find(addr => 
                String.Equals(addr.AddressLine1, vmAddress.AddressLine1, StringComparison.CurrentCultureIgnoreCase)
                &&
                String.Equals(addr.AddressLine2, vmAddress.AddressLine2, StringComparison.CurrentCultureIgnoreCase)
                &&
                String.Equals(addr.City, vmAddress.City, StringComparison.CurrentCultureIgnoreCase)
                &&
                String.Equals(addr.ZipCode, vmAddress.ZipCode, StringComparison.CurrentCultureIgnoreCase)
                &&
                String.Equals(addr.Region, vmAddress.Region, StringComparison.CurrentCultureIgnoreCase)
                && 
                String.Equals(addr.Country, vmAddress.Country, StringComparison.CurrentCultureIgnoreCase)
            ).ToList();
            
            if(foundAddresses.Count > 0) {
                return foundAddresses.First(); 
            } 
            return null; 
        }
    }
}