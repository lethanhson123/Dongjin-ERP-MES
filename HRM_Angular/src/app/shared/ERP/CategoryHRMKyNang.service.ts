import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryHRMKyNang } from './CategoryHRMKyNang.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryHRMKyNangService extends BaseService{
 DisplayColumns001: string[] = ['Code', 'Name', 'Month', 'Display', 'Description', 'Note', 'Save'];

    List: CategoryHRMKyNang[] | undefined;
    ListFilter: CategoryHRMKyNang[] | undefined;
    FormData!: CategoryHRMKyNang;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryHRMKyNang";
    }
}

