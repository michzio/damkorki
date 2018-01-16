using System;
using System.ComponentModel.DataAnnotations;

namespace DamkorkiWebApi.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }
        public int ReservationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // FK Properties
        [Required]
        public int LearnerId { get; set; }
        [Required]
        public int TutorId { get; set; }
        [Required]
        public int LessonOfferId { get; set; }
        [Required]
        public int TermId { get; set; }

        // Navigation Properties
        public Learner Learner { get; set; } 
        public Tutor Tutor { get; set; }
        public LessonOffer LessonOffer { get; set; }
        public Term Term { get; set; }

        public Feedback Feedback { get; set; }


    }
}