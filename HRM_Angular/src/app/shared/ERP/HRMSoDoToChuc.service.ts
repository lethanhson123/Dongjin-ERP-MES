import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HRMSoDoToChuc } from './HRMSoDoToChuc.model';
import { BaseService } from './Base.service';

@Injectable({
    providedIn: 'root'
})
export class HRMSoDoToChucService extends BaseService {
    DisplayColumns001: string[] = ['ID', 'No', 'Active', 'CategoryDepartment', 'CreateUser', 'Membership', 'CategoryStatusName', 'Complete', 'Point', 'StartDate', 'EndDate',  'Description', 'Save'];

    List: HRMSoDoToChuc[] | undefined;
    ListFilter: HRMSoDoToChuc[] | undefined;
    FormData!: HRMSoDoToChuc;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "HRMSoDoToChuc";
    }   
}
