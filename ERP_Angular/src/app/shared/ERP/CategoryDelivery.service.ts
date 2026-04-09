import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryDelivery } from './CategoryDelivery.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryDeliveryService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Code', 'Name', 'Display', 'Note', 'SortOrder', 'Active', 'Save'];

    List: CategoryDelivery[] | undefined;
    ListFilter: CategoryDelivery[] | undefined;
    FormData!: CategoryDelivery;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryDelivery";
    }
}

