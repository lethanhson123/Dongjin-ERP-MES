import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { WarehouseOutputDetailBarcodeMaterial } from './WarehouseOutputDetailBarcodeMaterial.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseOutputDetailBarcodeMaterialService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'WarehouseOutputDetailBarcodeID', 'Display', 'MaterialName01', 'MaterialName', 'CategoryUnitName', 'Quantity', 'ECN', 'BOMECNVersion'];

    List: WarehouseOutputDetailBarcodeMaterial[] | undefined;
    ListFilter: WarehouseOutputDetailBarcodeMaterial[] | undefined;
    FormData!: WarehouseOutputDetailBarcodeMaterial;
    APIURL: string = environment.APIWarehouseURL;
    APIRootURL: string = environment.APIWarehouseRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseOutputDetailBarcodeMaterial";
    }

     GetByWarehouseOutputDetailBarcodeIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByWarehouseOutputDetailBarcodeIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

