using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DamkorkiWebApi.Models
{
    public class Tutor
    {
	        [Key]
	        public int TutorId { get; set; }
	        public string Description { get; set; }
	        public string Qualifications { get; set; }
	        public bool IsSuperTutor { get; set; }

	        // FK properties
	        public int PersonId { get; set; }

	        // Navigation properties
	        public Person Person { get; set; }

	        public List<LessonOffer> LessonOffers { get; set; }
	        public List<Reservation> Reservations { get; set; }
			public List<Experience> Experiences { get; set; }

			public List<TutorSkill> TutorSkills { get; set; }


    }
}
