import { Component, NgZone, PLATFORM_ID, OnInit, Inject } from "@angular/core";
import { isPlatformBrowser } from "@angular/common"; 
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http"; 
import { AuthService, AUTH_WEB_API_URL } from "../../services/auth.service"; 
import { IAuthObject } from "../../models/auth-object.model";
import { WEB_API_URL } from "../../shared/constants/webapi.constants";

// global window object
declare var window : any; 

@Component({
    selector: "external-login",
    templateUrl: "./external-login.component.html" 
})
export class ExternalLoginComponent implements OnInit { 

    private static authUrl = AUTH_WEB_API_URL;

    externalLoginWindow : any; 

    constructor(private httpClient : HttpClient, 
                private router : Router, 
                private authService : AuthService, 
                private localZone : NgZone, 
                @Inject(PLATFORM_ID) private platformId) { }
    
    ngOnInit(): void {
        
        if(!isPlatformBrowser(this.platformId)) { 
            return; 
        }

        // close previously opened external login window (if any)
        this.closeExternalLoginWindow(); 

        // define externalLoginCallback function for 'message' event from popup window
        var self = this; 
        function externalLoginCallback(event) { 
            if(event.origin == WEB_API_URL) {            
                console.log("External login succeded with access token: " + (<IAuthObject>event.data).access_token);
            
                self.localZone.run(() => {
                    self.authService.setAuth(<IAuthObject>event.data);
                           
                    // redirect user 
                    if(self.authService.redirectUrl) { 
                        self.router.navigate([self.authService.redirectUrl]); 
                        self.authService.redirectUrl = null; 
                    } else { 
                        self.router.navigate([""]); 
                    }
                });
            }
        }
        window.addEventListener('message', externalLoginCallback, false);
        
    }

    private closeExternalLoginWindow() { 
        if(this.externalLoginWindow) { 
            this.externalLoginWindow.close();
        }
        this.externalLoginWindow = null; 
    }
    
    login(providerName : string) { 

       if(!isPlatformBrowser(this.platformId)) { 
           return; 
       }

       var url = ExternalLoginComponent.authUrl + "/" + providerName;   

       // window size calculation 
       var width = (screen.width >= 1000) ? 1000 : screen.width;
       var height = (screen.height >= 500) ? 500 : screen.height; 
       var params = "toolbar=yes,scrollbars=yes,resizable=yes,width=" + width + ",height=" + height; 

       // close previously opened external login window
       this.closeExternalLoginWindow(); 

       // open external login window
       var title = providerName[0].toUpperCase() + providerName.substr(1) + " Login"; 
       this.externalLoginWindow = window.open(url, title, params, false);

    }
}