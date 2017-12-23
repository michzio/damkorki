import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router"; 

import { ProfileComponent } from './components/profile/profile.component'; 
import { ProfileBasicEditComponent } from "./components/profile-basic-edit/profile-basic-edit.component";
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
                redirectTo: 'basic/edit',
                pathMatch: 'full'
            },
            {
                path: 'edit', 
                redirectTo: 'basic/edit',
                pathMatch: 'full'
            },
            { 
                path: 'basic/edit',
                component: ProfileBasicEditComponent,
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