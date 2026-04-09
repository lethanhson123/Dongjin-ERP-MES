import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { BOMCompare } from './BOMCompare.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class BOMCompareService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Code', 'MaterialName', 'ECN01', 'Date01', 'Quantity01', 'QuantityGAP01', 'ECN02', 'Date02', 'Quantity02', 'QuantityGAP02', 'ECN03', 'Date03', 'Quantity03', 'QuantityGAP03', 'ECN04', 'Date04', 'Quantity04', 'QuantityGAP04', 'ECN05', 'Date05', 'Quantity05', 'QuantityGAP05', 'ECN06', 'Date06', 'Quantity06', 'QuantityGAP06', 'ECN07', 'Date07', 'Quantity07', 'QuantityGAP07', 'ECN08', 'Date08', 'Quantity08', 'QuantityGAP08', 'ECN09', 'Date09', 'Quantity09', 'QuantityGAP09'];
    DisplayColumns002: string[] = ['No', 'ID', 'Code', 'MaterialName', 'ECN01', 'Note01', 'Date01', 'Quantity01', 'QuantityGAP01', 'ECN02', 'Note02', 'Date02', 'Quantity02', 'QuantityGAP02', 'ECN03', 'Note03', 'Date03', 'Quantity03', 'QuantityGAP03', 'ECN04', 'Note04', 'Date04', 'Quantity04', 'QuantityGAP04', 'ECN05', 'Note05', 'Date05', 'Quantity05', 'QuantityGAP05', 'ECN06', 'Note06', 'Date06', 'Quantity06', 'QuantityGAP06', 'ECN06', 'Note07', 'Date07', 'Quantity07', 'QuantityGAP07', 'ECN08', 'Note08', 'Date08', 'Quantity08', 'QuantityGAP08', 'ECN09', 'Note09', 'Date09', 'Quantity09', 'QuantityGAP09'];

    List: BOMCompare[] | undefined;
    ListFilter: BOMCompare[] | undefined;
    FormData!: BOMCompare;
    APIURL: string = environment.APIMaterialURL;
    APIRootURL: string = environment.APIMaterialRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "BOMCompare";
    }

    GetCompanyID_YearBegin_YearEndToListAsync() {
        let url = this.APIURL + this.Controller + '/GetCompanyID_YearBegin_YearEndToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncByCompanyID_YearBegin_YearEndToListAsync() {
        let url = this.APIURL + this.Controller + '/SyncByCompanyID_YearBegin_YearEndToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

