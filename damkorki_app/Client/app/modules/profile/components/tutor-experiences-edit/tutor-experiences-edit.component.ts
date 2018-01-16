import { Component, Input, Output, EventEmitter } from '@angular/core'
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { IPerson } from '../../../../models/person.model';
import { ITutor } from '../../../../models/tutor.model';
import { TutorService } from '../../../../services/tutor.service';
import { AuthService } from '../../../../services/auth.service';
import { IExperience } from '../../../../models/experience.model';
import { ExperienceEditComponent } from '../experience-edit/experience-edit.component';


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

    constructor(private fb : FormBuilder, 
                private dialog: MatDialog,
                private tutorService : TutorService, 
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
            }
        })
        .afterClosed().subscribe( (result) => {
            console.log(result); 
            /*
            if(result.status === 'yes') { 

            } */
        });
    }

    get areExperiencesAdded() { return this.experiences != null && this.experiences.length > 0; }
} 