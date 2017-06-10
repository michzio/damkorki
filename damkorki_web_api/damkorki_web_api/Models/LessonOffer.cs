using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DamkorkiWebApi.Models
{
    public class LessonOffer
    {
        public enum LessonType
        {
            None = 0,
            AtTutor = 1,
            AtLearner = 2,
            Online = 4
        }
    
        public enum LevelType
        {
            None = 0,
            Beginner = 1,
            PreIntermediate = 2,
            Intermediate = 4,
            UpperIntermediate = 8,
            Advanced = 16
        }

        [Key]
        public int LessonOfferId { get; set; }
		[Required]
		public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int Cost { get; set; }
        [Required]
        public LessonType Type { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public LevelType Level { get; set; }

        // FK Properties
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int TutorId { get; set; }

        // Navigation Properties
        public Subject Subject { get; set; }
        public Tutor Tutor { get; set; }

        public List<LessonOfferTerm> LessonOfferTerms { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
