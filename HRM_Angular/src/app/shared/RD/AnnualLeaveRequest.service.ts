import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { AnnualLeaveRequest, AnnualLeaveRequestExcelDownload } from './AnnualLeaveRequest.model';
import { BaseService } from '../ERP/Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})

export class AnnualLeaveRequestService extends BaseService {
  DisplayColumns001: string[] = ['No', 'Id', 'Status', 'RequestCode', 'EmployeeCode', 'EmployeeName', 'LeaveDays', 'SubmitDate', 'CompletedDate', 'Detail'];
  DisplayColumns002: string[] = ['EmployeeCode', 'EmployeeName', 'LeaveDays'];
  DisplayColumns004: string[] = ['No', 'Id', 'RequestCode', 'EmployeeCode', 'EmployeeName', 'DepartmentName', 'LeaveType', 'LeaveDays', 'FromDate', 'ToDate', 'LeaveReason', 'Status', 'CreatedDate'];

  List: AnnualLeaveRequest[] | undefined;
  ListFilter: AnnualLeaveRequest[] | undefined;
  ListSummary: AnnualLeaveRequest[] | undefined;
  DataSourceSummary: any;
  FormData!: AnnualLeaveRequest;
  APIURL: string = environment.APIRDURL;
  APIRootURL: string = environment.APIRDRootURL;
  constructor(public httpClient: HttpClient) {
    super(httpClient);
    this.Controller = 'AnnualLeaveRequest';
  }

  GetAnnualLeaveTypesAsync() {
    const url = this.APIURL + 'AnnualLeaveType/GetByLeaveTypeAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }

  SaveFullAsync(model: any) {
    const url = this.APIURL + this.Controller + '/SaveFullAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(model));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }

  GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync() {
    let url = this.APIURL + this.Controller + '/GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }

  GetByCompanyID_DateBegin_DateEnd_SearchStringToExcelAsync() {
    const url = this.APIURL + this.Controller + '/GetByCompanyID_DateBegin_DateEnd_SearchStringToExcelAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, {
      headers: this.Headers,
      observe: 'response',
      responseType: 'blob',
    }).pipe(
      map((response: HttpResponse<Blob>) => this.CreateExcelDownload(response)),
    );
  }
  GetByDateBegin_DateEnd_SearchStringToListAsync() {
    let url = this.APIURL + this.Controller + '/GetByDateBegin_DateEnd_SearchStringToListAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }

  private CreateExcelDownload(response: HttpResponse<Blob>): AnnualLeaveRequestExcelDownload {
    return {
      Blob: response.body || new Blob(),
      FileName: this.GetFileNameFromContentDisposition(response.headers.get('Content-Disposition'))
        || 'AnnualLeaveRequest.xlsx',
    };
  }

  private GetFileNameFromContentDisposition(contentDisposition: string | null): string | null {
    if (!contentDisposition) {
      return null;
    }

    const encodedFileNameMatch = contentDisposition.match(/filename\*=UTF-8''([^;]+)/i);
    if (encodedFileNameMatch && encodedFileNameMatch[1]) {
      try {
        return decodeURIComponent(encodedFileNameMatch[1].trim().replace(/^"|"$/g, ''));
      } catch {
        return encodedFileNameMatch[1].trim().replace(/^"|"$/g, '');
      }
    }

    const fileNameMatch = contentDisposition.match(/filename="?([^";]+)"?/i);
    return fileNameMatch && fileNameMatch[1] ? fileNameMatch[1].trim() : null;
  }
}
