import { Component, OnInit, Input } from '@angular/core';

@Component({ 
    selector: 'grid-card', 
    templateUrl: './grid-card.component.html', 
    styleUrls: ['./grid-card.component.css']
})
export class GridCardComponent implements OnInit { 

    // generic content object passed to each grid card to be displayed
    // here displayed generically as the list of key: value pairs in the card
    // derived classes of GridCardComponent may handle this logic differently  
    @Input() content : any; 

    constructor() {  }

    ngOnInit() { 
            
    }
}