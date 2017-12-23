import { Injectable, EventEmitter } from "@angular/core";
import { PLATFORM_ID, Inject } from "@angular/core"; 
import { isPlatformBrowser, isPlatformServer } from "@angular/common"; 
import { HttpClient, HttpHeaders } from "@angular/common/http"; 
import { Observable } from "rxjs/Observable";
import { retry } from "rxjs/operators/retry";
import { JwtHelperService } from '@auth0/angular-jwt'
import { IAuthObject } from "../models/auth-object.model";
import { IAccessToken } from "../models/access-token.model";
import { CookieHelper } from "../shared/cookie";

@Injectable() 
export class AuthService { 

    authKey: string = "auth";
    clientId: string = "DamkorkiApp"; 

    // JwtProvider's path
    private authTokenUrl = "http://localhost:5050/auth/token";
    private authLogoutUrl = "http://localhost:5050/auth/logout"; 

    private jwtHelper =  new JwtHelperService({}); 
    
    // store the URL so we can redirect after logging in
    public redirectUrl: string; 

    constructor(private http: HttpClient, 
                @Inject(PLATFORM_ID) private platformId: Object) {  }

    login(email: string, password: string) : Observable<boolean>  { 
        
        var data = { 
            username: email, 
            password: password, 
            client_id: this.clientId, 
            client_secret: "", // not implemented
            grant_type: "password", // when signing up with username/password
            scope: "", // not implemented
        }

        return this.authTokenRequest(data);
    }

    logout() : Observable<boolean> { 

        return this.http.post(this.authLogoutUrl, null)
                .map(response => {
                    // delete token from local storage 
                    this.setAuth(null)
                    // delete cookie 
                    new CookieHelper().deleteCookie("Identity.External"); 
                    return true; 
                })
                .catch(error => { 
                    return Observable.throw(error);
                });
    }

    toUrlEncodedString(data: any) : string { 
        var encoded = "";
        for(var key in data) { 
            if(encoded.length) { 
                encoded += "&";
            }
            encoded += key + "="; 
            encoded += encodeURIComponent(data[key]); 
        }
        return encoded;  
    }

    // persist authObject into localStorage 
    // or remove it if NULL argument passed. 
    setAuth(authObject: IAuthObject | null) : boolean { 
        if (isPlatformBrowser(this.platformId)) {
            if(authObject) { 
                // persist 
                localStorage.setItem(this.authKey, JSON.stringify(authObject))
            } else {
                // remove 
                localStorage.removeItem(this.authKey); 
            }
            return true; 
        }
    }

    // retrieve authObject (or NULL) from localStorage  
    getAuth(): IAuthObject | null { 
        if(isPlatformBrowser(this.platformId)) {
            var authItem = localStorage.getItem(this.authKey);
            if(authItem) { 
                return JSON.parse(authItem); // authObject
            } else { 
                return null; 
            }
        }
    }
    
    // return true if the user is logged in and false otherwise 
    isLoggedIn() : boolean {  
        var authObject = this.getAuth();
        if(authObject && authObject.access_token) {
            const isAccessTokenValid : boolean = !this.jwtHelper.isTokenExpired(authObject.access_token);
            return isAccessTokenValid;
        }
        return false; 
    }

    decodedAccessToken() : IAccessToken { 
        const authObject = this.getAuth(); 
        if(authObject && authObject.access_token) { 
            return this.jwtHelper.decodeToken(authObject.access_token); 
        }
        return null; 
    }

    // tries to refresh expired access token using refresh token 
    refreshToken() : Observable<boolean> { 

        var data = { 
            client_id: this.clientId, 
            grant_type: "refresh_token", 
            refresh_token: this.getAuth()!.refresh_token, 
            scope: "", // not implemented
        }

        return this.authTokenRequest(data); 
    }

    // retrieves the access & refresh tokens from the server 
    authTokenRequest(data: any): Observable<boolean> { 

        // make authorization request 
        return this.http.post<IAuthObject>(
                this.authTokenUrl,       // URL  
                this.toUrlEncodedString(data),  // body 
                {   
                    headers: new HttpHeaders()  // headers 
                        .set('Content-Type', 'application/x-www-form-urlencoded')
                        .set('Accept', 'application/json'),
                })
                .map( (authObject) => { 
                    console.log("Auth object received:"); 
                    console.log(authObject);

                    let access_token = authObject && authObject.access_token; 
                    if(access_token) { 
                        // successful login 
                        this.setAuth(authObject);
                        return true;
                    }
        
                    // failed login 
                    return Observable.throw("Unauthorized"); 
                }).catch(error => {
                    return new Observable<any>(error);   
                });
    }
}
