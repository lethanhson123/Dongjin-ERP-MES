import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryProcess } from './CategoryProcess.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryProcessService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'ParentID', 'Code', 'Name', 'Display', 'Note', 'SortOrder', 'Active', 'Save'];

    List: CategoryProcess[] | undefined;
    ListFilter: CategoryProcess[] | undefined;
    FormData!: CategoryProcess;    
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryProcess";
    }
}

