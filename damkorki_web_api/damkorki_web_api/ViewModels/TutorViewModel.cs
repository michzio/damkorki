using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.ViewModels { 

    public class TutorViewModel { 

        public TutorViewModel() { }
        
        public TutorViewModel(Tutor tutor, bool includeReferences = true) { 

            // wrap Tutor repository object into TutorViewModel object 
            TutorId = tutor.TutorId; 
            Description = tutor.Description; 
            Qualifications = tutor.Qualifications; 
            IsSuperTutor = tutor.IsSuperTutor; 
            PersonId = tutor.PersonId;
            if(includeReferences && tutor.Person != null) { 
                Person = new PersonViewModel(tutor.Person, false); 
            }
            if(includeReferences && tutor.LessonOffers != null) { 
                LessonOffers = tutor.LessonOffers.Select(lo => new LessonOfferViewModel(lo, false) ).ToList(); 
            }
            if(includeReferences && tutor.Reservations != null) { 
                Reservations = tutor.Reservations.Select(r => new ReservationViewModel(r, false) ).ToList(); 
            }
            if(includeReferences && tutor.Experiences != null) { 
                Experiences = tutor.Experiences.Select(e => new ExperienceViewModel(e, false) ).ToList(); 
            }
            if(includeReferences && tutor.TutorSkills != null) { 
                Skills = tutor.TutorSkills.Select(ts => new SkillViewModel(ts.Skill, false)).ToList();
            }
        }

        public int TutorId { get; set; }
        public string Description { get; set; }
	    public string Qualifications { get; set; }
	    public bool IsSuperTutor { get; set; }
        [Required]
        public int PersonId { get; set; }
        public PersonViewModel Person { get; set; }
        public List<LessonOfferViewModel> LessonOffers { get; set; }
        public List<ReservationViewModel> Reservations { get; set; }
        public List<ExperienceViewModel> Experiences { get; set; }
        public List<SkillViewModel> Skills { get; set; }
	
    }

}