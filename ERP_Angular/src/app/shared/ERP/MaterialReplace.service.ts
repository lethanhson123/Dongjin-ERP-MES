import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { MaterialReplace } from './MaterialReplace.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class MaterialReplaceService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'ParentName', 'ECN', 'MaterialName01', 'Quantity01', 'MaterialName02', 'Quantity02', 'Active', 'Save'];
  
    List: MaterialReplace[] | undefined;
    ListFilter: MaterialReplace[] | undefined;
    FormData!: MaterialReplace;
    APIURL: string = environment.APIMaterialURL;
    APIRootURL: string = environment.APIMaterialRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "MaterialReplace";
    } 
}

