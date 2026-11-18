import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { HRMCaLamViecKeHoach } from './HRMCaLamViecKeHoach.model';
import { BaseService } from './Base.service';

@Injectable({
  providedIn: 'root',
})
export class HRMCaLamViecKeHoachService extends BaseService {
  DisplayColumns001: string[] = [
    'No',
    'Active',
    'Save',
    'Date',
    'MembershipCode',
    'CategoryHRMCaLamViecID',
    'CategoryHRMPhongBanName',
    'Quantity01',
    'Quantity02',
    'Quantity03',
    'Quantity04',
    'Note',
  ];

  DisplayColumns002: string[] = [
    'CategoryHRMCaLamViecName',
    'Quantity01',
  ];
  DisplayColumns003: string[] = [
    'CategoryHRMPhongBanName',
    'Quantity01',
  ];
  DisplayColumns004: string[] = [
    'CategoryHRMCaLamViecName',
    'CategoryHRMPhongBanName',
    'Quantity01',
  ];
  DisplayColumns005: string[] = [
    'No',
    'MembershipCode',
    'MembershipName',
    'CategoryHRMCaLamViecName',
    'Quantity01',
    'Quantity02',
    'Quantity03',
    'Quantity04',
    'Date',
  ];
  DisplayColumns006: string[] = [
    'No',
    'MembershipCode',
    'MembershipName',
    'CategoryHRMPhongBanName',
    'CategoryHRMCaLamViecName',
    'Quantity01',
    'Quantity02',
    'Quantity03',
    'Quantity04',
    'Date',
  ];

  FileName: string = environment.InitializationString;
  DataSource: MatTableDataSource<HRMCaLamViecKeHoach> | undefined;
  URLTemplate: string;

  List: HRMCaLamViecKeHoach[] | undefined;
  ListFilter: HRMCaLamViecKeHoach[] | undefined;
  ListCa: HRMCaLamViecKeHoach[] | undefined;
  ListDepartment: HRMCaLamViecKeHoach[] | undefined;
  ListSum: HRMCaLamViecKeHoach[] | undefined;
  Sum: number = 0;
  FormData!: HRMCaLamViecKeHoach;
  APIURL: string = environment.APIHRMURL;
  APIRootURL: string = environment.APIHRMRootURL;
  constructor(public httpClient: HttpClient) {
    super(httpClient);
    this.Controller = 'HRMCaLamViecKeHoach';
    this.URLTemplate = this.APIRootURL + 'Download/HRMTemplateCaLamViecKeHoach.xlsx';
  }
  GetByCompanyIDAndCategoryPhongBanIDAndDateToListAsync() {
    let url = this.APIURL + this.Controller + '/GetByCompanyIDAndCategoryPhongBanIDAndDateToListAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  GetByCompanyIDAndCategoryPhongBanIDAndDateToExcelAsync() {
    let url = this.APIURL + this.Controller + '/GetByCompanyIDAndCategoryPhongBanIDAndDateToExcelAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  Save20260930Async() {
    this.Initialization();
    let url = this.APIURL + this.Controller + '/Save20260930Async';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  SaveListAsync() {
    this.Initialization();
    let url = this.APIURL + this.Controller + '/SaveListAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }

  SaveManyAsync(list: HRMCaLamViecKeHoach[]) {
    this.Initialization();
    const url = this.APIURL + this.Controller + '/SaveListAsync';
    const formUpload: FormData = new FormData();
    const baseParameter = { ...this.BaseParameter, List: list };
    formUpload.append('BaseParameter', JSON.stringify(baseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  ReadExcelToListAsync() {
    this.Initialization();
    let url = this.APIURL + this.Controller + '/ReadExcelToListAsync';
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
  Save20261007Async() {
    this.Initialization();
    let url = this.APIURL + this.Controller + '/Save20261007Async';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
}
