import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryHRMPhongBan } from './CategoryHRMPhongBan.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryHRMPhongBanService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'ParentID', 'ParentName', 'Code', 'Name', 'Display', 'Description', 'SortOrder', 'IsOffice', 'Active', 'Save'];

    List: CategoryHRMPhongBan[] | undefined;
    ListFilter: CategoryHRMPhongBan[] | undefined;
    CategoryHRMPhongBanSearchString: string = environment.InitializationString;
    ListCategoryHRMPhongBanFiltered: CategoryHRMPhongBan[] = [];
    FormData!: CategoryHRMPhongBan;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryHRMPhongBan";
    }

    CategoryHRMPhongBanFilter() {
        this.ListCategoryHRMPhongBanFiltered = this.List || [];
        if (!this.CategoryHRMPhongBanSearchString || this.CategoryHRMPhongBanSearchString.length == 0) {
            return;
        }
        const SearchString = this.CategoryHRMPhongBanSearchString.trim().toLowerCase();
        this.ListCategoryHRMPhongBanFiltered = (this.List || []).filter((o) => o.Name && o.Name.toLowerCase().includes(SearchString));
        if (this.ListCategoryHRMPhongBanFiltered.length == 0) {
            this.ListCategoryHRMPhongBanFiltered = (this.List || []).filter((o) => o.Display && o.Display.toLowerCase().includes(SearchString));
        }
        if (this.ListCategoryHRMPhongBanFiltered.length == 0) {
            this.ListCategoryHRMPhongBanFiltered = (this.List || []).filter((o) => o.Code && o.Code.toLowerCase().includes(SearchString));
        }
    }

    ComponentGetByCompanyIDAndActiveToList(Service: BaseService, isOffice?: boolean) {
        this.BaseParameter.Active = true;
        this.GetByCompanyIDAndActiveToListAsync().subscribe(
            res => {
                const list = ((res as BaseResult).List || [])
                    .filter(item => isOffice === undefined || item.IsOffice === isOffice)
                    .sort((a, b) => (a.Name > b.Name ? 1 : -1));
                this.ListFilter = list;
                this.List = list;
                this.ListFilter01 = list;
                this.ListFilter02 = list;
                this.ListFilter03 = list;
            },
            err => {
            },
            () => {
            }
        );
    }

    ComponentGetByCompanyIDAndEmptyToListAsync(CompanyID: number) {
        this.BaseParameter.CompanyID = CompanyID;
        this.GetByCompanyIDAndEmptyToListAsync().subscribe(
            res => {
                this.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            },
            err => {
            },
            () => {
            }
        );
    }
}
