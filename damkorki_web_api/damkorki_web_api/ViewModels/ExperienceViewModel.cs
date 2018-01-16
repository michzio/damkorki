
using System.ComponentModel.DataAnnotations;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.ViewModels { 

    public class ExperienceViewModel { 

        public ExperienceViewModel() { }

        public ExperienceViewModel(Experience experience, bool includeReferences = true) { 

            ExperienceId = experience.ExperienceId; 
            StartYear = experience.StartYear; 
            EndYear = experience.EndYear; 
            Description = experience.Description; 
            TutorId = experience.TutorId; 
            if(includeReferences && experience.Tutor != null) { 
                Tutor = new TutorViewModel(experience.Tutor, false); 
            }
        }

        public int ExperienceId { get; set; }
        [Required]
        public int StartYear { get; set; }
        [Required]
        public int EndYear { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int TutorId { get; set; }

        public TutorViewModel Tutor { get; set; }

    }
}