import { Component, ViewEncapsulation } from "@angular/core";

@Component({
    selector: 'filter-lesson-offers', 
    templateUrl: 'filter-lesson-offers.component.html', 
    styleUrls: ['./filter-lesson-offers.component.css'], 
    encapsulation : ViewEncapsulation.None,
})
export class FilterLessonOffersComponent { 
    
    lowerPrice : number = 10; 
    upperPrice : number = 500; 

    sliderConfig: any = { connect: true, behaviour: 'drag'} 
    priceRange = [10, 500];           

    constructor() { 
    
    }

}