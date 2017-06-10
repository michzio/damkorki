using System;
using System.ComponentModel.DataAnnotations;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.ViewModels { 

    public class TutorViewModel { 

        public TutorViewModel() { }
        
        public TutorViewModel(Tutor tutor) { 
            TutorId = tutor.TutorId; 
            Description = tutor.Description; 
            Qualifications = tutor.Qualifications; 
            IsSuperTutor = tutor.IsSuperTutor; 
            PersonId = tutor.PersonId; 
        }

        public int TutorId { get; set; }
        public string Description { get; set; }
	    public string Qualifications { get; set; }
	    public bool IsSuperTutor { get; set; }
        [Required]
        public int PersonId { get; set; }

	
    }

}