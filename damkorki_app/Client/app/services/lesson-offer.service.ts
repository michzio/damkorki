import {Injectable} from '@angular/core'; 
import { HttpClient, HttpHeaders } from '@angular/common/http'; 
import {Observable} from 'rxjs/Observable'; 
import 'rxjs/add/operator/map'; 

import {ILessonOffer} from "../models/lesson-offer.model";


@Injectable()
export class LessonOfferService { 

    private lessonOfferUrl = "http://localhost:5050/lesson-offers";

    constructor(private http: HttpClient) { }

    getLessonOffers() : Observable<ILessonOffer> { 
        return this.http.get<ILessonOffer>(this.lessonOfferUrl); 
    }

    getLessonOffersEagerly() : Observable<ILessonOffer> { 
        return this.http.get<ILessonOffer>(this.lessonOfferUrl + "/eagerly"); 
    }

    createLessonOffer(lessonOffer: ILessonOffer) : Observable<any> {

        return this.http.post(this.lessonOfferUrl, lessonOffer, {
            headers: new HttpHeaders()
                    .set('Content-Type', 'application/json')
        }).catch(this.handleError);
    }

    handleError(err, caught) : any { 
        console.log("Some error occured while hiting Web API endpoint.", err, caught); 
    }

}