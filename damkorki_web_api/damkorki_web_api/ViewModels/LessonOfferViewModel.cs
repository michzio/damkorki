using System.ComponentModel.DataAnnotations;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.ViewModels { 

    public class LessonOfferViewModel { 

        public LessonOfferViewModel() { }

        public LessonOfferViewModel(LessonOffer lessonOffer) { 

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
                Subject = lessonOffer.Subject; 
                Tutor = lessonOffer.Tutor; 
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

        public Subject Subject { get; set; }
        public Tutor Tutor { get; set; }
    }
}