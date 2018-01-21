import { Injectable } from '@angular/core'; 
import { HttpClient } from '@angular/common/http'; 
import { Observable } from 'rxjs/Rx'; 
import { WEB_API_URL } from "../shared/constants/webapi.constants";

@Injectable()
export class FileService { 

    private baseUrl : string = WEB_API_URL;
    
    constructor(private httpClient : HttpClient) { }

    upload(endpoint, formData, params) : Observable<any> 
    { 
        let uploadUrl = this.baseUrl + "/" + endpoint; 
        return this.httpClient.post<any>(uploadUrl, formData, params); 
    }
}