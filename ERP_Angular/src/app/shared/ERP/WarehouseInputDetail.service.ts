import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { WarehouseInputDetail } from './WarehouseInputDetail.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseInputDetailService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'FileName', 'DateScan', 'Barcode', 'CategoryLocationName', 'Display', 'CategoryUnitName', 'QuantityInvoice', 'Price', 'TotalInvoice', 'Quantity', 'Total', 'Active', 'Save'];
    DisplayColumns002: string[] = ['No', 'FileName', 'DateScan', 'Barcode', 'CategoryLocationName', 'Code', 'Display', 'CategoryUnitName', 'Quantity', 'QuantityInventory', 'IsExport', 'Active'];
    DisplayColumns003: string[] = ['DateScan', 'Display', 'QuantityRequest', 'Quantity', 'Active'];
    DisplayColumns004: string[] = ['No', 'FileName', 'DateScan', 'Barcode', 'CategoryLocationName', 'Code', 'Display', 'CategoryUnitName', 'Quantity', 'QuantityInventory', 'QuantitySNP', 'IsSNP', 'IsExport', 'Active'];
    DisplayColumns005: string[] = ['No', 'ID', 'FileName', 'DateScan', 'Barcode', 'CategoryLocationName', 'Display', 'CategoryUnitName', 'QuantityInvoice', 'Price', 'TotalInvoice', 'Quantity', 'Total', 'QuantitySNP', 'IsSNP', 'Active', 'Save'];
    DisplayColumns006: string[] = ['DateScan', 'Display', 'QuantityRequest', 'Quantity', 'QuantitySNP', 'IsSNP', 'Active'];
    DisplayColumns007: string[] = ['No', 'ID', 'Display', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityInvoice', 'Price', 'TotalInvoice', 'Quantity', 'Total', 'Active', 'Save'];
    DisplayColumns008: string[] = ['No', 'ID', 'Display', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityInvoice', 'Price', 'TotalInvoice', 'Quantity', 'QuantityActual', 'QuantityGAP', 'Total', 'Active', 'Save'];
    DisplayColumns009: string[] = ['No', 'ID', 'Display', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityInvoice', 'Price', 'TotalInvoice', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityInventory', 'IsExport', 'Active', 'Save'];
    DisplayColumns010: string[] = ['No', 'Code', 'Display', 'CategoryUnitName', 'Quantity', 'QuantityInventory', 'QuantitySNP', 'IsSNP', 'IsExport', 'Active'];
    DisplayColumns011: string[] = ['No', 'ID', 'Display', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityInvoice', 'Price', 'TotalInvoice', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityOutput', 'QuantityInventory', 'IsExport', 'Active', 'Save'];
    DisplayColumns012: string[] = ['No', 'ID', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityInvoice', 'Price', 'TotalInvoice', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityOutput', 'QuantityInventory', 'IsExport', 'Active', 'Save'];
    DisplayColumns013: string[] = ['No', 'ID', 'MaterialName', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'QuantityInvoice', 'Price', 'TotalInvoice', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityOutput', 'QuantityInventory', 'IsExport', 'Active', 'UpdateDate', 'Save'];
    DisplayColumns014: string[] = ['No', 'ID', 'MaterialName', 'Name', 'Display', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityOutput', 'QuantityInventory', 'IsExport', 'Active', 'Save'];
    DisplayColumns015: string[] = ['No', 'ID', 'MaterialName', 'Name', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'Quantity', 'QuantityActual', 'QuantityGAP', 'Active'];
    DisplayColumns016: string[] = ['No', 'ID', 'MaterialName', 'Name', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'Quantity', 'QuantityActual', 'QuantityGAP', 'Active', 'Save'];
    DisplayColumns017: string[] = ['No', 'ID', 'Description', 'MaterialName', 'Name', 'Display', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityOutput', 'QuantityInventory', 'IsExport', 'Active', 'Save'];
    DisplayColumns018: string[] = ['No', 'ID', 'MaterialName', 'Name', 'Display', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityOutput', 'QuantityInventory', 'IsExport', 'Active', 'Save'];
    DisplayColumns019: string[] = ['No', 'ID', 'MaterialName', 'Name', 'Display', 'Description', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityOutput', 'QuantityInventory', 'IsExport', 'Active', 'Save'];
    DisplayColumns020: string[] = ['No', 'ID', 'MaterialID', 'MaterialName', 'Name', 'Display', 'Description', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityOutput', 'QuantityInventory', 'IsExport', 'Active', 'Save'];
    DisplayColumns021: string[] = ['Save', 'Active', 'No', 'ID', 'MaterialName', 'Name', 'Display', 'Description', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityInvoice', 'QuantityInvoiceGAP', 'QuantityOutput', 'QuantityInventory', 'IsExport',];
    DisplayColumns022: string[] = ['Save', 'Active', 'No', 'ID', 'MaterialName', 'Name', 'Display', 'Description', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityInvoice', 'QuantityInvoiceGAP', 'QuantityOutput', 'QuantityInventory', 'IsExport', 'UpdateDate', 'UpdateUserCode',];
    DisplayColumns023: string[] = ['Save', 'Active', 'No', 'ID', 'MaterialID', 'MaterialName', 'Name', 'Display', 'Description', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityOutput', 'QuantityInventory', 'QuantityStock', 'QuantityStockInput', 'QuantityStockOuput', 'QuantityStockGAP', 'DateBegin', 'DateEnd'];
    DisplayColumns024: string[] = ['Save', 'Active', 'No', 'ID', 'MaterialID', 'MaterialName', 'QuantitySNP', 'Quantity', 'QuantityInventory', 'QuantityStock', 'QuantityStockInput', 'QuantityStockOuput', 'QuantityStockGAP', 'DateBegin', 'DateEnd'];
    DisplayColumns025: string[] = ['Save', 'Active', 'No', 'ID', 'MaterialName', 'Name', 'Display', 'Description', 'Price', 'TotalInvoice', 'Total', 'CategoryUnitName', 'IsSNP', 'QuantitySNP', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityInvoice', 'QuantityInvoiceGAP', 'QuantityOutput', 'QuantityInventory', 'IsExport', 'UpdateDate', 'UpdateUserCode',];
    DisplayColumns026: string[] = ['Save', 'Active', 'No', 'ID', 'MaterialID', 'FileName', 'CategoryFamilyName', 'MaterialName', 'QuantitySNP', 'Quantity', 'QuantityInventory', 'QuantityStock', 'QuantityStockInput', 'QuantityStockOuput', 'QuantityStockGAP', 'DateBegin', 'DateEnd'];
    DisplayColumns027: string[] = ['Save', 'Active', 'No', 'MaterialName', 'Name', 'Display', 'Description', 'IsSNP', 'QuantitySNP', 'Quantity', 'QuantityActual', 'QuantityGAP', 'QuantityInvoice', 'QuantityInvoiceGAP', 'QuantityOutput', 'QuantityInventory', 'CategoryFamilyName', 'Price', 'TotalInvoice', 'Total', 'CategoryUnitName', 'IsExport', 'UpdateDate', 'UpdateUserCode', 'ID',];
    DisplayColumns028: string[] = ['Save', 'Active', 'No', 'ID', 'MaterialID', 'FileName', 'CategoryFamilyName', 'MaterialName', 'QuantitySNP', 'Quantity', 'QuantityInventory', 'QuantityStock', 'QuantityStockInput', 'QuantityStockOuput', 'QuantityStockGAP', 'DateBegin', 'DateEnd', 'CreateDate', 'CreateUserCode', 'UpdateDate', 'UpdateUserCode',];
    DisplayColumns029: string[] = ['No', 'ID', 'ParentID', 'ParentName', 'MaterialName', 'Name', 'QuantitySNP', 'Quantity', 'QuantityActual', 'QuantityGAP', 'Active',];

    List: WarehouseInputDetail[] | undefined;
    ListFilter: WarehouseInputDetail[] | undefined;
    FormData!: WarehouseInputDetail;
    APIURL: string = environment.APIWarehouseInputDetailURL;
    APIRootURL: string = environment.APIWarehouseInputDetailRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseInputDetail";
    }

    GetByYear_Month_Day_SearchString_InventoryToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByYear_Month_Day_SearchString_InventoryToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByParentIDAndEmpty_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByParentIDAndEmpty_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
     GetByQuantityGAPToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByQuantityGAPToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveListAndSyncWarehouseInputDetailBarcodeAsync() {
        let url = this.APIURL + this.Controller + '/SaveListAndSyncWarehouseInputDetailBarcodeAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

