import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { MaterialUnitCovert } from './MaterialUnitCovert.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class MaterialUnitCovertService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'CompanyName', 'ParentName', 'Quantity01', 'CategoryUnitName01', 'Quantity02', 'CategoryUnitName02',  'Note', 'SortOrder', 'Active', 'Save'];

    List: MaterialUnitCovert[] | undefined;
    ListFilter: MaterialUnitCovert[] | undefined;
    FormData!: MaterialUnitCovert;
    APIURL: string = environment.APIMaterialURL;
    APIRootURL: string = environment.APIMaterialRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "MaterialUnitCovert";
    }
}

