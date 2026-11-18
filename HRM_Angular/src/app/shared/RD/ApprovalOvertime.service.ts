import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { BaseService } from '../ERP/Base.service';
import {
  HRMTangCaExcelDownload,
  HRMTangCaFilter,
  HRMTangCaOrderPagedResult,
} from './HRMTangCaOrder.model';

@Injectable({
  providedIn: 'root',
})
export class ApprovalOvertimeService extends BaseService {
  APIURL: string = environment.APIRDURL;

  constructor(public httpClient: HttpClient) {
    super(httpClient);
    this.Controller = 'ApprovalOvertime';
  }

  GetApprovedOrdersToListAsync(filter: HRMTangCaFilter): Observable<HRMTangCaOrderPagedResult> {
    const url = this.APIURL + this.Controller + '/GetApprovedOrdersToListAsync';
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(filter));
    return this.httpClient.post<HRMTangCaOrderPagedResult>(url, formUpload, { headers: this.Headers });
  }

  ExportApprovedOrdersAsync(
    filter: HRMTangCaFilter,
    companyCode: string,
  ): Observable<HRMTangCaExcelDownload> {
    const url = this.APIURL + this.Controller + '/ExportApprovedOrdersAsync';
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(filter));
    formUpload.append('CompanyCode', companyCode || '');

    return this.httpClient.post(url, formUpload, {
      headers: this.Headers,
      observe: 'response',
      responseType: 'blob',
    }).pipe(
      map((response: HttpResponse<Blob>) => this.CreateExcelDownload(response, 'OvertimeOrders.xlsx')),
    );
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
