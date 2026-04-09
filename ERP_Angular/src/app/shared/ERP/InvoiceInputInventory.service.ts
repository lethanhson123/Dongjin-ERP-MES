import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { InvoiceInputInventory } from './InvoiceInputInventory.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class InvoiceInputInventoryService extends BaseService {
    DisplayColumns000: string[] = ['No', 'Code', 'ParentName', 'QuantityInput00'];
    DisplayColumns001: string[] = ['No', 'Code', 'ParentName', 'Year', 'Month', 'QuantityInput00', 'QuantityOutput00', 'Quantity00', 'QuantityInput01', 'QuantityOutput01', 'Quantity01', 'QuantityInput02', 'QuantityOutput02', 'Quantity02', 'QuantityInput03', 'QuantityOutput03', 'Quantity03', 'QuantityInput04', 'QuantityOutput04', 'Quantity04', 'QuantityInput05', 'QuantityOutput05', 'Quantity05', 'QuantityInput06', 'QuantityOutput06', 'Quantity06', 'QuantityInput07', 'QuantityOutput07', 'Quantity07', 'QuantityInput08', 'QuantityOutput08', 'Quantity08', 'QuantityInput09', 'QuantityOutput09', 'Quantity09', 'QuantityInput10', 'QuantityOutput10', 'Quantity10', 'QuantityInput11', 'QuantityOutput11', 'Quantity11', 'QuantityInput12', 'QuantityOutput12', 'Quantity12', 'QuantityInput13', 'QuantityOutput13', 'Quantity13', 'QuantityInput14', 'QuantityOutput14', 'Quantity14', 'QuantityInput15', 'QuantityOutput15', 'Quantity15', 'QuantityInput16', 'QuantityOutput16', 'Quantity16', 'QuantityInput17', 'QuantityOutput17', 'Quantity17', 'QuantityInput18', 'QuantityOutput18', 'Quantity18', 'QuantityInput19', 'QuantityOutput19', 'Quantity19', 'QuantityInput20', 'QuantityOutput20', 'Quantity20', 'QuantityInput21', 'QuantityOutput21', 'Quantity21', 'QuantityInput22', 'QuantityOutput22', 'Quantity22', 'QuantityInput23', 'QuantityOutput23', 'Quantity23', 'QuantityInput24', 'QuantityOutput24', 'Quantity24', 'QuantityInput25', 'QuantityOutput25', 'Quantity25', 'QuantityInput26', 'QuantityOutput26', 'Quantity26', 'QuantityInput27', 'QuantityOutput27', 'Quantity27', 'QuantityInput28', 'QuantityOutput28', 'Quantity28', 'QuantityInput29', 'QuantityOutput29', 'Quantity29', 'QuantityInput30', 'QuantityOutput30', 'Quantity30', 'QuantityInput31', 'QuantityOutput31', 'Quantity31'];
    DisplayColumns002: string[] = ['No', 'Code', 'ParentName', 'Year', 'Month', 'QuantityInput00', 'QuantityOutput00', 'Quantity00', 'QuantityInput01', 'QuantityOutput01', 'Quantity01', 'QuantityInput02', 'QuantityOutput02', 'Quantity02', 'QuantityInput03', 'QuantityOutput03', 'Quantity03', 'QuantityInput04', 'QuantityOutput04', 'Quantity04', 'QuantityInput05', 'QuantityOutput05', 'Quantity05', 'QuantityInput06', 'QuantityOutput06', 'Quantity06', 'QuantityInput07', 'QuantityOutput07', 'Quantity07', 'QuantityInput08', 'QuantityOutput08', 'Quantity08', 'QuantityInput09', 'QuantityOutput09', 'Quantity09', 'QuantityInput10', 'QuantityOutput10', 'Quantity10', 'QuantityInput11', 'QuantityOutput11', 'Quantity11', 'QuantityInput12', 'QuantityOutput12', 'Quantity12'];
    DisplayColumns003: string[] = ['No', 'Code', 'ParentName', 'Year', 'Month', 'QuantityInput00', 'QuantityOutput00', 'Quantity00', 'QuantityInput01', 'QuantityOutput01', 'Quantity01', 'QuantityInput02', 'QuantityOutput02', 'Quantity02', 'QuantityInput03', 'QuantityOutput03', 'Quantity03', 'QuantityInput04', 'QuantityOutput04', 'Quantity04', 'QuantityInput05', 'QuantityOutput05', 'Quantity05', 'QuantityInput06', 'QuantityOutput06', 'Quantity06', 'QuantityInput07', 'QuantityOutput07', 'Quantity07', 'QuantityInput08', 'QuantityOutput08', 'Quantity08', 'QuantityInput09', 'QuantityOutput09', 'Quantity09',];

    List: InvoiceInputInventory[] | undefined;
    ListFilter: InvoiceInputInventory[] | undefined;
    FormData!: InvoiceInputInventory;
    APIURL: string = environment.APIInvoiceURL;
    APIRootURL: string = environment.APIInvoiceRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "InvoiceInputInventory";
    }
   
    GetByCategoryDepartmentIDAndYearAndMonthToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryDepartmentIDAndYearAndMonthToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    CreateAutoAsync() {
        let url = this.APIURL + this.Controller + '/CreateAutoAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

