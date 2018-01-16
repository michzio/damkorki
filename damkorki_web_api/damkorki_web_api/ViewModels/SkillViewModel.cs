
using System.Collections.Generic;
using System.Linq;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.ViewModels { 

    public class SkillViewModel { 

        public SkillViewModel() { }

        public SkillViewModel(Skill skill, bool includeReferences = true) { 
            
            // wrap Skill repository object into SkillViewModel object 
            SkillId = skill.SkillId; 
            Name = skill.Name; 
            if(includeReferences  && skill.TutorSkills != null) { 
                Tutors = skill.TutorSkills.Select(ts => new TutorViewModel(ts.Tutor, false)).ToList(); 
            }
        }

        public int SkillId { get; set; }
        public string Name { get; set; }

        public List<TutorViewModel> Tutors { get; set; }
    }
}