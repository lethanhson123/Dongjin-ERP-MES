import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { ProjectTaskHistory } from './ProjectTaskHistory.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ProjectTaskHistoryService extends BaseService {
    DisplayColumns001: string[] = ['Save', 'Active', 'ProjectTaskID', 'MembershipCode', 'MembershipName', 'Code', 'DateBegin', 'HourBegin', 'HourEnd', 'Hour', 'Description',];

    List: ProjectTaskHistory[] | undefined;
    ListFilter: ProjectTaskHistory[] | undefined;
    FormData!: ProjectTaskHistory;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "ProjectTaskHistory";
    }
}

