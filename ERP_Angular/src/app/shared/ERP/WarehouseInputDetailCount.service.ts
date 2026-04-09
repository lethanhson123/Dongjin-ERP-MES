import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { WarehouseInputDetailCount } from './WarehouseInputDetailCount.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseInputDetailCountService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Date', 'MaterialName', 'Count', 'ECN', 'Name', 'BOMID', 'Save'];

    List: WarehouseInputDetailCount[] | undefined;
    ListFilter: WarehouseInputDetailCount[] | undefined;
    FormData!: WarehouseInputDetailCount;
    APIURL: string = environment.APIWarehouseURL;
    APIRootURL: string = environment.APIWarehouseRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseInputDetailCount";
    }    
}

