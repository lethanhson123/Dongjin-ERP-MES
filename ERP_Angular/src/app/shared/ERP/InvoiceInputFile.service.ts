import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { InvoiceInputFile } from './InvoiceInputFile.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class InvoiceInputFileService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Display', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'Name', 'FileName', 'Save'];

    List: InvoiceInputFile[] | undefined;
    ListFilter: InvoiceInputFile[] | undefined;
    FormData!: InvoiceInputFile;
    APIURL: string = environment.APIInvoiceURL;
    APIRootURL: string = environment.APIInvoiceRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "InvoiceInputFile";
    }
}

