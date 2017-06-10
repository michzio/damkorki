using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DamkorkiWebApi.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }
        [Required]
        [MaxLength(1)]
        public int Rating { get; set; }
        [Required]
        public string Comment { get; set; }
        public string Demmenti { get; set; }

        // FK properties
        public int ReservationId { get; set; }

        // Navigation properties
        public Reservation Reservation { get; set; }
    }
}
