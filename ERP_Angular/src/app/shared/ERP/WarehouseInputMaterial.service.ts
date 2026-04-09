import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { WarehouseInputMaterial } from './WarehouseInputMaterial.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseInputMaterialService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'MaterialName', 'Quantity', 'Save'];

    List: WarehouseInputMaterial[] | undefined;
    ListFilter: WarehouseInputMaterial[] | undefined;
    FormData!: WarehouseInputMaterial;
    APIURL: string = environment.APIWarehouseURL;
    APIRootURL: string = environment.APIWarehouseRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseInputMaterial";
    }    
}

