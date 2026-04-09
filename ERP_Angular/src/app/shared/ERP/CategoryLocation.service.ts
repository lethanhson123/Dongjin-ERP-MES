import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryLocation } from './CategoryLocation.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryLocationService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'FileName', 'ParentName', 'CategoryLayerName', 'Code', 'Name', 'Height', 'Width', 'Length', 'Weight', 'SortOrder', 'Active', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'FileName', 'ParentName', 'CategoryLayerName', 'Code', 'Name', 'Height', 'Width', 'Length', 'Weight', 'SortOrder', 'IsTemporary', 'Active', 'Save'];
    DisplayColumns003: string[] = ['No', 'ID', 'ParentName', 'CategoryLayerName', 'Code', 'Name', 'SortOrder', 'IsTemporary', 'Active', 'Save'];

    List: CategoryLocation[] | undefined;
    ListFilter: CategoryLocation[] | undefined;
    FormData!: CategoryLocation;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryLocation";
    }
    GetByCategoryDepartmentIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByParentID_CategoryLayerIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByParentID_CategoryLayerIDToListAsync';
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
    PrintByCategoryDepartmentIDAsync() {
        let url = this.APIURL + this.Controller + '/PrintByCategoryDepartmentIDAsync';
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
}

