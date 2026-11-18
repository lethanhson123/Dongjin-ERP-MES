import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { Company } from 'src/app/shared/ERP/Company.model';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { NotificationService } from 'src/app/shared/Notification.service';
import { environment } from 'src/environments/environment';
import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';
import { LanguageService } from 'src/app/shared/Language.service';
import { ApprovalOvertimeEmployeeService } from 'src/app/shared/RD/ApprovalOvertimeEmployee.service';
import {
  HRMTangCaEmployee,
  HRMTangCaEmployeeFilter,
} from 'src/app/shared/RD/HRMTangCaOrder.model';

@Component({
  selector: 'app-hrm-tang-ca-employees',
  templateUrl: './hrm-tang-ca-employees.component.html',
  styleUrls: ['./hrm-tang-ca-employees.component.css'],
})
export class HRMTangCaEmployeesComponent implements OnInit {
  FromDate: string = '';
  ToDate: string = '';
  CompanyID: number = 0;
  CategoryHRMPhongBanID: number = 0;
  EmployeeSearchString: string = '';
  EmployeeIsOffice: boolean = false;
  EmployeePageIndex: number = 0;
  EmployeePageSize: number = 20;
  EmployeeTotalCount: number = 0;
  EmployeeSortBy: string = 'EmployeeCode';
  EmployeeSortDirection: 'asc' | 'desc' = 'asc';
  IsEmployeeExporting: boolean = false;
  EmployeeColumns: string[] = ['No', 'EmployeeCode', 'EmployeeName', 'Department', 'Division', 'RequestCode', 'RequestDepartment', 'OvertimeDate', 'StartTime', 'EndTime', 'TotalHours', 'CompletedDate', 'Note'];
  EmployeeDataSource: MatTableDataSource<HRMTangCaEmployee> = new MatTableDataSource<HRMTangCaEmployee>([]);

  constructor(
    public CompanyService: CompanyService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
    public LanguageService: LanguageService,
    public NotificationService: NotificationService,
    public ApprovalOvertimeEmployeeService: ApprovalOvertimeEmployeeService,
  ) {}

