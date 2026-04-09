import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ZaloToken } from 'src/app/shared/ERP/ZaloToken.model';
import { BaseService } from './Base.service';
@Injectable({
    providedIn: 'root'
})
export class ZaloTokenService extends BaseService {    
    DisplayColumns002: string[] = ['No', 'ID', 'Date', 'AppID', 'SecretKey', 'URL', 'OARefreshToken', 'OAAccessToken', 'Note', 'Active', 'Save'];
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "ZaloToken";
    }
}

