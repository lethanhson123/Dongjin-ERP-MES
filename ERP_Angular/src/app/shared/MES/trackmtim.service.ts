import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { trackmtim } from './trackmtim.model';
import { BaseService } from '../ERP/Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class trackmtimService extends BaseService {
    DisplayColumns001: string[] = ['Active', 'No', 'TRACK_IDX', 'RACKDTM', 'POCode', 'FinishGoodsCode', 'ECN', 'LEAD_NM', 'BARCODE_NM', 'QTY', 'QuantityGAP', 'FinishGoodsList', 'UpdateDate', 'UpdateUserCode',];
    DisplayColumns002: string[] = ['No', 'TRACK_IDX', 'RACKDTM', 'LEAD_NM', 'QTY', 'BARCODE_NM', 'FinishGoodsCode', 'ECN', 'POCode', 'FinishGoodsList',];
    DisplayColumns003: string[] = ['Save', 'RACKCODE', 'No', 'TRACK_IDX', 'RACKDTM', 'LEAD_NM', 'QTY', 'BARCODE_NM', 'FinishGoodsCode', 'ECN', 'POCode', 'FinishGoodsList',];


    List: trackmtim[] | undefined;
    ListFilter: trackmtim[] | undefined;
    FormData!: trackmtim;
    APIURL: string = environment.APIMESURL;
    APIRootURL: string = environment.APIMESRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "trackmtim";
    }    
    GetByCompanyID_LeadNo_FinishGoodsToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_LeadNo_FinishGoodsToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }    
    SaveByListID_PO_FinishGoods_ECNAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveByListID_PO_FinishGoods_ECNAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }  
    GetByCompanyID_LEADNM_Begin_EndToListAsync() {        
        let url = this.APIURL + this.Controller + '/GetByCompanyID_LEADNM_Begin_EndToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }  
     SaveAsync() {        
        let url = this.APIURL + this.Controller + '/SaveAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }  
}

