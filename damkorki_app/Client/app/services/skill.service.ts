import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { WEB_API_URL } from "../shared/constants/webapi.constants";
import { ISkill } from "../models/skill.model";

export const SKILL_WEB_API_URL = WEB_API_URL + "/skills";

@Injectable() 
export class SkillService { 
    
    private skillUrl = SKILL_WEB_API_URL; 

    constructor(private httpClient: HttpClient) { }

    createSkill(skill : ISkill) : Observable<ISkill> { 
        return this.httpClient.post<ISkill>(this.skillUrl, skill); 
    }

    deleteSkillByTutor(skillId : number, tutorId : number) : Observable<any> { 

        let queryParams = new HttpParams(); 
        queryParams = queryParams.append('by-tutor', String(tutorId)); 

        return this.httpClient.delete<any>(this.skillUrl + `/${skillId}`, { params: queryParams });
    }
}