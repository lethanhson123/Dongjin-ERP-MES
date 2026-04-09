import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { ProjectTaskMembership } from './ProjectTaskMembership.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ProjectTaskMembershipService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'ProjectTaskID', 'CategoryStatusID', 'CategoryLevelID', 'MembershipCode', 'MembershipName', 'Hour', 'Active', 'Save'];

    List: ProjectTaskMembership[] | undefined;
    ListFilter: ProjectTaskMembership[] | undefined;
    FormData!: ProjectTaskMembership;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "ProjectTaskMembership";
    }
}

