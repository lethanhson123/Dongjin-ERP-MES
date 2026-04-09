import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { MaterialConvert } from './MaterialConvert.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class MaterialConvertService extends BaseService {
    DisplayColumns001: string[] = ['No', 'Code', 'Name', 'Save'];
    DisplayColumns002: string[] = ['No', 'Code', 'Save'];
    DisplayColumns003: string[] = ['No', 'ID','Code', 'SortOrder', 'Active', 'Save'];
    DisplayColumns004: string[] = ['No', 'ID','Code', 'SortOrder', 'UpdateUserCode', 'UpdateDate', 'Active', 'Save'];

    List: MaterialConvert[] | undefined;
    ListFilter: MaterialConvert[] | undefined;
    ListFilter001: MaterialConvert[] | undefined;
    FormData!: MaterialConvert;
    APIURL: string = environment.APIMaterialURL;
    APIRootURL: string = environment.APIMaterialRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "MaterialConvert";
    }
    CreateAutoAsync() {
        let url = this.APIURL + this.Controller + '/CreateAutoAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

