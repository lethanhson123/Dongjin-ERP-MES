import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { ProductionOrderBOM } from './ProductionOrderBOM.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ProductionOrderBOMService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'BOMCode', 'BOMVersion', 'MaterialCode'];
    

    List: ProductionOrderBOM[] | undefined;
    ListFilter: ProductionOrderBOM[] | undefined;
    FormData!: ProductionOrderBOM;
    APIURL: string = environment.APIProductionOrderURL;
    APIRootURL: string = environment.APIProductionOrderRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "ProductionOrderBOM";
    }
}

