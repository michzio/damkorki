import { LessonType } from "./enums/lesson-type.enum";
import { ITutor } from "./tutor.model";
import { LevelType } from "./enums/level-type.enum";
import { ISubject } from "./subject.model";
import { IReservation } from "./reservation.model";
import { ITerm } from "./term.model";

export interface ILessonOffer { 
    
    lessonOfferId?: number;
    title: string; 
    description?: string; 
    cost: number; 
    type: LessonType; 
    location: string; 
    level: LevelType; 
    subjectId: number; 
    tutorId: number; 

    subject?: ISubject;  
    tutor?: ITutor;

    terms?: Array<ITerm>
    reservations?: Array<IReservation>

}