import { Component, forwardRef, EventEmitter, OnInit, Input, HostListener } from "@angular/core";
import { FormGroup, FormControl, AbstractControl, NgControl } from "@angular/forms";
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from "@angular/forms";
import { NG_VALIDATORS, Validators } from "@angular/forms";

export function validateAddress(c: FormControl) { 

    var isValid = true; 

    let address = c.value; 

    if(!address['addressLine1'] || !address['addressLine2'] || 
       !address['zipCode'] || !address['city'] || !address['region'] ||
       !address['country'])  
       { 
           isValid = false; 
       }

    return isValid ? null : { address : true }; 
}

@Component({
    selector: 'address-area', 
    templateUrl: 'address-area.component.html', 
    styleUrls: ['./address-area.component.css'],
    providers: [
        /* {
            provide: NG_VALUE_ACCESSOR, 
            useExisting: forwardRef(() => AddressAreaComponent),
            multi: true
        } */ 
        /* { 
            provide: NG_VALIDATORS, 
            useExisting: forwardRef(() => AddressAreaComponent), 
            multi: true
        } */ 
    ]
})
export class AddressAreaComponent implements ControlValueAccessor, OnInit { 

    @Input('form-failed') formFailed : boolean  = false;
    @Input('control-errors') controlErrors : boolean = true; 

    addressForm : FormGroup = new FormGroup({
        addressLine1: new FormControl('', Validators.required),    // street 
        addressLine2: new FormControl('', Validators.required),    // home number 
        zipCode : new FormControl('', Validators.required), 
        city: new FormControl('', Validators.required), 
        region: new FormControl('', Validators.required),          // region, state, province, voivodeship
        country: new FormControl('', Validators.required), 
    })

    onChange: EventEmitter<any> = new EventEmitter();
    // function to call when inputs are touched 
    onTouched: () => void = () => {}; 

    constructor(private hostControl: NgControl) {
        hostControl.valueAccessor = this; 

        this.addressForm.valueChanges.subscribe((val : any) => { 
        
            this.onChange.emit(val);
            this.propagateErrors(); 
        })
    }

    ngOnInit() { 
        this.propagateErrors();  
    }

    propagateErrors() { 

       var hostErrors = this.hostControl.control.errors || {}; 

        Object.keys(this.addressForm.controls).forEach( (key) => { 
            let errors = this.addressForm.get(key).errors; 
            if(errors) { 
                hostErrors[key] = errors;
            } else { 
                delete hostErrors[key];
            }
        });

        if(Object.keys(hostErrors).length == 0) hostErrors = null; 

        this.hostControl.control.setErrors(hostErrors);
    }

    writeValue(val: any): void {
        val && this.addressForm.setValue(val, { emitEvent: false }); 
    }
    registerOnChange(fn: (val:any) => void ): void {
        this.onChange.subscribe(fn);
    }
    registerOnTouched(fn: () => void): void {
        this.onTouched = fn; 
    }
    setDisabledState?(isDisabled: boolean): void {
        isDisabled ? this.addressForm.disable() 
                   : this.addressForm.enable(); 
    }

    get addressLine1() { return this.addressForm.get('addressLine1'); }
    get addressLine2() { return this.addressForm.get('addressLine2'); }
    get zipCode() { return this.addressForm.get('zipCode'); }
    get city() { return this.addressForm.get('city'); }
    get region() { return this.addressForm.get('region'); }
    get country() { return this.addressForm.get('country'); }
}