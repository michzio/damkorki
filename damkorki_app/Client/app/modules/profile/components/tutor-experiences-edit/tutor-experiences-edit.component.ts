import { Component, Input, Output, EventEmitter } from '@angular/core'
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { IPerson } from '../../../../models/person.model';
import { ITutor } from '../../../../models/tutor.model';
import { TutorService } from '../../../../services/tutor.service';
import { AuthService } from '../../../../services/auth.service';
import { IExperience } from '../../../../models/experience.model';
import { ExperienceEditComponent, ExperiencEditType } from '../experience-edit/experience-edit.component';
import { ExperienceService } from '../../../../services/experience.service';


@Component({
    selector: 'tutor-experiences-edit',
    templateUrl: 'tutor-experiences-edit.component.html',
    styleUrls: ['./tutor-experiences-edit.component.css']
})
export class TutorExperiencesEditComponent { 

    @Input() person : IPerson; 

    private _tutor : ITutor; 
    get tutor() : ITutor { 
        return this._tutor; 
    }
    @Input() 
    set tutor(tutor: ITutor) { 
        this._tutor = tutor; 

        if(!this.experiences) { 
            this.experiences = tutor && tutor.experiences;
            this.experiences = this.experiences && this.experiences.sort( (a, b) => {
                if(a.startYear < b.startYear) return 1; 
                if(a.startYear > b.startYear) return -1; 
                return b.endYear - a.endYear;
            });
        }
    }

    @Output() tutorChange = new EventEmitter<ITutor>(); 

    // helper array of tutor's expiriences 
    // as if tutor member is modified (after update)
    // it can lose eagerly loaded navigation properties 
    private _experiences : Array<IExperience>; 
    get experiences() : Array<IExperience> { 
        return this._experiences;
    }
    set experiences(experiences : Array<IExperience>) { 
        this._experiences = experiences;
        this.updateExperiencesList();
    }

    @Output() statusChange : EventEmitter<any> = new EventEmitter(); 

    constructor(private fb : FormBuilder, 
                private dialog: MatDialog,
                private tutorService : TutorService, 
                private experienceService : ExperienceService, 
                private authService : AuthService) {

    }

    private updateExperiencesList() { 
        
    }

    onAddExperience(event) { 
        event.preventDefault(); 

        this.dialog.open(ExperienceEditComponent, { 
            data: { 
                tutor: this.tutor,
                person: this.person
            }, 
            width: '60%', 
            minWidth: '320px',
            minHeight: '50%'
        })
        .afterClosed().subscribe( (result) => {
            console.log(result); 
            if(result.status == 'yes') { 
                this.tutorChange.emit(result.tutor); 
                this.experiences = this.experiences ? [...this.experiences, result.experience] : [result.experience];
                
                this.experiences = this.experiences.sort( (a, b) => {
                    if(a.startYear < b.startYear) return 1; 
                    if(a.startYear > b.startYear) return -1; 
                    return b.endYear - a.endYear;
                });
            }
        });
    }

    onEditExperience(event, experience) { 
        event.preventDefault(); 
        
        this.dialog.open(ExperienceEditComponent, { 
            data: {
                tutor: this.tutor, 
                person: this.person,
                experience: experience
            },
            width: '60%', 
            minWidth: '320px',
            minHeight: '50%'
        })
        .afterClosed().subscribe( (result) => { 
            console.log(result); 
            if(result.status == 'yes') { 
                this.tutorChange.emit(result.tutor); 
                this.experiences = this.experiences && this.experiences.filter(e => e.experienceId != result.experience.experienceId); 

                if(result.type == ExperiencEditType.UPDATE) {
                    this.experiences = this.experiences ? [...this.experiences, result.experience] : [result.experience];
                }

                this.experiences = this.experiences && this.experiences.sort( (a, b) => {
                    if(a.startYear < b.startYear) return 1; 
                    if(a.startYear > b.startYear) return -1; 
                    return b.endYear - a.endYear;
                });
            }
        });

    }

    get areExperiencesAdded() { return this.experiences != null && this.experiences.length > 0; }
} 