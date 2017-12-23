export interface IUser { 

    userId?: string; 
    userName: string; 
    password: string;
    passwordNew?: string;  
    email: string; 
    registrationDate?: Date; 
    lastModifiedDate?: Date; 
    lastLoginDate?: Date; 
    failedLoginDate?: Date; 
     
}