using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamkorkiWebApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? FailedLoginDate { get; set; }

        // Navigation Properties
        public Person Person { get; set; }
    }
}
