import { Component } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { AuthService } from "../../services/auth.service";
import { HttpErrorResponse } from "@angular/common/http";
import { UserService } from "../../services/user.service";
import { LoginEventType } from "../login/login.component";

@Component({
    selector: 'login-page',
    templateUrl: 'login-page.component.html',
    styleUrls: ['login-page.component.css']
})
export class LoginPageComponent { 

    infoAlert : string = null; 

    isEmailConfirmed : boolean = false; 
    confirmedEmail : string; 
    confirmationError : string; 
   
    constructor(private activatedRoute : ActivatedRoute, 
                private router : Router, 
                private authService: AuthService, 
                private userService: UserService) {

        if(this.activatedRoute.snapshot.url.toString() === "confirm-email") { 
            // get query params: userId and confirmationToken 
           let params = this.activatedRoute.snapshot.queryParams; 
           let userId = params['userId']; 
           let confirmationToken = params['confirmationToken'];
           this.userService.confirmEmail(userId, confirmationToken)
                           .subscribe((confirmation) => { 
                                this.isEmailConfirmed = true;
                                this.confirmedEmail = confirmation.email;
                           }, (error : HttpErrorResponse) => { 
                                console.log(error); 
                                this.confirmationError = "Incorrect user id or confirmation token."; 
                           })
        }

        if(this.authService.isLoggedIn()) { 
            // if user is logged in then redirect it to home page 
            this.router.navigate([""]);
        }

    }

    onLoginEvent(evt) { 
        if(evt.type == LoginEventType.PasswordResetTokenCreated && evt.success) { 
            this.infoAlert = evt.message;
        }
    }

    get isInfoAlert() { 
        return this.infoAlert != null; 
    }
}
