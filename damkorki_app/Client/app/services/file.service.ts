import { Injectable } from '@angular/core'; 
import { HttpClient } from '@angular/common/http'; 
import { Observable } from 'rxjs/Rx'; 

@Injectable()
export class FileService { 

    private baseUrl : string = 'http://localhost:5050';
    
    constructor(private httpClient : HttpClient) { }

    upload(endpoint, formData, params) : Observable<any> 
    { 
        let uploadUrl = this.baseUrl + "/" + endpoint; 
        return this.httpClient.post<any>(uploadUrl, formData, params); 
    }
}