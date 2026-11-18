import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryHRMTinhTrangLamViec } from './CategoryHRMTinhTrangLamViec.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryHRMTinhTrangLamViecService extends BaseService{
 DisplayColumns001: string[] = ['No', 'ID', 'Code', 'Name', 'Display', 'Note', 'SortOrder', 'Active', 'Save'];

    List: CategoryHRMTinhTrangLamViec[] | undefined;
    ListFilter: CategoryHRMTinhTrangLamViec[] | undefined;
    FormData!: CategoryHRMTinhTrangLamViec;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryHRMTinhTrangLamViec";
    }
}

