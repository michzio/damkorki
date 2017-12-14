import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router"; 

import { ProfileComponent } from './components/profile/profile.component'; 
import { ProfileEditComponent } from "./components/profile-edit/profile-edit.component";
import { PublicProfileComponent } from "./components/public-profile/public-profile.component"; 
import { AuthGuard } from "../../services/auth-guard.service";
import { CanDeactivateGuard } from "../../services/can-deactivate-guard.service";

const profileRoutes: Routes = [ 
    { 
        path: 'profile', 
        component: ProfileComponent,
        canActivate: [AuthGuard], 
        children: [
            {
                path: '', 
                redirectTo: 'edit',
                pathMatch: 'full'
            },
            { 
                path: 'edit',
                component: ProfileEditComponent,
                canDeactivate: [CanDeactivateGuard]
            }, 
        ]
    }, 
    {
        path: 'profile/:id', 
        component: PublicProfileComponent,
    }
]; 

@NgModule({
    imports: [
        RouterModule.forChild(profileRoutes)
    ], 
    exports: [
        RouterModule
    ]
})
export class ProfileRoutingModule { }