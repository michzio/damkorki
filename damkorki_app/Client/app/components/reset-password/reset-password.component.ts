import { Component, 
         Inject, InjectionToken, Injector, 
         Output, EventEmitter } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { FormGroup, FormBuilder } from "@angular/forms";
import { Validators } from "@angular/forms";
import { PasswordValidators } from "../../shared/validators/password.validators";
import { UserService } from "../../services/user.service";
import { HttpErrorResponse } from "@angular/common/http";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { Observable } from "rxjs/Observable";

@Component({
    selector: 'reset-password', 
    templateUrl: 'reset-password.component.html', 
    styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent { 

    private dialogRef : MatDialogRef<ResetPasswordComponent>;
    private data : any;

    resetPasswordForm : FormGroup = null; 
    resetPasswordFailed : boolean = false; 

    private resetToken : string = null; 
    private userId : string = null; 

    @Output('onResetEvent') resetEventEmitter : EventEmitter<any> = new EventEmitter();  
    
    constructor(private injector : Injector,   
                private activatedRoute : ActivatedRoute, 
                private fb : FormBuilder, 
                private userService : UserService) {

        this.resetToken = activatedRoute.snapshot.queryParams['resetToken']; 
        if(!this.resetToken) { 
             this.dialogRef  = this.injector.get(MatDialogRef);
             this.data = this.injector.get(MAT_DIALOG_DATA);
        } else { 
            this.userId = activatedRoute.snapshot.queryParams['userId'];
        }

        this.createForm(); 
    }

    private createForm() { 

        if(this.resetToken) {
            this.resetPasswordForm = this.fb.group({
                newPassword: ["", [ Validators.required,
                                    Validators.minLength(6), 
                                    PasswordValidators.containsDigit, 
                                    PasswordValidators.containsLowercaseLetter, 
                                    PasswordValidators.containsUppercaseLetter, 
                                    PasswordValidators.containsSpecialCharacter] ], 
                confirmPassword: ["", [ Validators.required] ], 
            }, { 
                validator: PasswordValidators.matchValuesOfInputs('newPassword', 'confirmPassword')
            })
        } else { 
            this.resetPasswordForm = this.fb.group({
                emailOrUsername: [ this.data.email , [ Validators.required ]] // can also be username 
            });
        }
    }

    onCreatePasswordResetToken(evt) { 
        evt.preventDefault(); 
        console.log(evt); 

        if(this.resetPasswordForm.valid) {
            
            this.userService.createPasswordResetToken(this.emailOrUsername.value)
                    .subscribe( (created) => { 
                        console.log(created); 
                        
                        // success
                        this.resetPasswordFailed = false;

                        // close dialog
                        this.dialogRef.close({
                             status : 'yes', 
                             message : "Password reset token created successfully and sent to user's email address"
                            });

                    }, (error : HttpErrorResponse) => { 
                        // resetting password failed
                        this.resetPasswordFailed = true; 
                        this.resetPasswordForm.setErrors({
                            "resetPassword" : "Could not create password reset token"
                        })
                    });
        } else { 
            // resetting password failed
            this.resetPasswordFailed = true; 
            this.resetPasswordForm.setErrors({
                "resetPassword" : "Reset password form invalid"
            });
        }
    }

    onResetPassword(evt) { 
        evt.preventDefault(); 
        console.log(evt); 

        if(this.resetPasswordForm.valid) { 

            this.userService.resetPassword(this.userId, this.resetToken, this.newPassword.value)
                    .subscribe( (result) => {
                        console.log(result); 
                        if(result && result.email) {
                            // success
                            this.resetPasswordFailed = false;
                            this.resetEventEmitter.emit({
                                name: "Password Reset",
                                type: ResetPasswordEventType.PasswordReset,
                                success: true,
                                message: `Password reset for application user with email: ${result.email}`
                            }); 
                        } 
                    }, (error : HttpErrorResponse) => {
                        // resetting password failed 
                        this.resetPasswordFailed = true;
                        this.resetPasswordForm.setErrors({
                            "resetPassword" : "Could not reset password"
                        })
                    });
        } else { 
            // resetting password failed
            this.resetPasswordFailed = true; 
            this.resetPasswordForm.setErrors({
                "resetPassword" : "Reset password form invalid"
            })
        }
    }

    get emailOrUsername() { return this.resetPasswordForm.get('emailOrUsername'); }
    get newPassword() { return this.resetPasswordForm.get('newPassword'); }
    get confirmPassword() { return this.resetPasswordForm.get('confirmPassword'); }
}

enum ResetPasswordEventType { 
    PasswordReset,
}