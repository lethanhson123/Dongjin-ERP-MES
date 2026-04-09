import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { WarehouseInputDetailBarcode } from './WarehouseInputDetailBarcode.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseInputDetailBarcodeService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'FileName', 'DateScan', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'Active', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'FileName', 'DateScan', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'Active'];
    DisplayColumns003: string[] = ['No', 'ID', 'FileName', 'DateScan', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'Active', 'Save'];
    DisplayColumns004: string[] = ['DateScan', 'Display', 'QuantitySNP', 'IsSNP', 'Quantity', 'Active'];
    DisplayColumns005: string[] = ['No', 'ID', 'DateScan', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'Active', 'Save'];
    DisplayColumns006: string[] = ['No', 'ID', 'ParentName', 'DateScan', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'Active'];
    DisplayColumns007: string[] = ['No', 'ID', 'Code', 'DateScan', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'Active', 'Save'];
    DisplayColumns008: string[] = ['DateScan', 'Display', 'QuantitySNP', 'IsSNP', 'Quantity', 'Active'];
    DisplayColumns009: string[] = ['Display', 'IsSNP', 'QuantitySNP', 'DateScan', 'Quantity', 'Active'];
    DisplayColumns010: string[] = ['Display', 'IsSNP', 'QuantitySNP', 'Quantity', 'DateScan', 'UpdateUserCode', 'Active'];
    DisplayColumns011: string[] = ['No', 'ID', 'Code', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'DateScan', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns012: string[] = ['No', 'ID', 'ParentName', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'DateScan', 'Active', 'Save'];
    DisplayColumns013: string[] = ['No', 'ID', 'ParentName', 'Code', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'DateScan', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns014: string[] = ['No', 'Barcode', 'CategoryLocationName'];
    DisplayColumns015: string[] = ['No', 'ID', 'Code', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'DateScan', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns016: string[] = ['No', 'ID', 'ParentName', 'Code', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns017: string[] = ['No', 'ID', 'Code', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns018: string[] = ['No', 'ID', 'ParentName', 'Code', 'Display', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active'];
    DisplayColumns019: string[] = ['Display', 'IsSNP', 'QuantitySNP', 'Quantity', 'DateScan', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns020: string[] = ['No', 'ID', 'ParentName', 'Code', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active'];
    DisplayColumns021: string[] = ['No', 'ID', 'ParentName', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'DateScan', 'Active', 'Save'];
    DisplayColumns022: string[] = ['No', 'ID', 'ParentName', 'Barcode', 'Quantity', 'DateScan', 'Active'];
    DisplayColumns023: string[] = ['No', 'ID', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns024: string[] = ['No', 'Barcode', 'CategoryLocationName', 'PageSize'];
    DisplayColumns025: string[] = ['No', 'Barcode', 'CategoryLocationName', 'Quantity'];
    DisplayColumns026: string[] = ['No', 'Barcode', 'CategoryLocationName', 'DateScan'];
    DisplayColumns027: string[] = ['No', 'ID', 'ParentName', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'DateScan', 'Active', 'UpdateDate', 'UpdateUserCode', 'Save'];
    DisplayColumns028: string[] = ['No', 'ID', 'ParentName', 'MaterialName', 'Barcode', 'CategoryLocationName', 'QuantityInvoice', 'QuantitySNP', 'Quantity', 'QuantityOutput', 'QuantityInventory', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns029: string[] = ['No', 'MaterialName', 'Barcode', 'MESID', 'PKG_GRP', 'BARCD_LOC', 'PKG_QTY', 'PKG_OUTQTY', 'PKG_QTYActual', 'CREATE_DTM', 'OUT_DTM', 'ID', 'CategoryLocationName', 'CategoryLocationName', 'QuantityInvoice', 'QuantitySNP', 'Quantity', 'QuantityOutput', 'QuantityInventory', 'DateScan', 'UpdateDate', 'Active', 'Save'];
    DisplayColumns030: string[] = ['No', 'Description', 'Barcode', 'CategoryLocationName', 'DateScan'];
    DisplayColumns031: string[] = ['No', 'ID', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'Description', 'IsExport', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns032: string[] = ['No', 'MaterialName', 'Barcode', 'MESID', 'PKG_GRP', 'BARCD_LOC', 'PKG_QTY', 'PKG_OUTQTY', 'QuantityMES', 'CREATE_DTM', 'OUT_DTM', 'ID', 'CategoryLocationName', 'QuantityInvoice', 'QuantitySNP', 'Quantity', 'QuantityOutput', 'QuantityInventory', 'PKG_QTYActual', 'DateScan', 'UpdateDate', 'Active', 'Save'];
    DisplayColumns033: string[] = ['No', 'ID', 'WarehouseInputDetailID', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'Description', 'IsExport', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns034: string[] = ['No', 'MaterialName', 'Display', 'Barcode', 'MESID', 'PKG_GRP', 'BARCD_LOC', 'PKG_QTY', 'PKG_OUTQTY', 'QuantityMES', 'CREATE_DTM', 'OUT_DTM', 'ID', 'CategoryLocationName', 'QuantityInvoice', 'QuantitySNP', 'Quantity', 'QuantityOutput', 'QuantityInventory', 'PKG_QTYActual', 'DateScan', 'UpdateDate', 'Active', 'Save'];
    DisplayColumns035: string[] = ['No', 'CategoryFamilyName', 'MaterialName', 'Display', 'QuantitySNP', 'CategoryUnitName', 'Barcode', 'MESID', 'PKG_GRP', 'BARCD_LOC', 'PKG_QTY', 'PKG_OUTQTY', 'QuantityMES', 'CREATE_DTM', 'OUT_DTM', 'ID', 'CategoryLocationName', 'QuantityInvoice', 'Quantity', 'QuantityOutputMES', 'QuantityInventory', 'PKG_QTYActual', 'DateScan', 'UpdateDate', 'Active'];
    DisplayColumns036: string[] = ['No', 'CategoryFamilyName', 'MaterialName', 'Display', 'QuantitySNP', 'CategoryUnitName', 'PKG_QTY', 'PKG_OUTQTY', 'QuantityMES', 'QuantityInvoice', 'Quantity', 'QuantityOutputMES', 'QuantityInventory', 'PKG_QTYActual'];
    DisplayColumns037: string[] = ['No', 'ID', 'WarehouseInputDetailID', 'MaterialID', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'Description', 'IsExport', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns038: string[] = ['No', 'ID', 'ParentName', 'Code', 'MaterialName', 'Barcode', 'CategoryLocationName', 'QuantityInvoice', 'Quantity', 'QuantityInventory', 'IsExport', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active'];
    DisplayColumns039: string[] = ['No', 'Barcode', 'CategoryLocationName', 'Quantity', 'UpdateDate', 'UpdateUserCode', 'Quantity02', 'UpdateDate02', 'UpdateUserCode02', 'Quantity03', 'UpdateDate03', 'UpdateUserCode03'];
    DisplayColumns040: string[] = ['No', 'Description', 'Barcode', 'CategoryLocationName', 'DateScan', 'Quantity', 'UpdateDate', 'UpdateUserCode', 'Quantity02', 'UpdateDate02', 'UpdateUserCode02', 'Quantity03', 'UpdateDate03', 'UpdateUserCode03'];
    DisplayColumns041: string[] = ['No', 'CategoryFamilyName', 'MaterialName', 'Display', 'QuantitySNP', 'CategoryUnitName', 'Barcode', 'MESID', 'PKG_GRP', 'BARCD_LOC', 'PKG_QTY', 'PKG_OUTQTY', 'QuantityMES', 'CREATE_DTM', 'OUT_DTM', 'ID', 'CategoryLocationName', 'DateScan', 'QuantityInvoice', 'Quantity', 'QuantityOutputMES', 'QuantityInventory', 'PKG_QTYActual', 'UpdateDate', 'UpdateUserCode', 'Quantity02', 'QuantityGAP02', 'UpdateDate02', 'UpdateUserCode02', 'Quantity03', 'QuantityGAP03', 'UpdateDate03', 'UpdateUserCode03', 'Active'];
    DisplayColumns042: string[] = ['No', 'CategoryFamilyName', 'MaterialName', 'Display', 'QuantitySNP', 'CategoryUnitName', 'PKG_QTY', 'PKG_OUTQTY', 'QuantityMES', 'QuantityInvoice', 'Quantity', 'QuantityOutputMES', 'QuantityInventory', 'PKG_QTYActual', 'Quantity02', 'QuantityGAP02', 'Quantity03', 'QuantityGAP03',];
    DisplayColumns043: string[] = ['No', 'Barcode', 'CategoryLocationName', 'DateScan', 'Quantity', 'UpdateDate', 'UpdateUserCode', 'Quantity02', 'UpdateDate02', 'UpdateUserCode02', 'Quantity03', 'UpdateDate03', 'UpdateUserCode03'];
    DisplayColumns044: string[] = ['No', 'ID', 'WarehouseInputDetailID', 'MaterialID', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationID', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'Description', 'IsExport', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns045: string[] = ['No', 'SortOrder', 'Note', 'Barcode', 'CategoryLocationName', 'DateScan', 'Quantity', 'UpdateDate', 'UpdateUserCode', 'Quantity02', 'UpdateDate02', 'UpdateUserCode02', 'Quantity03', 'UpdateDate03', 'UpdateUserCode03'];
    DisplayColumns046: string[] = ['Save', 'No', 'ID', 'MaterialName', 'Barcode', 'CategoryLocationName', 'QuantityInvoice', 'Quantity', 'QuantityInventory', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active'];
    DisplayColumns047: string[] = ['Save', 'No', 'ID', 'ParentName', 'MaterialName', 'Barcode', 'CategoryLocationName', 'QuantitySNP', 'QuantityInvoice', 'Quantity', 'QuantityOutput', 'QuantityInventory', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active',];
    DisplayColumns048: string[] = ['No', 'ID', 'MaterialName', 'Barcode', 'Description', 'ECN', 'Quantity', 'QuantitySNP', 'QuantityInvoice', 'QuantityOutput', 'QuantityInventory', 'CategoryLocationName', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'BOMID', 'IsExport', 'Active', 'Save'];
    DisplayColumns049: string[] = ['Save', 'No', 'ID', 'ParentName', 'MaterialName', 'Barcode', 'Description', 'CategoryLocationName', 'QuantitySNP', 'QuantityInvoice', 'Quantity', 'QuantityOutput', 'QuantityInventory', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active',];
    DisplayColumns050: string[] = ['No', 'ID', 'WarehouseInputDetailID', 'MaterialID', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'Description', 'ECN', 'BOMID', 'IsExport', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns051: string[] = ['No', 'ID', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'ECN', 'BOMID', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'DateScan', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns052: string[] = ['No', 'ID', 'WarehouseInputDetailID', 'MaterialID', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'Description', 'ECN', 'BOMECNVersion', 'BOMID', 'IsExport', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns053: string[] = ['No', 'ID', 'MaterialName', 'Barcode', 'Description', 'ECN', 'BOMECNVersion', 'Quantity', 'QuantitySNP', 'QuantityInvoice', 'QuantityOutput', 'QuantityInventory', 'CategoryLocationName', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'BOMID', 'IsExport', 'Active', 'Save'];
    DisplayColumns054: string[] = ['Save', 'No', 'ID', 'ParentID', 'ParentName', 'MaterialName', 'Barcode', 'Description', 'CategoryLocationName', 'QuantitySNP', 'QuantityInvoice', 'Quantity', 'QuantityOutput', 'QuantityInventory', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active',];
    DisplayColumns055: string[] = ['No', 'ID', 'MaterialName', 'Barcode', 'Description', 'ECN', 'BOMECNVersion', 'Quantity', 'QuantitySNP', 'QuantityInvoice', 'QuantityOutput', 'QuantityInventory', 'Price', 'Total', 'TotalInvoice', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'BOMID', 'IsExport', 'Active', 'Save'];
    DisplayColumns056: string[] = ['Save', 'No', 'ID', 'ParentID', 'ParentName', 'MaterialName', 'Barcode', 'Description', 'CategoryLocationName', 'QuantitySNP', 'QuantityInvoice', 'Quantity', 'QuantityOutput', 'QuantityInventory', 'Price', 'Total', 'TotalInvoice', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active',];
    DisplayColumns057: string[] = ['Save', 'No', 'ID', 'FileName', 'ParentID', 'ParentName', 'MaterialName', 'Barcode', 'Description', 'CategoryLocationName', 'QuantitySNP', 'QuantityInvoice', 'Quantity', 'QuantityOutput', 'QuantityInventory', 'Price', 'Total', 'TotalInvoice', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active',];
    DisplayColumns058: string[] = ['No', 'ID', 'FileName', 'MaterialName', 'Barcode', 'Description', 'ECN', 'BOMECNVersion', 'Quantity', 'QuantitySNP', 'QuantityInvoice', 'QuantityOutput', 'QuantityInventory', 'Price', 'Total', 'TotalInvoice', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'BOMID', 'IsExport', 'Active', 'Save'];
    DisplayColumns059: string[] = ['No', 'ID', 'FileName', 'WarehouseInputDetailID', 'MaterialID', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'Description', 'ECN', 'BOMECNVersion', 'BOMID', 'IsExport', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'Active', 'Save'];
    DisplayColumns060: string[] = ['No', 'ID', 'CategoryLocationName', 'MaterialName', 'Barcode', 'Description', 'ECN', 'BOMECNVersion', 'Quantity', 'QuantitySNP', 'QuantityInvoice', 'QuantityOutput', 'QuantityInventory', 'Price', 'Total', 'TotalInvoice', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'BOMID', 'IsExport', 'Active', 'Save'];
    DisplayColumns061: string[] = ['Save', 'Active', 'No', 'ID', 'CategoryLocationName', 'MaterialName', 'Barcode', 'Description', 'Quantity', 'QuantitySNP', 'QuantityInvoice', 'QuantityOutput', 'QuantityInventory', 'Price', 'Total', 'TotalInvoice', 'DateScan', 'UpdateDate', 'UpdateUserCode', 'IsExport', 'MaterialName01', 'ECN', 'BOMECNVersion', 'MaterialID01', 'BOMID'];
    DisplayColumns062: string[] = ['No', 'ID', 'MaterialName', 'Barcode', 'Quantity', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'IsExport', 'DateScan', 'UpdateUserCode', 'Active', 'MaterialName01', 'ECN', 'BOMECNVersion', 'MaterialID01', 'BOMID', 'Save'];
    DisplayColumns063: string[] = ['No', 'ID', 'ParentID', 'Barcode', 'Quantity', 'DateScan', 'CategoryLocationName', 'UpdateUserCode', 'Active'];
    DisplayColumns064: string[] = ['Save', 'Active', 'No', 'ID', 'Barcode', 'Quantity', 'DateScan', 'FileName', 'WarehouseInputDetailID', 'MaterialID', 'MaterialName', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'Description', 'ECN', 'BOMECNVersion', 'BOMID', 'IsExport', 'UpdateDate', 'UpdateUserCode',];
    DisplayColumns065: string[] = ['Save', 'Active', 'No', 'ID', 'FileName', 'ParentID', 'ParentName', 'MaterialName', 'Barcode', 'Description', 'CategoryLocationName', 'QuantitySNP', 'QuantityInvoice', 'Quantity', 'QuantityOutput', 'QuantityInventory', 'Price', 'Total', 'TotalInvoice', 'DateScan', 'UpdateDate', 'UpdateUserCode',];
    DisplayColumns066: string[] = ['Save', 'Active', 'No', 'ID', 'ParentID', 'ParentName', 'FileName', 'MaterialName', 'Barcode', 'QuantityInventory', 'Quantity', 'QuantityOutput', 'DateScan', 'Description', 'CategoryLocationName', 'QuantitySNP', 'QuantityInvoice', 'Price', 'Total', 'TotalInvoice', 'UpdateDate', 'UpdateUserCode',];
    DisplayColumns067: string[] = ['Save', 'Active', 'No', 'ID', 'Barcode', 'Quantity', 'DateScan', 'FileName', 'WarehouseInputDetailID', 'MaterialID', 'MaterialName', 'CategoryLocationName', 'QuantitySNP', 'QuantityMES', 'QuantityOutput', 'QuantityInventory', 'Description', 'ECN', 'BOMECNVersion', 'BOMID', 'IsExport', 'UpdateDate', 'UpdateUserCode',];
    DisplayColumns068: string[] = ['Save', 'Active', 'No', 'ID', 'FileName', 'MaterialName', 'Barcode', 'QuantitySNP', 'QuantityMES', 'Quantity', 'QuantityOutput', 'QuantityInventory',];
    DisplayColumns069: string[] = ['Save', 'Active', 'No', 'ID', 'Barcode', 'Quantity', 'DateScan', 'FileName', 'WarehouseInputDetailID', 'MaterialID', 'MaterialName', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'Description', 'Price', 'TotalInvoice', 'Total', 'ECN', 'BOMECNVersion', 'BOMID', 'IsExport', 'UpdateDate', 'UpdateUserCode',];
    DisplayColumns070: string[] = ['Save', 'Active', 'No', 'ID', 'MaterialID', 'FileName', 'CategoryFamilyName', 'MaterialName', 'Barcode', 'QuantitySNP', 'QuantityMES', 'Quantity', 'QuantityOutput', 'QuantityInventory',];
    DisplayColumns071: string[] = ['Save', 'Active', 'No', 'ID', 'Barcode', 'Quantity', 'DateScan', 'FileName', 'WarehouseInputDetailID', 'MaterialID', 'MaterialName', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'Description', 'CategoryFamilyName', 'Price', 'TotalInvoice', 'Total', 'ECN', 'BOMECNVersion', 'BOMID', 'IsExport', 'UpdateDate', 'UpdateUserCode',];
    DisplayColumns072: string[] = ['Save', 'Active', 'No', 'ID', 'Barcode', 'Quantity', 'DateScan', 'FileName', 'WarehouseInputDetailID', 'MaterialID', 'MaterialName', 'CategoryLocationName', 'QuantityInvoice', 'QuantityInventory', 'Description', 'CategoryFamilyName', 'Price', 'TotalInvoice', 'Total', 'ECN', 'BOMECNVersion', 'BOMID', 'IsExport', 'CreateDate', 'UpdateDate', 'UpdateUserCode',];
    DisplayColumns073: string[] = ['Save', 'Active', 'No', 'ID', 'Barcode', 'Quantity', 'DateScan', 'FileName', 'WarehouseInputDetailID', 'MaterialID', 'MaterialName', 'CategoryLocationName', 'QuantitySNP', 'QuantityMES', 'QuantityOutput', 'QuantityInventory', 'Description', 'ECN', 'BOMECNVersion', 'BOMID', 'IsExport', 'CreateDate', 'UpdateDate', 'UpdateUserCode',];


    List: WarehouseInputDetailBarcode[] | undefined;
    ListFilter: WarehouseInputDetailBarcode[] | undefined;
    ListParent: WarehouseInputDetailBarcode[] | undefined;
    FormData!: WarehouseInputDetailBarcode;
    APIURL: string = environment.APIWarehouseInputDetailBarcodeURL;
    APIRootURL: string = environment.APIWarehouseInputDetailBarcodeRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseInputDetailBarcode";
        let CompanyID = Number(localStorage.getItem(environment.CompanyID));        
        switch (CompanyID) {
            case 17:
                this.APIURL = environment.APIWarehouseInputDetailBarcode02URL;                
                this.APIRootURL = environment.APIWarehouseInputDetailBarcode02RootURL;
                break;
        }        
    }
    GetByWarehouseInputDetailIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByWarehouseInputDetailIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByParentID_MaterialIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByParentID_MaterialIDToListAsync';
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
    GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_FIFO_StockToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_FIFO_StockToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByBarcodeToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByBarcodeToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentID_BarcodeAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentID_BarcodeAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByBarcodeFromtmbrcdAsync() {
        let url = this.APIURL + this.Controller + '/GetByBarcodeFromtmbrcdAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByBarcodeFromtdpdmtimAsync() {
        let url = this.APIURL + this.Controller + '/GetByBarcodeFromtdpdmtimAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByBarcodeFromtrackmtimAsync() {
        let url = this.APIURL + this.Controller + '/GetByBarcodeFromtrackmtimAsync';
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
    GetByMaterialID_CategoryLocationIDFromDiagramToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMaterialID_CategoryLocationIDFromDiagramToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentID_MaterialID_CategoryLocationIDFromDiagramToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentID_MaterialID_CategoryLocationIDFromDiagramToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentID_MaterialID_CategoryLocationNameFromDiagramToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentID_MaterialID_CategoryLocationNameFromDiagramToListAsync';
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
    GetCompareMESAndERPToListAsync() {
        let url = this.APIURL + this.Controller + '/GetCompareMESAndERPToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetPARTNOCompareMESAndERPToListAsync() {
        let url = this.APIURL + this.Controller + '/GetPARTNOCompareMESAndERPToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    PrintAsync() {
        let url = this.APIURL + this.Controller + '/PrintAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    PrintByListAsync() {
        let url = this.APIURL + this.Controller + '/PrintByListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    PrintBarcode_WarehouseOutputIDAsync() {
        let url = this.APIURL + this.Controller + '/PrintBarcode_WarehouseOutputIDAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    PrintByBarcodeAsync() {
        let url = this.APIURL + this.Controller + '/PrintByBarcodeAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    PrintByParentIDAsync() {
        let url = this.APIURL + this.Controller + '/PrintByParentIDAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    PrintByListWarehouseInputDetailIDAsync() {
        let url = this.APIURL + this.Controller + '/PrintByListWarehouseInputDetailIDAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    PrintByWarehouseInputDetailIDAsync() {
        let url = this.APIURL + this.Controller + '/PrintByWarehouseInputDetailIDAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    PrintByListWarehouseInputDetailID2025Async() {
        let url = this.APIURL + this.Controller + '/PrintByListWarehouseInputDetailID2025Async';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    CreateAutoAsync() {
        let url = this.APIURL + this.Controller + '/CreateAutoAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncByParrentIDAsync() {
        let url = this.APIURL + this.Controller + '/SyncByParrentIDAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncStockAsync() {
        let url = this.APIURL + this.Controller + '/SyncStockAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByHOOKRACK_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByHOOKRACK_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByKOMAX_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByKOMAX_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetBySHIELDWIRE_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetBySHIELDWIRE_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByLP_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByLP_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetBySPST_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetBySPST_SearchStringToListAsync';
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
    AutoSyncAsync() {
        let url = this.APIURL + this.Controller + '/AutoSyncAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

