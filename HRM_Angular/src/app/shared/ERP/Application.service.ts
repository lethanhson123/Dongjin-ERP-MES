import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { Application } from './Application.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ApplicationService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ApplicationName', 'Active'];

    List: Application[] | undefined;
    ListFilter: Application[] | undefined;
    FormData!: Application;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "Application";
    }
    GetByMembershipID_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByMembershipID_ActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

