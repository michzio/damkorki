import { Component } from '@angular/core';

import { OffersType } from '../../modules/lesson-offers/components/lesson-offers-grid/offers-type.enumeration'; 

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
})
export class HomeComponent {

     // enumeration type needs to be defined 
     // as property on parent component in order 
     // to use it in child components property bindings! 
     public offersType = OffersType; 
  
}
