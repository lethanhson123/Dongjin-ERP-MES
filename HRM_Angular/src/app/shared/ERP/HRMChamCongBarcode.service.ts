import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { HRMChamCongBarcode } from './HRMChamCongBarcode.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class HRMChamCongBarcodeService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'CompanyName', 'Active', 'MESID', 'Date', 'MaterialCode', 'BOMID', 'BundleSize', 'Quantity', 'Barcode', ];
    

    List: HRMChamCongBarcode[] | undefined;
    ListFilter: HRMChamCongBarcode[] | undefined;
    FormData!: HRMChamCongBarcode;    
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "HRMChamCongBarcode";
    }

    GetBySearchString_RandomToListAsync() {
        let url = this.APIURL + this.Controller + '/GetBySearchString_RandomToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }   
    GetByCategoryDepartmentID_SearchString_RandomToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentID_SearchString_RandomToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }   
    GetCodeAsync() {
        let url = this.APIURL + this.Controller + '/GetCodeAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }   
}

