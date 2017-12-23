import { Component } from '@angular/core'
import { CanDeactivateComponent } from '../../../../services/can-deactivate-guard.service';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';

@Component({ 
    selector: 'profile-basic-edit', 
    templateUrl: 'profile-basic-edit.component.html',
    styleUrls: ['./profile-basic-edit.component.css']
})
export class ProfileBasicEditComponent implements CanDeactivateComponent { 

    canDeactivate(currentRoute: ActivatedRouteSnapshot, 
                  currentState: RouterStateSnapshot, 
                  nextState: RouterStateSnapshot) : boolean | Observable<boolean> | Promise<boolean> { 

        // ask the user with the dialog service and return its 
        // observable which resolves to true or false when the user decides 
        // return this.dialogService.confirm("Discard changes?"); 
        return window.confirm('Discard changes?');
    }

}