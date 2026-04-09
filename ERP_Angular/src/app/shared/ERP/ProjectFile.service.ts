import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { ProjectFile } from './ProjectFile.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ProjectFileService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'ProjectTaskID', 'Name', 'Display', 'FileName', 'Save'];

    List: ProjectFile[] | undefined;
    ListFilter: ProjectFile[] | undefined;
    FormData!: ProjectFile;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "ProjectFile";
    }
}

