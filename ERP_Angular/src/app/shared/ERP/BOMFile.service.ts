import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { BOMFile } from './BOMFile.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class BOMFileService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Display', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'Name', 'FileName', 'Save'];
    DisplayColumns003: string[] = ['No', 'ID', 'UpdateDate', 'UpdateUserCode', 'UpdateUserName', 'Name', 'FileName', 'Save'];

    List: BOMFile[] | undefined;
    ListFilter: BOMFile[] | undefined;
    FormData!: BOMFile;
    APIURL: string = environment.APIProductionOrderURL;
    APIRootURL: string = environment.APIProductionOrderRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "BOMFile";
    }
}

