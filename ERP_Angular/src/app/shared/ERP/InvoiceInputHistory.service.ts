import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { InvoiceInputHistory } from './InvoiceInputHistory.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class InvoiceInputHistoryService extends BaseService {

    DisplayColumns001: string[] = ['Save', 'Active', 'No', 'ID', 'SupplierID', 'Code', 'Name', 'Display', 'DateATA', 'DateETD', 'DateETA', 'DateFT', 'DateReal', ];   

    List: InvoiceInputHistory[] | undefined;
    ListFilter: InvoiceInputHistory[] | undefined;
    FormData!: InvoiceInputHistory;
    APIURL: string = environment.APIInvoiceURL;
    APIRootURL: string = environment.APIInvoiceRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "InvoiceInputHistory";
    }   
}

