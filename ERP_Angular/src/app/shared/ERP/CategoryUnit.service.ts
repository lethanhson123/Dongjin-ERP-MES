import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryUnit } from './CategoryUnit.model';
import { BaseService } from './Base.service';
@Injectable({
    providedIn: 'root'
})
export class CategoryUnitService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Code', 'Name', 'Display', 'Note', 'SortOrder', 'Active', 'Save'];

    List: CategoryUnit[] | undefined;
    ListFilter: CategoryUnit[] | undefined;
    FormData!: CategoryUnit;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryUnit";
    }
}

