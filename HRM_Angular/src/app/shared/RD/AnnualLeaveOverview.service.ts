import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { BaseService } from '../ERP/Base.service';
import {
  AnnualLeaveOverviewFilter,
  AnnualLeaveOverviewExcelDownload,
  AnnualLeaveOverviewListResult,
} from './AnnualLeaveOverview.model';

@Injectable({
  providedIn: 'root',
})
export class AnnualLeaveOverviewService extends BaseService {
  APIURL: string = environment.APIRDURL;

  constructor(public httpClient: HttpClient) {
    super(httpClient);
    this.Controller = 'AnnualLeaveOverview';
  }

  GetPagedListAsync(filter: AnnualLeaveOverviewFilter) {
    const url = this.APIURL + this.Controller + '/GetPagedListAsync';
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(filter));
    return this.httpClient.post<AnnualLeaveOverviewListResult>(url, formUpload, { headers: this.Headers });
  }

  ExportExcelAsync(filter: AnnualLeaveOverviewFilter): Observable<AnnualLeaveOverviewExcelDownload> {
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
          || `AnnualLeaveOverview_${this.GetCurrentTimestamp()}.xlsx`,
      })),
    );
  }

  private GetFileNameFromContentDisposition(contentDisposition: string | null): string | null {
    if (!contentDisposition) {
      return null;
    }

    const encodedFileName = contentDisposition.match(/filename\*=UTF-8''([^;]+)/i)?.[1];
    if (encodedFileName) {
      return decodeURIComponent(encodedFileName);
    }

    return contentDisposition.match(/filename="?([^";]+)"?/i)?.[1] || null;
  }

  private GetCurrentTimestamp(): string {
    const currentDate = new Date();
    const twoDigit = (value: number) => (`0${value}`).slice(-2);
    return `${currentDate.getFullYear()}${twoDigit(currentDate.getMonth() + 1)}${twoDigit(currentDate.getDate())}`
      + `${twoDigit(currentDate.getHours())}${twoDigit(currentDate.getMinutes())}${twoDigit(currentDate.getSeconds())}`;
  }
}
