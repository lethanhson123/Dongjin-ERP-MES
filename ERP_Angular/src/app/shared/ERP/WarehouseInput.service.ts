import { Injectable, ViewChild } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { WarehouseInput } from './WarehouseInput.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseInputService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Date', 'Code', 'Name', 'SupplierName', 'CustomerName', 'InvoiceInputName', 'WarehouseOutputName', 'Root', 'IsSync', 'IsComplete', 'Active', 'Save'];
    DisplayColumns002: string[] = ['Date', 'Code', 'SupplierName', 'IsComplete', 'Save'];
    DisplayColumns003: string[] = ['No', 'ID', 'Date', 'Code', 'Name', 'SupplierName', 'CustomerName', 'InvoiceInputName', 'WarehouseOutputName', 'ParentName', 'Root', 'IsSync', 'IsComplete', 'Active', 'Save'];
    DisplayColumns004: string[] = ['Date', 'Code', 'SupplierName', 'IsComplete'];
    DisplayColumns005: string[] = ['Date', 'Code', 'SupplierName', 'IsComplete', 'Save'];
    DisplayColumns006: string[] = ['ID', 'Date', 'Code', 'SupplierName', 'IsComplete', 'Save'];
    DisplayColumns007: string[] = ['No', 'ID', 'CompanyName', 'SupplierName', 'CustomerName', 'Date', 'Code', 'Name', 'InvoiceInputName', 'WarehouseOutputName', 'ParentName', 'Root', 'IsSync', 'IsComplete', 'Active', 'Save'];
    DisplayColumns008: string[] = ['No', 'ID', 'CompanyName', 'SupplierName', 'CustomerName', 'Date', 'Code', 'Name', 'InvoiceInputName', 'WarehouseOutputName', 'ParentName', 'Root', 'IsSync', 'IsComplete', 'Active'];
    DisplayColumns009: string[] = ['No', 'ID', 'CompanyName', 'SupplierName', 'CustomerName', 'Date', 'Code', 'Name', 'InvoiceInputName', 'WarehouseOutputName', 'ParentName', 'Total', 'TotalFinal', 'Root', 'IsSync', 'IsComplete', 'Active'];
    DisplayColumns010: string[] = ['No', 'ID', 'CompanyName', 'SupplierName', 'CustomerName', 'Date', 'Code', 'IsComplete', 'Active'];
    DisplayColumns011: string[] = ['Code', 'SupplierName', 'IsComplete', 'Save'];

    List: WarehouseInput[] | undefined;
    ListFilter: WarehouseInput[] | undefined;
    FormData!: WarehouseInput;
    APIURL: string = environment.APIWarehouseInputURL;
    APIRootURL: string = environment.APIWarehouseInputRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseInput";
    }
    SaveAndUploadFileStockAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveAndUploadFileStockAsync';
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
    ComponentGetByCustomerID_Active_IsCompleteToListAsync(Service: BaseService) {
        this.GetByCustomerID_Active_IsCompleteToListAsync().subscribe(
            res => {
                this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            },
            err => {
            },
            () => {
            }
        );
    }
    GetByCustomerID_Active_IsCompleteToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCustomerID_Active_IsCompleteToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCustomerID_Active_IsComplete_ActionToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCustomerID_Active_IsComplete_ActionToListAsync';
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
    GetByMembershipID_Active_IsCompleteToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipID_Active_IsCompleteToListAsync';
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
    GetByBarcodeAsync() {
        let url = this.APIURL + this.Controller + '/GetByBarcodeAsync';
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
    CreateAutoByPageIndexAsync() {
        let url = this.APIURL + this.Controller + '/CreateAutoByPageIndexAsync';
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
}

