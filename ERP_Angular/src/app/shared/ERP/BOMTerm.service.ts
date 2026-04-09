import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { BOMTerm } from './BOMTerm.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class BOMTermService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Code', 'CCH1', 'CCW1', 'ICH1', 'ICW1', 'CCH2', 'CCW2', 'ICH2', 'ICW2', 'Note', 'Active', 'Save'];   

    List: BOMTerm[] | undefined;
    ListFilter: BOMTerm[] | undefined;
    FormData!: BOMTerm;
    APIURL: string = environment.APIMaterialURL;
    APIRootURL: string = environment.APIMaterialRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "BOMTerm";
    }
}

