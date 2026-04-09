import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { CategoryMenu } from './CategoryMenu.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryMenuService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'ParentID', 'Name', 'Code', 'Display', 'SortOrder', 'Active', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'ParentID', 'NameEnglish', 'NameVietNam', 'NameKorea', 'Code', 'Display', 'SortOrder', 'Active', 'Save'];

    List: CategoryMenu[] | undefined;
    ListFilter: CategoryMenu[] | undefined;
    FormData!: CategoryMenu;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryMenu";
    }
    SearchGetAllAndEmptyToListAsync(sort: MatSort, paginator: MatPaginator) {
        this.IsShowLoading = true;
        this.GetAllAndEmptyToListAsync().subscribe(
            res => {
                this.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
                this.DataSource = new MatTableDataSource(this.List);
                this.DataSource.sort = sort;
                this.DataSource.paginator = paginator;
                this.ListFilter = this.List.filter(o => o.ID > 0);
            },
            err => {
            },
            () => {
                this.IsShowLoading = false;
            }
        );
    }
     GetByMembershipID_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipID_ActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

