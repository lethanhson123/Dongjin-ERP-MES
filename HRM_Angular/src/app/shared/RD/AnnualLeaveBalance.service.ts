import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import {
  AnnualLeaveBalanceExcelDownload,
  AnnualLeaveBalanceFilter,
  AnnualLeaveBalanceImportResult,
  AnnualLeaveBalanceListResult,
  AnnualLeaveBalanceAdjust,
  AnnualLeaveBalanceSyncApiResult,
  AnnualLeaveBalanceSyncFilter,
} from './AnnualLeaveBalance.model';
import { BaseService } from '../ERP/Base.service';

@Injectable({
  providedIn: 'root',
})
export class AnnualLeaveBalanceService extends BaseService {
  APIURL: string = environment.APIRDURL;

  constructor(public httpClient: HttpClient) {
    super(httpClient);
    this.Controller = 'AnnualLeaveBalance';
  }

  GetPagedListAsync(filter: AnnualLeaveBalanceFilter): Observable<AnnualLeaveBalanceListResult> {
    const url = this.APIURL + this.Controller + '/GetPagedListAsync';
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(filter));
    return this.httpClient.post<AnnualLeaveBalanceListResult>(url, formUpload, { headers: this.Headers });
  }

  AdjustSingleAsync(request: AnnualLeaveBalanceAdjust): Observable<AnnualLeaveBalanceListResult> {
    const url = this.APIURL + this.Controller + '/SaveSingleAsync';
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(request));
    return this.httpClient.post<AnnualLeaveBalanceListResult>(url, formUpload, { headers: this.Headers });
  }

  SyncBalanceAsync(filter: AnnualLeaveBalanceSyncFilter): Observable<AnnualLeaveBalanceSyncApiResult> {
    const url = this.APIURL + this.Controller + '/SyncAsync';
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(filter));
    return this.httpClient.post<AnnualLeaveBalanceSyncApiResult>(url, formUpload, { headers: this.Headers });
  }

  DownloadTemplateAsync(filter: AnnualLeaveBalanceFilter): Observable<AnnualLeaveBalanceExcelDownload> {
    const url = this.APIURL + this.Controller + '/DownloadTemplateAsync';
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(filter));
    return this.httpClient.post(url, formUpload, {
      headers: this.Headers,
      observe: 'response',
      responseType: 'blob',
    }).pipe(
      map((response: HttpResponse<Blob>) => this.CreateExcelDownload(response, 'AnnualLeaveBalance_Template.xlsx')),
    );
  }

  ImportExcelAsync(
    file: File,
    year: number,
    openingDate: string,
    reason?: string,
  ): Observable<AnnualLeaveBalanceImportResult> {
    const url = this.APIURL + this.Controller + '/ImportExcelAsync';
    const formUpload = new FormData();
    formUpload.append('file', file, file.name);
    formUpload.append('year', year.toString());
    formUpload.append('openingDate', openingDate);
    formUpload.append('reason', reason || '');
    return this.httpClient.post<AnnualLeaveBalanceImportResult>(url, formUpload, { headers: this.Headers });
  }

  ExportExcelAsync(filter: AnnualLeaveBalanceFilter): Observable<AnnualLeaveBalanceExcelDownload> {
    const url = this.APIURL + this.Controller + '/ExportExcelAsync';
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(filter));
    return this.httpClient.post(url, formUpload, {
      headers: this.Headers,
      observe: 'response',
      responseType: 'blob',
    }).pipe(
      map((response: HttpResponse<Blob>) => this.CreateExcelDownload(response, 'AnnualLeaveBalance.xlsx')),
    );
  }

  private CreateExcelDownload(
    response: HttpResponse<Blob>,
    fallbackFileName: string,
  ): AnnualLeaveBalanceExcelDownload {
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
