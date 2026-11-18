import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { BaseService } from '../ERP/Base.service';
import {
  AnnualLeaveBalanceHistoryExcelDownload,
  AnnualLeaveBalanceHistoryFilter,
  AnnualLeaveBalanceHistoryListResult,
} from './AnnualLeaveBalanceHistory.model';

@Injectable({
  providedIn: 'root',
})
export class AnnualLeaveBalanceHistoryService extends BaseService {
  APIURL: string = environment.APIRDURL;

  constructor(public httpClient: HttpClient) {
    super(httpClient);
    this.Controller = 'AnnualLeaveBalanceHistory';
  }

  GetPagedListAsync(filter: AnnualLeaveBalanceHistoryFilter): Observable<AnnualLeaveBalanceHistoryListResult> {
    const url = this.APIURL + this.Controller + '/GetPagedListAsync';
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(filter));
    return this.httpClient.post<AnnualLeaveBalanceHistoryListResult>(url, formUpload, { headers: this.Headers });
  }

  ExportExcelAsync(filter: AnnualLeaveBalanceHistoryFilter): Observable<AnnualLeaveBalanceHistoryExcelDownload> {
    const url = this.APIURL + this.Controller + '/ExportExcelAsync';
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(filter));
    return this.httpClient.post(url, formUpload, {
      headers: this.Headers,
      observe: 'response',
      responseType: 'blob',
    }).pipe(
      map((response: HttpResponse<Blob>) => ({
        Blob: response.body || new Blob(),
        FileName: this.GetFileNameFromContentDisposition(response.headers.get('Content-Disposition'))
          || 'AnnualLeaveBalanceHistory.xlsx',
      })),
    );
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
