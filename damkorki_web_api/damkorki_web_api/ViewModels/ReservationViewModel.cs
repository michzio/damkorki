
using System;
using System.ComponentModel.DataAnnotations;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.ViewModels { 

    public class ReservationViewModel { 

        public ReservationViewModel() { }

        public ReservationViewModel(Reservation reservation, bool includeReferences = true) { 

            // wrap Reservation repository object into ReservationViewModel object 
            ReservationId = reservation.ReservationId; 
            // TODO
        }

        public int ReservationId { get; set; }
        public int ReservationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public int LearnerId { get; set; }
        [Required]
        public int TutorId { get; set; }
        [Required]
        public int LessonOfferId { get; set; }
        [Required]
        public int TermId { get; set; }

        public LearnerViewModel Learner { get; set; }
        public TutorViewModel Tutor { get; set; }
        public LessonOfferViewModel LessonOffer { get; set; }
        public TermViewModel Term { get; set; }
        public FeedbackViewModel Feedback { get; set; }
    }
}