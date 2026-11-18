import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { HRMChamCongTongHopService } from 'src/app/shared/ERP/HRMChamCongTongHop.service';

@Component({
  selector: 'app-hrm-cham-cong-tong-hop001',
  templateUrl: './hrm-cham-cong-tong-hop001.component.html',
  styleUrls: ['./hrm-cham-cong-tong-hop001.component.css']
})
export class HrmChamCongTongHop001Component implements OnInit {
  @ViewChild('TongHopSort') TongHopSort: MatSort;
  @ViewChild('TongHopPaginator') TongHopPaginator: MatPaginator;

  UserName = localStorage.getItem(environment.UserName) || '';
  SelectedMonthNumber: number;
  SelectedYear: number;
  MonthOptions = Array.from({ length: 12 }, (_, index) => index + 1);
  YearOptions: number[];

  constructor(public HRMChamCongTongHopService: HRMChamCongTongHopService) {
    const today = new Date();
    this.HRMChamCongTongHopService.BaseParameter.CompanyID = Number(localStorage.getItem(environment.CompanyID));
    this.HRMChamCongTongHopService.BaseParameter.CategoryHRMPhongBanID = 0;
    this.HRMChamCongTongHopService.BaseParameter.SearchString = this.UserName;
    this.HRMChamCongTongHopService.BaseParameter.BaseModel = {
      ...(this.HRMChamCongTongHopService.BaseParameter.BaseModel || {}),
      IsOffice: true,
    };
    this.SelectedMonthNumber = today.getMonth() + 1;
    this.SelectedYear = today.getFullYear();
    this.YearOptions = Array.from({ length: 11 }, (_, index) => today.getFullYear() - 5 + index);
    this.SetMonthParameter();
  }

  ngOnInit(): void { }

  ngAfterViewInit(): void {
    this.Search();
  }

  MonthChange(value: number | string): void {
    this.SelectedMonthNumber = Number(value);
    this.SetMonthParameter();
  }

  YearChange(value: number | string): void {
    this.SelectedYear = Number(value);
    this.SetMonthParameter();
  }

  private SetMonthParameter(): void {
    this.HRMChamCongTongHopService.BaseParameter.Year = this.SelectedYear;
    this.HRMChamCongTongHopService.BaseParameter.Month = this.SelectedMonthNumber;
  }

  Search(): void {
    // Luôn lấy username từ localStorage, không cho người dùng đổi đối tượng tra cứu.
    this.HRMChamCongTongHopService.BaseParameter.SearchString = this.UserName;
    this.HRMChamCongTongHopService.SearchMonth(this.TongHopSort, this.TongHopPaginator);
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

  get DisplayColumns(): string[] { return this.HRMChamCongTongHopService.DisplayColumns; }
}
