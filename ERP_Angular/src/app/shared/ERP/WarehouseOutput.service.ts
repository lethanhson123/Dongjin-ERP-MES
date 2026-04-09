import { Injectable, ViewChild } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { WarehouseOutput } from './WarehouseOutput.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseOutputService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Date', 'Code', 'Name', 'SupplierName', 'CustomerName', 'WarehouseRequestName', 'IsSync', 'IsComplete', 'Active', 'Save'];
    DisplayColumns002: string[] = ['Date', 'Code', 'CustomerName', 'MembershipName', 'IsComplete', 'Save'];
    DisplayColumns003: string[] = ['Date', 'Code', 'MembershipName', 'IsComplete', 'Save'];
    DisplayColumns004: string[] = ['ID', 'Date', 'Code', 'CustomerName', 'MembershipName', 'IsComplete', 'Save'];
    DisplayColumns005: string[] = ['No', 'ID', 'Date', 'Description', 'Name', 'SupplierName', 'CustomerName', 'WarehouseRequestName', 'IsSync', 'IsComplete', 'Active', 'Save'];
    DisplayColumns006: string[] = ['ID', 'Code', 'CustomerName', 'MembershipName', 'IsComplete', 'Save'];
    DisplayColumns007: string[] = ['No', 'ID', 'CompanyName', 'SupplierName', 'CustomerName', 'Date', 'Description', 'Name', 'WarehouseRequestName', 'IsSync', 'IsComplete', 'Active', 'Save'];
    DisplayColumns008: string[] = ['No', 'ID', 'CompanyName', 'SupplierName', 'CustomerName', 'Date', 'Description', 'Name', 'WarehouseRequestName', 'IsSync', 'IsComplete', 'Active'];
    DisplayColumns009: string[] = ['No', 'ID', 'Date', 'CustomerName', 'Name', 'IsSync', 'IsComplete', 'Active'];
    DisplayColumns010: string[] = ['No', 'ID', 'Date', 'CustomerName', 'Name', 'Total', 'IsSync', 'IsComplete', 'Active'];
    DisplayColumns011: string[] = ['No', 'ID', 'CompanyName', 'SupplierName', 'CustomerName', 'Date', 'Code', 'IsComplete', 'Active'];
    DisplayColumns012: string[] = ['Code', 'CustomerName', 'MembershipName', 'IsComplete', 'Save'];
    DisplayColumns013: string[] = ['No', 'ID', 'CompanyName', 'SupplierName', 'CustomerName', 'Date', 'Description', 'Code', 'Name', 'WarehouseRequestName', 'IsSync', 'IsComplete', 'Active', 'Save'];

    List: WarehouseOutput[] | undefined;
    ListFilter: WarehouseOutput[] | undefined;
    ListFilter1: WarehouseOutput[] | undefined;
    FormData!: WarehouseOutput;
    APIURL: string = environment.APIWarehouseOutputURL;
    APIRootURL: string = environment.APIWarehouseOutputRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseOutput";
    }
    SearchGetAllToListAsync(sort: MatSort, paginator: MatPaginator) {
        this.IsShowLoading = true;
        this.GetAllToListAsync().subscribe(
            res => {
                this.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));
                this.DataSource = new MatTableDataSource(this.List);
                this.DataSource.sort = sort;
                this.DataSource.paginator = paginator;
            },
            err => {
            },
            () => {
                this.IsShowLoading = false;
            }
        );
    }
    ComponentGetBySupplierID_ActiveToListAsync(Service: BaseService) {
        this.GetBySupplierID_ActiveToListAsync().subscribe(
            res => {
                this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            },
            err => {
            },
            () => {
            }
        );
    }
    GetBySupplierID_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetBySupplierID_ActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    ComponentGetBySupplierID_Active_IsCompleteToListAsync(Service: BaseService) {
        this.GetBySupplierID_Active_IsCompleteToListAsync().subscribe(
            res => {
                this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            },
            err => {
            },
            () => {
            }
        );
    }
    ComponentGetByMembershipID_Active_IsCompleteToListAsync(Service: BaseService) {
        if (this.ListFilter) {
            if (this.ListFilter.length == 0) {
                this.BaseParameter.Active = true;
                this.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
                this.GetByMembershipID_Active_IsCompleteToListAsync().subscribe(
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
    GetBySupplierID_Active_IsCompleteToListAsync() {
        let url = this.APIURL + this.Controller + '/GetBySupplierID_Active_IsCompleteToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetBySupplierID_Active_IsComplete_ActionToListAsync() {
        let url = this.APIURL + this.Controller + '/GetBySupplierID_Active_IsComplete_ActionToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByMembershipIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByMembershipID_DateBegin_DateEndToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipID_DateBegin_DateEndToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByMembershipID_Active_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipID_Active_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByMembershipID_Active_IsCompleteToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipID_Active_IsCompleteToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByMembershipID_Active_IsComplete_ActionToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipID_Active_IsComplete_ActionToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByBarcodeFreedomNoFIFOAsync() {
        let url = this.APIURL + this.Controller + '/GetByBarcodeFreedomNoFIFOAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByBarcodeFreedomAsync() {
        let url = this.APIURL + this.Controller + '/GetByBarcodeFreedomAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByBarcodeAsync() {
        let url = this.APIURL + this.Controller + '/GetByBarcodeAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByBarcodeNoFIFOAsync() {
        let url = this.APIURL + this.Controller + '/GetByBarcodeNoFIFOAsync';
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
    PrintGroupAsync() {
        let url = this.APIURL + this.Controller + '/PrintGroupAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    PrintGroup2025Async() {
        let url = this.APIURL + this.Controller + '/PrintGroup2025Async';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    PrintGroup2026Async() {
        let url = this.APIURL + this.Controller + '/PrintGroup2026Async';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    PrintSumAsync() {
        let url = this.APIURL + this.Controller + '/PrintSumAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_CategoryDepartmentID_Action_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryDepartmentID_Action_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncFromMESByCompanyID_CategoryDepartmentIDAsync() {
        let url = this.APIURL + this.Controller + '/SyncFromMESByCompanyID_CategoryDepartmentIDAsync';
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

