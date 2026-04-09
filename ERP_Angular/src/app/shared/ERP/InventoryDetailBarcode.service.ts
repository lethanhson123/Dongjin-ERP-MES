import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { InventoryDetailBarcode } from './InventoryDetailBarcode.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class InventoryDetailBarcodeService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'FileName', 'MaterialName', 'Barcode', 'CategoryLocationName', 'Quantity', 'Quantity01', 'QuantityGAP01', 'Date01', 'UpdateUserCode01', 'Quantity02', 'QuantityGAP02', 'Date02', 'UpdateUserCode02', 'Quantity03', 'QuantityGAP03', 'Date03', 'UpdateUserCode03', 'Active'];
    DisplayColumns002: string[] = ['No', 'ProductID', 'ParentName', 'Barcode', 'CategoryLocationName', 'Quantity', 'Quantity01', 'Quantity02', 'Quantity03'];
    DisplayColumns003: string[] = ['No', 'ProductID', 'ParentName', 'Barcode', 'Note', 'CategoryLocationName', 'Quantity', 'Quantity01', 'Quantity02', 'Quantity03'];
    DisplayColumns004: string[] = ['No', 'ProductID', 'ParentName', 'Barcode', 'Description', 'CategoryLocationName', 'Quantity', 'Note', 'Quantity01', 'Quantity02', 'Quantity03'];
    DisplayColumns005: string[] = ['No', 'ID', 'FileName', 'MaterialName', 'Barcode', 'CategoryLocationName', 'Description', 'Note', 'Quantity', 'Quantity01', 'QuantityGAP01', 'Date01', 'UpdateUserCode01', 'Quantity02', 'QuantityGAP02', 'Date02', 'UpdateUserCode02', 'Quantity03', 'QuantityGAP03', 'Date03', 'UpdateUserCode03', 'Active'];
    DisplayColumns006: string[] = ['No', 'Barcode', 'Description', 'CategoryLocationName', 'Quantity', 'Quantity01', 'Quantity02', 'Note',];
    DisplayColumns007: string[] = ['No', 'Week', 'Barcode', 'Description', 'CategoryLocationName', 'Quantity', 'Note', 'Quantity01', 'Quantity02'];
    DisplayColumns008: string[] = ['No', 'ID', 'Week', 'FileName', 'MaterialName', 'Barcode', 'CategoryLocationName', 'Description', 'Note', 'Quantity', 'Quantity01', 'QuantityGAP01', 'Date01', 'UpdateUserCode01', 'Quantity02', 'QuantityGAP02', 'Date02', 'UpdateUserCode02', 'Quantity03', 'QuantityGAP03', 'Date03', 'UpdateUserCode03', 'Active'];
    DisplayColumns009: string[] = ['Save', 'Active', 'No', 'ID', 'Week', 'FileName', 'MaterialName', 'Barcode', 'CategoryLocationName', 'Description', 'Note', 'Quantity', 'Quantity01', 'QuantityGAP01', 'Date01', 'UpdateUserCode01', 'Quantity02', 'QuantityGAP02', 'Date02', 'UpdateUserCode02', 'Quantity03', 'QuantityGAP03', 'Date03', 'UpdateUserCode03', 'UpdateDate', 'UpdateUserCode', ];
    DisplayColumns010: string[] = ['Save', 'No', 'ID', 'Week', 'MaterialName', 'Barcode', 'CategoryLocationName', 'Quantity', 'Quantity01', 'QuantityGAP01', 'Active'];
    DisplayColumns011: string[] = ['No', 'ID', 'Barcode', 'Description', 'CategoryLocationName', 'Quantity', 'Quantity01', 'Quantity02', 'Note',];
    DisplayColumns012: string[] = ['No', 'ID', 'Barcode', 'Description', 'CategoryLocationName', 'Quantity', 'Quantity01', 'Note',];


    List: InventoryDetailBarcode[] | undefined;
    ListFilter: InventoryDetailBarcode[] | undefined;
    FormData!: InventoryDetailBarcode;
    APIURL: string = environment.APIReportURL;
    APIRootURL: string = environment.APIReportRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "InventoryDetailBarcode";
    }
    GetByCategoryDepartmentIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    ExportToExcelAsync() {
        let url = this.APIURL + this.Controller + '/ExportToExcelAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    ExportWithCategoryLocationNameToExcelAsync() {
        let url = this.APIURL + this.Controller + '/ExportWithCategoryLocationNameToExcelAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    ExportWithNotExistToExcelAsync() {
        let url = this.APIURL + this.Controller + '/ExportWithNotExistToExcelAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    ExportWithQuantityToExcelAsync() {
        let url = this.APIURL + this.Controller + '/ExportWithQuantityToExcelAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

