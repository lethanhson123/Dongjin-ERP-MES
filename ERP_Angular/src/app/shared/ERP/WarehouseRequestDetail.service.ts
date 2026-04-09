import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { WarehouseRequestDetail } from './WarehouseRequestDetail.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseRequestDetailService extends BaseService {
    DisplayColumns001: string[] = ['No', 'Display', 'CategoryUnitName', 'QuantityInvoice', 'Quantity', 'Save'];
    DisplayColumns002: string[] = ['No', 'Display', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityInvoice', 'Quantity', 'Active', 'Save'];
    DisplayColumns003: string[] = ['No', 'Display', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityWIP','QuantityStock','QuantityInvoice00','QuantityInventory','QuantityInventory00','QuantityInvoice', 'Quantity', 'Active', 'Save'];
    DisplayColumns004: string[] = ['No', 'Display', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityInventory00', 'QuantityInvoice', 'Quantity', 'Active', 'Save'];
    DisplayColumns005: string[] = ['No', 'Display', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityStock', 'QuantityBegin', 'QuantityEnd', 'QuantityInvoice', 'Quantity', 'Active', 'Save'];
    DisplayColumns006: string[] = ['No', 'Display', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityBegin', 'QuantityEnd', 'QuantityInvoice', 'Quantity', 'Active', 'Save'];
    DisplayColumns007: string[] = ['No', 'ID', 'Display', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityStock', 'QuantityBegin', 'QuantityEnd', 'QuantityInvoice', 'Quantity', 'Active', 'Save'];
    DisplayColumns008: string[] = ['No', 'ID', 'Display', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityStock', 'QuantityBegin', 'QuantityEnd', 'QuantityInvoice', 'QuantityGAP', 'Quantity', 'Active', 'Save'];
    DisplayColumns009: string[] = ['No', 'ID', 'MaterialName', 'CategoryUnitName', 'Quantity', 'Save'];
    DisplayColumns010: string[] = ['No', 'ID', 'FileName', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityStock', 'QuantityBegin', 'QuantityEnd', 'QuantityInvoice', 'QuantityGAP', 'Quantity', 'Active', 'Save'];
    DisplayColumns011: string[] = ['No', 'ID', 'CategoryFamilyName', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityStock', 'QuantityBegin', 'QuantityEnd', 'QuantityInvoice', 'QuantityGAP', 'Quantity', 'Active', 'Save'];

    List: WarehouseRequestDetail[] | undefined;
    ListFilter: WarehouseRequestDetail[] | undefined;
    FormData!: WarehouseRequestDetail;
    APIURL: string = environment.APIWarehouseURL;
    APIRootURL: string = environment.APIWarehouseRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseRequestDetail";
    }
}

