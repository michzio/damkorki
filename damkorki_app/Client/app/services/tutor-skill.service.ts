import { Injectable } from "@angular/core";
import { WEB_API_URL } from "../shared/constants/webapi.constants";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { ITutorSkill } from "../models/tutor-skill.model";

export const TUTOR_SKILL_WEB_API_URL = WEB_API_URL + "/tutors-have-skills"; 

@Injectable() 
export class TutorSkillService { 
    
    private tutorSkillUrl = TUTOR_SKILL_WEB_API_URL; 

    constructor(private httpClient: HttpClient) { }

    addSkillToTutor(skillId: number, tutorId: number) : Observable<ITutorSkill> { 
        return this.httpClient.post<ITutorSkill>(this.tutorSkillUrl + `/${tutorId}has${skillId}`, null); 
    }

    removeSkillFromTutor(skillId: number, tutorId: number ) : Observable<any> { 
        return this.httpClient.delete<any>(this.tutorSkillUrl + `/${tutorId}has${skillId}`);
    }
}