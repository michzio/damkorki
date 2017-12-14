import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "../../services/auth.service";

@Component({
    selector: 'login-page',
    templateUrl: 'login-page.component.html',
    styleUrls: ['login-page.component.css']
})
export class LoginPageComponent { 
   
    constructor(private authService : AuthService, 
                private router : Router) { 
       
        if(this.authService.isLoggedIn()) { 
            // if user is logged in then redirect it to home page 
            this.router.navigate([""]);
        }
    }
}
