import { Component, OnInit, Input } from '@angular/core';

import { GridCardComponent } from '../grid-card/grid-card.component'; 

@Component({
    selector: 'lesson-offer-grid-card', 
    templateUrl: './lesson-offer-grid-card.component.html', 
    styleUrls: ['./lesson-offer-grid-card.component.css']
})
export class LessonOfferGridCardComponent extends GridCardComponent implements OnInit { 

    @Input() content : any;  // <- TODO: we refactor it into specific type of Lesson Offer to be displayed later

    constructor() { 
       super(); 
    }

    ngOnInit() { 
        console.log(this.content)
    }
}