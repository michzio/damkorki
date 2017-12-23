import { Component } from "@angular/core"; 
import { FormBuilder, FormGroup, FormControl, Validators } from "@angular/forms"; 
import { Router } from "@angular/router"; 
import { HttpClient } from "@angular/common/http"; 
import { IUser } from "../../models/user.model";
import { HttpErrorResponse } from "@angular/common/http";
import { IPerson } from "../../models/person.model";
import { AuthService } from "../../services/auth.service";
import { UserService } from "../../services/user.service";
import { PasswordValidators } from "../../shared/validators/password.validators"

@Component({
    selector: "register", 
    templateUrl: "./register.component.html", 
    styleUrls: ['./register.component.css']
})
export class RegisterComponent { 

    // Web Api person profile path 
    private personUrl = "http://localhost:5050/persons"; 

    registerForm : FormGroup = null; 
    registerFailed = false;

    constructor(private fb: FormBuilder, 
                private router: Router, 
                private httpClient: HttpClient, 
                private authService: AuthService, 
                private userService: UserService) { 

                if(this.authService.isLoggedIn()) { 
                    this.router.navigate(["/"]); 
                }

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
            this.userService.createUser(user)
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