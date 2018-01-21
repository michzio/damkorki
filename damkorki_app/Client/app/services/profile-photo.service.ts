import { Injectable } from "@angular/core"; 
import { HttpClient } from "@angular/common/http"; 
import { Observable } from "rxjs/Observable"; 
import { IProfilePhoto } from "../models/profile-photo.model";
import { HttpParams } from "@angular/common/http/src/params";
import { WEB_API_URL } from "../shared/constants/webapi.constants";
import { PERSON_WEB_API_URL } from "./person.service";

export const PROFILE_PHOTO_WEB_API_URL = WEB_API_URL + "/profile-photos"; 

@Injectable()
export class ProfilePhotoService { 

    private profilePhotoUrl = PROFILE_PHOTO_WEB_API_URL; 
    private personUrl = PERSON_WEB_API_URL;

    constructor(private httpClient: HttpClient) { }

    deleteProfilePhoto(profilePhotoId : number) : Observable<boolean> { 

        let url = this.profilePhotoUrl + `/${profilePhotoId}`;
        return this.httpClient.delete(url).map((ok) => { return true; });  
    }

    getProfilePhotosByPerson(personId : number) : Observable<IProfilePhoto[]> { 

        let url = this.personUrl + `/${personId}/photos`;
        return this.httpClient.get<IProfilePhoto[]>(url); 
    } 

    uncheckAllByPerson(personId : number) : Observable<boolean> { 

        let url = this.personUrl + `/${personId}/photos/uncheck-all`;
        return this.httpClient.get(url).map( (ok) => { return true; } );
    }

    checkMain(profilePhotoId: number) : Observable<IProfilePhoto> { 

        let url = this.profilePhotoUrl + `/${profilePhotoId}/check-main`
        return this.httpClient.get<IProfilePhoto>(url); 
    }

    public get ProfilePhotoUrl() { 
        return this.profilePhotoUrl; 
    } 
}