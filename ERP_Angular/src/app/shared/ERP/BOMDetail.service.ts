import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { BOMDetail } from './BOMDetail.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class BOMDetailService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Code', 'Display', 'Description', 'CategoryMaterialName02', 'MaterialName02', 'CategoryUnitName02', 'Quantity02', 'Active', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'ParentID', 'Code', 'Display', 'CategoryMaterialName02', 'MaterialName02', 'CategoryUnitName02', 'Quantity02', 'Active'];
    DisplayColumns003: string[] = ['No', 'ID', 'Code', 'Display', 'Description', 'CategoryMaterialName02', 'MaterialCode02', 'CategoryUnitName02', 'Quantity02', 'Active', 'Save'];
    DisplayColumns004: string[] = ['No', 'ID', 'Code', 'Display', 'Description', 'CategoryMaterialName02', 'MaterialName02', 'CategoryUnitName02', 'Quantity02', 'QuantityActual', 'Active', 'Save'];
    DisplayColumns005: string[] = ['No', 'ID', 'ParentName', 'MaterialName02', 'CategoryUnitName02', 'QuantityActual', 'Active'];
    DisplayColumns006: string[] = ['No', 'ID', 'ParentName', 'MaterialName02', 'CategoryUnitName02', 'QuantityActual', 'QuantityCompare', 'Percent', 'Active'];
    DisplayColumns007: string[] = ['No', 'ID', 'Code', 'Display', 'Description', 'CategoryMaterialName02', 'MaterialName02', 'CategoryUnitName02', 'Quantity02', 'QuantityActual', 'Active', 'Save'];
    DisplayColumns008: string[] = ['No', 'ID', 'MaterialName02', 'CategoryUnitName02', 'Quantity02', 'QuantityActual', 'QuantityCompare', 'Percent', 'Code', 'Display', 'Description', 'Category', 'WRNo', 'Wire', 'Diameter', 'Color', 'Active', 'Save',];
    DisplayColumns009: string[] = ['No', 'ID', 'MaterialCode02', 'CategoryUnitName02', 'Quantity02', 'QuantityActual', 'QuantityCompare', 'Percent', 'Code', 'Display', 'Description', 'Category', 'WRNo', 'Wire', 'Diameter', 'Color', 'Note', 'Active', 'Save',];
    DisplayColumns010: string[] = ['No', 'UpdateDate', 'Code', 'Name', 'Display', 'Active', 'SortOrder', 'RowVersion', 'QuantityActual', 'Quantity01', 'Quantity02', 'Description'];
    DisplayColumns011: string[] = ['Save', 'Active',  'No', 'ID', 'MaterialCode02', 'Quantity02', 'QuantitySumActual', 'QuantitySumCompare', 'PercentSum', 'Code', 'Note', 'Wire', 'Diameter', 'Color', 'Display', 'Description', 'Category', 'WRNo', 'FileName',];

    List: BOMDetail[] | undefined;
    ListFilter: BOMDetail[] | undefined;
    FormData!: BOMDetail;
    APIURL: string = environment.APIMaterialURL;
    APIRootURL: string = environment.APIMaterialRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "BOMDetail";
    }

    GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync() {
        let url = this.APIURL + this.Controller + '/GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncFinishGoodsListOftrackmtimAsync() {
        let url = this.APIURL + this.Controller + '/SyncFinishGoodsListOftrackmtimAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

