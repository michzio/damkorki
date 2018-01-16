import { Injectable } from "@angular/core"; 
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable"; 
import { ITutor } from "../models/tutor.model"; 
import { WEB_API_URL } from "../shared/constants/webapi.constants"; 
import { PERSON_WEB_API_URL } from "./person.service";
import { IfObservable } from "rxjs/observable/IfObservable";

export const TUTOR_WEB_API_URL = WEB_API_URL + "/tutors"; 

@Injectable() 
export class TutorService { 

    private tutorUrl = TUTOR_WEB_API_URL; 
    private personUrl = PERSON_WEB_API_URL; 

    constructor(private httpClient: HttpClient) { }

    createTutor(tutor : ITutor) : Observable<ITutor> { 
        return this.httpClient.post<ITutor>(this.tutorUrl, tutor); 
    }

    updateTutor(tutorId : number, tutor : ITutor) : Observable<ITutor> { 
        return this.httpClient.put<ITutor>(this.tutorUrl + `/${tutorId}`, tutor); 
    }

    getTutorByPerson(personId : number) : Observable<ITutor> { 
        return this.httpClient.get<ITutor>(this.personUrl + `/${personId}/tutor`);
    }

    getTutorEagerlyByPerson(personId : number) : Observable<ITutor> { 
        return this.httpClient.get<ITutor>(this.personUrl + `/${personId}/tutor/eagerly`); 
    }
}