import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { WarehouseOutputDetail } from './WarehouseOutputDetail.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseOutputDetailService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'FileName', 'DateScan', 'DateInput', 'Barcode', 'CategoryLocationName', 'Display', 'CategoryUnitName', 'QuantityRequest', 'Quantity', 'Note', 'Active', 'Save'];
    DisplayColumns002: string[] = ['DateInput', 'DateScan', 'Display', 'QuantityRequest', 'Quantity', 'Note', 'Active', 'Save'];
    DisplayColumns003: string[] = ['No', 'FileName', 'DateScan', 'Barcode', 'CategoryLocationName', 'Code', 'Display', 'CategoryUnitName', 'QuantityRequest', 'Quantity', 'Active'];
    DisplayColumns004: string[] = ['Code', 'Display', 'DateInput', 'Barcode', 'CategoryLocationName', 'DateScan', 'Active'];
    DisplayColumns005: string[] = ['No', 'ID', 'FileName', 'DateScan', 'DateInput', 'Barcode', 'CategoryLocationName', 'Display', 'CategoryUnitName', 'QuantityRequest', 'Quantity', 'Note', 'QuantitySNP', 'IsSNP', 'Active', 'Save'];
    DisplayColumns006: string[] = ['No', 'FileName', 'DateScan', 'Barcode', 'CategoryLocationName', 'Code', 'Display', 'CategoryUnitName', 'QuantityRequest', 'Quantity', 'QuantitySNP', 'IsSNP', 'Active'];
    DisplayColumns007: string[] = ['DateInput', 'DateScan', 'Display', 'QuantityRequest', 'Quantity', 'Note', 'QuantitySNP', 'IsSNP', 'Active', 'Save'];
    DisplayColumns008: string[] = ['No', 'ID', 'Display', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityRequest', 'Quantity', 'QuantityActual', 'QuantityGAP', 'Note', 'Active', 'Save'];
    DisplayColumns009: string[] = ['No', 'Code', 'Display', 'CategoryUnitName', 'QuantityRequest', 'Quantity', 'QuantitySNP', 'IsSNP', 'Active'];
    DisplayColumns010: string[] = ['Save', 'No', 'ID', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityRequest', 'Quantity', 'QuantityActual', 'QuantityGAP', 'Note', 'Active', ];
    DisplayColumns011: string[] = ['No', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityRequest', 'Quantity', 'QuantityActual', 'QuantityGAP'];
    DisplayColumns012: string[] = ['No', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityRequest', 'Quantity', 'QuantityActual', 'QuantityGAP', 'Active'];
    DisplayColumns013: string[] = ['Save', 'Active', 'No', 'ID', 'MaterialID', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityRequest', 'Quantity', 'QuantityActual', 'QuantityGAP', 'Note',];
    DisplayColumns014: string[] = ['Save', 'No', 'ID', 'CategoryFamilyName', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityRequest', 'Quantity', 'QuantityActual', 'QuantityGAP', 'Note', 'Active', ];

    List: WarehouseOutputDetail[] | undefined;
    ListFilter: WarehouseOutputDetail[] | undefined;
    FormData!: WarehouseOutputDetail;
    APIURL: string = environment.APIWarehouseOutputDetailURL;
    APIRootURL: string = environment.APIWarehouseOutputDetailRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseOutputDetail";
    }

    GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

