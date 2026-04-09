import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { InvoiceInput } from './InvoiceInput.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class InvoiceInputService extends BaseService {

    DisplayColumns001: string[] = ['No', 'ID', 'Date', 'PurchaseCode', 'SupplierName', 'CustomerName', 'Total', 'Active', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'Date', 'Code', 'SupplierName', 'CustomerName', 'Total', 'Active', 'Save'];
    DisplayColumns003: string[] = ['No', 'ID', 'Date', 'Code', 'SupplierName', 'CustomerName', 'Total', 'Active', 'IsFuture', 'Save'];
    DisplayColumns004: string[] = ['No', 'ID', 'DateETD', 'DateETA', 'Code', 'SupplierName', 'CustomerName', 'Total', 'Active', 'IsFuture', 'Save'];
    DisplayColumns005: string[] = ['No', 'DateETD', 'DateETA', 'Code', 'SupplierName', 'CustomerName', 'Active', 'IsFuture'];
    DisplayColumns006: string[] = ['No', 'ID', 'DateETD', 'DateETA', 'Code', 'SupplierName', 'CustomerName', 'Total', 'Active', 'IsSync', 'IsFuture', 'Save'];
    DisplayColumns007: string[] = ['No', 'ID', 'DateETD', 'DateETA', 'Code', 'SupplierName', 'CustomerName', 'Total', 'TotalQuantity', 'Active', 'IsComplete', 'IsFuture', 'IsSync'];
    DisplayColumns008: string[] = ['No', 'ID', 'DateETD', 'DateETA', 'Code', 'SupplierName', 'CustomerName', 'Total', 'TotalQuantity', 'Active', 'IsComplete', 'IsFuture', 'IsSync', 'Save'];

    List: InvoiceInput[] | undefined;
    ListFilter: InvoiceInput[] | undefined;
    FormData!: InvoiceInput;
    APIURL: string = environment.APIInvoiceURL;
    APIRootURL: string = environment.APIInvoiceRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "InvoiceInput";
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
    GetByActive_IsFutureToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByActive_IsFutureToListAsync';
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
    GetByMembershipID_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipID_ActiveToListAsync';
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
    GetByCompanyID_Year_Month_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_Year_Month_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

