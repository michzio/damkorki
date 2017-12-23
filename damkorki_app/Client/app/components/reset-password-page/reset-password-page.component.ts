import { Component } from "@angular/core";

@Component({
    selector: 'reset-password-page',
    template: `
        <div class="alert alert-info" *ngIf="resetPasswordMessage">{{resetPasswordMessage}}</div>
        <reset-password (onResetEvent)="onReset($event)"></reset-password>
    `,
    styleUrls: ['./reset-password-page.component.css']
})
export class ResetPasswordPageComponent { 

    resetPasswordMessage : string = null; 

    onReset(evt) { 
        this.resetPasswordMessage = evt.message; 
    }
}