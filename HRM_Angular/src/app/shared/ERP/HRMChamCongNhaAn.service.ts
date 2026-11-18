import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { HRMChamCongNhaAn } from './HRMChamCongNhaAn.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
  providedIn: 'root',
})
export class HRMChamCongNhaAnService extends BaseService {
  DisplayColumns001: string[] = ['No', 'ID', 'MembershipCode', 'Date', 'CategoryHRMSuatAnName'];
  DisplayColumns002: string[] = ['CategoryHRMSuatAnName', 'SortOrder',];

  List: HRMChamCongNhaAn[] | undefined;
  ListFilter: HRMChamCongNhaAn[] | undefined;
  ListFilter01: HRMChamCongNhaAn[] | undefined;
  ListFilter02: HRMChamCongNhaAn[] | undefined;
  ListFilter03: HRMChamCongNhaAn[] | undefined;
  FormData!: HRMChamCongNhaAn;

  constructor(public httpClient: HttpClient) {
    super(httpClient);
    this.Controller = 'HRMChamCongNhaAn';
  }
  SaveByBarcodeAsync() {
    this.Initialization();
    let url = this.APIURL + this.Controller + '/SaveByBarcodeAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }

  SaveByUserNameAsync() {
    this.Initialization();
    let url = this.APIURL + this.Controller + '/SaveByUserNameAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  SaveByUserName20261116Async() {
    this.Initialization();
    let url = this.APIURL + this.Controller + '/SaveByUserName20261116Async';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  UpdateAsync() {
    this.Initialization();
    let url = this.APIURL + this.Controller + '/UpdateAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync() {
    let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync() {
    let url = this.APIURL + this.Controller + '/GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  GetByCompanyID_DateBegin_DateEnd_SearchStringToExcelAsync() {
    let url = this.APIURL + this.Controller + '/GetByCompanyID_DateBegin_DateEnd_SearchStringToExcelAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
}
