import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { BaseService } from '../ERP/Base.service';
import {
  AnnualLeaveActualWorkdaysCalendarFilter,
  AnnualLeaveActualWorkdaysCalendarResult,
  AnnualLeaveActualWorkdaysEmployeeFilter,
  AnnualLeaveActualWorkdaysListResult,
} from './AnnualLeaveActualWorkdays.model';

@Injectable({
  providedIn: 'root',
})
export class AnnualLeaveActualWorkdaysService extends BaseService {
  APIURL: string = environment.APIRDURL;

  constructor(public httpClient: HttpClient) {
    super(httpClient);
    this.Controller = 'AnnualLeaveActualWorkdays';
  }

  GetEmployeesAsync(filter: AnnualLeaveActualWorkdaysEmployeeFilter) {
    const url = this.APIURL + this.Controller + '/GetEmployeesAsync';
    const formData = new FormData();
    formData.append('BaseParameter', JSON.stringify(filter));
    return this.httpClient.post<AnnualLeaveActualWorkdaysListResult>(url, formData, { headers: this.Headers });
  }

  GetCalendarAsync(filter: AnnualLeaveActualWorkdaysCalendarFilter) {
    const url = this.APIURL + this.Controller + '/GetCalendarAsync';
    const formData = new FormData();
    formData.append('BaseParameter', JSON.stringify(filter));
    return this.httpClient.post<AnnualLeaveActualWorkdaysCalendarResult>(url, formData, { headers: this.Headers });
  }
}
