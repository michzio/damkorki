import { ITutor } from "./tutor.model";
import { ISkill } from "./skill.model";


export interface ITutorSkill { 

    tutorId : number; 
    skillId : number; 

    tutor? : ITutor; 
    skill? : ISkill; 
}