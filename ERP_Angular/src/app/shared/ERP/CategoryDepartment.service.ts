import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryDepartment } from './CategoryDepartment.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryDepartmentService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Code', 'Name', 'Display', 'Note', 'SortOrder', 'Active', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'CompanyName', 'MESID', 'Code', 'Name', 'Display', 'Note', 'SortOrder', 'Active', 'Save'];
    DisplayColumns003: string[] = ['No', 'ID', 'CompanyName', 'ParentName', 'MESID', 'Code', 'Name', 'Display', 'Note', 'SortOrder', 'Active', 'Save'];
    DisplayColumns004: string[] = ['No', 'ID', 'CompanyName', 'ParentName', 'MESID', 'Code', 'Name', 'Display', 'Note', 'SortOrder', 'IsSync', 'Active', 'Save'];
    DisplayColumns005: string[] = ['No', 'ID', 'CompanyName', 'ParentName', 'MESID', 'Code', 'Name', 'Display', 'Note', 'SortOrder', 'IsSync', 'IsFinishGoods', 'Active', 'Save'];

    List: CategoryDepartment[] | undefined;
    ListFilter: CategoryDepartment[] | undefined;
    FormData!: CategoryDepartment;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryDepartment";
    }
    ComponentGetByMembershipID_ActiveToListAsync(Service: BaseService) {
        if (this.ListFilter) {
            if (this.ListFilter.length == 0) {
                this.BaseParameter.Active = true;
                this.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
                this.GetByMembershipID_ActiveToListAsync().subscribe(
                    res => {
                        this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
                    },
                    err => {
                    },
                    () => {
                    }
                );
            }
        }
    }
    ComponentGetByCompanyIDAndActiveToList(Service: BaseService) {
        this.BaseParameter.Active = true;
        this.GetByCompanyIDAndActiveToList().subscribe(
            res => {
                this.ListParent = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));                
            },
            err => {
            },
            () => {
            }
        );
    }
    ComponentGetByMembershipID_CompanyID_ActiveToListAsync(Service: BaseService) {
        this.BaseParameter.Active = true;
        this.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
        this.GetByMembershipID_CompanyID_ActiveToListAsync().subscribe(
            res => {
                this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
                // if (this.ListFilter && this.ListFilter.length > 0) {
                //     Service.BaseParameter.CategoryDepartmentID = this.ListFilter[0].ID;
                // }
            },
            err => {
            },
            () => {
            }
        );
    }
    CreateAutoAsync() {
        let url = this.APIURL + this.Controller + '/CreateAutoAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByMembershipID_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipID_ActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByMembershipID_CompanyID_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipID_CompanyID_ActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

