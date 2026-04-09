import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { BOMStage } from './BOMStage.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class BOMStageService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Code', 'Percent', 'Note', 'Active', 'Save'];   

    List: BOMStage[] | undefined;
    ListFilter: BOMStage[] | undefined;
    FormData!: BOMStage;
    APIURL: string = environment.APIMaterialURL;
    APIRootURL: string = environment.APIMaterialRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "BOMStage";
    }
}

