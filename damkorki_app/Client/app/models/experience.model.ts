import { ITutor } from "./tutor.model";


export interface IExperience { 
    
    experienceId? : number; 
    startYear : number; 
    endYear : number; 
    description : string; 
    tutorId : number;

    tutor? : ITutor;
}