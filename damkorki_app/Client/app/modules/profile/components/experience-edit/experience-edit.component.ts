import { Component, Inject } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs/Observable";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ITutor } from "../../../../models/tutor.model";
import { IPerson } from "../../../../models/person.model";
import { TutorService } from "../../../../services/tutor.service";
import { ExperienceService } from "../../../../services/experience.service";
import { IExperience } from "../../../../models/experience.model";

export enum ExperiencEditType {
    INSERT, UPDATE, DELETE
}; 

@Component({ 
    selector: 'experience-edit',
    templateUrl: 'experience-edit.component.html',
    styleUrls: ['./experience-edit.component.css']
})
export class ExperienceEditComponent { 

    experience : IExperience = null; 
    tutor : ITutor = null; 
    person : IPerson = null; 

    experienceForm : FormGroup = null;
    
    updateFailed : boolean = false; 
    updateMessage : string = null; 

    constructor(private fb : FormBuilder,
                private experienceService : ExperienceService, 
                private tutorService : TutorService, 
                @Inject(MAT_DIALOG_DATA) private data : any, 
                private dialogRef : MatDialogRef<ExperienceEditComponent>) {

        this.experience = data.experience; 
        this.tutor = data.tutor; 
        this.person = data.person;

        this.createForm(); 
    }

    private createForm() { 

        this.experienceForm = this.fb.group({
            startYear: [ this.experience ? this.experience.startYear : '', Validators.required], 
            endYear: [this.experience ? this.experience.endYear : '', Validators.required],
            experienceDescription: [this.experience ? this.experience.description : '', Validators.required]
        });
    }

    onSubmit(event) { 
        event.preventDefault(); 

        if(!this.experienceForm.valid) { 
            // experience update failed 
            this.updateFailed = true; 
            this.experienceForm.setErrors({
                "update" : "Experience form invalid. All fields required."
            })
            return; 
        }

        let tutorSource;

        if(!this.tutor.tutorId) { 
            // create empty Tutor profile if it doesn't exists
            var tempTutor = <ITutor>{}; 
            tempTutor.personId = this.person.personId; 
            tutorSource = this.tutorService.createTutor(tempTutor); 
        } else { 
            tutorSource = Observable.of(this.tutor); 
        }

        tutorSource.subscribe( (tutor) => { 

            // build a temporary experience object from form values 
            var tempExperience = <IExperience>{}; 
            tempExperience.startYear = this.startYear.value; 
            tempExperience.endYear = this.endYear.value; 
            tempExperience.description = this.experienceDescription.value; 
            if(this.experience && this.experience.experienceId) { 
                // update experience 
                tempExperience.experienceId = this.experience.experienceId; 
                tempExperience.tutorId = this.experience.tutorId; 

                this.experienceService.updateExperience(tempExperience.experienceId, tempExperience)
                        .subscribe((experience) => {
                            // success 
                            this.updateFailed = false; 
                            this.updateMessage = `Experience has been updated successfully.`; 

                            this.dialogRef.close({
                                status: 'yes', 
                                type: ExperiencEditType.UPDATE,
                                message: this.updateMessage, 
                                tutor: <ITutor>tutor, 
                                experience: <IExperience>experience
                            })
                        }, (error) => { 
                            // experience edit failed 
                            this.updateFailed = true; 
                            this.experienceForm.setErrors({
                                "update" : "Could not update experience."
                            });
                        });
            } else { 
                // create experience 
                tempExperience.tutorId = tutor.tutorId; 

                this.experienceService.createExperience(tempExperience)
                        .subscribe((experience) => { 
                            // sucess 
                            this.updateFailed = false; 
                            this.updateMessage = "Experience has been added to Tutor.";

                            this.dialogRef.close({
                                status: 'yes', 
                                type: ExperiencEditType.INSERT,
                                message: this.updateMessage, 
                                tutor: <ITutor>tutor, 
                                experience: <IExperience>experience
                            })
                        }, (error) => { 
                            // experience addition failed
                            this.updateFailed = true; 
                            this.experienceForm.setErrors({
                                "update" : "Could not add experience."
                            });
                        });
            }
        }); 
    }

    onDelete(event) { 
        event.preventDefault(); 

        if(!this.isExperience) { 
            return; 
        }

        this.experienceService.deleteExperience(this.experience.experienceId)
            .subscribe((success) => { 
                // success 
                this.updateFailed = false; 
                this.updateMessage = "Experience has been deleted";

                this.dialogRef.close({
                    status: 'yes', 
                    type: ExperiencEditType.DELETE,
                    message: this.updateMessage, 
                    tutor: <ITutor>this.tutor, 
                    experience: <IExperience>this.experience
                })
            }, (error) => { 
                // experience deletion failed
                this.updateFailed = true; 
                this.experienceForm.setErrors({
                    "update" : "Could not delete experience."
                })
            }); 
    }

    get isExperience() { return this.experience != null; }

    get startYear() { return this.experienceForm.get('startYear'); }
    get endYear() { return this.experienceForm.get('endYear'); }
    get experienceDescription() { return this.experienceForm.get('experienceDescription'); }
}