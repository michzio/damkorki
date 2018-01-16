import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
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
}