import { IUser } from "./user.model";
import { IAddress } from "./address.model";
import { Gender } from "./enums/gender.enum";

export interface IPerson {

    personId? : number; 
    firstName : string; 
    lastName : string; 
    gender : Gender; 
    birthdate? : Date;
    skype? : string; 
    phoneNumber? : string; 
    addressId? : number; 
    userId : string; 
    
    address? : IAddress;  
    user? : IUser; 
}