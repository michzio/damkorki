import { IUser } from "./user.model";
import { IAddress } from "./address.model";

export enum Gender { 
    male, 
    female, 
    other
}

export enum Month { 
    January, February, March, April, May, June, 
    July, August, September, October, November, December
}

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