import { Injectable, Injector } from '@angular/core'; 
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs/Observable'; 
import { AuthService } from '../services/auth.service';

@Injectable() 
export class AuthInterceptor implements HttpInterceptor { 

    constructor(private injector : Injector) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
       
        // inject Authentication Service manually 
        let authService = this.injector.get(AuthService);

        // get the authObject from the service. 
        const authObject = authService.getAuth(); 

        // clone the request to add the new header
        const authRequest = request.clone({
            setHeaders: { 
                Authorization: `Bearer ${authObject? authObject.access_token : ""}`
            }
        });

        // pass on the cloned request instead of the original one. 
        return next.handle(authRequest); 
    }

}