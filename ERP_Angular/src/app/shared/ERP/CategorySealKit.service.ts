import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategorySealKit } from './CategorySealKit.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategorySealKitService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'UpdateDate', 'Name', 'Code',  'Note', 'Active', 'Save'];

    List: CategorySealKit[] | undefined;
    ListFilter: CategorySealKit[] | undefined;
    FormData!: CategorySealKit;    
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategorySealKit";
    }
}

