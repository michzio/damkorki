import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { IUser } from "../models/user.model";
import { Observable } from "rxjs/Observable";

@Injectable() 
export class UserService { 

    private userUrl = "http://localhost:5050/users"

    constructor(private httpClient: HttpClient) { }

    createUser(user: IUser) : Observable<IUser>  {
        return this.httpClient.post<IUser>(this.userUrl, user);
    }

    confirmEmail(userId: string, confirmationToken: string): Observable<any> { 
        
        if(userId && userId.length > 0) { 
            let url =  this.userUrl + "/" + userId 
                       + "/confirm-email?confirmationToken=" + encodeURIComponent(confirmationToken); 

            return this.httpClient.get<any>(url);
        }
        return Observable.throw("Incorrect user id or confirmation token.");
    }

    createPasswordResetToken(emailOrUsername : string) : Observable<any> { 

        let url = this.userUrl + "/reset-password";
        let encodedData = "username=" + encodeURIComponent(emailOrUsername);

        return this.httpClient.post<any>(
                url,
                encodedData, 
                {   
                    headers: new HttpHeaders()
                        .set('Content-Type', 'application/x-www-form-urlencoded')
                        .set('Accept', 'application/json')
                }); 
    }

    resetPassword(userId: string, resetToken: string, passwordNew: string) : Observable<any> { 

        let url = this.userUrl + `/${userId}/reset-password`; 

        let data = { 
            resetToken : resetToken,
            passwordNew : passwordNew
        };

        return this.httpClient.put<any>(url, data); 
    }
}