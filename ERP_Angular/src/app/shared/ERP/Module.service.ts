import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { Module } from './Module.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ModuleService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID',  'Code', 'Name', 'Display', 'Note', 'SortOrder', 'Active', 'Save'];

    List: Module[] | undefined;
    ListFilter: Module[] | undefined;
    FormData!: Module;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "Module";
    }
}

