import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { ProjectTask } from './ProjectTask.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ProjectTaskService extends BaseService{
 DisplayColumns001: string[] = ['Save', 'Active', 'IsComplete', 'CategoryDepartmentID', 'CategorySystemID', 'ModuleID', 'CategoryTypeID', 'CategoryLevelID', 'Code', 'Name', 'CategoryStatusID', 'Requirement', 'Development', 'Testing', 'Training', 'Handover', 'DateBegin', 'DateEnd', 'Hour', 'Description', 'Display', 'Note',];

    List: ProjectTask[] | undefined;
    ListFilter: ProjectTask[] | undefined;
    FormData!: ProjectTask;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "ProjectTask";
    }
}

