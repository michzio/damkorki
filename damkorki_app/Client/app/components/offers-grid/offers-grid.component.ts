import { Component, OnInit, Input, ElementRef, Injector } from '@angular/core';
import { Observable } from 'rxjs/Observable'; 

import { OffersType } from './offers-type.enumeration';
import { GridComponent } from '../grid/grid.component'; 

import { LessonOfferService } from '../../services/lesson-offer.service'; 

@Component({
    selector: 'offers-grid',
    templateUrl: './offers-grid.component.html',
    styleUrls: ['./offers-grid.component.css'],
    providers: [LessonOfferService],
})
export class OffersGridComponent extends GridComponent implements OnInit { 

    // it's specific only to offers grid 
    @Input() type : OffersType; 
    @Input("offerIds") offerIds : Array<number> = [];
    @Input('offerObjects') contentObjects : Array<any> = []; // <- TODO: refactor to array of IOffer json object 

    public contentObjectsObservable : Observable<any[]>; // <- TODO; refactor to Observable of array of IOffer json objects

    constructor(injector: Injector, private _lessonOfferService : LessonOfferService) { 
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

    addItem() { 
        console.log("Add Item"); 
        this.contentObjects.push({ "new key" : "with new value" }); 
        this.createLessonOffer();  
    }


    getLessonOffers() { 

        this._lessonOfferService.getLessonOffers()
                                .subscribe(lessonOffers => console.log(lessonOffers));
    }


    // Experimantal method that creates Lesson Offer - only for testing purposes 
    createLessonOffer() { 
        this._lessonOfferService.createLessonOffer(
            { title: "Client app Lesson Offer", 
              description: "Here goes some offer description",
              cost: 50, 
              type: 3, 
              location: "some location", 
              level: 24, 
              subjectId: 5, 
              tutorId: 5
            }
        ).subscribe(response => console.log(response));
    }
}