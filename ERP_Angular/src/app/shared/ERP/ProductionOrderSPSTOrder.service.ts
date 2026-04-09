import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { ProductionOrderSPSTOrder } from './ProductionOrderSPSTOrder.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ProductionOrderSPSTOrderService extends BaseService {
    DisplayColumns001: string[] = ['Save', 'Active', 'No', 'ID', 'Code', 'BOMECN01', 'MaterialCode01', 'MaterialCode', 'QuantityTotal', 'Quantity', 'Machine', 'BundleSize', 'Date', 'SPST',];
   

    List: ProductionOrderSPSTOrder[] | undefined;
    ListFilter: ProductionOrderSPSTOrder[] | undefined;
    FormData!: ProductionOrderSPSTOrder;
    APIURL: string = environment.APIProductionOrderURL;
    APIRootURL: string = environment.APIProductionOrderRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "ProductionOrderSPSTOrder";
    }
    GetByParentID_DateToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByParentID_DateToListAsync';
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
    SyncByParentID_DateToListAsync() {
        let url = this.APIURL + this.Controller + '/SyncByParentID_DateToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncMESByParentID_DateToListAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SyncMESByParentID_DateToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

