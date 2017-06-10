using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamkorkiWebApi.Models
{
    public class LessonOfferTerm
    {
        // FK Properties
        public int TermId { get; set; }
        public int LessonOfferId { get; set; }

        // Navigation Properties
        public Term Term { get; set; }
        public LessonOffer LessonOffer { get; set; }
    }
}
