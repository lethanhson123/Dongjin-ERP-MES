import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MembershipContact } from './MembershipContact.model';
import { BaseService } from './Base.service';

@Injectable({
    providedIn: 'root'
})
export class MembershipContactService extends BaseService {
    DisplayColumns001: string[] = ['No', 'Name', 'Code', 'Description', 'Save'];
    List: MembershipContact[] | undefined;
    ListFilter: MembershipContact[] | undefined;
    FormData!: MembershipContact;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "MembershipContact";
    }
}
