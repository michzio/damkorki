using System;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.ViewModels { 

    public class ApplicationUserViewModel { 

        public ApplicationUserViewModel() { }

        public ApplicationUserViewModel(ApplicationUser applicationUser, bool includeReferences = true) { 
            
            // wrap ApplicationUser repository object into ApplicationUserViewModel object
            UserId = applicationUser.Id; 
            UserName = applicationUser.UserName; 
            Password = applicationUser.PasswordHash; 
            Email = applicationUser.Email;
            RegistrationDate = applicationUser.RegistrationDate;
            LastModifiedDate = applicationUser.LastModifiedDate; 
            LastLoginDate = applicationUser.LastLoginDate; 
            FailedLoginDate = applicationUser.FailedLoginDate; 
            if(includeReferences && applicationUser.Person != null) { 
                Person = new PersonViewModel(applicationUser.Person, false);
            }
        }
        
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordNew { get; set; }
        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public DateTime? FailedLoginDate { get; set; }

        public PersonViewModel Person { get; set; }
    }
}