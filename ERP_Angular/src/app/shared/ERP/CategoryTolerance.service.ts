import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryTolerance } from './CategoryTolerance.model';
import { BaseService } from './Base.service';
@Injectable({
    providedIn: 'root'
})
export class CategoryToleranceService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Begin', 'End', 'CCH1', 'CCW1', 'ICH1', 'ICW1', 'Note', 'SortOrder', 'Active', 'Save'];

    List: CategoryTolerance[] | undefined;
    ListFilter: CategoryTolerance[] | undefined;
    FormData!: CategoryTolerance;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryTolerance";
    }
}

