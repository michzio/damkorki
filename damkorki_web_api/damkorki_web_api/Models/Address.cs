using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DamkorkiWebApi.Models
{
    public class Address
    {
        private string _addressLine1; // _AddressLine1, m_addressLine1, m_AddressLine1
        private string _validatedZipCode;

        [Key]
        public int AddressId { get; set; }
        [Required]
        [MaxLength(150)]
        public string AddressLine1 { // Street
            get { return _addressLine1; }
            set { _addressLine1 = value; }
        }
        [Required]
        [MaxLength(150)]
        public string AddressLine2 { get; set; } // House Number
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
        public string Region { get; set; } // State, Province, Voivodeship
        [Required]
        [MaxLength(150)]
        public string Country { get; set; }

        // Navigation properties
        public List<Person> People { get; set; }

    }
}
