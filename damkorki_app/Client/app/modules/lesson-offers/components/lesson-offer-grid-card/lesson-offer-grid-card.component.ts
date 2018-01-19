import { Component, OnInit, Input } from '@angular/core';

import { GridCardComponent } from '../grid-card/grid-card.component'; 
import { ILessonOffer } from '../../../../models/lesson-offer.model';
import { LessonType } from '../../../../models/enums/lesson-type.enum';
import { LevelType } from '../../../../models/enums/level-type.enum';

@Component({
    selector: 'lesson-offer-grid-card', 
    templateUrl: './lesson-offer-grid-card.component.html', 
    styleUrls: ['./lesson-offer-grid-card.component.css']
})
export class LessonOfferGridCardComponent extends GridCardComponent implements OnInit { 

    @Input() content : ILessonOffer;

    // export lesson offer's enums
    public lessonTypes = LessonType; 
    public levels = LevelType; 

    constructor() { 
       super(); 
    }

    ngOnInit() { 
        console.log(this.content)        
    }
}