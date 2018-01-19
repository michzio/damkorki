import { ILessonOffer } from "./lesson-offer.model";


export interface ISubject { 
   
   subjectId? : number; 
   name : string; 
   description? : string; 
   image? : string; 
   superSubjectId? : number; 

   superSubject? : ISubject; 
   subSubjects? : Array<ISubject>; 
   lessonOffers? : Array<ILessonOffer>; 
}

