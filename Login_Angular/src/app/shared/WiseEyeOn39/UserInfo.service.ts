import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { UserInfo } from './UserInfo.model';
import { BaseService } from '../ERP/Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class UserInfoService extends BaseService {
    DisplayColumns001: string[] = ['No', 'UserEnrollNumber', 'UserFullCode', 'UserFullName', 'UserLastName',];


    List: UserInfo[] | undefined;
    ListFilter: UserInfo[] | undefined;
    FormData!: UserInfo;
    APIURL: string = environment.APIWiseEyeOn39URL;
    APIRootURL: string = environment.APIWiseEyeOn39RootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "UserInfo";
    }
    RemoveAndUploadFileAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/RemoveAndUploadFileAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

