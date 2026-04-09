import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { WarehouseInventory } from './WarehouseInventory.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseInventoryService extends BaseService {
    DisplayColumns000: string[] = ['No', 'ID', 'FileName', 'Code', 'ParentName', 'Year', 'Month', 'QuantityBegin', 'QuantityInput00', 'QuantityOutput00', 'Quantity00', 'QuantityEnd', 'QuantityBegin01', 'QuantityInput01', 'QuantityOutput01', 'Quantity01', 'QuantityEnd01', 'QuantityBegin02', 'QuantityInput02', 'QuantityOutput02', 'Quantity02', 'QuantityEnd02', 'QuantityBegin03', 'QuantityInput03', 'QuantityOutput03', 'Quantity03', 'QuantityEnd03', 'QuantityBegin04', 'QuantityInput04', 'QuantityOutput04', 'Quantity04', 'QuantityEnd04', 'QuantityBegin05', 'QuantityInput05', 'QuantityOutput05', 'Quantity05', 'QuantityEnd05', 'QuantityBegin06', 'QuantityInput06', 'QuantityOutput06', 'Quantity06', 'QuantityEnd06', 'QuantityBegin07', 'QuantityInput07', 'QuantityOutput07', 'Quantity07', 'QuantityEnd07', 'QuantityBegin08', 'QuantityInput08', 'QuantityOutput08', 'Quantity08', 'QuantityEnd08', 'QuantityBegin09', 'QuantityInput09', 'QuantityOutput09', 'Quantity09', 'QuantityEnd09', 'QuantityBegin10', 'QuantityInput10', 'QuantityOutput10', 'Quantity10', 'QuantityEnd10', 'QuantityBegin11', 'QuantityInput11', 'QuantityOutput11', 'Quantity11', 'QuantityEnd11', 'QuantityBegin12', 'QuantityInput12', 'QuantityOutput12', 'Quantity12', 'QuantityEnd12', 'QuantityBegin13', 'QuantityInput13', 'QuantityOutput13', 'Quantity13', 'QuantityEnd13', 'QuantityBegin14', 'QuantityInput14', 'QuantityOutput14', 'Quantity14', 'QuantityEnd14', 'QuantityBegin15', 'QuantityInput15', 'QuantityOutput15', 'Quantity15', 'QuantityEnd15', 'QuantityBegin16', 'QuantityInput16', 'QuantityOutput16', 'Quantity16', 'QuantityEnd16', 'QuantityBegin17', 'QuantityInput17', 'QuantityOutput17', 'Quantity17', 'QuantityEnd17', 'QuantityBegin18', 'QuantityInput18', 'QuantityOutput18', 'Quantity18', 'QuantityEnd18', 'QuantityBegin19', 'QuantityInput19', 'QuantityOutput19', 'Quantity19', 'QuantityEnd19', 'QuantityBegin20', 'QuantityInput20', 'QuantityOutput20', 'Quantity20', 'QuantityEnd20', 'QuantityBegin21', 'QuantityInput21', 'QuantityOutput21', 'Quantity21', 'QuantityEnd21', 'QuantityBegin22', 'QuantityInput22', 'QuantityOutput22', 'Quantity22', 'QuantityEnd22', 'QuantityBegin23', 'QuantityInput23', 'QuantityOutput23', 'Quantity23', 'QuantityEnd23', 'QuantityBegin24', 'QuantityInput24', 'QuantityOutput24', 'Quantity24', 'QuantityEnd24', 'QuantityBegin25', 'QuantityInput25', 'QuantityOutput25', 'Quantity25', 'QuantityEnd25', 'QuantityBegin26', 'QuantityInput26', 'QuantityOutput26', 'Quantity26', 'QuantityEnd26', 'QuantityBegin27', 'QuantityInput27', 'QuantityOutput27', 'Quantity27', 'QuantityEnd27', 'QuantityBegin28', 'QuantityInput28', 'QuantityOutput28', 'Quantity28', 'QuantityEnd28', 'QuantityBegin29', 'QuantityInput29', 'QuantityOutput29', 'Quantity29', 'QuantityEnd29', 'QuantityBegin30', 'QuantityInput30', 'QuantityOutput30', 'Quantity30', 'QuantityEnd30', 'QuantityBegin31', 'QuantityInput31', 'QuantityOutput31', 'Quantity31', 'QuantityEnd31', 'UpdateDate',];
    DisplayColumns001: string[] = ['No', 'ID', 'FileName', 'Code', 'ParentName', 'Year', 'Month', 'QuantityBegin', 'QuantityInput00', 'QuantityOutput00', 'Quantity00', 'QuantityEnd', 'QuantityBegin01', 'QuantityInput01', 'QuantityOutput01', 'Quantity01', 'QuantityEnd01', 'QuantityBegin02', 'QuantityInput02', 'QuantityOutput02', 'Quantity02', 'QuantityEnd02', 'QuantityBegin03', 'QuantityInput03', 'QuantityOutput03', 'Quantity03', 'QuantityEnd03', 'QuantityBegin04', 'QuantityInput04', 'QuantityOutput04', 'Quantity04', 'QuantityEnd04', 'QuantityBegin05', 'QuantityInput05', 'QuantityOutput05', 'Quantity05', 'QuantityEnd05', 'QuantityBegin06', 'QuantityInput06', 'QuantityOutput06', 'Quantity06', 'QuantityEnd06', 'QuantityBegin07', 'QuantityInput07', 'QuantityOutput07', 'Quantity07', 'QuantityEnd07', 'QuantityBegin08', 'QuantityInput08', 'QuantityOutput08', 'Quantity08', 'QuantityEnd08', 'QuantityBegin09', 'QuantityInput09', 'QuantityOutput09', 'Quantity09', 'QuantityEnd09', 'QuantityBegin10', 'QuantityInput10', 'QuantityOutput10', 'Quantity10', 'QuantityEnd10', 'QuantityBegin11', 'QuantityInput11', 'QuantityOutput11', 'Quantity11', 'QuantityEnd11', 'QuantityBegin12', 'QuantityInput12', 'QuantityOutput12', 'Quantity12', 'QuantityEnd12', 'QuantityBegin13', 'QuantityInput13', 'QuantityOutput13', 'Quantity13', 'QuantityEnd13', 'QuantityBegin14', 'QuantityInput14', 'QuantityOutput14', 'Quantity14', 'QuantityEnd14', 'QuantityBegin15', 'QuantityInput15', 'QuantityOutput15', 'Quantity15', 'QuantityEnd15', 'QuantityBegin16', 'QuantityInput16', 'QuantityOutput16', 'Quantity16', 'QuantityEnd16', 'QuantityBegin17', 'QuantityInput17', 'QuantityOutput17', 'Quantity17', 'QuantityEnd17', 'QuantityBegin18', 'QuantityInput18', 'QuantityOutput18', 'Quantity18', 'QuantityEnd18', 'QuantityBegin19', 'QuantityInput19', 'QuantityOutput19', 'Quantity19', 'QuantityEnd19', 'QuantityBegin20', 'QuantityInput20', 'QuantityOutput20', 'Quantity20', 'QuantityEnd20', 'QuantityBegin21', 'QuantityInput21', 'QuantityOutput21', 'Quantity21', 'QuantityEnd21', 'QuantityBegin22', 'QuantityInput22', 'QuantityOutput22', 'Quantity22', 'QuantityEnd22', 'QuantityBegin23', 'QuantityInput23', 'QuantityOutput23', 'Quantity23', 'QuantityEnd23', 'QuantityBegin24', 'QuantityInput24', 'QuantityOutput24', 'Quantity24', 'QuantityEnd24', 'QuantityBegin25', 'QuantityInput25', 'QuantityOutput25', 'Quantity25', 'QuantityEnd25', 'QuantityBegin26', 'QuantityInput26', 'QuantityOutput26', 'Quantity26', 'QuantityEnd26', 'QuantityBegin27', 'QuantityInput27', 'QuantityOutput27', 'Quantity27', 'QuantityEnd27', 'QuantityBegin28', 'QuantityInput28', 'QuantityOutput28', 'Quantity28', 'QuantityEnd28', 'QuantityBegin29', 'QuantityInput29', 'QuantityOutput29', 'Quantity29', 'QuantityEnd29', 'QuantityBegin30', 'QuantityInput30', 'QuantityOutput30', 'Quantity30', 'QuantityEnd30', 'QuantityBegin31', 'QuantityInput31', 'QuantityOutput31', 'Quantity31', 'QuantityEnd31', 'UpdateDate',];
    DisplayColumns002: string[] = ['No', 'ID', 'FileName', 'Code', 'ParentName', 'Year', 'Month', 'QuantityBegin', 'QuantityInput00', 'QuantityOutput00', 'Quantity00', 'QuantityEnd', 'QuantityBegin01', 'QuantityInput01', 'QuantityOutput01', 'Quantity01', 'QuantityEnd01', 'QuantityBegin02', 'QuantityInput02', 'QuantityOutput02', 'Quantity02', 'QuantityEnd02', 'QuantityBegin03', 'QuantityInput03', 'QuantityOutput03', 'Quantity03', 'QuantityEnd03', 'QuantityBegin04', 'QuantityInput04', 'QuantityOutput04', 'Quantity04', 'QuantityEnd04', 'QuantityBegin05', 'QuantityInput05', 'QuantityOutput05', 'Quantity05', 'QuantityEnd05', 'QuantityBegin06', 'QuantityInput06', 'QuantityOutput06', 'Quantity06', 'QuantityEnd06', 'QuantityBegin07', 'QuantityInput07', 'QuantityOutput07', 'Quantity07', 'QuantityEnd07', 'QuantityBegin08', 'QuantityInput08', 'QuantityOutput08', 'Quantity08', 'QuantityEnd08', 'QuantityBegin09', 'QuantityInput09', 'QuantityOutput09', 'Quantity09', 'QuantityEnd09', 'QuantityBegin10', 'QuantityInput10', 'QuantityOutput10', 'Quantity10', 'QuantityEnd10', 'QuantityBegin11', 'QuantityInput11', 'QuantityOutput11', 'Quantity11', 'QuantityEnd11', 'QuantityBegin12', 'QuantityInput12', 'QuantityOutput12', 'Quantity12', 'QuantityEnd12', 'UpdateDate',];
    DisplayColumns003: string[] = ['No', 'ID', 'FileName', 'Code', 'ParentName', 'Year', 'Month', 'QuantityBegin', 'QuantityInput00', 'QuantityOutput00', 'Quantity00', 'QuantityEnd', 'QuantityBegin01', 'QuantityInput01', 'QuantityOutput01', 'Quantity01', 'QuantityEnd01', 'QuantityBegin02', 'QuantityInput02', 'QuantityOutput02', 'Quantity02', 'QuantityEnd02', 'UpdateDate',];
    DisplayColumns004: string[] = ['No', 'ID', 'Code', 'ParentName', 'Year', 'Month', 'QuantityBegin', 'QuantityInput00', 'QuantityOutput00', 'Quantity00', 'QuantityEnd', 'QuantityInput01', 'QuantityOutput01', 'Quantity01', 'QuantityInput02', 'QuantityOutput02', 'Quantity02', 'QuantityInput03', 'QuantityOutput03', 'Quantity03', 'QuantityInput04', 'QuantityOutput04', 'Quantity04', 'QuantityInput05', 'QuantityOutput05', 'Quantity05', 'QuantityInput06', 'QuantityOutput06', 'Quantity06', 'QuantityInput07', 'QuantityOutput07', 'Quantity07', 'QuantityInput08', 'QuantityOutput08', 'Quantity08', 'QuantityInput09', 'QuantityOutput09', 'Quantity09', 'QuantityInput10', 'QuantityOutput10', 'Quantity10', 'QuantityInput11', 'QuantityOutput11', 'Quantity11', 'QuantityInput12', 'QuantityOutput12', 'Quantity12', 'QuantityInput13', 'QuantityOutput13', 'Quantity13', 'QuantityInput14', 'QuantityOutput14', 'Quantity14', 'QuantityInput15', 'QuantityOutput15', 'Quantity15', 'QuantityInput16', 'QuantityOutput16', 'Quantity16', 'QuantityInput17', 'QuantityOutput17', 'Quantity17', 'QuantityInput18', 'QuantityOutput18', 'Quantity18', 'QuantityInput19', 'QuantityOutput19', 'Quantity19', 'QuantityInput20', 'QuantityOutput20', 'Quantity20', 'QuantityInput21', 'QuantityOutput21', 'Quantity21', 'QuantityInput22', 'QuantityOutput22', 'Quantity22', 'QuantityInput23', 'QuantityOutput23', 'Quantity23', 'QuantityInput24', 'QuantityOutput24', 'Quantity24', 'QuantityInput25', 'QuantityOutput25', 'Quantity25', 'QuantityInput26', 'QuantityOutput26', 'Quantity26', 'QuantityInput27', 'QuantityOutput27', 'Quantity27', 'QuantityInput28', 'QuantityOutput28', 'Quantity28', 'QuantityInput29', 'QuantityOutput29', 'Quantity29', 'QuantityInput30', 'QuantityOutput30', 'Quantity30', 'QuantityInput31', 'QuantityOutput31', 'Quantity31'];
    DisplayColumns005: string[] = ['No', 'Code', 'QuantityEnd', 'Quantity01', 'Quantity02', 'Quantity03', 'Quantity04', 'Quantity05', 'Quantity06', 'Quantity07'];

    DisplayColumns006: string[] = ['No', 'ID', 'Code', 'ParentName', 'Year', 'Month', 'QuantityStock', 'QuantityBegin', 'QuantityInput00', 'QuantityOutput00', 'Quantity00', 'QuantityEnd', 'QuantityInput01', 'QuantityOutput01', 'Quantity01', 'QuantityInput02', 'QuantityOutput02', 'Quantity02', 'QuantityInput03', 'QuantityOutput03', 'Quantity03', 'QuantityInput04', 'QuantityOutput04', 'Quantity04', 'QuantityInput05', 'QuantityOutput05', 'Quantity05', 'QuantityInput06', 'QuantityOutput06', 'Quantity06', 'QuantityInput07', 'QuantityOutput07', 'Quantity07', 'QuantityInput08', 'QuantityOutput08', 'Quantity08', 'QuantityInput09', 'QuantityOutput09', 'Quantity09', 'QuantityInput10', 'QuantityOutput10', 'Quantity10', 'QuantityInput11', 'QuantityOutput11', 'Quantity11', 'QuantityInput12', 'QuantityOutput12', 'Quantity12', 'QuantityInput13', 'QuantityOutput13', 'Quantity13', 'QuantityInput14', 'QuantityOutput14', 'Quantity14', 'QuantityInput15', 'QuantityOutput15', 'Quantity15', 'QuantityInput16', 'QuantityOutput16', 'Quantity16', 'QuantityInput17', 'QuantityOutput17', 'Quantity17', 'QuantityInput18', 'QuantityOutput18', 'Quantity18', 'QuantityInput19', 'QuantityOutput19', 'Quantity19', 'QuantityInput20', 'QuantityOutput20', 'Quantity20', 'QuantityInput21', 'QuantityOutput21', 'Quantity21', 'QuantityInput22', 'QuantityOutput22', 'Quantity22', 'QuantityInput23', 'QuantityOutput23', 'Quantity23', 'QuantityInput24', 'QuantityOutput24', 'Quantity24', 'QuantityInput25', 'QuantityOutput25', 'Quantity25', 'QuantityInput26', 'QuantityOutput26', 'Quantity26', 'QuantityInput27', 'QuantityOutput27', 'Quantity27', 'QuantityInput28', 'QuantityOutput28', 'Quantity28', 'QuantityInput29', 'QuantityOutput29', 'Quantity29', 'QuantityInput30', 'QuantityOutput30', 'Quantity30', 'QuantityInput31', 'QuantityOutput31', 'Quantity31'];
    DisplayColumns007: string[] = ['No', 'ID', 'Code', 'ParentName', 'Year', 'Month', 'QuantityStock', 'QuantityBegin', 'QuantityInput00', 'QuantityOutput00', 'Quantity00', 'QuantityEnd', 'QuantityInput01', 'QuantityOutput01', 'Quantity01', 'QuantityInput02', 'QuantityOutput02', 'Quantity02', 'QuantityInput03', 'QuantityOutput03', 'Quantity03', 'QuantityInput04', 'QuantityOutput04', 'Quantity04', 'QuantityInput05', 'QuantityOutput05', 'Quantity05', 'QuantityInput06', 'QuantityOutput06', 'Quantity06', 'QuantityInput07', 'QuantityOutput07', 'Quantity07', 'QuantityInput08', 'QuantityOutput08', 'Quantity08', 'QuantityInput09', 'QuantityOutput09', 'Quantity09', 'QuantityInput10', 'QuantityOutput10', 'Quantity10', 'QuantityInput11', 'QuantityOutput11', 'Quantity11', 'QuantityInput12', 'QuantityOutput12', 'Quantity12', 'QuantityInput13', 'QuantityOutput13', 'Quantity13', 'QuantityInput14', 'QuantityOutput14', 'Quantity14', 'QuantityInput15', 'QuantityOutput15', 'Quantity15', 'QuantityInput16', 'QuantityOutput16', 'Quantity16', 'QuantityInput17', 'QuantityOutput17', 'Quantity17', 'QuantityInput18', 'QuantityOutput18', 'Quantity18', 'QuantityInput19', 'QuantityOutput19', 'Quantity19', 'QuantityInput20', 'QuantityOutput20', 'Quantity20', 'QuantityInput21', 'QuantityOutput21', 'Quantity21', 'QuantityInput22', 'QuantityOutput22', 'Quantity22', 'QuantityInput23', 'QuantityOutput23', 'Quantity23', 'QuantityInput24', 'QuantityOutput24', 'Quantity24', 'QuantityInput25', 'QuantityOutput25', 'Quantity25', 'QuantityInput26', 'QuantityOutput26', 'Quantity26', 'QuantityInput27', 'QuantityOutput27', 'Quantity27', 'QuantityInput28', 'QuantityOutput28', 'Quantity28', 'QuantityInput29', 'QuantityOutput29', 'Quantity29', 'QuantityInput30', 'QuantityOutput30', 'Quantity30', 'QuantityInput31', 'QuantityOutput31', 'Quantity31'];
    DisplayColumns008: string[] = ['No', 'ID', 'Code', 'ParentName', 'Year', 'Month', 'QuantityStock', 'QuantityBegin', 'QuantityInput00', 'QuantityOutput00', 'Quantity00', 'QuantityEnd', 'QuantityInput01', 'QuantityOutput01', 'Quantity01', 'QuantityInput02', 'QuantityOutput02', 'Quantity02', 'QuantityInput03', 'QuantityOutput03', 'Quantity03', 'QuantityInput04', 'QuantityOutput04', 'Quantity04', 'QuantityInput05', 'QuantityOutput05', 'Quantity05', 'QuantityInput06', 'QuantityOutput06', 'Quantity06', 'QuantityInput07', 'QuantityOutput07', 'Quantity07', 'QuantityInput08', 'QuantityOutput08', 'Quantity08', 'QuantityInput09', 'QuantityOutput09', 'Quantity09', 'QuantityInput10', 'QuantityOutput10', 'Quantity10', 'QuantityInput11', 'QuantityOutput11', 'Quantity11', 'QuantityInput12', 'QuantityOutput12', 'Quantity12'];


    List: WarehouseInventory[] | undefined;
    ListFilter: WarehouseInventory[] | undefined;
    FormData!: WarehouseInventory;
    APIURL: string = environment.APIReportURL;
    APIRootURL: string = environment.APIReportRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseInventory";
    }

    GetByActionAndYearAndMonthAsync() {
        let url = this.APIURL + this.Controller + '/GetByActionAndYearAndMonthAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentIDAndActionAndYearAndMonthAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentIDAndActionAndYearAndMonthAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentIDAndYearAndMonthToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentIDAndYearAndMonthToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentIDAndYearAndMonthAndActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentIDAndYearAndMonthAndActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndSearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndSearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndActionAndSearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndActionAndSearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyIDAndYearAndMonthToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyIDAndYearAndMonthToListAsync';
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
    GetByCategoryDepartmentIDAndYearAndMonthAndMaterialSpecialToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentIDAndYearAndMonthAndMaterialSpecialToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryDepartmentIDViaCompanyIDAndYearAndMonthToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentIDViaCompanyIDAndYearAndMonthToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncByCategoryDepartmentIDAndYearAndMonthAsync() {
        let url = this.APIURL + this.Controller + '/SyncByCategoryDepartmentIDAndYearAndMonthAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncInvoiceByCategoryDepartmentIDAndYearAndMonthAsync() {
        let url = this.APIURL + this.Controller + '/SyncInvoiceByCategoryDepartmentIDAndYearAndMonthAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncValueByCategoryDepartmentIDAndYearAndMonthAsync() {
        let url = this.APIURL + this.Controller + '/SyncValueByCategoryDepartmentIDAndYearAndMonthAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

