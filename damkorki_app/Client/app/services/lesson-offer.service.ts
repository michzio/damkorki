import {Injectable} from '@angular/core'; 
import { HttpClient, HttpHeaders } from '@angular/common/http'; 
import {Observable} from 'rxjs/Observable'; 
import 'rxjs/add/operator/map'; 

import {ILessonOffer} from "../models/lesson-offer.model";
import { WEB_API_URL } from '../shared/constants/webapi.constants';

export const LESSON_OFFER_WEB_API_URL = WEB_API_URL + "/lesson-offers"; 

@Injectable()
export class LessonOfferService { 

    private lessonOfferUrl = LESSON_OFFER_WEB_API_URL;

    constructor(private httpClient: HttpClient) { }

    getLessonOffers() : Observable<ILessonOffer[]> { 
        return this.httpClient.get<ILessonOffer[]>(this.lessonOfferUrl); 
    }

    getLessonOffersEagerly() : Observable<ILessonOffer[]> { 
        return this.httpClient.get<ILessonOffer[]>(this.lessonOfferUrl + "/eagerly"); 
    }

    createLessonOffer(lessonOffer: ILessonOffer) : Observable<ILessonOffer> {
        return this.httpClient.post<ILessonOffer>(this.lessonOfferUrl, lessonOffer, {
            headers: new HttpHeaders()
                    .set('Content-Type', 'application/json')
        });
    }

    handleError(err, caught) : any { 
        console.log("Some error occured while hiting Web API endpoint.", err, caught); 
    }

}