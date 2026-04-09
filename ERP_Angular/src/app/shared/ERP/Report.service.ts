import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { Report } from './Report.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ReportService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'ParentID', 'ParentName', 'CreateDate', 'CreateUserID', 'CreateUserCode', 'CreateUserName', 'UpdateDate', 'UpdateUserID', 'UpdateUserCode', 'UpdateUserName', 'RowVersion', 'SortOrder', 'Active', 'Code', 'Name', 'Display', 'Description', 'Note', 'FileName', 'CompanyID', 'CompanyName', 'Date', 'Year', 'Month', 'Day', 'Save'];

    List: Report[] | undefined;
    ListFilter: Report[] | undefined;
    FormData!: Report;
    APIURL: string = environment.APIReportURL;
    APIRootURL: string = environment.APIReportRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "Report";
    }

    GetWarehouse001_001ToListAsync() {
        let url = this.APIURL + this.Controller + '/GetWarehouse001_001ToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetWarehouse001_002ToListAsync() {
        let url = this.APIURL + this.Controller + '/GetWarehouse001_002ToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetWarehouse001_003ToListAsync() {
        let url = this.APIURL + this.Controller + '/GetWarehouse001_003ToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetWarehouse001_004ToListAsync() {
        let url = this.APIURL + this.Controller + '/GetWarehouse001_004ToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetWarehouse001_005ToListAsync() {
        let url = this.APIURL + this.Controller + '/GetWarehouse001_005ToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetWarehouse001_006ToListAsync() {
        let url = this.APIURL + this.Controller + '/GetWarehouse001_006ToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetWarehouse001_007ToListAsync() {
        let url = this.APIURL + this.Controller + '/GetWarehouse001_007ToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByProductionTrackingToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByProductionTrackingToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByProductionTracking2026Async() {
        let url = this.APIURL + this.Controller + '/GetByProductionTracking2026Async';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncByProductionTracking2026Async() {
        let url = this.APIURL + this.Controller + '/SyncByProductionTracking2026Async';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByWarehouseStockLongTermAsync() {
        let url = this.APIURL + this.Controller + '/GetByWarehouseStockLongTermAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncByWarehouseStockLongTermAsync() {
        let url = this.APIURL + this.Controller + '/SyncByWarehouseStockLongTermAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    HookRackGetByCompanyID_Begin_End_SearchStringAsync() {
        let url = this.APIURL + this.Controller + '/HookRackGetByCompanyID_Begin_End_SearchStringAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

