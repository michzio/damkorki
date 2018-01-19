import { Component, Inject } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs/Observable";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ISkill } from "../../../../models/skill.model";
import { ITutor } from "../../../../models/tutor.model";
import { IPerson } from "../../../../models/person.model";
import { SkillService } from "../../../../services/skill.service";
import { TutorService } from "../../../../services/tutor.service";
import { TutorSkillService } from "../../../../services/tutor-skill.service";

@Component({
    selector: 'skill-edit', 
    templateUrl: 'skill-edit.component.html',
    styleUrls: ['./skill-edit.component.css']    
})
export class SkillEditComponent { 

    tutor : ITutor = null; 
    person : IPerson = null; 

    skillForm : FormGroup = null;

    updateFailed : boolean = false; 
    updateMessage : string = null;
    
    constructor(private fb : FormBuilder, 
                private skillService : SkillService,
                private tutorService : TutorService,  
                private tutorSkillService : TutorSkillService, 
                @Inject(MAT_DIALOG_DATA) private data : any, 
                private dialogRef : MatDialogRef<SkillEditComponent>) { 
        
        this.tutor = data.tutor; 
        this.person = data.person;

        this.createForm(); 
    }

    private createForm() { 
        this.skillForm = this.fb.group({
            skillName: ['', Validators.required]
        });
    }

    onSubmit(event) { 
        event.preventDefault(); 

        if(!this.skillForm.valid) { 
            // skill addition failed
            this.updateFailed = true; 
            this.skillForm.setErrors({
                "update" : "Skill form invalid"
            });
            return; 
        }

        let sources = []; 

        if(!this.tutor.tutorId) {
            // create empty Tutor profile if it doesn't exists
            var tempTutor = <ITutor>{}; 
            tempTutor.personId = this.person.personId; 

            sources.push(this.tutorService.createTutor(tempTutor)); 
        } else {
            sources.push(Observable.of(this.tutor));
        }
        if(this.skillName.value) {
            // create new Skill if there is skill name value 
            var tempSkill = <ISkill>{}; 
            tempSkill.name = this.skillName.value;
            
            sources.push(this.skillService.createSkill(tempSkill)); 
        } else { 
            sources.push(Observable.of(null)); 
        }

        Observable.forkJoin(...sources).subscribe( (values) => {

            // if there is no new Skill
            if(values[1] == null) Observable.throw(null);

            // add new Skill to Tutor 
            this.tutorSkillService.addSkillToTutor((<ISkill>values[1]).skillId, 
                                                   (<ITutor>values[0]).tutorId)
                    .subscribe( (tutorSkill) => { 
                        // success 
                        this.updateFailed = false; 
                        this.updateMessage = `Skill ${(<ISkill>values[1]).name} assigned to tutor.`;

                        this.dialogRef.close({
                            status : 'yes', 
                            message : this.updateMessage,
                            tutor: <ITutor>values[0],
                            skill: <ISkill>values[1],
                            tutorSkill: tutorSkill
                        }); 
                    }, (error) => { 
                        // skill addition to tutor failed
                        this.updateFailed = true; 
                        this.skillForm.setErrors({
                            "update" : "Could not add skill to tutor"
                        });
                    }); 
            }, (error) => { 
                // skill addition failed 
                this.updateFailed = true; 
                this.skillForm.setErrors({
                    "update" : "Could not create skill"
                })
            });
    }

    get skillName() { return this.skillForm.get('skillName'); }
}