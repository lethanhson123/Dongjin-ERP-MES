import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { WarehouseOutputMaterial } from './WarehouseOutputMaterial.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseOutputMaterialService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'MaterialName', 'Quantity', 'Save'];

    List: WarehouseOutputMaterial[] | undefined;
    ListFilter: WarehouseOutputMaterial[] | undefined;
    FormData!: WarehouseOutputMaterial;
    APIURL: string = environment.APIWarehouseURL;
    APIRootURL: string = environment.APIWarehouseRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseOutputMaterial";
    }    
}

