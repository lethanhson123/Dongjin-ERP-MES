import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { BaseService } from '../ERP/Base.service';
import { AnnualLeaveType } from './AnnualLeaveType.model';

@Injectable({
  providedIn: 'root',
})
export class AnnualLeaveTypeService extends BaseService {
  List: AnnualLeaveType[] = [];
  APIURL: string = environment.APIRDURL;
  APIRootURL: string = environment.APIRDRootURL;

  constructor(public httpClient: HttpClient) {
    super(httpClient);
    this.Controller = 'AnnualLeaveType';
  }

  GetAllAndEmptyToListAsync() {
    const url = this.APIURL + this.Controller + '/GetAllAndEmptyToListAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post<BaseResult>(url, formUpload, { headers: this.Headers });
  }

  SaveAnnualLeaveTypeAsync(annualLeaveType: AnnualLeaveType) {
    const url = this.APIURL + this.Controller + '/SaveAsync';
    const formUpload: FormData = new FormData();
    const baseParameter = {
      ...this.BaseParameter,
      BaseModel: annualLeaveType,
    };
    formUpload.append('BaseParameter', JSON.stringify(baseParameter));
    return this.httpClient.post<BaseResult>(url, formUpload, { headers: this.Headers });
  }
}
