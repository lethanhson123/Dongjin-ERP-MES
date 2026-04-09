import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { InventoryDetail } from './InventoryDetail.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class InventoryDetailService extends BaseService {
    DisplayColumns001: string[] = ['Save', 'No', 'ID', 'Week', 'FileName', 'MaterialName', 'CategoryLocationName', 'Note', 'Quantity', 'QuantityActual', 'QuantityGAP', 'CreateDate', 'CreateUserCode', 'Active'];
    DisplayColumns002: string[] = ['No', 'Week', 'MaterialName', 'CategoryLocationName', 'Quantity'];
    DisplayColumns003: string[] = ['Save', 'No', 'ID', 'Week', 'MaterialName', 'CategoryLocationName', 'Quantity', 'QuantityActual', 'QuantityGAP'];
    DisplayColumns004: string[] = ['Save', 'Active', 'No', 'ID', 'Week',  'MaterialID', 'MaterialName', 'QuantityActual', 'Quantity', 'QuantityGAP', 'CategoryLocationName', 'FileName', 'Note', 'Display', 'CreateDate', 'CreateUserCode', ];
    DisplayColumns005: string[] = ['No', 'Week', 'MaterialName', 'Quantity', 'Count'];

    List: InventoryDetail[] | undefined;
    ListFilter: InventoryDetail[] | undefined;
    FormData!: InventoryDetail;
    APIURL: string = environment.APIReportURL;
    APIRootURL: string = environment.APIReportRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "InventoryDetail";
    }
    PrintByIDAsync() {
        let url = this.APIURL + this.Controller + '/PrintByIDAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    Print2025ByIDAsync() {
        let url = this.APIURL + this.Controller + '/Print2025ByIDAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    PrintByListAsync() {
        let url = this.APIURL + this.Controller + '/PrintByListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    Print2025ByListAsync() {
        let url = this.APIURL + this.Controller + '/Print2025ByListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncByParentIDToListAsync() {
        let url = this.APIURL + this.Controller + '/SyncByParentIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncByParentIDCategoryLocationNameToListAsync() {
        let url = this.APIURL + this.Controller + '/SyncByParentIDCategoryLocationNameToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    ExportToExcelAsync() {
        let url = this.APIURL + this.Controller + '/ExportToExcelAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

