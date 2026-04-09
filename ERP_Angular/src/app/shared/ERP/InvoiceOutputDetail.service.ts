import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { InvoiceOutputDetail } from './InvoiceOutputDetail.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class InvoiceOutputDetailService extends BaseService {
    DisplayColumns001: string[] = ['No', 'MaterialName', 'CategoryUnitName', 'Description', 'PalletNo', 'ShippedNo', 'Quantity', 'Price', 'Total', 'Active', 'Save'];

    List: InvoiceOutputDetail[] | undefined;
    ListFilter: InvoiceOutputDetail[] | undefined;
    FormData!: InvoiceOutputDetail;
    APIURL: string = environment.APIInvoiceURL;
    APIRootURL: string = environment.APIInvoiceRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "InvoiceOutputDetail";
    }
}

