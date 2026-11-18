import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { NotificationService } from 'src/app/shared/Notification.service';
import { environment } from 'src/environments/environment';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';
import { HRMChamCongTongHopService } from 'src/app/shared/ERP/HRMChamCongTongHop.service';

@Component({
  selector: 'app-hrmcham-cong-tong-hop',
  templateUrl: './hrmcham-cong-tong-hop.component.html',
  styleUrls: ['./hrmcham-cong-tong-hop.component.css']
})
export class HRMChamCongTongHopComponent implements OnInit {
  @ViewChild('TongHopSort') TongHopSort: MatSort;
  @ViewChild('TongHopPaginator') TongHopPaginator: MatPaginator;


  constructor(
    public HRMChamCongTongHopService: HRMChamCongTongHopService,
    public CompanyService: CompanyService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
    public NotificationService: NotificationService,
  ) {
    this.HRMChamCongTongHopService.BaseParameter.CompanyID = 16;
    this.HRMChamCongTongHopService.BaseParameter.CategoryHRMPhongBanID = 0;
    this.HRMChamCongTongHopService.BaseParameter.Date = new Date();
    this.HRMChamCongTongHopService.BaseParameter.DateBegin = this.HRMChamCongTongHopService.BaseParameter.Date;
    this.HRMChamCongTongHopService.BaseParameter.DateEnd = this.HRMChamCongTongHopService.BaseParameter.Date;
    this.HRMChamCongTongHopService.BaseParameter.BaseModel = { IsOffice: this.IsOffice };

  }

  DateChange(value: string) {
    this.SelectedDate = value;
    const selectedDate = value ? new Date(`${value}`) : null; 
    this.HRMChamCongTongHopService.BaseParameter.Date = selectedDate;
    this.HRMChamCongTongHopService.BaseParameter.DateBegin = selectedDate;
    this.HRMChamCongTongHopService.BaseParameter.DateEnd = selectedDate;
  }

  ngOnInit(): void {
    this.CompanyService.ComponentGetByActiveToListAsync(this.HRMChamCongTongHopService);
    this.CategoryHRMPhongBanSearch();
  }

  ngAfterViewInit(): void {
    this.Search();
  }

  CompanyChange() {
    this.HRMChamCongTongHopService.BaseParameter.CategoryHRMPhongBanID = 0;
    this.CategoryHRMPhongBanSearch();
    this.Search();
  }

  CategoryHRMPhongBanSearch() {
    this.CategoryHRMPhongBanService.BaseParameter.CompanyID = this.HRMChamCongTongHopService.BaseParameter.CompanyID;
    this.CategoryHRMPhongBanService.BaseParameter.Active = true;
    this.CategoryHRMPhongBanService.ComponentGetByCompanyIDAndActiveToList(
      this.HRMChamCongTongHopService,
      this.IsOffice
    );
    this.HRMChamCongTongHopService.BaseParameter.BaseModel = {
      ...(this.HRMChamCongTongHopService.BaseParameter.BaseModel || {}),
      IsOffice: this.IsOffice,
    }; 
  }

  SetOffice(isOffice: boolean) {
    this.IsOffice = isOffice;
    this.HRMChamCongTongHopService.BaseParameter.CategoryHRMPhongBanID = 0;
    this.CategoryHRMPhongBanSearch();
    this.Search();
  }

  ExportExcel() {
    this.HRMChamCongTongHopService.BaseParameter.BaseModel = {
      ...(this.HRMChamCongTongHopService.BaseParameter.BaseModel || {}),
      IsOffice: this.IsOffice,
    };
    this.HRMChamCongTongHopService.IsShowLoading = true;
    this.HRMChamCongTongHopService.GetByCompanyID_CategoryHRMPhongBanID_DateAsyncToExcel().subscribe(
      (res) => {
        const url = (res as BaseResult).Message;
        if (url) {
          window.open(url, '_blank');
        } else {
          this.NotificationService.warn(environment.SaveNotSuccess);
        }
      },
      () => this.NotificationService.warn(environment.SaveNotSuccess),
      () => { this.HRMChamCongTongHopService.IsShowLoading = false; },
    );
  }

  Search() {
    this.HRMChamCongTongHopService.Search(this.TongHopSort, this.TongHopPaginator);
  }

  getStatusBadgeStyle(status: string): { [key: string]: string } {
    switch (status) {
      case 'Đã đi làm':
      case 'Đã chấm công':
        return { 'background-color': '#e6f4ea', 'color': '#137333' };
      case 'Nghỉ làm':
      case 'Không có ca kế hoạch':
        return { 'background-color': '#fce8e6', 'color': '#c5221f' };
      case 'Chưa chấm công giờ vào':
      case 'Chưa chấm công giờ ra':
      case 'Thiếu chấm công':
        return { 'background-color': '#fef7e0', 'color': '#b06000' };
      default:
        return { 'background-color': '#f1f3f4', 'color': '#5f6368' };
    }
  }

  getDayOfWeek(value: string | Date): string {
    const day = new Date(value).getDay();
    return ['Chủ nhật', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7'][day];
  }

  isSunday(value: string | Date): boolean {
    return new Date(value).getDay() === 0;
  }

  get IsOffice() { return this.HRMChamCongTongHopService.IsOffice; }
  set IsOffice(value: boolean) { this.HRMChamCongTongHopService.IsOffice = value; }
  get SelectedDate() { return this.HRMChamCongTongHopService.SelectedDate; }
  set SelectedDate(value: string) { this.HRMChamCongTongHopService.SelectedDate = value; }
  get DisplayColumns() { return this.HRMChamCongTongHopService.DisplayColumns; }
  get Summary() { return this.HRMChamCongTongHopService.Summary; }

}
