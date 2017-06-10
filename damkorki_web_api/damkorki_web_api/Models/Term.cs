using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DamkorkiWebApi.Models
{
    public class Term
    {
        [Key]
        public int TermId { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }

        // FK Properties

        // Navigation Properties
        public List<LessonOfferTerm> LessonOfferTerms { get; set; }
        public List<Reservation> Reservations { get; set; }

    }
}
