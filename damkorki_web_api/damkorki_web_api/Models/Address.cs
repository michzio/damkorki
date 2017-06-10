using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DamkorkiWebApi.Models
{
    public class Address
    {
        private string _street; // _Street, m_street, m_Street
        private string _validatedZipCode;

        [Key]
        public int AddressId { get; set; }
        [Required]
        [MaxLength(150)]
        public string Street {
            get { return _street; }
            set { _street = value; }
        }
        [Required]
        [MaxLength(10)]
        public string HomeNumber { get; set; }
        [Required]
        [MaxLength(6)]
        [MinLength(6)]
        public string ZipCode
        {
            get { return _validatedZipCode; }
            set { _validatedZipCode = value; }
        }
        [Required]
        [MaxLength(150)]
        public string City { get; set; }
        [Required]
        [MaxLength(150)]
        public string Voivodeship { get; set; }
        [Required]
        [MaxLength(150)]
        public string Country { get; set; }

        // Navigation properties
        public List<Person> People { get; set; }

    }
}
