import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { WarehouseStock } from './WarehouseStock.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class WarehouseStockService extends BaseService {       
    DisplayColumns009: string[] = ['No', 'ID', 'Year', 'Month', 'Day', 'Description', 'FileName', 'Code', 'QuantityInput00', 'QuantityOutput00', 'Quantity00'];
    DisplayColumns010: string[] = ['No', 'ID', 'Year', 'Month', 'Day', 'Date', 'Display', 'Description', 'FileName', 'Code', 'QuantityStock', 'QuantityInput00', 'QuantityOutput00', 'Quantity00'];
    DisplayColumns011: string[] = ['No', 'ID', 'Date', 'Description', 'FileName', 'Code', 'Quantity00', 'Quantity01', 'Quantity02'];
    DisplayColumns012: string[] = ['No', 'ID', 'Code', 'QuantityStock', 'Input00', 'Input01', 'Input02', 'Input03', 'Input04', 'Input05', 'Input06', 'Input07', 'Input08', 'Input09', 'Input10', 'Input11', 'Input12', 'Input13', 'Input14', 'Input15', 'Input16', 'Input17', 'Input18', 'Input19', 'Input20', 'Input21', 'Input22', 'Input23', 'Input24', 'Input25', 'Input26', 'Input27', 'Input28', 'Input29', 'Input30', 'Input31'];
    DisplayColumns013: string[] = ['No', 'ID', 'Code', 'QuantityStock', 'Description',];

    List: WarehouseStock[] | undefined;
    ListFilter: WarehouseStock[] | undefined;
    FormData!: WarehouseStock;
    APIURL: string = environment.APIReportURL;
    APIRootURL: string = environment.APIReportRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "WarehouseStock";
    }

    
    GetByCompanyIDAndCategoryDepartmentIDAndYearAndMonthAndDayAndActionToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyIDAndCategoryDepartmentIDAndYearAndMonthAndDayAndActionToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncAsync() {
        let url = this.APIURL + this.Controller + '/SyncAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

