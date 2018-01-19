import { Component, OnInit, Input, ElementRef, Injector } from '@angular/core';
import { Observable } from 'rxjs/Observable'; 

import { OffersType } from './offers-type.enumeration';
import { GridComponent } from '../grid/grid.component'; 

import { LessonOfferService } from '../../../../services/lesson-offer.service'; 
import { ILessonOffer } from '../../../../models/lesson-offer.model';

@Component({
    selector: 'lesson-offers-grid',
    templateUrl: './lesson-offers-grid.component.html',
    styleUrls: ['./lesson-offers-grid.component.css'],
    providers: [LessonOfferService],
})
export class LessonOffersGridComponent extends GridComponent implements OnInit { 

    // it's specific only to offers grid 
    @Input() type : OffersType; 
    @Input("lessonOfferIds") offerIds : Array<number> = [];
    @Input('lessonOfferObjects') contentObjects : Array<any> = []; 
    @Input() query : string; 

    public contentObjectsObservable : Observable<any>;

    constructor(injector: Injector, 
               private lessonOfferService : LessonOfferService) 
    {            
        super(injector);
    }

    ngOnInit() { 

        /* Dummy content objects for testing purposes 
            this.offerIds.forEach(offerId => {
                var key = "offer key " + offerId; 
                var val = "offer val " + offerId; 
                this.contentObjects.push( { key : val } );
            });
        */ 

        // let's get some lesson offers from Web Api endpoint
        this.getLessonOffers(); 
         
        super.ngOnInit(); 
    }

    getLessonOffers() { 
       
        this.lessonOfferService.getLessonOffers().subscribe(
            (lessonOffers) => { 
                this.contentObjects = lessonOffers; 
            });
    }
}