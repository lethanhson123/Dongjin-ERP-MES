import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { InvoiceOutput } from './InvoiceOutput.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class InvoiceOutputService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Date', 'PurchaseCode', 'SupplierName', 'CustomerName', 'Total', 'Active', 'Save'];

    List: InvoiceOutput[] | undefined;
    ListFilter: InvoiceOutput[] | undefined;
    FormData!: InvoiceOutput;
    APIURL: string = environment.APIInvoiceURL;
    APIRootURL: string = environment.APIInvoiceRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "InvoiceOutput";
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
}

