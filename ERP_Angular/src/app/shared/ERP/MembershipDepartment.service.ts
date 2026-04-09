import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { MembershipDepartment } from './MembershipDepartment.model';
import { BaseService } from './Base.service';
@Injectable({
    providedIn: 'root'
})
export class MembershipDepartmentService extends BaseService {    
    DisplayColumns001: string[] = ['No', 'ID', 'CategoryDepartmentName', 'Active'];

    List: MembershipDepartment[] | undefined;
    ListFilter: MembershipDepartment[] | undefined;
    FormData!: MembershipDepartment;
    APIURL: string = environment.APIMembershipURL;
    APIRootURL: string = environment.APIMembershipRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "MembershipDepartment";
    }
}

