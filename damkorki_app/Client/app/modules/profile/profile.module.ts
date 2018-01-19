// import modules
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms"; 

// import material design modules 
import { MatRadioModule } from "@angular/material/radio"; 
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule } from "@angular/material/dialog";

// import components 
import { ProfileComponent } from "./components/profile/profile.component";
import { ProfileBasicEditComponent } from "./components/profile-basic-edit/profile-basic-edit.component"; 
import { PublicProfileComponent } from "./components/public-profile/public-profile.component";  
import { ProfileMenuComponent } from "./components/profile-menu/profile-menu.component";
import { PhotosEditComponent } from "./components/photos-edit/photos-edit.component"; 
import { AddressesEditComponent } from "./components/addresses-edit/addresses-edit.component";
import { FileUploaderComponent } from "./components/file-uploader/file-uploader.component";
import { AddressAreaComponent } from "./components/address-area/address-area.component";
import { TutorEditComponent } from "./components/tutor-edit/tutor-edit.component";
import { TutorBasicEditComponent } from "./components/tutor-basic-edit/tutor-basic-edit.component";
import { TutorExperiencesEditComponent } from "./components/tutor-experiences-edit/tutor-experiences-edit.component";
import { TutorSkillsEditComponent } from "./components/tutor-skills-edit/tutor-skills-edit.component";

import { ExperienceEditComponent } from "./components/experience-edit/experience-edit.component";
import { SkillEditComponent } from "./components/skill-edit/skill-edit.component";

// import services 
import { PersonService } from "../../services/person.service";
import { ProfilePhotoService } from "../../services/profile-photo.service";
import { FileService } from "../../services/file.service";
import { AddressService } from "../../services/address.service"; 
import { TutorService } from "../../services/tutor.service"; 
import { SkillService } from "../../services/skill.service";
import { TutorSkillService } from "../../services/tutor-skill.service";
import { ExperienceService } from "../../services/experience.service";

// import pipes
import { EnumMembersPipe } from "../../pipes/enum-members.pipe";
import { ObjectKeysPipe } from "../../pipes/object-keys.pipe"; 
import { ObjectEntriesPipe } from "../../pipes/object-entries.pipe";

// import routing module 
import { ProfileRoutingModule } from "./profile-routing.module";

@NgModule({
  imports: [
      CommonModule,
      FormsModule, 
      ReactiveFormsModule, 
      // material design
      MatRadioModule,
      MatSelectModule, 
      MatDialogModule,
      // custom 
      ProfileRoutingModule,
  ],
   declarations: [
       ProfileComponent,
       ProfileMenuComponent,
       ProfileBasicEditComponent,
       PublicProfileComponent,
       PhotosEditComponent, 
       AddressesEditComponent,
       FileUploaderComponent, 
       AddressAreaComponent, 
       TutorEditComponent,
       TutorBasicEditComponent,
       TutorExperiencesEditComponent,
       TutorSkillsEditComponent,
       ExperienceEditComponent,
       SkillEditComponent,
       // custom pipes  
       EnumMembersPipe, 
       ObjectKeysPipe, 
       ObjectEntriesPipe,
   ],
   entryComponents: [
        SkillEditComponent, 
        ExperienceEditComponent
   ],
   providers: [ 
       // custom services
       PersonService,
       ProfilePhotoService,
       FileService, 
       AddressService, 
       TutorService,
       SkillService, 
       TutorSkillService,
       ExperienceService, 
       // custom pipes
       EnumMembersPipe, 
       ObjectKeysPipe, 
       ObjectEntriesPipe, 
   ]
})
export class ProfileModule { }