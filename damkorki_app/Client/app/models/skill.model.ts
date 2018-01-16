import { ITutor } from "./tutor.model";


export interface ISkill { 
    
    skillId? : number; 
    name : string; 

    tutors? : Array<ITutor>; 
}