  ngOnInit(): void {
    const currentDate = new Date();
    const firstDayOfCurrentMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
    this.FromDate = this.FormatDate(firstDayOfCurrentMonth);
    this.ToDate = this.FormatDate(currentDate);
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

        this.CompanyID = this.CompanyService.ListFilter.length > 0
          ? this.CompanyService.ListFilter[0].ID
          : 0;
        this.CategoryHRMPhongBanSearch();
        if (this.CompanyID > 0) {
          this.SearchEmployees();
        }
      }
    );
  }

  EmployeeCompanyChange() {
    this.CategoryHRMPhongBanID = 0;
    this.CategoryHRMPhongBanSearch();
    this.SearchEmployees();
  }

  EmployeeOfficeChange() {
    this.SearchEmployees();
  }

  CategoryHRMPhongBanSearch() {
    this.CategoryHRMPhongBanService.BaseParameter.CompanyID = this.CompanyID;
    this.CategoryHRMPhongBanService.BaseParameter.Active = true;
    this.CategoryHRMPhongBanService.GetByCompanyIDAndActiveToListAsync().subscribe(
      res => {
        this.CategoryHRMPhongBanService.ListFilter = ((res as BaseResult).List || [])
          .sort((firstDepartment, secondDepartment) => firstDepartment.Name > secondDepartment.Name ? 1 : -1);
      }
    );
  }

  GetDepartmentName(department: any): string {
    if (this.LanguageService.Language === 'vi') {
      return department.Description || department.Name || department.Display || '';
    }
    if (this.LanguageService.Language === 'kr') {
      return department.Display || department.Name || department.Description || '';
    }
    return department.Name || department.Display || department.Description || '';
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

  SearchEmployees(resetPage: boolean = true) {
    if (resetPage) {
      this.EmployeePageIndex = 0;
    }

    if (this.CompanyID <= 0) {
      this.EmployeeDataSource.data = [];
      this.EmployeeTotalCount = 0;
      return;
    }

    const filter = this.BuildEmployeeFilter();
    filter.Page = this.EmployeePageIndex + 1;
    filter.PageSize = this.EmployeePageSize;

    this.ApprovalOvertimeEmployeeService.IsShowLoading = true;
    this.ApprovalOvertimeEmployeeService.GetApprovedEmployeesToListAsync(filter).subscribe(
      result => {
        this.EmployeeDataSource.data = result.Items || [];
        this.EmployeeTotalCount = result.TotalCount || 0;
      },
      error => {
        console.error('Không thể tải danh sách tăng ca theo nhân viên.', error);
        this.EmployeeDataSource.data = [];
        this.EmployeeTotalCount = 0;
        this.ApprovalOvertimeEmployeeService.IsShowLoading = false;
      },
      () => {
        this.ApprovalOvertimeEmployeeService.IsShowLoading = false;
      }
    );
  }

  ExportEmployees() {
    if (this.CompanyID <= 0 || this.IsEmployeeExporting) {
      return;
    }

    const companyCode = this.GetSelectedCompanyCode();
    this.IsEmployeeExporting = true;
    this.ApprovalOvertimeEmployeeService.ExportApprovedEmployeesAsync(this.BuildEmployeeFilter(), companyCode).subscribe(
      excelDownload => this.DownloadExcelFile(excelDownload.Blob, excelDownload.FileName),
      error => {
        console.error('Không thể xuất chi tiết tăng ca ra Excel.', error);
        this.ShowExportError(error.status);
        this.IsEmployeeExporting = false;
      },
      () => {
        this.IsEmployeeExporting = false;
      }
    );
  }

  EmployeePageChange(pageEvent: PageEvent) {
    this.EmployeePageIndex = pageEvent.pageIndex;
    this.EmployeePageSize = pageEvent.pageSize;
    this.SearchEmployees(false);
  }

  EmployeeSortChange(sort: Sort) {
    this.EmployeeSortBy = sort.active || 'EmployeeCode';
    this.EmployeeSortDirection = sort.direction === 'desc' ? 'desc' : 'asc';
    this.SearchEmployees();
  }

  ResetEmployeeFilters() {
    const currentDate = new Date();
    const firstDayOfCurrentMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
    this.FromDate = this.FormatDate(firstDayOfCurrentMonth);
    this.ToDate = this.FormatDate(currentDate);
    this.CategoryHRMPhongBanID = 0;
    this.EmployeeIsOffice = false;
    this.EmployeeSearchString = '';
    this.EmployeeSortBy = 'EmployeeCode';
    this.EmployeeSortDirection = 'asc';
    this.SearchEmployees();
  }

  GetRadApprovalUrl(approvalRequestId: number): string {
    return `${environment.RADWebURL}ApprovalOvertime/Detail/${approvalRequestId}`;
  }

  private BuildEmployeeFilter(): HRMTangCaEmployeeFilter {
    const filter = new HRMTangCaEmployeeFilter();
    filter.CompanyId = this.CompanyID;
    filter.FromDate = this.FromDate || undefined;
    filter.ToDate = this.ToDate || undefined;
    filter.DepartmentId = this.CategoryHRMPhongBanID > 0 ? this.CategoryHRMPhongBanID : undefined;
    filter.IsOffice = this.EmployeeIsOffice ? true : undefined;
    filter.Keyword = this.EmployeeSearchString.trim() || undefined;
    filter.SortBy = this.EmployeeSortBy;
    filter.SortDirection = this.EmployeeSortDirection;
    return filter;
  }

  private GetSelectedCompanyCode(): string {
    const selectedCompany = (this.CompanyService.ListFilter || [])
      .find(company => company.ID === this.CompanyID);
    return selectedCompany?.Code || '';
  }

  private DownloadExcelFile(fileBlob: Blob, fileName: string) {
    const downloadUrl = URL.createObjectURL(fileBlob);
    const downloadLink = document.createElement('a');
    downloadLink.href = downloadUrl;
    downloadLink.download = fileName;
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
    window.setTimeout(() => URL.revokeObjectURL(downloadUrl), 0);
  }

  private ShowExportError(statusCode: number) {
    const messageKey = statusCode === 404
      ? 'hrm-tang-ca.exportNoData'
      : 'hrm-tang-ca.exportFailed';
    this.NotificationService.warn(this.LanguageService.TranslateService.instant(messageKey));
  }

  private FormatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
}
