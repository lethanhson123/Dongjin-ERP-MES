import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { Company } from './Company.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CompanyService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'ParentID', 'Code', 'Name', 'Person', 'Phone', 'Email', 'Address', 'Note', 'SortOrder', 'Active', 'Save'];

    List: Company[] | undefined;
    ListFilter: Company[] | undefined;
    FormData!: Company;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "Company";
    }
    ComponentGetByActiveToListAsync(Service: BaseService) {
        if (this.ListFilter) {
            if (this.ListFilter.length == 0) {
                this.GetByActiveToListAsync().subscribe(
                    res => {
                        this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
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
    ComponentGetByMembershipID_ActiveToListAsync(Service: BaseService) {
        if (this.ListFilter) {
            if (this.ListFilter.length == 0) {
                this.BaseParameter.Active = true;
                this.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
                this.GetByMembershipID_ActiveToListAsync().subscribe(
                    res => {
                        this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
                        // if (this.ListFilter && this.ListFilter.length > 0) {
                        //     Service.BaseParameter.CompanyID = this.ListFilter[0].ID;
                        // }
                    },
                    err => {
                    },
                    () => {
                    }
                );
            }
        }
    }
    GetByMembershipID_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipID_ActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

