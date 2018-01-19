import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router"; 

import { ProfileComponent } from './components/profile/profile.component'; 
import { ProfileBasicEditComponent } from "./components/profile-basic-edit/profile-basic-edit.component";
import { PublicProfileComponent } from "./components/public-profile/public-profile.component"; 
import { PhotosEditComponent } from "./components/photos-edit/photos-edit.component";
import { AddressesEditComponent } from "./components/addresses-edit/addresses-edit.component";
import { TutorEditComponent } from "./components/tutor-edit/tutor-edit.component";

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
            {
                path: 'photos/edit', 
                component: PhotosEditComponent
            }, 
            {
                path: 'addresses/edit', 
                component: AddressesEditComponent, 
                canDeactivate: [CanDeactivateGuard]
            }, 
            {
                path: 'tutor/edit', 
                component: TutorEditComponent, 
                canDeactivate: [CanDeactivateGuard]
            }
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