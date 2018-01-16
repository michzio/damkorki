import { Component, Input, Output, EventEmitter } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { IPerson } from "../../../../models/person.model";
import { ITutor } from "../../../../models/tutor.model";
import { ISkill } from "../../../../models/skill.model";
import { AuthService } from "../../../../services/auth.service";
import { TutorService } from "../../../../services/tutor.service";
import { SkillEditComponent } from "../skill-edit/skill-edit.component";

@Component({
    selector: 'tutor-skills-edit',
    templateUrl: 'tutor-skills-edit.component.html',
    styleUrls: ['./tutor-skills-edit.component.css']
})
export class TutorSkillsEditComponent { 

    @Input() person : IPerson; 
    
    private _tutor : ITutor;
    get tutor() : ITutor {
        return this._tutor; 
    }
    @Input()
    set tutor(tutor: ITutor) { 
        this._tutor = tutor; 

        if(!this.skills) { 
            this.skills = tutor && tutor.skills; 
        }
    }

    @Output() tutorChange = new EventEmitter<ITutor>(); 

    // helper array of tutor's skills 
    // as if tutor member is modified (after update)
    // it can lose eagerly loaded navigation properties 
    private _skills : Array<ISkill>; 
    get skills() : Array<ISkill> { 
        return this._skills; 
    }
    set skills(skills : Array<ISkill>) { 
        this._skills = skills; 
        this.updateSkillsList(); 
    }

    constructor(private fb : FormBuilder, 
                private dialog : MatDialog, 
                private tutorService : TutorService, 
                private authService : AuthService) { 

    }

    private updateSkillsList() { 

    }

    onAddSkill(event) { 
        event.preventDefault(); 

        this.dialog.open(SkillEditComponent, { 
            data: { 
                tutor: this.tutor, 
                person: this.person
            }, 
            width: '60%', 
            height: '50%',
            minWidth: '320px',
            minHeight: '120px'
        })
        .afterClosed().subscribe( (result) => { 
            console.log(result);

        });

    }

    get areSkillsAdded() { return this.skills != null && this.skills.length > 0; }
}