// import modules
import { NgModule } from "@angular/core"
import { CommonModule } from "@angular/common"

// import components 
import { ProfileComponent } from "./components/profile/profile.component";
import { ProfileEditComponent } from "./components/profile-edit/profile-edit.component"; 
import { PublicProfileComponent } from "./components/public-profile/public-profile.component";  

// import services 

// import routing module 
import { ProfileRoutingModule } from "./profile-routing.module";

@NgModule({
  imports: [
      CommonModule,
      ProfileRoutingModule
  ],
   declarations: [
       ProfileComponent, 
       ProfileEditComponent,
       PublicProfileComponent,  
   ], 
   providers: [ ]
})
export class ProfileModule { }