import { Injectable, ViewChild } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { ProductionOrder } from './ProductionOrder.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ProductionOrderService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Date', 'DateEnd', 'Code', 'Name', 'SupplierName', 'CustomerName', 'Active', 'IsComplete', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'UpdateDate', 'UpdateUserName', 'Date', 'DateEnd', 'Code', 'Name', 'SupplierName', 'CustomerName', 'Active', 'IsComplete', 'Save'];

    List: ProductionOrder[] | undefined;
    ListFilter: ProductionOrder[] | undefined;
    FormData!: ProductionOrder;
    APIURL: string = environment.APIProductionOrderURL;
    APIRootURL: string = environment.APIProductionOrderRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "ProductionOrder";
    }
    SearchGetAllToListAsync(sort: MatSort, paginator: MatPaginator) {
        this.IsShowLoading = true;
        this.GetAllToListAsync().subscribe(
            res => {
                this.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));
                this.DataSource = new MatTableDataSource(this.List);
                this.DataSource.sort = sort;
                this.DataSource.paginator = paginator;
            },
            err => {
            },
            () => {
                this.IsShowLoading = false;
            }
        );
    }
    ComponentGetByActive_IsCompleteToListAsync(Service: BaseService) {
        this.GetByActive_IsCompleteToListAsync().subscribe(
            res => {
                this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            },
            err => {
            },
            () => {
            }
        );
    }
    ComponentGetByMembershipID_Active_IsCompleteToListAsync(Service: BaseService) {
        if (this.ListFilter) {
            if (this.ListFilter.length == 0) {
                this.BaseParameter.Active = true;                
                this.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
                this.GetByMembershipID_Active_IsCompleteToListAsync().subscribe(
                    res => {
                        this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
                    },
                    err => {
                    },
                    () => {
                    }
                );
            }
        }
    }
    GetByActive_IsCompleteToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByActive_IsCompleteToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByMembershipIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByMembershipID_Active_IsCompleteToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipID_Active_IsCompleteToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_Year_Month_ActionToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_Year_Month_ActionToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_DateBegin_DateEnd_SearchString_ActionToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_DateBegin_DateEnd_SearchString_ActionToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

