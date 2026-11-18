import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { MembershipHistoryURL } from './MembershipHistoryURL.model';
import { BaseService } from './Base.service';
@Injectable({
    providedIn: 'root'
})
export class MembershipHistoryURLService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'Code', 'ParentName', 'Name', 'Date', 'URL', 'IPAddress', 'IPAddressLocal', 'Type', 'Longitude', 'Latitude', 'Country', 'Region', 'City', 'Token',];

    List: MembershipHistoryURL[] | undefined;
    ListFilter: MembershipHistoryURL[] | undefined;
    FormData!: MembershipHistoryURL;
    APIURL: string = environment.APIMembershipURL;
    APIRootURL: string = environment.APIMembershipRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "MembershipHistoryURL";
    }
    GetByParentName_DateToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByParentName_DateToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByParentName_DateBegin_DateEndToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByParentName_DateBegin_DateEndToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

