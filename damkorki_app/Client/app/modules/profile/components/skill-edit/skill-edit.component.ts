import { Component, Inject } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ISkill } from "../../../../models/skill.model";
import { ITutor } from "../../../../models/tutor.model";
import { IPerson } from "../../../../models/person.model";
import { SkillService } from "../../../../services/skill.service";

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

        var tempSkill = <ISkill>{}; 
        tempSkill.name = this.skillName.value;

        this.skillService.createSkill(tempSkill)
            .subscribe( (skill : ISkill) => {

                
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