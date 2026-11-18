import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { HRMChamCongLichSu } from './HRMChamCongLichSu.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { HRMChamCongLichSuSummary } from './HRMChamCongLichSuSummary.model';

@Injectable({
  providedIn: 'root',
})
export class HRMChamCongLichSuService extends BaseService {
  DisplayColumns001: string[] = ['No', 'ID', 'Active', 'IsCaDem', 'IsTangCa', 'CategoryHRMPhongBanName', 'CategoryHRMCaLamViecName', 'MembershipCode', 'Date', 'Save'];
  DisplayColumns002: string[] = ['CategoryHRMCaLamViecName', 'SortOrder',];
  DisplayColumns003: string[] = ['No', 'Active', 'IsCaDem', 'IsTangCa', 'VanTay', 'IP', 'MembershipCode', 'Date', 'CategoryHRMPhongBanName', 'CategoryHRMCaLamViecName',];
  DisplayColumns004: string[] = ['No', 'MembershipCode', 'HasFingerprint', 'CategoryHRMPhongBanName', 'Date', 'DayOfWeek', 'CheckIn01', 'CheckOut01', 'CheckIn02', 'CheckOut02', 'CheckIn03', 'CheckOut03', 'Workday', 'Hour', 'Overtime01', 'Overtime02', 'Overtime03', 'ShiftName', 'Symbol', 'CategoryHRMSuatAnName'];
  SummaryList: HRMChamCongLichSuSummary[] = [];
  SummaryDataSource = new MatTableDataSource<HRMChamCongLichSuSummary>([]);

  List: HRMChamCongLichSu[] | undefined;
  ListFilter: HRMChamCongLichSu[] | undefined;
  FormData!: HRMChamCongLichSu;
  APIURL: string = environment.APIHRMURL;
  APIRootURL: string = environment.APIHRMRootURL;
  APIZKURL: string = environment.APIZKURL;

  constructor(public httpClient: HttpClient) {
    super(httpClient);
    this.Controller = 'HRMChamCongLichSu';
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
  CreateAutoAsync() {
    let url = this.APIURL + this.Controller + '/CreateAutoAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  GetByCompanyID_CategoryHRMPhongBanID_DateBegin_DateEnd_SearchStringToListAsync() {
    let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryHRMPhongBanID_DateBegin_DateEnd_SearchStringToListAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  GetHRMMayChamCongToHRMChamCongLichSuViaZK() {
    let url = this.APIZKURL + 'ZK/GetHRMMayChamCongToHRMChamCongLichSuViaZK';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  GetByCompanyID_CategoryHRMPhongBanID_DateBegin_DateEnd_SearchStringToSummaryAsync() {
    let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryHRMPhongBanID_DateBegin_DateEnd_SearchStringToSummaryAsync'; 
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  GetByCompanyID_CategoryHRMPhongBanID_DateBegin_DateEnd_SearchStringToExcelAsync() {
    let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryHRMPhongBanID_DateBegin_DateEnd_SearchStringToExcelAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  GetByDateBegin_DateEnd_SearchStringToListAsync() {
    let url = this.APIURL + this.Controller + '/GetByDateBegin_DateEnd_SearchStringToListAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
}
