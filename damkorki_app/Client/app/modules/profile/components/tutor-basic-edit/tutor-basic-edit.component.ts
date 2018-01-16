import { Component, Input, Output, EventEmitter } from '@angular/core'; 
import { FormGroup, FormBuilder } from '@angular/forms';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { CanDeactivateComponent } from '../../../../services/can-deactivate-guard.service';
import { AuthService } from '../../../../services/auth.service';
import { ITutor } from '../../../../models/tutor.model';
import { TutorService } from '../../../../services/tutor.service';
import { IPerson } from '../../../../models/person.model';

@Component({
    selector: 'tutor-basic-edit', 
    templateUrl: 'tutor-basic-edit.component.html', 
    styleUrls: ['./tutor-basic-edit.component.css'],
})
export class TutorBasicEditComponent implements CanDeactivateComponent {

    @Input() person : IPerson;
    
    private _tutor : ITutor;  
    get tutor(): ITutor { 
        return this._tutor; 
    }
    @Input() 
    set tutor(tutor: ITutor) { 
        this._tutor = tutor; 
        
        this.updateFormValues(); 
    }

    @Output() tutorChange = new EventEmitter<ITutor>();

    tutorForm: FormGroup = null; 
    updateFailed = false; 
    updateMessage: string = null;
    
    constructor(private fb : FormBuilder, 
                private tutorService : TutorService, 
                private authService : AuthService) { 

            this.createForm();
    }

    private createForm() 
    { 
        this.tutorForm = this.fb.group({
            description: '',
            qualifications: '',
        }); 
    }

    private updateFormValues() 
    { 
        this.tutorForm.setValue({
            description: this.tutor && this.tutor.description ? this.tutor.description : '',
            qualifications: this.tutor && this.tutor.qualifications ? this.tutor.qualifications : '',
        }); 
    }

    canDeactivate(currentRoute: ActivatedRouteSnapshot,
                  currentState: RouterStateSnapshot,
                  nextState: RouterStateSnapshot) : boolean | Observable<boolean> | Promise<boolean> { 

        if(this.tutorForm.dirty) { 
            // ask the user with the dialog service and return its 
            // observable which resolves to true or false when the user decides
            // return this.dialogService.confirm("Discard changes?"); 
            return window.confirm('Discard changes?'); 
        }

        return true; 
    }

    onSubmit(event) 
    { 
        event.preventDefault(); 
       
        if(!this.tutorForm.valid) { 
            this.updateFailed = true; 
            this.tutorForm.setErrors({
                "update" : "Tutor data form invalid."
            })
            return; 
        }

        // build a temporary tutor object from form values 
        var tempTutor = <ITutor>{}; 
        tempTutor.description = this.description.value; 
        tempTutor.qualifications = this.qualifications.value;
        if(this.tutor.tutorId) {
            // update tutor
            tempTutor.tutorId = this.tutor.tutorId; 
            tempTutor.isSuperTutor = this.tutor.isSuperTutor; 
            tempTutor.personId = this.tutor.personId; 

            this.tutorService.updateTutor(tempTutor.tutorId, tempTutor)
                        .subscribe((tutor) => { 
                            this.tutor = tutor;
                            this.tutorChange.emit(tutor); 
                            console.log("Tutor with id " + this.tutor.tutorId + " has been updated.");  
                            this.updateFailed = false; 
                            this.updateMessage = "Tutor data has been updated successfully.";
                            this.resetForm(); 
                        }, (error) => { 
                            console.log(error); 
                            this.updateFailed = true; 
                            this.tutorForm.setErrors({
                                "update" : "Tutor data update has failed."
                            });
                        }); 
        } else { 
            // create tutor
            tempTutor.isSuperTutor = false;
            tempTutor.personId = this.person.personId; 

            this.tutorService.createTutor(tempTutor)
                        .subscribe((tutor) => { 
                            this.tutor = tutor; 
                            this.tutorChange.emit(tutor);
                            console.log("New tutor with id " + this.tutor.tutorId + " has been created.");
                            this.updateFailed = false; 
                            this.updateMessage = "Tutor has been created successfully.";
                            this.resetForm(); 
                        }, (error) => { 
                            console.log(error); 
                            this.updateFailed = true; 
                            this.tutorForm.setErrors({
                                "update" : "Tutor creation has failed."
                            })
                        })
        }
    }

    private resetForm() { 
        this.createForm(); 
        this.updateFormValues(); 
    }

    get isUpdateMessage() { return this.updateMessage; }

    get description() { return this.tutorForm.get('description'); }
    get qualifications() { return this.tutorForm.get('qualifications'); }
}