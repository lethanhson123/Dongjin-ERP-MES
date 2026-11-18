import { Component, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { Company } from 'src/app/shared/ERP/Company.model';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { NotificationService } from 'src/app/shared/Notification.service';
import { environment } from 'src/environments/environment';
import { LanguageService } from 'src/app/shared/Language.service';
import { ApprovalOvertimeEmployeeService } from 'src/app/shared/RD/ApprovalOvertimeEmployee.service';
import {
  HRMTangCaReport,
  HRMTangCaReportFilter,
} from 'src/app/shared/RD/HRMTangCaOrder.model';

@Component({
  selector: 'app-hrm-tang-ca-reports',
  templateUrl: './hrm-tang-ca-reports.component.html',
  styleUrls: ['./hrm-tang-ca-reports.component.css'],
})
export class HRMTangCaReportsComponent implements OnInit {
  ReportFromDate: string = '';
  ReportToDate: string = '';
  ReportCompanyID: number = 0;
  ReportData: HRMTangCaReport = this.CreateEmptyReport();
  DepartmentChartLabels: Label[] = [];
  DepartmentChartData: ChartDataSets[] = [{ data: [], label: 'Overtime Hours', backgroundColor: '#26a69a' }];
  RequestDepartmentChartLabels: Label[] = [];
  RequestDepartmentChartData: ChartDataSets[] = [{ data: [], label: 'Overtime Hours', backgroundColor: '#5c6bc0' }];
  DivisionChartLabels: Label[] = [];
  DivisionChartData: ChartDataSets[] = [{ data: [], backgroundColor: ['#42a5f5', '#ffb74d'] }];
  TrendChartLabels: Label[] = [];
  TrendChartData: ChartDataSets[] = [{
    data: [],
    label: 'Overtime Hours',
    fill: false,
    borderColor: '#0288d1',
    backgroundColor: '#0288d1',
  }];
  DepartmentChartType: ChartType = 'bar';
  DivisionChartType: ChartType = 'doughnut';
  TrendChartType: ChartType = 'line';
  DepartmentChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    scales: { yAxes: [{ ticks: { beginAtZero: true } }] },
  };
  DivisionChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    legend: { position: 'bottom' },
  };
  TrendChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    scales: { yAxes: [{ ticks: { beginAtZero: true } }] },
  };

  constructor(
    public CompanyService: CompanyService,
    public LanguageService: LanguageService,
    public NotificationService: NotificationService,
    public ApprovalOvertimeEmployeeService: ApprovalOvertimeEmployeeService,
  ) {}

  ngOnInit(): void {
    const currentDate = new Date();
    const firstDayOfCurrentMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
    this.ReportFromDate = this.FormatDate(firstDayOfCurrentMonth);
    this.ReportToDate = this.FormatDate(currentDate);
    this.CompanySearch();
  }

  CompanySearch() {
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      res => {
        const companyList = ((res as BaseResult).List || []) as Company[];
        this.CompanyService.ListFilter = companyList.sort((firstCompany, secondCompany) =>
          firstCompany.SortOrder > secondCompany.SortOrder ? 1 : -1
        );

        this.ReportCompanyID = this.CompanyService.ListFilter.length > 0
          ? this.CompanyService.ListFilter[0].ID
          : 0;
        this.SearchReport();
      }
    );
  }

  ReportCompanyChange() {
    this.SearchReport();
  }

  SearchReport() {
    if (this.ReportCompanyID <= 0) {
      this.ReportData = this.CreateEmptyReport();
      this.UpdateReportCharts();
      return;
    }

    const filter = new HRMTangCaReportFilter();
    filter.CompanyId = this.ReportCompanyID;
    filter.FromDate = this.ReportFromDate || undefined;
    filter.ToDate = this.ReportToDate || undefined;

    this.ApprovalOvertimeEmployeeService.IsShowLoading = true;
    this.ApprovalOvertimeEmployeeService.GetApprovedReportAsync(filter).subscribe(
      report => {
        this.ReportData = report || this.CreateEmptyReport();
        this.UpdateReportCharts();
      },
      error => {
        console.error('Không thể tải báo cáo tăng ca.', error);
        this.ReportData = this.CreateEmptyReport();
        this.UpdateReportCharts();
        this.ApprovalOvertimeEmployeeService.IsShowLoading = false;
      },
      () => {
        this.ApprovalOvertimeEmployeeService.IsShowLoading = false;
      }
    );
  }

  ResetReportFilters() {
    const currentDate = new Date();
    const firstDayOfCurrentMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
    this.ReportFromDate = this.FormatDate(firstDayOfCurrentMonth);
    this.ReportToDate = this.FormatDate(currentDate);
    this.ReportCompanyID = this.CompanyService.ListFilter.length > 0
      ? this.CompanyService.ListFilter[0].ID
      : 0;
    this.SearchReport();
  }

  FormatHoursToTime(decimalHours: number | null | undefined): string {
    if (decimalHours == null || !Number.isFinite(decimalHours) || decimalHours <= 0) {
      return '00:00';
    }

    const totalMinutes = Math.round(decimalHours * 60);
    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;
    return `${hours}:${String(minutes).padStart(2, '0')}`;
  }

  FormatShortDate(dateValue: string): string {
    const [year, month, day] = dateValue.substring(0, 10).split('-');
    return year && month && day ? `${day}/${month}` : dateValue;
  }

  private UpdateReportCharts() {
    this.DepartmentChartLabels = this.ReportData.DepartmentHours.map(department => department.DepartmentName || '—');
    this.DepartmentChartData = [{
      data: this.ReportData.DepartmentHours.map(department => department.TotalHours),
      label: 'Overtime Hours',
      backgroundColor: '#26a69a',
    }];
    this.RequestDepartmentChartLabels = this.ReportData.RequestDepartmentHours
      .map(requestDepartment => requestDepartment.RequestDepartmentName || '—');
    this.RequestDepartmentChartData = [{
      data: this.ReportData.RequestDepartmentHours.map(requestDepartment => requestDepartment.TotalHours),
      label: 'Overtime Hours',
      backgroundColor: '#5c6bc0',
    }];

    this.DivisionChartLabels = this.ReportData.DivisionHours.map(division =>
      this.GetDivisionChartLabel(division.IsOffice)
    );
    this.TrendChartLabels = this.ReportData.DailyTrends.map(item => this.FormatShortDate(item.Date));
    this.TrendChartData = [{
      data: this.ReportData.DailyTrends.map(item => item.TotalHours),
      label: 'Overtime Hours',
      fill: false,
      borderColor: '#0288d1',
      backgroundColor: '#0288d1',
    }];
    this.DivisionChartData = [{
      data: this.ReportData.DivisionHours.map(division => division.TotalHours),
      backgroundColor: ['#42a5f5', '#ffb74d'],
    }];
  }

  private CreateEmptyReport(): HRMTangCaReport {
    return {
      TotalHours: 0,
      EmployeeCount: 0,
      AverageHoursPerEmployee: 0,
      TopDepartment: '',
      TopDepartmentHours: 0,
      DepartmentHours: [],
      RequestDepartmentHours: [],
      DivisionHours: [],
      DailyTrends: [],
    };
  }

  private GetDivisionChartLabel(isOffice: boolean): string {
    if (this.LanguageService.Language === 'vi') {
      return isOffice ? 'Văn phòng' : 'Sản xuất';
    }
    if (this.LanguageService.Language === 'kr') {
      return isOffice ? '사무직' : '생산직';
    }
    return isOffice ? 'Office' : 'Production';
  }

  private FormatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
}
