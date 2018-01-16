
using System;
using System.ComponentModel.DataAnnotations;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.ViewModels { 

    public class ProfilePhotoViewModel { 

        public ProfilePhotoViewModel() { }

        public ProfilePhotoViewModel(ProfilePhoto photo, bool includeReferences = true) { 
            
            // wrap ProfilePhoto repository object into ProfilePhotoViewModel object 
            ProfilePhotoId = photo.ProfilePhotoId; 
            FileName = photo.FileName; 
            IsProfilePhoto = photo.IsProfilePhoto; 
            Caption = photo.Caption; 
            PersonId = photo.PersonId;
        }

        public int ProfilePhotoId { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public Boolean IsProfilePhoto { get; set; }
        public string Caption { get; set; }
        [Required]
        public int PersonId { get; set; }
    }
}