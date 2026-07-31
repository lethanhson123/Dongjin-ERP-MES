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
    DisplayColumns002: string[] = ['No', 'ID', 'CompanyName', 'CategoryHRMPhongBanName', 'CategoryHRMChucDanhName', 'UserName', 'Name', 'Email', 'Phone', 'CCCD', 'CategoryHRMTinhTrangLamViecName', 'NgayVaoLam', 'NgayNghiViec', 'Active'];
    DisplayColumns003: string[] = ['Save', 'No', 'ID', 'CompanyName', 'CategoryHRMPhongBanName', 'CategoryHRMChucDanhName', 'UserName', 'Name', 'Email', 'Phone', 'CCCD', 'CategoryHRMTinhTrangLamViecName', 'NgayVaoLam', 'NgayNghiViec', 'Active'];
    DisplayColumns004: string[] = ['No', 'ID', 'UserName', 'Name', 'CategoryHRMKyNangName', 'CategoryDepartmentName', 'CategoryProcessName', 'CategoryHRMCaLamViecName', 'NgayVaoLam', 'Year', 'Month', 'Day', 'IsOnline'];
    DisplayColumns005: string[] = ['Save', 'No', 'ID', 'CompanyName', 'CategoryHRMPhongBanName', 'CategoryHRMChucDanhName', 'UserName', 'Name', 'Active'];
    DisplayColumns006: string[] = ['UserName', 'Name', 'CategoryHRMKyNangName', 'CategoryProcessName', 'CategoryHRMCaLamViecName', 'IsOnline'];
    DisplayColumns101: string[] = ['Active', 'ID', 'Code', 'Name', 'Display'];


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
    Save2026Async() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/Save2026Async';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
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
    PrintAsync() {
        let url = this.APIURL + this.Controller + '/PrintAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_SearchString_DateToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_SearchString_DateToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

