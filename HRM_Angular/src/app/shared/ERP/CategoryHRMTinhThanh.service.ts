import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryHRMTinhThanh } from './CategoryHRMTinhThanh.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryHRMTinhThanhService extends BaseService{
 DisplayColumns001: string[] = ['No', 'ID', 'Code', 'Name', 'Display', 'Note', 'SortOrder', 'Active', 'Save'];

    List: CategoryHRMTinhThanh[] | undefined;
    ListFilter: CategoryHRMTinhThanh[] | undefined;
    FormData!: CategoryHRMTinhThanh;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryHRMTinhThanh";
    }
}

