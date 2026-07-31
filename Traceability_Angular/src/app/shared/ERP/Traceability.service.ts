import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { Traceability } from './Traceability.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class TraceabilityService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'CompanyName', 'Active', 'MESID', 'Date', 'MaterialCode', 'BOMID', 'BundleSize', 'Quantity', 'Barcode',];
    DisplayColumns002: string[] = ['CompanyName', 'Date', 'Barcode',];

    List: Traceability[] | undefined;
    ListFilter: Traceability[] | undefined;
    FormData!: Traceability;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "Traceability";
    }

    GetByCompanyID_Begin_End_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_Begin_End_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByRandomToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByRandomToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetBySearchStringAsync() {
        let url = this.APIURL + this.Controller + '/GetBySearchStringAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_Begin_End_SearchString_ActionToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_Begin_End_SearchString_ActionToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

