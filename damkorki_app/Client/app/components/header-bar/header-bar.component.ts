import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'header-bar',
    templateUrl: './header-bar.component.html',
    styleUrls: ['./header-bar.component.css'],
})
export class HeaderBarComponent {

   isProfileDropdownOn = false; 

   constructor(public authService: AuthService) { }

   toggleProfileDropdown() { 
       this.isProfileDropdownOn = !this.isProfileDropdownOn; 
   }

}
