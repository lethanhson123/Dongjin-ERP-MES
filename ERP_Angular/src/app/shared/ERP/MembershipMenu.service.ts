import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { MembershipMenu } from './MembershipMenu.model';
import { BaseService } from './Base.service';
@Injectable({
    providedIn: 'root'
})
export class MembershipMenuService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Display', 'CategoryMenuName', 'Active'];
    DisplayColumns002: string[] = ['No', 'ID', 'SortOrder', 'Display', 'CategoryMenuName', 'Active'];

    List: MembershipMenu[] | undefined;
    ListFilter: MembershipMenu[] | undefined;
    FormData!: MembershipMenu;
    APIURL: string = environment.APIMembershipURL;
    APIRootURL: string = environment.APIMembershipRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "MembershipMenu";
    }
}

