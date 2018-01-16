import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable"; 
import { IPerson } from "../models/person.model";  
import { WEB_API_URL } from "../shared/constants/webapi.constants";
import { USER_WEB_API_URL } from "./user.service";

export const PERSON_WEB_API_URL = WEB_API_URL + "/persons"; 

@Injectable() 
export class PersonService { 

    private personUrl = PERSON_WEB_API_URL;
    private userUrl = USER_WEB_API_URL; 

    constructor(private httpClient: HttpClient) { }

    createPerson(person : IPerson) : Observable<IPerson> { 
        return this.httpClient.post<IPerson>(this.personUrl, person)
                    .map( (person) => { 
                        // deserialize birthdate property as Date object
                        person.birthdate = new Date(person.birthdate);
                        return person; 
                    }); 
    }

    updatePerson(personId : number, person: IPerson) : Observable<IPerson> { 
        return this.httpClient.put<IPerson>(this.personUrl + `/${personId}`, person)
                    .map( (person) => {
                        // deserialize birthdate property as Date object
                        person.birthdate = new Date(person.birthdate); 
                        return person; 
                    }); 
    }

    getPersonByUser(userId: string) : Observable<IPerson> { 
        return this.httpClient.get<IPerson>(this.userUrl + `/${userId}/person`)
                    .map( (person) => {
                        // deserialize birthdate property as Date object
                        person.birthdate = new Date(person.birthdate); 
                        return person; 
                    }); 
    }
}