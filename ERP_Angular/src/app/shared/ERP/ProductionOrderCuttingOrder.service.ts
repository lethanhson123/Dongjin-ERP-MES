import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { ProductionOrderCuttingOrder } from './ProductionOrderCuttingOrder.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ProductionOrderCuttingOrderService extends BaseService {
    DisplayColumns001: string[] = ['Save', 'Active', 'No', 'ID', 'Code', 'BOMECN01', 'MaterialCode01', 'MaterialCode', 'Project', 'QuantityTotal', 'Quantity', 'CurrentLeads', 'CTLeads', 'CTLeadsPr', 'Group', 'Print', 'Date', 'Machine', 'BundleSize', 'HookRack', 'Wire', 'T1Dir', 'Term1', 'Strip1', 'Seal1', 'CCHW1', 'ICHW1', 'T2Dir', 'Term2', 'Strip2', 'Seal2', 'CCHW2', 'ICHW2', 'SPST',];
   

    List: ProductionOrderCuttingOrder[] | undefined;
    ListFilter: ProductionOrderCuttingOrder[] | undefined;
    FormData!: ProductionOrderCuttingOrder;
    APIURL: string = environment.APIProductionOrderURL;
    APIRootURL: string = environment.APIProductionOrderRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "ProductionOrderCuttingOrder";
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

