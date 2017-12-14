import { IUser } from "./user.model";

export interface IPerson { 

    personId? : number; 
    firstName : string; 
    lastName : string; 
    gender : number; 
    birthdate? : Date; 
    image? : string; 
    skype? : string; 
    phoneNumber? : string; 
    addressId? : number; 
    userId : string; 
    
    address? : Object; // IAddress 
    user? : IUser; 
}