import { Component } from "@angular/core"; 
import { FormGroup, FormBuilder } from "@angular/forms";
import { validateAddress } from "../address-area/address-area.component";
import { CanDeactivateComponent } from '../../../../services/can-deactivate-guard.service';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs/Observable";
import { PersonService } from "../../../../services/person.service";
import { AuthService } from "../../../../services/auth.service";
import { AddressService } from "../../../../services/address.service";
import { IPerson } from "../../../../models/person.model";
import { IAddress } from "../../../../models/address.model";

@Component({
    selector: "addresses-edit", 
    templateUrl: "addresses-edit.component.html", 
    styleUrls: ["./addresses-edit.component.css"]
})
export class AddressesEditComponent implements CanDeactivateComponent { 

    person : IPerson = null; 
    profileAddress : IAddress = null; 

    form : FormGroup = null; 

    updateFailed = false; 
    updateMessage : string = null; 

    constructor(private fb : FormBuilder, 
                private addressService : AddressService, 
                private personService : PersonService, 
                private authService : AuthService) {
        
        this.createForm(); 
        this.loadCurrentAddress(); 
     }

    createForm() { 
        this.form = this.fb.group({
            address: this.fb.control({
                addressLine1: '',
                addressLine2: '',
                zipCode: '',
                city: '', 
                region: '', 
                country: ''
            }, validateAddress)
        });
    }

    loadCurrentAddress() 
    {
        this.personService.getPersonByUser(this.authService.decodedAccessToken().uid)
                    .subscribe( (person) => { 
                        this.person = person; 
                        // load current address 
                        this.addressService.getAddressByPerson(person.personId)
                            .subscribe( (address) => { 
                                this.profileAddress = address; 
                            }, (error) => { 
                                this.profileAddress = <IAddress>{}; 
                            }, () => { // on complete
                                this.updateFormValues(); 
                            }) 
                    }, (error) => { 
                        this.person = null; 
                    });
    }

    updateFormValues() { 

        console.log(this.profileAddress); 

        this.form.setValue({
            address: {
                addressLine1: this.profileAddress.addressLine1,
                addressLine2: this.profileAddress.addressLine2,
                zipCode: this.profileAddress.zipCode,
                city: this.profileAddress.city, 
                region: this.profileAddress.region, 
                country: this.profileAddress.country
            }
        });
    }

    onSubmit(event) { 
        event.preventDefault(); 

        if(!this.form.valid) { 
            this.updateFailed = true; 
            this.form.setErrors({
                "update" : "Address form invalid."
            })
            return; 
        }

        // build a temporary address object from form values 
        var tempAddress = <IAddress>{}; 
        tempAddress = <IAddress>(this.address.value);

        if(this.profileAddress.addressId) { 
            // update address
            tempAddress.addressId = this.profileAddress.addressId; 

            this.addressService.updateAddressForPerson(this.person.personId, tempAddress.addressId, tempAddress)
                        .subscribe((address) => { 
                            this.profileAddress = address; 
                            console.log("Address with id " + this.profileAddress.addressId + " has been updated.");
                            this.updateFailed = false; 
                            this.updateMessage = "Profile address has been updated successfully.";
                            this.resetForm(); 
                        }, (error) => { 
                            console.log(error); 
                            this.updateFailed = true; 
                            this.form.setErrors({
                                "update" : "Address update has failed."
                            })
                        });
        } else { 
            // create profile address 
            this.addressService.createAddressForPerson(this.person.personId, tempAddress)
                        .subscribe((address) => { 
                            this.profileAddress = address; 
                            console.log("Address has been created with id " + this.profileAddress.addressId + "."); 
                            this.updateFailed = false; 
                            this.updateMessage = "Profile address has been created successfully."; 
                            this.resetForm(); 
                        }, (error) => { 
                            console.log(error); 
                            this.updateFailed = true; 
                            this.form.setErrors({
                                "update" : "Address creation has failed."
                            })
                        });
        }
    }

    canDeactivate(currentRoute: ActivatedRouteSnapshot, 
                  currentState: RouterStateSnapshot, 
                  nextState: RouterStateSnapshot) : boolean | Observable<boolean> | Promise<boolean> { 
              
            if(this.form.dirty) { 
                // ask the user with the dialog service and return its 
                // observable which resolves to true or false when the user decides
                return window.confirm('Discard changes?'); 
            }

            return true; 
    } 

    resetForm() { 
        this.createForm();
        this.updateFormValues(); 
    }

    get isProfileCreated() { return this.person != null; }
    get isUpdateMessage() { return this.updateMessage != null; }

    get address() { return this.form.get('address'); }
}