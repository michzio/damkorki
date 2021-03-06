﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DamkorkiWebApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? FailedLoginDate { get; set; }

        // Navigation Properties
        public Person Person { get; set; }
        public virtual List<RefreshToken> RefreshTokens { get; set; }
    }
}
