import { Component } from '@angular/core';
import { OffersType } from "../offers-grid/offers-type.enumeration";

@Component({
    selector: "lesson-offers",
    templateUrl: "./lesson-offers.component.html",
    styleUrls: ['./lesson-offers.component.css'],
})
export class LessonOffersComponent { 

    // enumeratin type needs to be defined 
     // as property on parent component in order 
     // to use it in child components property bindings! 
     public offersType = OffersType; 
}