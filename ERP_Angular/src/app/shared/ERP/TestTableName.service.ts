import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { TestTableName } from './TestTableName.model';
import { BaseService } from './Base.service';
@Injectable({
    providedIn: 'root'
})
export class TestTableNameService extends BaseService{
 DisplayColumns001: string[] = ['No', 'ID', 'Name', 'Save'];

    List: TestTableName[] | undefined;
    ListFilter: TestTableName[] | undefined;
    FormData!: TestTableName;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "v1/TestTableName";
    }
}

