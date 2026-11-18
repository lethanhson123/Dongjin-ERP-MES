import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryHRMCaLamViec } from './CategoryHRMCaLamViec.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryHRMCaLamViecService extends BaseService{
 DisplayColumns001: string[] = ['No', 'ID', 'Code', 'Name','Quantity01', 'Quantity02','Quantity03', 'Quantity04' ,'SortOrder', 'IsTangCa', 'IsCaDem', 'IsOffice', 'Active', 'Save'];

    List: CategoryHRMCaLamViec[] | undefined;
    ListFilter: CategoryHRMCaLamViec[] | undefined;
    FormData!: CategoryHRMCaLamViec;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryHRMCaLamViec";
    }

    SetOfficeFilter(isOffice: boolean) {
        this.BaseParameter.Active001 = isOffice;
        if (this.DataSource) {
            this.DataSource.filter = isOffice ? 'true' : 'false';
        }
    }

    SearchAllByOffice(sort: any, paginator: any, isOffice: boolean) {
        this.SetOfficeFilter(isOffice);
        this.IsShowLoading = true;
        const formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify({
            ...this.BaseParameter,
            Active: true,
            Active001: isOffice,
            CompanyID: this.BaseParameter.CompanyID,
            BaseModel: { ...(this.BaseParameter.BaseModel || {}), IsOffice: isOffice },
        }));
        this.httpClient.post(
            this.APIURL + this.Controller + '/GetByActiveAndOfficeToListAsync',
            formUpload,
            { headers: this.Headers },
        ).subscribe(
            (res) => {
                const list = ((res as BaseResult).List || []) as CategoryHRMCaLamViec[];
                const newItem = new CategoryHRMCaLamViec();
                newItem.ID = 0;
                newItem.CompanyID = this.BaseParameter.CompanyID;
                newItem.IsOffice = isOffice;
                newItem.Active = true;
                list.unshift(newItem);

                this.List = list;
                this.ListFilter = list;
                this.DataSource = new MatTableDataSource(list);

                this.DataSource.sort = sort;
                this.DataSource.paginator = paginator;
            },
            () => {},
            () => { this.IsShowLoading = false; },
        );
    }
}

