import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { MembershipCompany } from './MembershipCompany.model';
import { BaseService } from './Base.service';
@Injectable({
    providedIn: 'root'
})
export class MembershipCompanyService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'CompanyName', 'Active'];

    List: MembershipCompany[] | undefined;
    ListFilter: MembershipCompany[] | undefined;
    FormData!: MembershipCompany;
    APIURL: string = environment.APIMembershipURL;
    APIRootURL: string = environment.APIMembershipRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "MembershipCompany";
    }
}

