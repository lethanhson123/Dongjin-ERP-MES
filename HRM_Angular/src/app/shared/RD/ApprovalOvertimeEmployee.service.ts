import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { BaseService } from '../ERP/Base.service';
import {
  HRMTangCaEmployeeFilter,
  HRMTangCaEmployeePagedResult,
  HRMTangCaExcelDownload,
  HRMTangCaReport,
  HRMTangCaReportFilter,
} from './HRMTangCaOrder.model';

@Injectable({
  providedIn: 'root',
})
export class ApprovalOvertimeEmployeeService extends BaseService {
  APIURL: string = environment.APIRDURL;

  constructor(public httpClient: HttpClient) {
    super(httpClient);
    this.Controller = 'ApprovalOvertimeEmployee';
  }

  GetApprovedEmployeesToListAsync(filter: HRMTangCaEmployeeFilter): Observable<HRMTangCaEmployeePagedResult> {
    const url = this.APIURL + this.Controller + '/GetApprovedEmployeesToListAsync';
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(filter));
    return this.httpClient.post<HRMTangCaEmployeePagedResult>(url, formUpload, { headers: this.Headers });
  }

  ExportApprovedEmployeesAsync(
    filter: HRMTangCaEmployeeFilter,
    companyCode: string,
  ): Observable<HRMTangCaExcelDownload> {
    const url = this.APIURL + this.Controller + '/ExportApprovedEmployeesAsync';
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(filter));
    formUpload.append('CompanyCode', companyCode || '');

    return this.httpClient.post(url, formUpload, {
      headers: this.Headers,
      observe: 'response',
      responseType: 'blob',
    }).pipe(
      map((response: HttpResponse<Blob>) => this.CreateExcelDownload(response, 'OvertimeEmployees.xlsx')),
    );
  }

  GetApprovedReportAsync(filter: HRMTangCaReportFilter): Observable<HRMTangCaReport> {
    const url = this.APIURL + this.Controller + '/GetApprovedReportAsync';
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(filter));
    return this.httpClient.post<HRMTangCaReport>(url, formUpload, { headers: this.Headers });
  }

  private CreateExcelDownload(
    response: HttpResponse<Blob>,
    fallbackFileName: string,
  ): HRMTangCaExcelDownload {
    return {
      Blob: response.body || new Blob(),
      FileName: this.GetFileNameFromContentDisposition(response.headers.get('Content-Disposition')) || fallbackFileName,
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
