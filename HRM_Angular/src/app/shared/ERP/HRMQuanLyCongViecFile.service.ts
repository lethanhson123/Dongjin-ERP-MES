import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { HRMQuanLyCongViecFile } from './HRMQuanLyCongViecFile.model';
import { BaseService } from './Base.service';

@Injectable({
    providedIn: 'root'
})
export class HRMQuanLyCongViecFileService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Name', 'FileName', 'UpdateDate', 'UpdateUserCode', 'Save'];

    List: HRMQuanLyCongViecFile[] | undefined;
    ListFilter: HRMQuanLyCongViecFile[] | undefined;
    DataSource!: MatTableDataSource<HRMQuanLyCongViecFile>;
    FormData!: HRMQuanLyCongViecFile;
    FileToUpload: FileList = null;
    ListToUpload: HRMQuanLyCongViecFile[] = [];
    Names: string[] = [];
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "HRMQuanLyCongViecFile";
    }    
}
