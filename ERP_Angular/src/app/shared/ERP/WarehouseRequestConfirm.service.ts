import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { WarehouseRequestConfirm } from './WarehouseRequestConfirm.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseRequestConfirmService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Date', 'Description', 'MembershipName', 'Note', 'Active', 'Save'];

    List: WarehouseRequestConfirm[] | undefined;
    ListFilter: WarehouseRequestConfirm[] | undefined;
    FormData!: WarehouseRequestConfirm;
    APIURL: string = environment.APIWarehouseURL;
    APIRootURL: string = environment.APIWarehouseRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseRequestConfirm";
    }
}

