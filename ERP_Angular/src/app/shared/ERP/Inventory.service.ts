import { Injectable, ViewChild } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { Inventory } from './Inventory.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class InventoryService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'CompanyName', 'SupplierName', 'Date', 'Code', 'IsSync', 'IsComplete', 'Active'];

    List: Inventory[] | undefined;
    ListFilter: Inventory[] | undefined;
    FormData!: Inventory;
    APIURL: string = environment.APIReportURL;
    APIRootURL: string = environment.APIReportRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "Inventory";
    }
     SyncByIDAsync() {        
        let url = this.APIURL + this.Controller + '/SyncByIDAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentIDAsync() {        
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentIDAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentIDToListAsync';
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
     GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync() {        
        let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

