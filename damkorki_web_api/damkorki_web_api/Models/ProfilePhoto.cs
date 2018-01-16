using System; 
using System.ComponentModel.DataAnnotations;

namespace DamkorkiWebApi.Models { 

    public class ProfilePhoto { 

        [Key]
        public int ProfilePhotoId { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public Boolean IsProfilePhoto { get; set; }
        public string Caption { get; set; }

        // FK properties 
        public int PersonId { get; set; }
        // Navigation properties 
        public Person Person { get; set; }
        
    }
}

