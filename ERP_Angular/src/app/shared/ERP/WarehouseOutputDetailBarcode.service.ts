import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { WarehouseOutputDetailBarcode } from './WarehouseOutputDetailBarcode.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseOutputDetailBarcodeService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'FileName', 'DateScan', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'Active', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'FileName', 'DateScan', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'Active'];
    DisplayColumns003: string[] = ['DateScan', 'Display', 'Quantity', 'Note', 'QuantitySNP', 'IsSNP', 'Active', 'Save'];
    DisplayColumns004: string[] = ['No', 'ID', 'ParentName', 'DateScan', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'Active'];
    DisplayColumns005: string[] = ['No', 'ID', 'DateScan', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'Active', 'Save'];
    DisplayColumns006: string[] = ['Display', 'DateScan', 'Quantity', 'Active', 'Note', 'Save'];
    DisplayColumns007: string[] = ['No', 'ID', 'DateScan', 'Display', 'Barcode', 'Week', 'Quantity', 'CategoryLocationName', 'Active'];
    DisplayColumns008: string[] = ['No', 'ID', 'ParentName', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'DateScan', 'UpdateUserCode', 'Active'];
    DisplayColumns009: string[] = ['No', 'ID', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'DateScan', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns010: string[] = ['No', 'ID', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'DateScan', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns011: string[] = ['No', 'ID', 'DateScan', 'Display', 'Barcode', 'DateInput', 'Week', 'Quantity', 'CategoryLocationName', 'Active'];
    DisplayColumns012: string[] = ['No', 'ID', 'DateScan', 'Display', 'Barcode', 'DateInput', 'Week', 'QuantityRequest', 'Quantity', 'CategoryLocationName', 'Active'];
    DisplayColumns013: string[] = ['No', 'ID', 'ParentName', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'DateScan', 'Active'];
    DisplayColumns014: string[] = ['No', 'ID', 'ParentName', 'Barcode', 'Quantity', 'DateScan', 'Active'];
    DisplayColumns015: string[] = ['No', 'ID', 'MaterialName', 'Barcode', 'Quantity', 'QuantityInput', 'CategoryLocationName', 'DateScan', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns016: string[] = ['No', 'ID', 'MaterialName', 'Barcode', 'Quantity', 'QuantityInput', 'DateScan', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns017: string[] = ['Display', 'DateScan', 'Quantity', 'QuantityInput', 'Active', 'Save'];
    DisplayColumns018: string[] = ['No', 'ID', 'ParentName', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'DateScan', 'UpdateUserCode', 'Active'];
    DisplayColumns019: string[] = ['No', 'ID', 'ParentName', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active'];
    DisplayColumns020: string[] = ['No', 'ID', 'ParentName', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active'];
    DisplayColumns021: string[] = ['Save', 'Display', 'DateScan', 'Quantity', 'Active', 'Note'];
    DisplayColumns022: string[] = ['Save', 'Display', 'DateScan', 'Quantity', 'Active', 'Note'];
    DisplayColumns023: string[] = ['No', 'ID', 'ParentID', 'ParentName', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active'];
    DisplayColumns024: string[] = ['No', 'ID', 'ParentID', 'ParentName', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'Price', 'Total', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active'];
    DisplayColumns025: string[] = ['No', 'ID', 'MaterialName', 'Barcode', 'Quantity', 'QuantityInput', 'Price', 'Total', 'DateScan', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns026: string[] = ['No', 'ID', 'ParentID', 'ParentName', 'FileName', 'MaterialName', 'Barcode', 'Quantity', 'DateScan', 'CategoryLocationName', 'Price', 'Total', 'UpdateDate', 'UpdateUserCode', 'Active'];
    DisplayColumns027: string[] = ['Save', 'Active', 'No', 'ID', 'DateScan', 'MaterialID', 'MaterialName', 'Barcode', 'Quantity', 'QuantityReturn', 'Price', 'Total', 'UpdateUserCode',];
    DisplayColumns028: string[] = ['Save', 'Display', 'DateScan', 'QuantityInput', 'Quantity', 'Active', 'Note'];
    DisplayColumns029: string[] = ['No', 'ID', 'MaterialName', 'Barcode', 'CategoryLocationName', 'Quantity', 'QuantityInput', 'QuantityInvoice', 'Price', 'Total', 'DateScan', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns030: string[] = ['Save', 'Active', 'No', 'DateScan', 'ID', 'MaterialName', 'Barcode', 'CategoryLocationName', 'Quantity', 'QuantityReturn', 'QuantityInvoice', 'Price', 'Total', 'UpdateUserCode',];
    DisplayColumns031: string[] = ['Save', 'Active', 'No', 'DateScan', 'ID', 'CategoryFamilyName', 'MaterialName', 'Barcode', 'CategoryLocationName', 'Quantity', 'QuantityReturn', 'QuantityInvoice', 'Price', 'Total', 'UpdateUserCode',];
    DisplayColumns032: string[] = ['Save', 'Display', 'Quantity', 'DateScan',];
    DisplayColumns033: string[] = ['No', 'ID', 'Active', 'IsFIFO', 'Note', 'ParentID', 'ParentName', 'MaterialName', 'DateScan', 'Quantity', 'Barcode',];
    DisplayColumns034: string[] = ['No', 'ID', 'WarehouseInputName', 'ParentName', 'FileName', 'MaterialName', 'Barcode', 'Quantity', 'DateScan', 'CategoryLocationName', 'Price', 'Total', 'UpdateDate', 'UpdateUserCode', 'Active'];
    DisplayColumns035: string[] = ['No', 'ID', 'WarehouseInputID', 'WarehouseInputName', 'ParentID', 'ParentName', 'FileName', 'MaterialName', 'Barcode', 'Quantity', 'DateScan', 'CategoryLocationName', 'Price', 'Total', 'UpdateDate', 'UpdateUserCode', 'Active'];
    DisplayColumns036: string[] = ['Save', 'Active', 'No', 'WarehouseOutputDetailID', 'ID', 'DateScan', 'MaterialID', 'MaterialName', 'Barcode', 'Quantity', 'QuantityReturn', 'Price', 'Total', 'UpdateUserCode',];
    DisplayColumns037: string[] = ['Save', 'Active', 'No', 'ID', 'Barcode', 'Quantity', 'QuantityReturn', 'DateScan', 'FileName', 'WarehouseOutputDetailID', 'MaterialID', 'MaterialName', 'ECN', 'BOMECNVersion', 'BOMID', 'UpdateUserCode',];


    List: WarehouseOutputDetailBarcode[] | undefined;
    ListFilter: WarehouseOutputDetailBarcode[] | undefined;
    FormData!: WarehouseOutputDetailBarcode;
    APIURL: string = environment.APIWarehouseOutputDetailBarcodeURL;
    APIRootURL: string = environment.APIWarehouseOutputDetailBarcodeRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseOutputDetailBarcode";

        let CompanyID = Number(localStorage.getItem(environment.CompanyID));
        switch (CompanyID) {
            case 17:
                this.APIURL = environment.APIWarehouseOutputDetailBarcode02URL;
                this.APIRootURL = environment.APIWarehouseOutputDetailBarcode02RootURL;
                break;
        }
    }
    GetByWarehouseOutputDetailIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByWarehouseOutputDetailIDToListAsync';
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
    GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_ActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByBarcode_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByBarcode_ActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentID_Active_FIFOToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentID_Active_FIFOToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    AutoSyncAsync() {
        let url = this.APIURL + this.Controller + '/AutoSyncAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveList2026Async() {
        let url = this.APIURL + this.Controller + '/SaveList2026Async';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

