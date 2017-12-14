export interface IUser { 

    userId?: string; 
    userName: string; 
    password: string; 
    email: string; 
    registrationDate?: Date; 
    lastModifiedDate?: Date; 
    lastLoginDate?: Date; 
    failedLoginDate?: Date; 
     
}