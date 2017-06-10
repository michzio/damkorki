import {Injectable} from '@angular/core'; 
import { Http, RequestOptions, Headers } from '@angular/http'; 
import {Observable} from 'rxjs/Observable'; 
import 'rxjs/add/operator/map'; 

import {ILessonOffer} from "../models/lesson-offer.models";


@Injectable()
export class LessonOfferService { 

    private _lesson_offer_url = "http://localhost:5050/lesson-offers";

    constructor(private _http: Http) { }

    getLessonOffers() : Observable<ILessonOffer> { 
        return this._http.get(this._lesson_offer_url) // <- here Http object returns Observable!
                         .map(response => response.json()); 
    }

    getLessonOffersEagerly() : Observable<ILessonOffer> { 
        return this._http.get(this._lesson_offer_url + "/eagerly") 
                         .map(response => response.json()); 
    }

    createLessonOffer(lessonOffer: ILessonOffer) : Observable<any> {

        let body = JSON.stringify(lessonOffer);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let req_options = new RequestOptions({ headers: headers });


        // here we send ILessonOffer object via HTTP POST
        return this._http.post(this._lesson_offer_url, body, req_options) 
                         .map(response => response.json())   
                         .catch(this.handleError);
    }

    handleError(err, caught) : any { 
        console.log("Some error occured while hiting Web API endpoint.", err, caught); 
    }

}