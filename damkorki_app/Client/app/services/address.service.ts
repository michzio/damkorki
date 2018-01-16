import { Injectable } from "@angular/core"; 
import { HttpClient } from "@angular/common/http"; 
import { Observable } from "rxjs/Observable"; 
import { IAddress } from "../models/address.model"; 
import { WEB_API_URL } from "../shared/constants/webapi.constants";
import { PERSON_WEB_API_URL } from "./person.service";

export const ADDRESS_WEB_API_URL = WEB_API_URL + "/addresses";

@Injectable() 
export class AddressService { 

    private addressUrl = ADDRESS_WEB_API_URL; 
    private personUrl = PERSON_WEB_API_URL; 

    constructor(private httpClient: HttpClient) { }

    createAddressForPerson(personId : number, address : IAddress) : Observable<IAddress> { 
        return this.httpClient.post<IAddress>(this.personUrl + `/${personId}/address`, address); 
    }

    updateAddressForPerson(personId : number, addressId : number, address: IAddress) : Observable<IAddress> { 
        return this.httpClient.put<IAddress>(this.personUrl + `/${personId}/addresses/${addressId}`, address);
    }

    getAddressByPerson(personId : number) : Observable<IAddress> { 
        return this.httpClient.get<IAddress>(this.personUrl + `/${personId}/address`);
    }
}