import { Component } from '@angular/core';
import { CanDeactivateComponent } from '../../../../services/can-deactivate-guard.service';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { PersonService } from "../../../../services/person.service"; 
import { AuthService } from '../../../../services/auth.service'; 
import { IPerson } from '../../../../models/person.model';
import { Gender }  from '../../../../models/enums/gender.enum';
import { Month }  from '../../../../models/enums/month.enum';
import { DateValidators } from '../../../../shared/validators/date.validators';
import { CountryDialingCodes } from '../../../../shared/country-dialing-codes';

@Component({ 
    selector: 'profile-basic-edit', 
    templateUrl: 'profile-basic-edit.component.html',
    styleUrls: ['./profile-basic-edit.component.css'], 
})
export class ProfileBasicEditComponent implements CanDeactivateComponent {
    
    private person : IPerson;
    public genders = Gender; // exporting Gender enum to use in template
    public months = Month; // exporting Month enum to use in template for birth date selection
    public countryDialingCodes = CountryDialingCodes; // exporting CountryDialingCodes map to use in template for coutry calling code selection

    basicProfileForm: FormGroup = null; 
    updateFailed = false; 
    updateMessage : string = null; 

    constructor(private fb : FormBuilder, 
                private personService : PersonService,
                private authService: AuthService ) { 
                
                this.createForm(); 
                this.loadCurrentPerson(); 
    }

    createForm() 
    { 
        this.basicProfileForm = this.fb.group({
            firstName: ["", [Validators.required]],
            lastName: ["", [Validators.required]],
            gender: ["", [Validators.required]],
            birthDate: this.fb.group({
                monthOfBirth: ["", []], 
                dayOfBirth: ["", []], 
                yearOfBirth: ["", []]
            }, { validator: DateValidators.validDay }), 
            phoneNumber: this.fb.group({
                countryDialingCode: ["", []], 
                number: ["", [Validators.pattern('^[0-9]+$')]]
            }), 
            skype: ["", []]  
        });
    }

    loadCurrentPerson() 
    { 
        this.personService.getPersonByUser(this.authService.decodedAccessToken().uid)
                        .subscribe( (person) => { 
                            this.person = person; 
                        }, (error) => { 
                            this.person = <IPerson>{}; 
                        }, 
                        () => { // on complete
                            this.updateFormValues();
                        }); 
    }

    updateFormValues() 
    {
        console.log(this.person); 

         this.basicProfileForm.setValue({
             firstName: this.person.firstName,
             lastName: this.person.lastName,
             gender: Gender[this.person.gender],
             birthDate: { 
                monthOfBirth: this.person.birthdate? 
                                String(this.person.birthdate.getMonth()) : '',
                dayOfBirth: this.person.birthdate? 
                                this.person.birthdate.getDate() : '',
                yearOfBirth: this.person.birthdate? 
                                this.person.birthdate.getFullYear() : ''
             }, 
             phoneNumber: { 
                countryDialingCode: this.person.phoneNumber? 
                                        this.person.phoneNumber.slice(0,2).toLowerCase() : '', 
                number: this.person.phoneNumber? 
                                        this.person.phoneNumber.slice(2) : ''
             },
             skype: this.person.skype, 
         }); 
    }

    canDeactivate(currentRoute: ActivatedRouteSnapshot, 
                  currentState: RouterStateSnapshot, 
                  nextState: RouterStateSnapshot) : boolean | Observable<boolean> | Promise<boolean> { 

        if(this.basicProfileForm.dirty) { 
            // ask the user with the dialog service and return its 
            // observable which resolves to true or false when the user decides 
            // return this.dialogService.confirm("Discard changes?"); 
            return window.confirm('Discard changes?');
        }
        return true;
    }

    hasChanged(...inputNames:  string[]) : boolean { 
        
        for(let name in inputNames) { 
            let control = this.basicProfileForm.get(name); 
            if(control && (control.dirty || control.touched)) { 
                return true; 
            }
        }

        return false; 
    }

    onSubmit(evt: Event) { 
        evt.preventDefault(); 
        console.log(evt); 

        if(!this.basicProfileForm.valid) { 
            this.updateFailed = true; 
            this.basicProfileForm.setErrors({
                "update" : "Profile form invalid." 
            });
            return; 
        }

        // build a temporary person object from form values 
        var tempPerson = <IPerson>{};
        tempPerson.firstName = this.firstName.value; 
        tempPerson.lastName = this.lastName.value; 
        tempPerson.gender = this.gender.value; 
        tempPerson.birthdate = new Date(+this.yearOfBirth.value, 
                                        +this.monthOfBirth.value, 
                                        +this.dayOfBirth.value); 
        tempPerson.skype = this.skype.value; 
        tempPerson.phoneNumber = (<string>this.countryDialingCode.value).toUpperCase()
                                 + this.number.value; 

        if(this.person.personId) { 
            // update person
            tempPerson.personId = this.person.personId; 
            tempPerson.userId = this.person.userId; 
            tempPerson.addressId = this.person.addressId; 

            this.personService.updatePerson(tempPerson.personId, tempPerson)
                        .subscribe((person) => { 
                            this.person = person; 
                            console.log("Person with id " + this.person.personId + " has been updated.");
                            this.updateFailed = false; 
                            this.updateMessage = "Profile has been updated successfully."; 
                            this.resetForm();
                        }, (error) => {
                            console.log(error)
                            this.updateFailed = true; 
                            this.basicProfileForm.setErrors({
                                "update" : "Profile update has failed."
                            }); 
                        });
        } else { 
            // create person
            tempPerson.userId = this.authService.decodedAccessToken().uid; 

            this.personService.createPerson(tempPerson)
                        .subscribe((person) => { 
                            this.person = person; 
                            console.log("New person with id " + this.person.personId + " has been created."); 
                            this.updateFailed = false; 
                            this.updateMessage = "Profile has been created successfully.";
                            this.resetForm(); 
                        }, (error) => { 
                            console.log(error); 
                            this.updateFailed = true; 
                            this.basicProfileForm.setErrors({
                                "update" : "Profile creation has failed."
                            });
                        });
        }
    }

    resetForm() { 
        this.createForm(); 
        this.updateFormValues(); 
    }

    get firstName() { return this.basicProfileForm.get('firstName'); }
    get lastName() { return this.basicProfileForm.get('lastName'); }
    get gender() { return this.basicProfileForm.get('gender'); }
    get birthDate() { return this.basicProfileForm.get('birthDate'); }
    get monthOfBirth() { return this.basicProfileForm.get('birthDate.monthOfBirth'); }
    get dayOfBirth() { return this.basicProfileForm.get('birthDate.dayOfBirth'); }
    get yearOfBirth() { return this.basicProfileForm.get('birthDate.yearOfBirth'); }
    get phoneNumber() { return this.basicProfileForm.get('phoneNumber'); }
    get countryDialingCode() { return this.basicProfileForm.get('phoneNumber.countryDialingCode'); }
    get number() { return this.basicProfileForm.get('phoneNumber.number'); }
    get skype() { return this.basicProfileForm.get('skype'); }
}