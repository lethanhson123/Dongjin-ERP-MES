import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { TraceabilityDetail } from './TraceabilityDetail.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class TraceabilityDetailService extends BaseService {
    DisplayColumns001: string[] = ['No', 'Date', 'MaterialCode', 'Barcode',];
    DisplayColumns0021: string[] = ['No', 'Date', 'MaterialCode', 'Barcode',];
    DisplayColumns002: string[] = ['No', 'CREATE_DTM', 'CREATE_USER', 'MC2', 'COLSIP', 'STRENGTH', 'RES_H', 'RES_V',];
    DisplayColumns003: string[] = ['No', 'CREATE_DTM', 'CREATE_USER', 'MC2', 'LOC_LRJ', 'COLSIP', 'WIRE_FORCE', 'STRENGTH', 'CCH', 'CCW', 'ICH', 'ICW',];
    DisplayColumns004: string[] = ['No', 'CREATE_DTM', 'CREATE_USER', 'MC2', 'CHK_LR', 'COLSIP', 'WIRE_FORCE', 'STRENGTH', 'CCH_W1', 'CCH1', 'CCW1', 'ICH_W1', 'ICH1', 'ICW1', 'CCH_W2', 'CCH2', 'CCW2', 'ICH_W2', 'ICH2', 'ICW2',];
    DisplayColumns005: string[] = ['No', 'CREATE_DTM', 'CREATE_USER', 'MC', 'MC2', 'COLSIP', 'WIRE_FORCE', 'WIRE_LENGTH', 'CCH_W1', 'CCH1', 'CCW1', 'ICH_W1', 'ICH1', 'ICW1', 'CCH_W2', 'CCH2', 'CCW2', 'ICH_W2', 'ICH2', 'ICW2',];
    DisplayColumns006: string[] = ['No', 'Date', 'UpdateUserCode', 'InvoiceInputName', 'MaterialName', 'Quantity', 'Barcode',];
    DisplayColumns007: string[] = ['No', 'ParentID', 'ParentID01', 'ParentID02', 'ParentID03', 'ParentID04', 'Quantity', 'UpdateDate', 'Note',];


    List: TraceabilityDetail[] | undefined;
    ListFilter: TraceabilityDetail[] | undefined;


    FormData!: TraceabilityDetail;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "TraceabilityDetail";
    }
    GetByParentIDAndActiveToListModelTranferAsync() {
        let url = this.APIURL + this.Controller + '/GetByParentIDAndActiveToListModelTranferAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

