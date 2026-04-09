import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryTerm } from './CategoryTerm.model';
import { BaseService } from './Base.service';
@Injectable({
    providedIn: 'root'
})
export class CategoryTermService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Code', 'Name', 'Display', 'SQ', 'CCH1','CCW1', 'ICH1','ICW1','Note', 'Active', 'Save'];

    List: CategoryTerm[] | undefined;
    ListFilter: CategoryTerm[] | undefined;
    FormData!: CategoryTerm;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryTerm";
    }
}

