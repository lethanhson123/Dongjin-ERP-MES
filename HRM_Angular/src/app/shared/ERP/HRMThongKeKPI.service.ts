import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HRMThongKeKPI } from './HRMThongKeKPI.model';
import { BaseService } from './Base.service';

@Injectable({
    providedIn: 'root'
})
export class HRMThongKeKPIService extends BaseService {
 
   DisplayColumns001: string[] = ['No', 'Team', 'Metric', 'Target', 'Current', 'Weight', 'Rate', 'Note', 'Status', 'Approval', 'ResultNote', 'Save'];
 

    List: HRMThongKeKPI[] | undefined;
    ListFilter: HRMThongKeKPI[] | undefined;
    ListChild: HRMThongKeKPI[] | undefined;
    ListParent: HRMThongKeKPI[] | undefined;
    List02: HRMThongKeKPI[] | undefined;
    List03: HRMThongKeKPI[] | undefined;

    DataSource: any;
    DataSourceFilter: any;
    DataSource01: any;
    DataSource02: any;
    DataSource03: any;

    FormData!: HRMThongKeKPI;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "HRMThongKeKPI";
    }

    GetByCompanyID_CategoryDepartmentID_SearchStringToExcelAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryDepartmentID_SearchStringToExcelAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_CategoryDepartmentID_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryDepartmentID_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}
