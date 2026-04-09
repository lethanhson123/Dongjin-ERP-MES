import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryRack } from './CategoryRack.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryRackService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'ParentID', 'ParentName', 'CreateDate', 'CreateUserID', 'CreateUserCode', 'CreateUserName', 'UpdateDate', 'UpdateUserID', 'UpdateUserCode', 'UpdateUserName', 'RowVersion', 'SortOrder', 'Active', 'Code', 'Name', 'Display', 'Description', 'Note', 'FileName', 'CompanyID', 'CompanyName', 'Count', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'CompanyName', 'Code', 'Name', 'Display', 'Note', 'Count', 'SortOrder', 'Active', 'Save'];
    DisplayColumns003: string[] = ['No', 'ID', 'CompanyName', 'ParentName', 'Code', 'Name', 'Display', 'Count', 'SortOrder', 'Active', 'Save'];

    List: CategoryRack[] | undefined;
    ListFilter: CategoryRack[] | undefined;
    FormData!: CategoryRack;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryRack";
    }

    ComponentGetByActiveToListAsync(Service: BaseService) {
        if (this.ListFilter) {
            if (this.ListFilter.length == 0) {
                this.GetByActiveToListAsync().subscribe(
                    res => {
                        this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder < b.SortOrder ? 1 : -1));
                        if (this.ListFilter) {
                            if (this.ListFilter.length > 100) {
                                this.ListFilter01 = this.ListFilter.slice(0, 20);
                                this.ListFilter02 = this.ListFilter.slice(0, 20);
                                this.ListFilter03 = this.ListFilter.slice(0, 20);
                                let List = this.ListFilter.filter(o => o.ID == this.BaseParameter.ID);
                                if (List) {
                                    if (List.length > 0) {
                                        this.ListFilter01.push(List[0]);
                                        this.ListFilter02.push(List[0]);
                                        this.ListFilter03.push(List[0]);
                                    }
                                }
                            }
                            else {
                                this.ListFilter01 = this.ListFilter;
                                this.ListFilter02 = this.ListFilter;
                                this.ListFilter03 = this.ListFilter;
                            }
                        }
                    },
                    err => {
                    },
                    () => {
                    }
                );
            }
        }
    }
    ComponentGetByParentIDAndCompanyIDAndActiveToListAsync(Service: BaseService) {
        this.GetByParentIDAndCompanyIDAndActiveToListAsync().subscribe(
            res => {
                this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.Name > b.Name ? 1 : -1));
                if (this.ListFilter) {
                    if (this.ListFilter.length > 100) {
                        this.ListFilter01 = this.ListFilter.slice(0, 20);
                        this.ListFilter02 = this.ListFilter.slice(0, 20);
                        this.ListFilter03 = this.ListFilter.slice(0, 20);
                        let List = this.ListFilter.filter(o => o.ID == this.BaseParameter.ID);
                        if (List) {
                            if (List.length > 0) {
                                this.ListFilter01.push(List[0]);
                                this.ListFilter02.push(List[0]);
                                this.ListFilter03.push(List[0]);
                            }
                        }
                    }
                    else {
                        this.ListFilter01 = this.ListFilter;
                        this.ListFilter02 = this.ListFilter;
                        this.ListFilter03 = this.ListFilter;
                    }
                }
            },
            err => {
            },
            () => {
            }
        );
    }
    GetByParentIDAndCompanyIDAndActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByParentIDAndCompanyIDAndActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }

}

