import { Component, OnInit, Input } from '@angular/core';

import { GridCardComponent } from '../grid-card/grid-card.component'; 

@Component({
    selector: 'offers-grid-card', 
    templateUrl: './offers-grid-card.component.html', 
    styleUrls: ['./offers-grid-card.component.css']
})
export class OffersGridCardComponent extends GridCardComponent implements OnInit { 

    @Input() content : any;  // <- TODO: we refactor it into specific type of Lesson Offer to be displayed later

    constructor() { 
       super(); 
    }

    ngOnInit() { }
}