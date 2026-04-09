import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { InvoiceInputDetail } from './InvoiceInputDetail.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class InvoiceInputDetailService extends BaseService {
    
    DisplayColumns001: string[] = ['No', 'MaterialName', 'CategoryUnitName', 'Description', 'PalletNo', 'ShippedNo', 'Quantity', 'Price', 'Total', 'Active', 'Save'];
    DisplayColumns002: string[] = ['No', 'MaterialName', 'Display', 'CategoryUnitName', 'Description', 'Note', 'Quantity', 'Price', 'Total', 'IsNew', 'Active', 'Save'];
    DisplayColumns003: string[] = ['No', 'MaterialName', 'Display', 'CategoryUnitName', 'Description', 'Note', 'QuantityInvoice', 'Quantity', 'Price', 'Total', 'IsNew', 'Active', 'Save'];
    DisplayColumns004: string[] = ['No', 'MaterialName', 'Display', 'CategoryUnitName', 'Description', 'Note', 'QuantitySNP', 'QuantityInvoice', 'Quantity', 'Price', 'Total', 'IsNew', 'Active', 'Save'];
    DisplayColumns005: string[] = ['No', 'MaterialID', 'MaterialName', 'Display', 'CategoryUnitName', 'Description', 'Note', 'QuantitySNP', 'QuantityInvoice', 'Quantity', 'Price', 'Total', 'IsNew', 'Active', 'Save'];
    DisplayColumns006: string[] = ['No', 'ID', 'MaterialName', 'Display', 'CategoryUnitName', 'Description', 'Note', 'QuantitySNP', 'QuantityInvoice', 'Quantity', 'Price', 'Total', 'IsNew', 'Active', 'Save'];

    List: InvoiceInputDetail[] | undefined;
    ListFilter: InvoiceInputDetail[] | undefined;
    FormData!: InvoiceInputDetail;
    APIURL: string = environment.APIInvoiceURL;
    APIRootURL: string = environment.APIInvoiceRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "InvoiceInputDetail";
    }
}

