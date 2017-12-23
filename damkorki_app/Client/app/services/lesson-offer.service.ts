import {Injectable} from '@angular/core'; 
import { HttpClient, HttpHeaders } from '@angular/common/http'; 
import {Observable} from 'rxjs/Observable'; 
import 'rxjs/add/operator/map'; 

import {ILessonOffer} from "../models/lesson-offer.model";


@Injectable()
export class LessonOfferService { 

    private static lessonOfferUrl = "http://localhost:5050/lesson-offers";

    constructor(private httpClient: HttpClient) { }

    getLessonOffers() : Observable<ILessonOffer> { 
        return this.httpClient.get<ILessonOffer>(LessonOfferService.lessonOfferUrl); 
    }

    getLessonOffersEagerly() : Observable<ILessonOffer> { 
        return this.httpClient.get<ILessonOffer>(LessonOfferService.lessonOfferUrl + "/eagerly"); 
    }

    createLessonOffer(lessonOffer: ILessonOffer) : Observable<any> {

        return this.httpClient.post(LessonOfferService.lessonOfferUrl, lessonOffer, {
            headers: new HttpHeaders()
                    .set('Content-Type', 'application/json')
        }).catch(this.handleError);
    }

    handleError(err, caught) : any { 
        console.log("Some error occured while hiting Web API endpoint.", err, caught); 
    }

}