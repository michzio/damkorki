import { Injectable } from "@angular/core"; 
import { HttpClient } from "@angular/common/http"; 
import { Observable } from "rxjs/Observable"; 
import { IProfilePhoto } from "../models/profile-photo.model";
import { HttpParams } from "@angular/common/http/src/params";

@Injectable()
export class ProfilePhotoService { 

    private profilePhotoUrl = "http://localhost:5050/profile-photos"; 
    private personUrl = "http://localhost:5050/persons";

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