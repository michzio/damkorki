import { Injectable } from "@angular/core";
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router"; 
import { Observable } from "rxjs/Observable"; 


@Injectable() 
export class CanDeactivateGuard implements CanDeactivate<CanDeactivateComponent> { 
    
    canDeactivate(component: CanDeactivateComponent, 
                  currentRoute: ActivatedRouteSnapshot, 
                  currentState: RouterStateSnapshot, 
                  nextState?: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
       
        return component.canDeactivate ? component.canDeactivate(currentRoute, currentState, nextState) : true; 
    }
}

export interface CanDeactivateComponent { 
    canDeactivate: (currentRoute: ActivatedRouteSnapshot, 
                    currentState: RouterStateSnapshot, 
                    nextState: RouterStateSnapshot) => Observable<boolean> | Promise<boolean> | boolean; 
}