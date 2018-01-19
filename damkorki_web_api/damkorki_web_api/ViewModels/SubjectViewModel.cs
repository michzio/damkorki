
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.ViewModels { 

    public class SubjectViewModel {

        public SubjectViewModel() { }

        public SubjectViewModel(Subject subject, bool includeReferences = true) { 
            
            // wrap Subject repository object into SubjectViewModel object
            SubjectId = subject.SubjectId; 
            Name = subject.Name; 
            Description = subject.Description; 
            Image = subject.Image; 
            SuperSubjectId = subject.SuperSubjectId; 
            if(includeReferences && subject.SuperSubject != null) { 
                SuperSubject = new SubjectViewModel(subject.SuperSubject, false);
            }
            if(includeReferences && subject.SubSubjects != null) { 
                SubSubjects = subject.SubSubjects.Select( ss => new SubjectViewModel(ss, false) ).ToList(); 
            }
            if(includeReferences && subject.LessonOffers != null) { 
                LessonOffers = subject.LessonOffers.Select(lo => new LessonOfferViewModel(lo, false) ).ToList(); 
            }
        }

        public int SubjectId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int? SuperSubjectId { get; set; }

        public SubjectViewModel SuperSubject { get; set; }
        public List<SubjectViewModel> SubSubjects { get; set; }
        public List<LessonOfferViewModel> LessonOffers { get; set; }
    }
}