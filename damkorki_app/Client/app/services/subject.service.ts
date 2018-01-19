import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { WEB_API_URL } from "../shared/constants/webapi.constants";
import { ISubject } from "../models/subject.model";

export const SUBJECT_WEB_API_URL = WEB_API_URL + "/subjects";

@Injectable() 
export class SubjectService { 
    
    private subjectUrl = SUBJECT_WEB_API_URL; 

    constructor(private httpClient: HttpClient) { }

    getSubjects() : Observable<ISubject[]> { 
        return this.httpClient.get<ISubject[]>(this.subjectUrl); 
    }

    getSubjectsNested() : Observable<ISubject[]> { 

        let queryParams = new HttpParams(); 
        queryParams = queryParams.append('nested', String(true)); 

        return this.httpClient.get<ISubject[]>(this.subjectUrl, { params: queryParams}); 
    } 
}