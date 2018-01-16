using System.ComponentModel.DataAnnotations;
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
    }
}