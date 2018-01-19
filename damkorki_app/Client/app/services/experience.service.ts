import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { WEB_API_URL } from "../shared/constants/webapi.constants";
import { IExperience } from "../models/experience.model";

export const EXPERIENCE_WEB_API_URL = WEB_API_URL + "/experiences"; 

@Injectable() 
export class ExperienceService { 
    
    private experienceUrl = EXPERIENCE_WEB_API_URL; 

    constructor(private httpClient: HttpClient) { }

    createExperience(experience: IExperience) : Observable<IExperience> { 
        return this.httpClient.post<IExperience>(this.experienceUrl, experience); 
    }

    updateExperience(experienceId: number, experience: IExperience) : Observable<IExperience> { 
        return this.httpClient.put<IExperience>(this.experienceUrl +  `/${experienceId}`, experience); 
    }

    deleteExperience(experienceId: number) : Observable<any> { 
        return this.httpClient.delete<any>(this.experienceUrl + `/${experienceId}`);
    }
}