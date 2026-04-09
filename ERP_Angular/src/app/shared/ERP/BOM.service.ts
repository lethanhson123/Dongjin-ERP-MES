import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { BOM } from './BOM.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class BOMService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'CategoryFamilyName', 'MaterialName', 'Code', 'Version', 'Date', 'CompanyName', 'Active'];
    DisplayColumns002: string[] = ['No', 'ID', 'ParentID', 'ParentID01', 'CategoryFamilyName', 'MaterialName', 'Code', 'Quantity', 'Version', 'Date', 'CompanyName', 'Active'];
    DisplayColumns003: string[] = ['No', 'ID', 'MaterialName', 'Code', 'Quantity', 'Version', 'Date', 'CompanyName', 'UpdateUserCode', 'UpdateUserName', 'UpdateDate', 'Active'];
    DisplayColumns004: string[] = ['No', 'ID', 'CompanyName', 'MaterialCode', 'Code', 'Version', 'Date', 'UpdateUserCode', 'UpdateUserName', 'UpdateDate', 'Active'];
    DisplayColumns005: string[] = ['No', 'ID', 'CompanyName', 'MaterialCode', 'Code', 'Version', 'Date', 'BundleSize', 'Project', 'Item', 'Group', 'UpdateUserCode', 'UpdateDate', 'Active'];
    DisplayColumns006: string[] = ['No', 'ID', 'ParentID', 'ParentID01', 'MaterialName', 'Code', 'Quantity', 'Version', 'Date', 'BundleSize', 'Project', 'Item', 'Group', 'Active'];
    DisplayColumns007: string[] = ['No', 'ID', 'CompanyName', 'MaterialCode', 'Code', 'Version', 'Date', 'BundleSize', 'Strip1', 'Strip2', 'Project', 'Item', 'Group', 'UpdateUserCode', 'UpdateDate', 'Active'];
    DisplayColumns008: string[] = ['No', 'ID', 'ParentID', 'ParentID01', 'MaterialName', 'Code', 'Quantity', 'Version', 'Date', 'BundleSize', 'Strip1', 'Strip2', 'Project', 'Item', 'Group', 'Active'];
    DisplayColumns009: string[] = ['No', 'ID', 'ParentID', 'ParentID01', 'ParentID02', 'ParentID03', 'ParentID04', 'MaterialName', 'Code', 'Quantity', 'Version', 'Date', 'BundleSize', 'Strip1', 'Strip2', 'Project', 'Item', 'Group', 'Active'];
    DisplayColumns010: string[] = ['No', 'ID', 'ParentID', 'ParentID01', 'ParentID02', 'ParentID03', 'ParentID04', 'MaterialName', 'Code', 'Quantity', 'Version', 'Date', 'BundleSize', 'DirT1', 'DirT2', 'Project', 'Item', 'Stage', 'Active'];
    DisplayColumns011: string[] = ['No', 'ID', 'ParentID', 'ParentID01', 'ParentID02', 'ParentID03', 'ParentID04', 'MaterialName', 'Code', 'Quantity', 'Version', 'Date', 'BundleSize', 'DirT1', 'DirT2', 'Project', 'Item', 'Stage', 'Strip1', 'Strip2', 'Active', 'Save'];
    DisplayColumns012: string[] = ['No', 'ID', 'ParentID', 'ParentID01', 'ParentID02', 'ParentID03', 'ParentID04', 'Code', 'Quantity', 'Version', 'Date', 'BundleSize', 'DirT1', 'DirT2', 'Project', 'Item', 'Stage', 'Active'];
    DisplayColumns013: string[] = ['No', 'ID', 'CompanyName', 'ParentID', 'MaterialCode', 'Code', 'Version', 'Date', 'BundleSize', 'Quantity', 'Project', 'Item', 'Group', 'UpdateUserCode', 'UpdateDate', 'Active'];
    DisplayColumns014: string[] = ['No', 'ID', 'CompanyName', 'ParentID', 'MaterialCode', 'Code', 'Version', 'Date', 'Quantity', 'BundleSize', 'Level', 'BOMCount', 'RawMaterialCount', 'UpdateUserCode', 'UpdateDate', 'Active'];
    DisplayColumns015: string[] = ['No', 'ID', 'ParentID', 'ParentID01', 'ParentID02', 'ParentID03', 'ParentID04', 'Code', 'Quantity', 'Version', 'Date', 'BundleSize', 'DirT1', 'DirT2', 'Project', 'Item', 'Stage', 'Level', 'BOMCount', 'RawMaterialCount', 'Active'];
    DisplayColumns016: string[] = ['No', 'ID', 'CompanyName', 'ParentID', 'MaterialCode', 'Code', 'Version', 'Date', 'Quantity', 'BundleSize', 'Level', 'BOMCount', 'RawMaterialCount', 'UpdateUserCode', 'UpdateDate', 'IsLeadNo', 'Active'];
    DisplayColumns017: string[] = ['No', 'ID', 'ParentID', 'ParentID01', 'ParentID02', 'ParentID03', 'ParentID04', 'Code', 'Quantity', 'Version', 'Date', 'BundleSize', 'DirT1', 'DirT2', 'Project', 'Item', 'Stage', 'Level', 'BOMCount', 'RawMaterialCount', 'IsLeadNo', 'Active'];
    DisplayColumns018: string[] = ['No', 'ID', 'CompanyName', 'ParentID', 'MaterialCode', 'Code', 'Version', 'Date', 'Quantity', 'BundleSize', 'Level', 'BOMCount', 'RawMaterialCount', 'UpdateUserCode', 'UpdateDate', 'IsSPST', 'IsLeadNo', 'Active'];
    DisplayColumns019: string[] = ['No', 'ID', 'ParentID', 'ParentID01', 'ParentID02', 'ParentID03', 'ParentID04', 'Code', 'Quantity', 'Version', 'Date', 'BundleSize', 'DirT1', 'DirT2', 'Project', 'Item', 'Stage', 'Level', 'BOMCount', 'RawMaterialCount', 'IsSPST', 'IsLeadNo', 'Active'];
    DisplayColumns020: string[] = ['No', 'ID', 'CompanyName', 'ParentID', 'MaterialCode', 'Code', 'Version', 'Date', 'Quantity', 'BundleSize', 'Level', 'BOMCount', 'RawMaterialCount', 'UpdateUserCode', 'UpdateDate', 'Active'];
    DisplayColumns021: string[] = ['No', 'ID', 'CompanyName', 'ParentID', 'MaterialCode', 'Code', 'Version', 'Date', 'Quantity', 'BundleSize', 'Level', 'BOMCount', 'RawMaterialCount', 'UpdateUserCode', 'UpdateDate', 'Active'];
    DisplayColumns022: string[] = ['No', 'UpdateDate', 'Code', 'ParentID', 'ParentID01', 'ParentID02', 'ParentID03', 'ParentID04',  'IsLeadNo', 'IsSPST', 'RowVersion', 'SortOrder', 'RawMaterialCount', 'Quantity', 'BOMCount', 'Description',];
    DisplayColumns023: string[] = ['No', 'Quantity', ];


    List: BOM[] | undefined;
    ListFilter: BOM[] | undefined;
    FormData!: BOM;
    APIURL: string = environment.APIMaterialURL;
    APIRootURL: string = environment.APIMaterialRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "BOM";
    }
    SaveListAndUploadFileAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveListAndUploadFileAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveByLEADAndUploadFileAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveByLEADAndUploadFileAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveByBOMRawMaterialAndUploadFileAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveByBOMRawMaterialAndUploadFileAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveOnlyLeadNoAndUploadFileAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveOnlyLeadNoAndUploadFileAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveByHookRackAndUploadFileAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveByHookRackAndUploadFileAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    CreateAutoAsync() {
        let url = this.APIURL + this.Controller + '/CreateAutoAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCode_MaterialCodeToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCode_MaterialCodeToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_Code_MaterialCodeToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_Code_MaterialCodeToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_PageAndPageSizeToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_PageAndPageSizeToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    ExportBOMLeadByIDToExcelAsync() {
        let url = this.APIURL + this.Controller + '/ExportBOMLeadByIDToExcelAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    ExportBOMLeadByECNToExcelAsync() {
        let url = this.APIURL + this.Controller + '/ExportBOMLeadByECNToExcelAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync() {
        let url = this.APIURL + this.Controller + '/GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncFinishGoodsListOftrackmtimAsync() {
        let url = this.APIURL + this.Controller + '/SyncFinishGoodsListOftrackmtimAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

