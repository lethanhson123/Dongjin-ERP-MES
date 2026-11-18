import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { HRMThongKeKPIFile } from './HRMThongKeKPIFile.model';
import { BaseService } from './Base.service';

@Injectable({
    providedIn: 'root'
})
export class HRMThongKeKPIFileService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Name', 'FileName', 'Note', 'UpdateDate', 'UpdateUserCode', 'Save'];

    List: HRMThongKeKPIFile[] | undefined;
    ListFilter: HRMThongKeKPIFile[] | undefined;
    DataSource!: MatTableDataSource<HRMThongKeKPIFile>;
    FormData!: HRMThongKeKPIFile;
    FileToUpload: FileList = null;
    ListToUpload: HRMThongKeKPIFile[] = [];
    Names: string[] = [];
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "HRMThongKeKPIFile";
    }
}
