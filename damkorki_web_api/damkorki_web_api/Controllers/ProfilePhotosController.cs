
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using DamkorkiWebApi.Repositories;
using DamkorkiWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DamkorkiWebApi.Controllers { 

    [Route("profile-photos")]
    public class ProfilePhotosController : BaseController { 

        private IUnitOfWork _unitOfWork; 
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProfilePhotosController(DatabaseContext databaseContext, 
                                       UserManager<ApplicationUser> userManager, 
                                       IUnitOfWork unitOfWork, 
                                       IHostingEnvironment hostingEnvironment) 
                                      : base(databaseContext, userManager)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment; 
        }

        // DELETE: /profile-photos/{profilePhotoId}
        [Authorize]
        [HttpDelete("{profilePhotoId}")]
        public async Task<IActionResult> DeleteProfilePhoto(int profilePhotoId) { 

            ProfilePhoto profilePhoto = await _unitOfWork.ProfilePhotos.GetAsync(profilePhotoId); 
            if(profilePhoto == null) { 
                return NotFound(new { error = String.Format("Profile photo with id {0} has not been found.", profilePhotoId) }); 
            }
            
            // remove profile photo from disk
            var profilePhotosPath = Path.Combine(_hostingEnvironment.WebRootPath,  "profile-photos"); 
            var filePath = Path.Combine(profilePhotosPath, profilePhoto.FileName);
            var fileInfo = new FileInfo(filePath); 
            if(fileInfo.Exists) { 
                fileInfo.Delete();
            }

            // remove profile photo from database 
            _unitOfWork.ProfilePhotos.Remove(profilePhoto); 
            _unitOfWork.Complete(); 

            return NoContent(); 
        }

        // GET: /profile-photos/{profilePhotoId}/check-main
        [Authorize]
        [HttpGet("{profilePhotoId}/check-main")]
        public async Task<IActionResult> CheckMainProfilePhoto(int profilePhotoId) { 

            ProfilePhoto profilePhoto = await _unitOfWork.ProfilePhotos.GetAsync(profilePhotoId); 
            if(profilePhoto == null) { 
                return NotFound(new { error = String.Format("Profile photo with id {0} has not been found.", profilePhotoId) }); 
            }

            var isMainPhotoChecked = await _unitOfWork.ProfilePhotos.CheckMainAsync(profilePhoto) > 0;
            if(isMainPhotoChecked) { 
                profilePhoto.IsProfilePhoto = true; 
            } else {
                return BadRequest(new { error = String.Format("Could not check photo with id {0} as main profile photo.", profilePhotoId) }); 
            }

            // return checked main profile photo 
            ProfilePhotoViewModel vmProfilePhoto = new ProfilePhotoViewModel(profilePhoto); 

            return Ok(vmProfilePhoto);
        }

        // GET: /persons/{personId}/photos/uncheck-all
        [Authorize]
        [Route("~/persons/{personId}/photos/uncheck-all")]
        [HttpGet]
        public async Task<IActionResult> UncheckAllProfilePhotosByPerson(int personId) { 
            
            Person person = await _unitOfWork.People.GetAsync(personId); 
            if(person == null) { 
                return NotFound(new { error = String.Format("Person with id {0} has not been found.", personId) }); 
            }

            // uncheck all photos for given person
            await _unitOfWork.ProfilePhotos.UncheckAllByPersonAsync(person); 

            return NoContent(); 
        }

        // GET: /persons/me/photos
        [Authorize]
        [Route("~/persons/me/photos")]
        [HttpGet]
        public async Task<IActionResult> GetProfilePhotosByCurrentPerson() { 

            string userId = GetCurrentUserId(); 
            
            ApplicationUser user = await _unitOfWork.Users.GetWithPersonProfileAsync(userId); 
            if(user == null) { 
                return NotFound(new { error = "Application user has not been found." });
            }
            if(user.Person == null) { 
                return NotFound(new { error = String.Format("Person has not been found for user with id {0}.", user.Id) }); 
            }
            if(user.Person.ProfilePhotos == null) { 
                return NotFound(new { error = String.Format("Profile photos have not been found for person with id {0}.", user.Person.PersonId) });
            }

            // map list of repository's entities into list of view model's objects
            List<ProfilePhotoViewModel> vmProfilePhotos = user.Person.ProfilePhotos.Select(pp => new ProfilePhotoViewModel(pp)).ToList();

            return Ok(vmProfilePhotos); 
        }

        // GET: /persons/{personId}/photos
        [Authorize]
        [Route("~/persons/{personId}/photos")]
        [HttpGet]
        public async Task<IActionResult> GetProfilePhotosByPerson(int personId) { 
            
            Person person = await _unitOfWork.People.GetEagerlyAsync(personId);
            if(person == null) { 
                return NotFound(new { error = String.Format("Person with id {0} has not been found.", personId) }); 
            } 
            if(person.ProfilePhotos == null) { 
                return NotFound(new { error = String.Format("Profile photos have not been found for person with id {0}.", personId) }); 
            }

            // map list of repository's entities into list of view model's objects 
            List<ProfilePhotoViewModel> vmProfilePhotos = person.ProfilePhotos.Select(pp => new ProfilePhotoViewModel(pp)).ToList(); 
            
            return Ok(vmProfilePhotos); 
        }

        // POST: /persons/{personId}/photos
        [Authorize]
        [Route("~/persons/{personId}/photos")]
        [HttpPost]
        public async Task<IActionResult> UploadProfilePhotosByPerson(int personId) { 

                var files = Request.Form.Files;
                if(files.Count < 1) { 
                    return BadRequest( new { error = "No files attached to the request." }); 
                }

                var profilePhotosPath = Path.Combine( /* Directory.GetCurrentDirectory(), "wwwroot", */
                                                     _hostingEnvironment.WebRootPath,  "profile-photos"); 
               
                var uploadedProfilePhotos = new List<ProfilePhoto>();

                foreach(var file in files) { 
                    
                    if(file.Length > 0) {  
                        
                        var fileExtension = ImageTypesToExtensions[file.ContentType]; 
                        if(fileExtension == null) { 
                            return BadRequest(new { error = "Some files have wrong MIME type." });
                        }
                        
                        var randomFileName = Path.ChangeExtension(Guid.NewGuid().ToString().Replace("-", string.Empty) + Path.GetRandomFileName(), fileExtension);
                        var filePath = Path.Combine(profilePhotosPath, randomFileName);

                        using(var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var profilePhoto = new ProfilePhoto { 
                            FileName = randomFileName,
                            IsProfilePhoto = false,
                            Caption = null, 
                            PersonId = personId
                        }; 

                         uploadedProfilePhotos.Add(profilePhoto);
                    }
                }

                if(uploadedProfilePhotos.Count > 0) {
                    // insert uploaded photos to repository
                    await _unitOfWork.ProfilePhotos.AddRangeAsync(uploadedProfilePhotos);
                    _unitOfWork.Complete();

                    // check only last photo as main profile photo
                    var isMainPhotoChecked = (await _unitOfWork.ProfilePhotos.CheckMainAsync(uploadedProfilePhotos.Last())) > 0; 
                    if(isMainPhotoChecked) { 
                        uploadedProfilePhotos.Last().IsProfilePhoto = true; 
                    } 
                }
                
                // create response
                long totalSize = files.Sum(f => f.Length);
                List<ProfilePhotoViewModel> vmProfilePhotos = uploadedProfilePhotos.Select(pp => new ProfilePhotoViewModel(pp)).ToList();
                return Ok(new { 
                            count = vmProfilePhotos.Count, 
                            totalSize = totalSize, 
                            photos = vmProfilePhotos 
                        }); 
        }

        private static Dictionary<string, string> ImageTypesToExtensions = 
                        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "image/png", ".png"}, 
            { "image/jpeg", ".jpeg"}, 
            { "image/gif", ".gif"}
        };

    }
}