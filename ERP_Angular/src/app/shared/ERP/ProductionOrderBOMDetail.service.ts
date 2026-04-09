import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { ProductionOrderBOMDetail } from './ProductionOrderBOMDetail.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ProductionOrderBOMDetailService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'ProductionOrderBOMID', 'Display', 'MaterialCode', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityBOM', 'QuantityPO', 'Quantity'];

    List: ProductionOrderBOMDetail[] | undefined;
    ListFilter: ProductionOrderBOMDetail[] | undefined;
    FormData!: ProductionOrderBOMDetail;
    APIURL: string = environment.APIProductionOrderURL;
    APIRootURL: string = environment.APIProductionOrderRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "ProductionOrderBOMDetail";
    }
}

