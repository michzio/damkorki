using System;
using System.ComponentModel.DataAnnotations;
using DamkorkiWebApi.Models;
using static DamkorkiWebApi.Models.Person;

namespace DamkorkiWebApi.ViewModels { 

    public class PersonViewModel { 

        public PersonViewModel() { }

        public PersonViewModel(Person person, bool includeReferences = true) { 

            // wrap Person repository object into PersonViewModel object
            PersonId = person.PersonId; 
            FirstName = person.FirstName; 
            LastName = person.LastName; 
            Gender = person.Gender;
            Birthdate = person.Birthdate;  
            Skype = person.Skype; 
            PhoneNumber = person.PhoneNumber;  
            AddressId = person.AddressId; 
            UserId = person.UserId; 
            if(includeReferences && person.Address != null) {
                Address = new AddressViewModel(person.Address, false); 
            }
            if(includeReferences && person.ApplicationUser != null) {
                ApplicationUser = new ApplicationUserViewModel(person.ApplicationUser, false); 
            }
        }

        public int PersonId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public GenderType Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Skype { get; set; }
        public string PhoneNumber { get; set; }
        public int? AddressId { get; set; }
        [Required]
        public string UserId { get; set; }
        public AddressViewModel Address { get; set; }
        public ApplicationUserViewModel ApplicationUser { get; set; }
    }
}