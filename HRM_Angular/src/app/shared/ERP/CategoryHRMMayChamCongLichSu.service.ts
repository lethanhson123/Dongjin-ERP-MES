import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { CategoryHRMMayChamCongLichSu } from './CategoryHRMMayChamCongLichSu.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryHRMMayChamCongLichSuService extends BaseService{
 DisplayColumns001: string[] = ['No', 'ID', 'Code', 'Name', 'Display', 'Description' ,'Note', 'SortOrder', 'Active', 'Save'];

    List: CategoryHRMMayChamCongLichSu[] | undefined;
    ListFilter: CategoryHRMMayChamCongLichSu[] | undefined;
    FormData!: CategoryHRMMayChamCongLichSu;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "HRMMayChamCong";
    }

    SearchGetAllAndEmptyToListAsync(sort: MatSort, paginator: MatPaginator) {
        this.IsShowLoading = true;
        this.GetAllAndEmptyToListAsync().subscribe(
            (res) => {
                const list = (res as BaseResult).List ?? [];
                this.List = list.sort((a, b) => {
                    const aIsNew = !a.ID || a.ID === 0;
                    const bIsNew = !b.ID || b.ID === 0;

                    if (aIsNew && !bIsNew) return -1;
                    if (!aIsNew && bIsNew) return 1;
                    return (a.SortOrder ?? 0) - (b.SortOrder ?? 0);
                }) as CategoryHRMMayChamCongLichSu[];
                this.ListFilter = this.List;
                this.DataSource = new MatTableDataSource(this.List);
                this.DataSource.sort = sort;
                this.DataSource.paginator = paginator;
            },
            (err) => {},
            () => {
                this.IsShowLoading = false;
            },
        );
    }
}

