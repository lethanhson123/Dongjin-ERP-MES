import { Injectable, ViewChild } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { WarehouseRequest } from './WarehouseRequest.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseRequestService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Date', 'Code', 'Name', 'SupplierName', 'CustomerName', 'ParentName', 'IsManagerSupplier', 'IsManagerCustomer', 'IsSync', 'IsDestroy', 'Active', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'Date', 'Code', 'Name', 'SupplierName', 'CustomerName', 'IsManagerSupplier', 'IsManagerCustomer', 'IsSync', 'IsDestroy', 'Active'];
    DisplayColumns003: string[] = ['No', 'ID', 'Date', 'Code', 'Note', 'SupplierName', 'CustomerName', 'ParentName', 'IsManagerSupplier', 'IsManagerCustomer', 'IsSync', 'IsDestroy', 'Active', 'Save'];
    DisplayColumns004: string[] = ['No', 'ID', 'Description', 'Name', 'Note', 'SupplierName', 'CustomerName', 'ParentName', 'IsManagerSupplier', 'Active', 'Save'];
    DisplayColumns005: string[] = ['No', 'ID', 'CompanyName', 'SupplierName', 'CustomerName', 'Description', 'Name', 'Note', 'ParentName', 'IsManagerSupplier', 'Active', 'Save'];
    DisplayColumns006: string[] = ['No', 'ID', 'CompanyName', 'SupplierName', 'CustomerName', 'Description', 'Name', 'Note', 'ParentName', 'IsManagerSupplier', 'Active'];
    DisplayColumns007: string[] = ['No', 'ID', 'Date', 'CustomerName', 'Name', 'IsManagerSupplier', 'Active'];

    List: WarehouseRequest[] | undefined;
    ListFilter: WarehouseRequest[] | undefined;
    ListFilter01: WarehouseRequest[] | undefined;
    FormData!: WarehouseRequest;
    APIURL: string = environment.APIWarehouseURL;
    APIRootURL: string = environment.APIWarehouseRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseRequest";
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
    ComponentGetByConfirmToListAsync(Service: BaseService) {
        if (this.ListFilter) {
            if (this.ListFilter.length == 0) {
                this.GetByConfirmToListAsync().subscribe(
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
    ComponentGetByMembershipID_ConfirmToListAsync(Service: BaseService) {
        if (this.ListFilter) {
            if (this.ListFilter.length == 0) {
                this.GetByMembershipID_ConfirmToListAsync().subscribe(
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
    GetByConfirmToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByConfirmToListAsync';
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
     GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByMembershipID_ConfirmToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipID_ConfirmToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
     GetByCategoryDepartmentID_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentID_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    ExportToExcelAsync() {
        let url = this.APIURL + this.Controller + '/ExportToExcelAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

