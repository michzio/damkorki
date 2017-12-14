import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "../../services/auth.service" 

@Component({
    selector: 'profile-dropdown', 
    templateUrl: './profile-dropdown.component.html', 
    styleUrls: ['./profile-dropdown.component.css']
})
export class ProfileDropdownComponent { 

    constructor(private authService: AuthService, 
                private router: Router) { }

    logout() { 
        if(this.authService.logout()) { 
            this.router.navigate(['/']);
        }
    }
}