import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { AnnualLeaveDetails } from './AnnualLeaveDetails.model';
import { BaseService } from '../ERP/Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class AnnualLeaveDetailsService extends BaseService{
    
    DisplayColumns001: string[] = ['No', 'FromDate', 'ToDate', 'LeaveDays', 'LeaveReason', 'LeaveType',];

    List: AnnualLeaveDetails[] | undefined;
    ListFilter: AnnualLeaveDetails[] | undefined;
    FormData!: AnnualLeaveDetails;
    APIURL: string = environment.APIRDURL;
    APIRootURL: string = environment.APIRDRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "AnnualLeaveDetails";
    }
     GetByParentIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByParentIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetAllToListAsync() {
        let url = this.APIURL + this.Controller + '/GetAllToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

