import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { Membership } from './Membership.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class MembershipService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'CompanyName', 'CategoryDepartmentName', 'CategoryPositionName', 'UserName', 'Name', 'Email', 'Phone', 'CreateDate', 'UpdateDate', 'USER_IDX', 'Active'];

    List: Membership[] | undefined;
    ListFilter: Membership[] | undefined;
    FormData!: Membership;
    APIURL: string = environment.APIMembershipURL;
    APIRootURL: string = environment.APIMembershipRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "Membership";
    }
    ComponentGetByCategoryDepartmentID_ActiveToListAsync(Service: BaseService) {
        this.GetByCategoryDepartmentID_ActiveToListAsync().subscribe(
            res => {
                this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            },
            err => {
            },
            () => {
            }
        );
    }
    ComponentGetByCategoryDepartmentID_CategoryPositionID_ActiveToListAsync(Service: BaseService) {
        this.GetByCategoryDepartmentID_CategoryPositionID_ActiveToListAsync().subscribe(
            res => {
                this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            },
            err => {
            },
            () => {
            }
        );
    }
    AuthenticationAsync() {
        let url = this.APIURL + this.Controller + '/AuthenticationAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }

    CreateAutoAsync() {
        let url = this.APIURL + this.Controller + '/CreateAutoAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentID_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentID_ActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentID_CategoryPositionID_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentID_CategoryPositionID_ActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    IsPasswordValidWithRegex() {
        let url = this.APIURL + this.Controller + '/IsPasswordValidWithRegex';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
   
}

