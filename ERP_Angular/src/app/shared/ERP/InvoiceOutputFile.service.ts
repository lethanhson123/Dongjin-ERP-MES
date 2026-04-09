import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { InvoiceOutputFile } from './InvoiceOutputFile.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class InvoiceOutputFileService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Display', 'Save'];

    List: InvoiceOutputFile[] | undefined;
    ListFilter: InvoiceOutputFile[] | undefined;
    FormData!: InvoiceOutputFile;
    APIURL: string = environment.APIInvoiceURL;
    APIRootURL: string = environment.APIInvoiceRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "InvoiceOutputFile";
    }
}

