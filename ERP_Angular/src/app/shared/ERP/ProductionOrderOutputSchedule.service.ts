import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { ProductionOrderOutputSchedule } from './ProductionOrderOutputSchedule.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ProductionOrderOutputScheduleService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Begin', 'End', 'Note', 'Active', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'Begin', 'End', 'Code', 'Note', 'Active', 'Save'];
    DisplayColumns003: string[] = ['No', 'ID', 'CreateDate', 'Begin', 'End', 'Code', 'Note', 'Active', 'Save'];

    List: ProductionOrderOutputSchedule[] | undefined;
    ListFilter: ProductionOrderOutputSchedule[] | undefined;
    FormData!: ProductionOrderOutputSchedule;
    APIURL: string = environment.APIProductionOrderURL;
    APIRootURL: string = environment.APIProductionOrderRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "ProductionOrderOutputSchedule";
    }
}

