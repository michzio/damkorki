import { IPerson } from "./person.model";

export class IProfilePhoto { 
    profilePhotoId? : number; 
    fileName? : string; 
    isProfilePhoto? : boolean; 
    caption? : string; 
    personId? : number;
    
    person? : IPerson; 
}