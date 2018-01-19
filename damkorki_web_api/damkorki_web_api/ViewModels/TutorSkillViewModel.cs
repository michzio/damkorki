
using System.ComponentModel.DataAnnotations;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.ViewModels { 

    public class TutorSkillViewModel { 

        public TutorSkillViewModel() { }

        public TutorSkillViewModel(TutorSkill tutorSkill,  bool includeReferences = true) { 

            TutorId = tutorSkill.TutorId; 
            SkillId = tutorSkill.SkillId; 
            if(includeReferences && tutorSkill.Tutor != null) {
                Tutor = new TutorViewModel(tutorSkill.Tutor); 
            }
            if(includeReferences && tutorSkill.Skill != null) { 
                Skill = new SkillViewModel(tutorSkill.Skill); 
            }
        }

        [Required]
        public int TutorId { get; set; }
        [Required]
        public int SkillId { get; set; }
        public TutorViewModel Tutor { get; set; }
        public SkillViewModel Skill { get; set; }
    }
}