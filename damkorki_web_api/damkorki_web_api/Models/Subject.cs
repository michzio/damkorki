using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DamkorkiWebApi.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        // FK Properties
        public int? SuperSubjectId { get; set; }

        // Navigation Properties
        public Subject SuperSubject { get; set; }
        public List<Subject> SubSubjects { get; set; }

        public List<LessonOffer> LessonOffers { get; set; }



    }
}
