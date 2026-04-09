import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { ReportDetail } from './ReportDetail.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ReportDetailService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'ParentID', 'ParentName', 'CreateDate', 'CreateUserID', 'CreateUserCode', 'CreateUserName', 'UpdateDate', 'UpdateUserID', 'UpdateUserCode', 'UpdateUserName', 'RowVersion', 'SortOrder', 'Active', 'Code', 'Name', 'Display', 'Description', 'Note', 'FileName', 'CompanyID', 'CompanyName', 'Date00', 'Name00', 'Quantity00', 'Date01', 'Name01', 'Quantity01', 'Date02', 'Name02', 'Quantity02', 'Date03', 'Name03', 'Quantity03', 'Date04', 'Name04', 'Quantity04', 'Date05', 'Name05', 'Quantity05', 'Date06', 'Name06', 'Quantity06', 'Date07', 'Name07', 'Quantity07', 'Date08', 'Name08', 'Quantity08', 'Date09', 'Name09', 'Quantity09', 'Date10', 'Name10', 'Quantity10', 'Date11', 'Name11', 'Quantity11', 'Date12', 'Name12', 'Quantity12', 'Date13', 'Name13', 'Quantity13', 'Date14', 'Name14', 'Quantity14', 'Date15', 'Name15', 'Quantity15', 'Date16', 'Name16', 'Quantity16', 'Date17', 'Name17', 'Quantity17', 'Date18', 'Name18', 'Quantity18', 'Date19', 'Name19', 'Quantity19', 'Date20', 'Name20', 'Quantity20', 'Date21', 'Name21', 'Quantity21', 'Date22', 'Name22', 'Quantity22', 'Date23', 'Name23', 'Quantity23', 'Date24', 'Name24', 'Quantity24', 'Date25', 'Name25', 'Quantity25', 'Date26', 'Name26', 'Quantity26', 'Date27', 'Name27', 'Quantity27', 'Date28', 'Name28', 'Quantity28', 'Date29', 'Name29', 'Quantity29', 'Date30', 'Name30', 'Quantity30', 'Date31', 'Name31', 'Quantity31', 'Save'];
    DisplayColumns002: string[] = ['No', 'Code', 'Name', 'Date00', 'Date01', 'Date02', 'Date03', 'Date04', 'Date05', 'Date06', 'Date07', 'Date08', 'Date09', 'Date10', 'Date11', 'Date12', 'Date13', 'Date14', 'Date15'];
    DisplayColumns003: string[] = ['No', 'Code', 'Name', 'Date00', 'Date01', 'Date02', 'Date03', 'Date04', 'Date05', 'Date06', 'Date07', 'Date08', 'Date09', 'Date10', 'Date11', 'Date12', 'Date13', 'Date14', 'Date15', 'Date16', 'Date17', 'Date18', 'Date19', 'Date20', 'Date21', 'Date22', 'Date23', 'Date24', 'Date25', 'Date26', 'Date27', 'Date28'];
    DisplayColumns004: string[] = ['No', 'Code', 'Quantity01', 'Quantity02', 'Quantity03', 'Quantity04'];
    DisplayColumns005: string[] = ['No', 'Description', 'Display', 'Name', 'Code', 'Quantity01', 'Quantity18', 'Quantity17', 'Quantity02', 'Quantity03', 'Quantity04', 'Quantity05', 'Quantity06', 'Quantity07', 'Quantity08', 'Quantity09', 'Quantity10', 'Quantity11', 'Quantity12', 'Quantity13', 'Quantity14', 'Quantity15', 'ID', 'ParentID'];
    DisplayColumns006: string[] = ['No', 'Name', 'Display', 'Note', 'Code', 'Active', 'Quantity31', 'Quantity30', 'Quantity01', 'Quantity02', 'Quantity05', 'Quantity06', 'Quantity10', 'Quantity13', 'Quantity11', 'Quantity12', 'Quantity20', 'Quantity21', 'Description', 'ID', 'ParentID'];
    DisplayColumns007: string[] = ['No', 'Date01', 'Date02', 'Date03', 'Display', 'Name', 'Code', 'Quantity01', 'CompanyID', 'ID', 'ParentID'];
    DisplayColumns008: string[] = ['No', 'Date01', 'Display', 'Name', 'Code', 'Quantity01', 'Description', 'Note', 'CompanyID', 'ID', 'ParentID'];
    DisplayColumns009: string[] = ['No', 'Code', 'Quantity00', 'Date01', 'Date02', 'Name', 'Display', 'Description', 'Note',];

    List: ReportDetail[] | undefined;
    ListFilter: ReportDetail[] | undefined;
    FormData!: ReportDetail;
    FormData001!: ReportDetail;
    FormData002!: ReportDetail;
    FormData003!: ReportDetail;
    FormData004!: ReportDetail;
    FormData005!: ReportDetail;
    FormData006!: ReportDetail;
    APIURL: string = environment.APIReportURL;
    APIRootURL: string = environment.APIReportRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "ReportDetail";
    }
    GetProductionTracking2026ByParentIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetProductionTracking2026ByParentIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetWarehouseStockLongTermByParentIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetWarehouseStockLongTermByParentIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetWarehouseStockLongTerm1000ByParentIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetWarehouseStockLongTerm1000ByParentIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    HookRackByParentIDExportToExcelAsync() {
        let url = this.APIURL + this.Controller + '/HookRackByParentIDExportToExcelAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

