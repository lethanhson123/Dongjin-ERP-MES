import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { MembershipToken } from './MembershipToken.model';
import { BaseService } from './Base.service';
@Injectable({
    providedIn: 'root'
})
export class MembershipTokenService extends BaseService{
 DisplayColumns001: string[] = ['No', 'ID', 'ParentID', 'ParentName', 'CreateDate', 'CreateUserID', 'CreateUserCode', 'CreateUserName', 'UpdateDate', 'UpdateUserID', 'UpdateUserCode', 'UpdateUserName', 'RowVersion', 'SortOrder', 'Active', 'Code', 'Name', 'Display', 'Description', 'Note', 'FileName', 'Token', 'DateCreated', 'Save'];

    List: MembershipToken[] | undefined;
    ListFilter: MembershipToken[] | undefined;
    FormData!: MembershipToken;
    APIURL: string = environment.APIMembershipURL;
    APIRootURL: string = environment.APIMembershipRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "MembershipToken";
    }
    GetAuthenticationByTokenAsync() {
        let url = this.APIURL + this.Controller + '/GetAuthenticationByTokenAsync';
        const params = new HttpParams()
            .set('Token', this.BaseParameter.Token)
        return this.httpClient.get(url, { params }).toPromise();
    }
    AuthenticationByTokenAsync() {
        let url = this.APIURL + this.Controller + '/AuthenticationByTokenAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

