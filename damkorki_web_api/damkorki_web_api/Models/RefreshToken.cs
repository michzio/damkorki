using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DamkorkiWebApi.Models
{
    public class RefreshToken { 

        [Key]
        [Required]
        public int RefreshTokenId { get; set; }

        [Required]
        public string ClientId { get; set; }
        [Required]
        public String TokenValue { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        // FK Properties 
        [Required]
        public string UserId { get; set; }

        // Navigation Properties 
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}