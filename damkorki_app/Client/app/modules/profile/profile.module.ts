// import modules
import { NgModule } from "@angular/core"
import { CommonModule } from "@angular/common"

// import components 
import { ProfileComponent } from "./components/profile/profile.component";
import { ProfileBasicEditComponent } from "./components/profile-basic-edit/profile-basic-edit.component"; 
import { PublicProfileComponent } from "./components/public-profile/public-profile.component";  
import { ProfileMenuComponent } from "./components/profile-menu/profile-menu.component";

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
       ProfileMenuComponent,
       ProfileBasicEditComponent,
       PublicProfileComponent,  
   ], 
   providers: [ ]
})
export class ProfileModule { }