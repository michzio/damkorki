using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DamkorkiWebApi.Models
{
    public class Learner
    {   
        [Key]
        public int LearnerId { get; set; }
        public string Description { get; set; }
    
        // FK properties
        public int PersonId { get; set; }

        // Navigation properties
        public Person Person { get; set; }

        public List<Reservation> Reservations { get; set; }
    }
}
