using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DamkorkiWebApi.Models
{
    [Table("People")]
    public class Person
    {
        public enum GenderType
        {
            Male,
            Female, 
            Other
        };

        // Properties
        [Key]
        public int PersonId { get; set; }
        [Required]
        [MaxLength(50)]
        [Column("first_name")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [Column("last_name")]
        public string LastName { get; set; }
        [Required]
        public GenderType Gender { get; set; }
        public DateTime Birthdate { get; set; }
		public string Skype { get; set; }
		public string PhoneNumber { get; set; }
        
        // FK properties
        public int? AddressId { get; set; }
        [Required]
        public string UserId { get; set; }

        // Navigation properties
        public Address Address { get; set; }

        public Learner Learner { get; set; }
        public Tutor Tutor { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public List<ProfilePhoto> ProfilePhotos { get; set; }

    }
}
