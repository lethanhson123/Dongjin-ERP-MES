import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryConfig } from './CategoryConfig.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryConfigService extends BaseService {
    
    DisplayColumns001: string[] = ['No', 'ID', 'Code', 'Font01', 'Color01', 'FontSize01', 'Font02', 'Color02', 'FontSize02', 'NumberView', 'SortOrder', 'Active', 'Save'];

    List: CategoryConfig[] | undefined;
    ListFilter: CategoryConfig[] | undefined;
    FormData!: CategoryConfig;    
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryConfig";
    }
}

