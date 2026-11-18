import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HRMQuanLyCongViec } from './HRMQuanLyCongViec.model';
import { BaseService } from './Base.service';

@Injectable({
    providedIn: 'root'
})
export class HRMQuanLyCongViecService extends BaseService {
    DisplayColumns001: string[] = ['ID', 'No', 'Active', 'CategoryDepartment', 'CreateUser', 'Membership',  'CategoryStatusName','Complete', 'Point', 'DateBegin', 'DateEnd',  'Description', 'File', 'Save'];
    DisplayColumns002: string[] = ['CategoryStatusName', 'SortOrder'];

    List: HRMQuanLyCongViec[] | undefined;
    ListFilter: HRMQuanLyCongViec[] | undefined;
    ListChild: HRMQuanLyCongViec[] | undefined;
    ListParent: HRMQuanLyCongViec[] | undefined;
    List02: HRMQuanLyCongViec[] | undefined;
    List03: HRMQuanLyCongViec[] | undefined;

    DataSourceFilter: any;
    DataSource01: any;
    DataSource02: any;
    DataSource03: any;
    DataSource04: any;

    FormData!: HRMQuanLyCongViec;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "HRMQuanLyCongViec";
    }

    GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToExcelAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToExcelAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync';
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
}
