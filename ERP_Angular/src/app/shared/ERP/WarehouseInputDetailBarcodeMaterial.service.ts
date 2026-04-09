import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { WarehouseInputDetailBarcodeMaterial } from './WarehouseInputDetailBarcodeMaterial.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseInputDetailBarcodeMaterialService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'WarehouseInputDetailBarcodeID', 'Display', 'MaterialName01', 'MaterialName', 'CategoryUnitName', 'Quantity', 'ECN', 'BOMECNVersion'];

    List: WarehouseInputDetailBarcodeMaterial[] | undefined;
    ListFilter: WarehouseInputDetailBarcodeMaterial[] | undefined;
    FormData!: WarehouseInputDetailBarcodeMaterial;
    APIURL: string = environment.APIWarehouseURL;
    APIRootURL: string = environment.APIWarehouseRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseInputDetailBarcodeMaterial";
    }

     GetByWarehouseInputDetailBarcodeIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByWarehouseInputDetailBarcodeIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

