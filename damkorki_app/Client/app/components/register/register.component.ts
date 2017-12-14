import { Component } from "@angular/core"; 
import { FormBuilder, FormGroup, FormControl, Validators } from "@angular/forms"; 
import { Router } from "@angular/router"; 
import { HttpClient } from "@angular/common/http"; 
import { IUser } from "../../models/user.model";
import { HttpErrorResponse } from "@angular/common/http/src/response";
import { IPerson } from "../../models/person.model";

@Component({
    selector: "register", 
    templateUrl: "./register.component.html", 
    styleUrls: ['./register.component.css']
})
export class RegisterComponent { 

    // Web Api register path 
    private registerUrl = "http://localhost:5050/users"; 
    // Web Api person profile path 
    private personUrl = "http://localhost:5050/persons"; 

    registerForm : FormGroup = null; 
    registerFailed = false;

    constructor(private fb: FormBuilder, 
                private router: Router, 
                private httpClient: HttpClient) { 

                this.createForm(); 
    }

    createForm() { 

        this.registerForm = this.fb.group({
            email: ["", [Validators.required, Validators.email]], 
            firstName: ["", [Validators.required]],
            lastName: ["", [Validators.required]], 
            password: ["", [Validators.required, 
                            Validators.minLength(6), 
                            PasswordValidators.containsDigit, 
                            PasswordValidators.containsLowercaseLetter, 
                            PasswordValidators.containsUppercaseLetter, 
                            PasswordValidators.containsSpecialCharacter]], 
            termsOfService: [true, [Validators.requiredTrue]]
        }, {
            validator: PasswordValidators.notContainsValuesOfInputs(['email', 'firstName', 'lastName'])
        }); 
    }

   onSubmit(evt: Event) { 
        evt.preventDefault(); 
        console.log(evt); 

        if(this.registerForm.valid) { 

            // create new user 
            let user : IUser = { 
                userName : this.email.value,
                email: this.email.value,
                password: this.password.value
            }; 

            // post user
            this.httpClient.post<IUser>(this.registerUrl, user)
                           .subscribe((user) => {
                                if(user && user.userId) { 
                                    console.log("User " + user.userName + " has been created with id: " + user.userId + ".");

                                    // create new person 
                                    let person : IPerson = { 
                                        firstName : this.firstName.value, 
                                        lastName : this.lastName.value, 
                                        gender: 0, 
                                        userId: user.userId
                                    }

                                    this.httpClient.post<IPerson>(this.personUrl, person)
                                                   .subscribe((person) => { 
                                                       if(person && person.personId) { 
                                                           console.log("Person " + person.firstName + " " + person.lastName + 
                                                                        " has been created with id: " + person.personId + "."); 

                                                             // redirect to login page
                                                            this.router.navigate(["/login"]); 
                                                       } else { 
                                                           // profile creation failed 
                                                            this.registerFailed = true; 
                                                            this.registerForm.setErrors({
                                                                "register" : "Profile creation failed"
                                                            }) 
                                                       }

                                                   }, (error : HttpErrorResponse) => { 
                                                        console.log(error);
                                                        // profile creation failed 
                                                        this.registerFailed = true; 
                                                        this.registerForm.setErrors({
                                                            "register" : error.error
                                                        }) 
                                                   });
                                } else { 
                                    // registration failed 
                                    this.registerFailed = true; 
                                    this.registerForm.setErrors({
                                        "register" : "User registration failed."
                                    });
                                }
                           }, (error : HttpErrorResponse) => { 
                                console.log(error);
                                // registration failed 
                                this.registerFailed = true; 
                                this.registerForm.setErrors({
                                    "register" : error.error
                                })
                           });

        } else {
            // registration failed 
            this.registerFailed = true; 
            this.registerForm.setErrors({
                "register" : "Registration form invalid."
            });
        }
   }

   onBack() { 
       this.router.navigate(["home"]); 
   }

   get email() { return this.registerForm.get('email'); }
   get firstName() { return this.registerForm.get('firstName'); }
   get lastName() { return this.registerForm.get('lastName'); }
   get password() { return this.registerForm.get('password'); }
   get termsOfService() { return this.registerForm.get('termsOfService'); }
}

class PasswordValidators { 

    public static containsDigit(control: FormControl) : any { 
        
        let containsDigit = /(?=.*\d)/; 

        return containsDigit.test(control.value) ? null : { containsDigit: true }; 
    }

    public static containsUppercaseLetter(control: FormControl) : any { 

        let containsUppercaseLetter = /(?=.*[A-Z])/;

        return containsUppercaseLetter.test(control.value) ? null : { containsUppercaseLetter: true }; 
    }

    public static containsLowercaseLetter(control: FormControl) : any { 

        let containsLowercaseLetter = /(?=.*[a-z])/;

        return containsLowercaseLetter.test(control.value) ? null : { containsLowercaseLetter: true }; 
    }

    public static containsSpecialCharacter(control: FormControl) : any {
        
        let containsSpecialCharacter = /(?=.*[$@$!%*#?&])/;

        return containsSpecialCharacter.test(control.value) ? null : { containsSpecialCharacter: true }; 
    }

    public static notContainsValuesOfInputs(inputs : [string]) { 
        
        return (control: FormControl) => { 

                var contains : boolean = false; 
        
                let form = control.root; 
                let password : string = form.get('password').value; 
        
                inputs.forEach((input) => { 
                    var inputValue : string = form.get(input).value;
                    if(inputValue.length > 2) {
                        contains = contains || password.toLowerCase().includes(inputValue.toLowerCase()); 
                    }
                }); 

                return contains ? { notContainsValuesOfInputs: true } : null; 
        }; 
    }
}