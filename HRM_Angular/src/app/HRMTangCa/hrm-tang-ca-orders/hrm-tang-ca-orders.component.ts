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
import { ApprovalOvertimeService } from 'src/app/shared/RD/ApprovalOvertime.service';
import { HRMTangCaFilter, HRMTangCaOrder } from 'src/app/shared/RD/HRMTangCaOrder.model';

@Component({
  selector: 'app-hrm-tang-ca-orders',
  templateUrl: './hrm-tang-ca-orders.component.html',
  styleUrls: ['./hrm-tang-ca-orders.component.css'],
})
export class HRMTangCaOrdersComponent implements OnInit {
  FromDate: string = '';
  ToDate: string = '';
  CompanyID: number = 0;
  CategoryHRMPhongBanID: number = 0;
  RequestDepartmentID: number = 0;
  IsOffice: boolean = false;
  SearchString: string = '';
  OrderPageIndex: number = 0;
  OrderPageSize: number = 20;
  OrderTotalCount: number = 0;
  OrderSortBy: string = 'OvertimeDate';
  OrderSortDirection: 'asc' | 'desc' = 'desc';
  IsOrderExporting: boolean = false;
  OrderColumns: string[] = ['No', 'Id', 'RequestCode', 'OvertimeDate', 'Division', 'RequestDepartment', 'Departments', 'EmployeeCount', 'TotalHours', 'CreatorName', 'CreatedDate', 'ApprovedDate'];
  OrderDataSource: MatTableDataSource<HRMTangCaOrder> = new MatTableDataSource<HRMTangCaOrder>([]);

  constructor(
    public CompanyService: CompanyService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
    public LanguageService: LanguageService,
    public NotificationService: NotificationService,
    public ApprovalOvertimeService: ApprovalOvertimeService,
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
          this.SearchOrders();
        }
      }
    );
  }

  CompanyChange() {
    this.CategoryHRMPhongBanID = 0;
    this.RequestDepartmentID = 0;
    this.OrderPageIndex = 0;
    this.CategoryHRMPhongBanSearch();
  }

  RequestDepartmentChange() {
    this.SearchOrders();
  }

  OfficeChange() {
    this.SearchOrders();
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

  GetDepartmentNamesForDisplay(departmentNames: string): string[] {
    if (!departmentNames) {
      return [];
    }

    return departmentNames
      .split(',')
      .map(departmentName => departmentName.trim())
      .filter(departmentName => departmentName.length > 0);
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

  GetRadApprovalUrl(approvalRequestId: number): string {
    return `${environment.RADWebURL}ApprovalOvertime/Detail/${approvalRequestId}`;
  }

  SearchOrders(resetPage: boolean = true) {
    if (resetPage) {
      this.OrderPageIndex = 0;
    }

    if (this.CompanyID <= 0) {
      this.OrderDataSource.data = [];
      this.OrderTotalCount = 0;
      return;
    }

    const filter = this.BuildOrderFilter();
    filter.Page = this.OrderPageIndex + 1;
    filter.PageSize = this.OrderPageSize;

    this.ApprovalOvertimeService.IsShowLoading = true;
    this.ApprovalOvertimeService.GetApprovedOrdersToListAsync(filter).subscribe(
      result => {
        this.OrderDataSource.data = result.Items || [];
        this.OrderTotalCount = result.TotalCount || 0;
      },
      error => {
        console.error('Không thể tải danh sách đơn tăng ca.', error);
        this.OrderDataSource.data = [];
        this.OrderTotalCount = 0;
        this.ApprovalOvertimeService.IsShowLoading = false;
      },
      () => {
        this.ApprovalOvertimeService.IsShowLoading = false;
      }
    );
  }

  ExportOrders() {
    if (this.CompanyID <= 0 || this.IsOrderExporting) {
      return;
    }

    const companyCode = this.GetSelectedCompanyCode();
    this.IsOrderExporting = true;
    this.ApprovalOvertimeService.ExportApprovedOrdersAsync(this.BuildOrderFilter(), companyCode).subscribe(
      excelDownload => this.DownloadExcelFile(excelDownload.Blob, excelDownload.FileName),
      error => {
        console.error('Không thể xuất danh sách đơn tăng ca ra Excel.', error);
        this.ShowExportError(error.status);
        this.IsOrderExporting = false;
      },
      () => {
        this.IsOrderExporting = false;
      }
    );
  }

  OrderPageChange(pageEvent: PageEvent) {
    this.OrderPageIndex = pageEvent.pageIndex;
    this.OrderPageSize = pageEvent.pageSize;
    this.SearchOrders(false);
  }

  OrderSortChange(sort: Sort) {
    this.OrderSortBy = sort.active || 'OvertimeDate';
    this.OrderSortDirection = sort.direction === 'asc' ? 'asc' : 'desc';
    this.SearchOrders();
  }

  ResetOrderFilters() {
    const currentDate = new Date();
    const firstDayOfCurrentMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
    this.FromDate = this.FormatDate(firstDayOfCurrentMonth);
    this.ToDate = this.FormatDate(currentDate);
    this.CategoryHRMPhongBanID = 0;
    this.RequestDepartmentID = 0;
    this.IsOffice = false;
    this.SearchString = '';
    this.OrderSortBy = 'OvertimeDate';
    this.OrderSortDirection = 'desc';
    this.SearchOrders();
  }

  private BuildOrderFilter(): HRMTangCaFilter {
    const filter = new HRMTangCaFilter();
    filter.CompanyId = this.CompanyID;
    filter.FromDate = this.FromDate || undefined;
    filter.ToDate = this.ToDate || undefined;
    filter.DepartmentId = this.CategoryHRMPhongBanID > 0 ? this.CategoryHRMPhongBanID : undefined;
    filter.RequestDepartmentId = this.RequestDepartmentID > 0 ? this.RequestDepartmentID : undefined;
    filter.IsOffice = this.IsOffice ? true : undefined;
    filter.Keyword = this.SearchString.trim() || undefined;
    filter.SortBy = this.OrderSortBy;
    filter.SortDirection = this.OrderSortDirection;
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
