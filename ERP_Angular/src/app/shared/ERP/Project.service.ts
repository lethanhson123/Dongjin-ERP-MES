import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { Project } from './Project.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ProjectService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'CompanyName', 'ParentName', 'DateBegin', 'DateEnd', 'Hour', 'Code', 'Name', 'Active', 'IsComplete', 'Save'];

    List: Project[] | undefined;
    ListFilter: Project[] | undefined;
    FormData!: Project;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "Project";
    }
    GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

