import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryLocationMaterial } from './CategoryLocationMaterial.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryLocationMaterialService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'MaterialName', 'Count', 'Active', 'Save'];

    List: CategoryLocationMaterial[] | undefined;
    ListFilter: CategoryLocationMaterial[] | undefined;
    FormData!: CategoryLocationMaterial;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryLocationMaterial";
    }
    CreateAutoAsync() {
        let url = this.APIURL + this.Controller + '/CreateAutoAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

