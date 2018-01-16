import { Component, Renderer2 } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms"; 
import { AuthService } from "../../../../services/auth.service";
import { PersonService } from "../../../../services/person.service"; 
import { ProfilePhotoService } from "../../../../services/profile-photo.service";
import { FileUploaderComponent, IUploadEvent } from "../file-uploader/file-uploader.component";
import { IProfilePhoto } from "../../../../models/profile-photo.model";
import { IPerson } from "../../../../models/person.model";

// default profile photo path
const  DEFAULT_PROFILE_PHOTO : string = require("./assets/images/profile_image.png");

interface IProfilePhotoPath { 
    pid: number; 
    path: string; 
}

@Component({
    selector: 'photos-edit', 
    templateUrl: './photos-edit.component.html',
    styleUrls: ['./photos-edit.component.css']
})
export class PhotosEditComponent { 

    person : IPerson = null; 
    profilePhotoPath : IProfilePhotoPath = { pid: 0, path: DEFAULT_PROFILE_PHOTO};  
    profilePhotos : IProfilePhotoPath[] = [ { pid: 0, path: DEFAULT_PROFILE_PHOTO } ]; 

    photoUploadEndpoint : string; 

    uploadMessage : string = null; 
    uploadFailed : boolean = false; 
    
    constructor(private fb: FormBuilder, 
                private renderer : Renderer2, 
                private personService : PersonService,
                private profilePhotoService : ProfilePhotoService, 
                private authService : AuthService) { 
            
        this.loadCurrentPhotos(); 
    }

    loadCurrentPhotos() { 

        this.personService.getPersonByUser(this.authService.decodedAccessToken().uid)
                    .subscribe( (person) => { 
                        this.person = person; 
                        // set photo upload endpoint on file uploader child component
                        this.photoUploadEndpoint = `persons/${person.personId}/photos`;
                        // load current photos
                        this.profilePhotoService.getProfilePhotosByPerson(person.personId)
                                .subscribe( (photos) => { 
                                        this.profilePhotos = photos.sort(this.photoOrderDescending)
                                                                   .map(this.photoToPath, this);
                                        this.profilePhotos.push({pid: 0, path: DEFAULT_PROFILE_PHOTO});

                                        let profilePhoto = photos.filter((photo) => photo.isProfilePhoto).pop(); 
                                        this.profilePhotoPath = profilePhoto ? 
                                            this.photoToPath(profilePhoto) : { pid: 0, path: DEFAULT_PROFILE_PHOTO}; 
                                });
                    }, (error) => { 
                        this.person = null; 
                    }); 
    }

    showPhotoDeleteButton( event ) 
    { 
        var photoDeleteButton = (<Element>event.target).querySelector('.photo-delete'); 
        this.renderer.setStyle(photoDeleteButton, "display", "block"); 
    }

    hidePhotoDeleteButton( event ) 
    { 
        var photoDeleteButton = (<Element>event.target).querySelector('.photo-delete'); 
        this.renderer.setStyle(photoDeleteButton, "display", "none"); 
    }

    onUploadChange( event : IUploadEvent ) { 
        this.uploadMessage = event.message; 
        this.uploadFailed = !event.success; 

        if(event.success) { 
            let newPhotos = <IProfilePhoto[]>event.response.photos;
            if(newPhotos.length > 0) {
                this.profilePhotos = [...newPhotos.sort(this.photoOrderDescending).map(this.photoToPath, this),
                                      ...this.profilePhotos];
                
                let newProfilePhoto = newPhotos.filter((photo) => photo.isProfilePhoto).pop();
                this.profilePhotoPath = newProfilePhoto ? 
                            this.photoToPath(newProfilePhoto) : { pid: 0, path: DEFAULT_PROFILE_PHOTO}; 
            }
        }
    }

    onPhotoItemDrag(event) { 

        event.dataTransfer.setData("profilePhotoId", event.target.getAttribute('data-pid'));
    }

    onPhotoItemDrop(event) { 
        event.preventDefault(); 
        var profilePhotoId = event.dataTransfer.getData("profilePhotoId");
        if(profilePhotoId > 0) {

            this.profilePhotoService.checkMain(profilePhotoId)
                    .subscribe((profilePhoto) => { 
                        this.profilePhotoPath = profilePhoto ? 
                                    this.photoToPath(profilePhoto) : { pid: 0, path: DEFAULT_PROFILE_PHOTO }; 
                    }, (error) => { 
                        this.uploadFailed = true; 
                        this.uploadMessage = "Could not change main profile photo."; 
                    });

        } else {

            this.profilePhotoService.uncheckAllByPerson(this.person.personId)
                    .subscribe((success) => { 
                        this.profilePhotoPath = { pid: 0, path: DEFAULT_PROFILE_PHOTO };
                    }, (error) => {
                        this.uploadFailed = true; 
                        this.uploadMessage = "Could not change main profile photo."; 
                    }); 
            return; 
        }
       
    }

    allowPhotoItemDrop(event) { 
        event.preventDefault(); 
    }

    deletePhoto(event) { 
        event.preventDefault(); 

        let photoId = event.currentTarget.getAttribute('data-pid');

        this.profilePhotoService.deleteProfilePhoto(photoId)
                    .subscribe((success) => { 
                        if(this.profilePhotoPath.pid == photoId) { 
                            // change main profile photo to default
                            this.profilePhotoPath = { pid: 0, path: DEFAULT_PROFILE_PHOTO }; 
                        }
                        // filter out deleted profile photo
                        this.profilePhotos = this.profilePhotos.filter(photo => photo.pid != photoId); 
                        
                        this.uploadFailed = false; 
                        this.uploadMessage = "Profile photo deleted successfully."; 
                    }, (error) => { 
                        
                        this.uploadFailed = true; 
                        this.uploadMessage = "Could not delete profile photo."; 
                    });
    }

    private photoToPath(photo : IProfilePhoto) : {pid:number, path: string}  { 
        return { pid: photo.profilePhotoId, path: this.profilePhotoService.ProfilePhotoUrl + "/" + photo.fileName};
    }

    private photoOrderDescending(photo1 : IProfilePhoto, photo2 : IProfilePhoto) : number { 
        return photo2.profilePhotoId - photo1.profilePhotoId; 
    }

    get isProfileCreated() { return this.person != null;  }
    get isUploadMessage() { return this.uploadMessage != null; }

}