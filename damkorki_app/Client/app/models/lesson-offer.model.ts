import { ITutor } from "./tutor.model";

export interface ILessonOffer { 
    
    lessonOfferId?: number;
    title: string; 
    description: string; 
    cost: number; 
    type: number;   // TODO <- refactor into enumeration type 
    location: string; 
    level: number;  // TOTO <- refactor into enumeration type 
    subjectId: number; 
    tutorId: number; 

    subject?: Object;   //ISubject 
    tutor?: ITutor;     //ITutor     
}