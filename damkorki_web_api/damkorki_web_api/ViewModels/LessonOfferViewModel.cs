using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.ViewModels { 

    public class LessonOfferViewModel { 

        public LessonOfferViewModel() { }

        public LessonOfferViewModel(LessonOffer lessonOffer, bool includeReferences = true) { 

                // wrap LessonOffer repository object into LessonOfferViewModel object 
                LessonOfferId = lessonOffer.LessonOfferId;
                Title = lessonOffer.Title; 
                Description = lessonOffer.Description; 
                Cost = lessonOffer.Cost;
                Type = lessonOffer.Type; 
                Location = lessonOffer.Location; 
                Level = lessonOffer.Level;
                SubjectId = lessonOffer.SubjectId;
                TutorId = lessonOffer.TutorId;
                if(includeReferences && lessonOffer.Subject != null) {
                    Subject = new SubjectViewModel(lessonOffer.Subject, false); 
                }
                if(includeReferences && lessonOffer.Tutor != null) { 
                    Tutor = new TutorViewModel(lessonOffer.Tutor, false); 
                }
                if(includeReferences && lessonOffer.LessonOfferTerms != null) { 
                    Terms = lessonOffer.LessonOfferTerms.Select( lot => new TermViewModel(lot.Term, false) ).ToList(); 
                }
                if(includeReferences && lessonOffer.Reservations != null) { 
                    Reservations = lessonOffer.Reservations.Select(r => new ReservationViewModel(r, false) ).ToList(); 
                }
        }
        
        public int LessonOfferId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        public int Cost { get; set; }
        [Required]
        public LessonOffer.LessonType Type { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public LessonOffer.LevelType Level { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int TutorId { get; set; }

        public SubjectViewModel Subject { get; set; }
        public TutorViewModel Tutor { get; set; }

        public List<TermViewModel> Terms { get; set; }
        public List<ReservationViewModel> Reservations { get; set; }
    }
}