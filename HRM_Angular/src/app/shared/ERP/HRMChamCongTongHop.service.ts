import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { environment } from 'src/environments/environment';
import { BaseService } from './Base.service';

@Injectable({ providedIn: 'root' })
export class HRMChamCongTongHopService extends BaseService {
  APIURL = environment.APIHRMURL;
  Controller = 'HRMChamCongTongHop';

  IsOffice = true;
  SelectedDate = this.FormatDate(new Date());
  DisplayColumns = [
    'No', 'MembershipCode', 'MembershipName', 'Status', 'Date', 'DayOfWeek',
    'CategoryHRMCaLamViecName', 'StandardCheckIn', 'StandardCheckOut',
    'CheckIn01', 'CheckOut01', 'StandardHour', 'ActualHour',
    'LateMinutes', 'EarlyLeaveMinutes', 'Workday', 'OvertimeHour', 'OvertimeHourThucTe'
  ];
  Summary = {
    EmployeeCount: 0,
    LateCount: 0,
    OvertimeCount: 0,
    NeedCheckCount: 0,
    AssignedShiftCount: 0,
  };

  FormatDate(date: Date): string {
    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  UpdateSummary(list: any[]) {
    this.Summary = {
      EmployeeCount: list.length,
      LateCount: list.filter(row => Number(row.LateMinutes) > 0).length,
      OvertimeCount: list.filter(row => Number(row.OvertimeHour) > 0).length,
      NeedCheckCount: list.filter(row =>
        row.Status === 'Thiếu chấm công' || row.Status === 'Không có ca kế hoạch'
      ).length,
      AssignedShiftCount: list.filter(row => row.CategoryHRMCaLamViecID).length,
    };
  }


  GetByCompanyID_CategoryHRMPhongBanID_DateAsyncToList() {
    const url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryHRMPhongBanID_DateAsyncToList';
    const form = new FormData();
    form.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, form, { headers: this.Headers });
  }

  GetByCompanyID_CategoryHRMPhongBanID_MonthAsyncToList() {
    const url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryHRMPhongBanID_MonthAsyncToList';
    const form = new FormData();
    form.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, form, { headers: this.Headers });
  }

  SearchMonth(tongHopSort: any, tongHopPaginator: any) {
    this.IsShowLoading = true;
    this.GetByCompanyID_CategoryHRMPhongBanID_MonthAsyncToList().subscribe(
      (res) => {
        const response = res as BaseResult;
        this.List = response.List || [];
        this.DataSource = new MatTableDataSource(this.List);
        this.DataSource.sort = tongHopSort;
        this.DataSource.paginator = tongHopPaginator;
      },
      () => {},
      () => { this.IsShowLoading = false; },
    );
  }

  Search(tongHopSort: any, tongHopPaginator: any) {
    this.BaseParameter.BaseModel = {
      ...(this.BaseParameter.BaseModel || {}),
      IsOffice: this.IsOffice,
    };
    this.IsShowLoading = true;
    this.GetByCompanyID_CategoryHRMPhongBanID_DateAsyncToList().subscribe(
      (res) => {
        const response = res as BaseResult;
        this.List = response.List || [];
        this.UpdateSummary(this.List);
        this.DataSource = new MatTableDataSource(this.List);
        this.DataSource.sort = tongHopSort;
        this.DataSource.paginator = tongHopPaginator;
      },
      () => {},
      () => { this.IsShowLoading = false; },
    );
  }

  GetByCompanyID_CategoryHRMPhongBanID_DateAsyncToExcel() {
    const url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryHRMPhongBanID_DateAsyncToExcel';
    const form = new FormData();
    form.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, form, { headers: this.Headers });
  }
}
