import { IPerson } from "./person.model";
import { ILessonOffer } from "./lesson-offer.model";
import { IReservation } from "./reservation.model";
import { IExperience } from "./experience.model";
import { ISkill } from "./skill.model";

export interface ITutor { 

    tutorId? : number; 
    description? : string; 
    qualifications? : string; 
    isSuperTutor? : boolean; 
    personId : number; 

    person? : IPerson; 
    lessonOffers? : Array<ILessonOffer>;
    reservations? : Array<IReservation>;
    experiences? : Array<IExperience>;
    skills? : Array<ISkill>; 
}