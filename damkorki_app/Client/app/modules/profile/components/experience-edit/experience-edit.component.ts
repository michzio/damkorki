import { Component, Inject } from "@angular/core";
import { FormGroup, FormBuilder } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";

@Component({ 
    selector: 'experience-edit',
    templateUrl: 'experience-edit.component.html',
    styleUrls: ['./experience-edit.component.css']
})
export class ExperienceEditComponent { 

    experienceForm : FormGroup = null;
    
    updateFailed : boolean = false; 
    updateMessage : string = null; 

    constructor(private fb : FormBuilder,
                @Inject(MAT_DIALOG_DATA) private data : any, 
                private dialogRef : MatDialogRef<ExperienceEditComponent>) {

        this.createForm(); 
    }

    private createForm() { 
        
    }
}