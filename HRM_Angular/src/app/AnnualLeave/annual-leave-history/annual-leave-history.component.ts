import { AfterViewInit, Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { NotificationService } from 'src/app/shared/Notification.service';
import { LanguageService } from 'src/app/shared/Language.service';
import {
  AnnualLeaveBalanceHistory,
  AnnualLeaveBalanceHistoryFilter,
} from 'src/app/shared/RD/AnnualLeaveBalanceHistory.model';
import { AnnualLeaveBalanceHistoryService } from 'src/app/shared/RD/AnnualLeaveBalanceHistory.service';

@Component({
  selector: 'app-annual-leave-history',
  templateUrl: './annual-leave-history.component.html',
  styleUrls: ['./annual-leave-history.component.css'],
})
export class AnnualLeaveHistoryComponent implements AfterViewInit {
  DisplayColumns: string[] = [
    'No',
    'EmployeeCode',
    'EmployeeName',
    'DepartmentName',
    'ChangeType',
    'OldBalance',
    'ChangeAmount',
    'NewBalance',
    'Reason',
    'CreatedDate',
    'CreatedBy',
  ];

  FromDate = '';
  ToDate = '';
  SelectedCompanyID = 0;
  SelectedDepartmentID = 0;
  SelectedIsOffice: boolean | null = true;
  SearchString = '';
  IsLoading = true;
  NoDataKey = 'hrm-nghi-phep.balance.historyColumns.noData';
  PageIndex = 0;
  PageSize = 20;
  TotalCount = 0;
  DataSource = new MatTableDataSource<AnnualLeaveBalanceHistory>([]);

  constructor(
    public AnnualLeaveBalanceHistoryService: AnnualLeaveBalanceHistoryService,
    public CompanyService: CompanyService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
    public NotificationService: NotificationService,
    public LanguageService: LanguageService,
  ) {
    this.SetDefaultDateRange();
  }

  ngAfterViewInit(): void {
    setTimeout(() => this.CompanySearch());
  }

  CompanySearch(): void {
    this.CompanyService.ListFilter = [];
    this.IsLoading = true;
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      response => {
        const result = response as BaseResult;
        if (result.StatusCode && result.StatusCode >= 400) {
          this.HandleCompanyLoadFailure();
          return;
        }

        this.CompanyService.ListFilter = (result.List || []).sort((firstCompany, secondCompany) =>
          firstCompany.SortOrder > secondCompany.SortOrder ? 1 : -1);
        if (this.CompanyService.ListFilter.length === 0) {
          this.DataSource.data = [];
          this.TotalCount = 0;
          this.NoDataKey = 'hrm-nghi-phep.overview.messages.noCompany';
          this.IsLoading = false;
          return;
        }

        this.SelectedCompanyID = this.CompanyService.ListFilter[0].ID;
        this.CategoryHRMPhongBanSearch();
        this.Search();
      },
      () => this.HandleCompanyLoadFailure(),
    );
  }

  private HandleCompanyLoadFailure(): void {
    this.CompanyService.ListFilter = [];
    this.DataSource.data = [];
    this.TotalCount = 0;
    this.NoDataKey = 'hrm-nghi-phep.overview.messages.loadCompanyFailed';
    this.IsLoading = false;
    this.NotificationService.warn(
      this.LanguageService.TranslateService.instant('hrm-nghi-phep.overview.messages.loadCompanyFailed'),
    );
  }

  CompanyChange(): void {
    this.SelectedDepartmentID = 0;
    this.PageIndex = 0;
    this.CategoryHRMPhongBanSearch();
    this.Search();
  }

  CategoryHRMPhongBanSearch(): void {
    this.CategoryHRMPhongBanService.ListFilter = [];
    this.CategoryHRMPhongBanService.BaseParameter.CompanyID = this.SelectedCompanyID;
    this.CategoryHRMPhongBanService.BaseParameter.Active = true;
    this.CategoryHRMPhongBanService.GetByCompanyIDAndActiveToListAsync().subscribe(
      response => {
        const result = response as BaseResult;
        if (result.StatusCode && result.StatusCode >= 400) {
          this.CategoryHRMPhongBanService.ListFilter = [];
          this.NotificationService.warn(
            this.LanguageService.TranslateService.instant('hrm-nghi-phep.overview.messages.loadDepartmentFailed'),
          );
          return;
        }

        this.CategoryHRMPhongBanService.ListFilter = (result.List || []).sort((firstDepartment, secondDepartment) =>
          firstDepartment.Name > secondDepartment.Name ? 1 : -1);
      },
      () => this.NotificationService.warn(
        this.LanguageService.TranslateService.instant('hrm-nghi-phep.overview.messages.loadDepartmentFailed'),
      ),
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

  Search(resetPage: boolean = true): void {
    if (resetPage) {
      this.PageIndex = 0;
    }
    if (this.SelectedCompanyID <= 0) {
      this.DataSource.data = [];
      this.TotalCount = 0;
      this.NoDataKey = 'hrm-nghi-phep.overview.messages.noCompany';
      this.IsLoading = false;
      return;
    }

    this.IsLoading = true;
    this.AnnualLeaveBalanceHistoryService.GetPagedListAsync(this.BuildHistoryFilter()).subscribe(
      result => {
        if (result.StatusCode && result.StatusCode >= 400) {
          this.DataSource.data = [];
          this.TotalCount = 0;
          this.NoDataKey = 'hrm-nghi-phep.balance.messages.loadHistoryFailed';
          this.NotificationService.warn(
            this.LanguageService.TranslateService.instant('hrm-nghi-phep.balance.messages.loadHistoryFailed'),
          );
          return;
        }

        this.DataSource.data = result.List || [];
        this.TotalCount = result.Count || 0;
        this.NoDataKey = 'hrm-nghi-phep.balance.historyColumns.noData';
      },
      () => {
        this.DataSource.data = [];
        this.TotalCount = 0;
        this.NoDataKey = 'hrm-nghi-phep.balance.messages.loadHistoryFailed';
        this.NotificationService.warn(
          this.LanguageService.TranslateService.instant('hrm-nghi-phep.balance.messages.loadHistoryFailed'),
        );
        this.IsLoading = false;
      },
      () => this.IsLoading = false,
    );
  }

  PageChange(pageEvent: PageEvent): void {
    this.PageIndex = pageEvent.pageIndex;
    this.PageSize = pageEvent.pageSize;
    this.Search(false);
  }

  ExportExcel(): void {
    if (this.SelectedCompanyID <= 0 || this.IsLoading) {
      return;
    }

    this.IsLoading = true;
    this.AnnualLeaveBalanceHistoryService.ExportExcelAsync(this.BuildHistoryFilter()).subscribe(
      file => this.DownloadFile(file.Blob, file.FileName),
      () => this.NotificationService.warn(
        this.LanguageService.TranslateService.instant('hrm-nghi-phep.requests.messages.exportFailed'),
      ),
      () => this.IsLoading = false,
    );
  }

  ResetFilters(): void {
    this.SetDefaultDateRange();
    this.PageSize = 20;
    this.SelectedIsOffice = true;
    this.SelectedDepartmentID = 0;
    this.SearchString = '';

    if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
      this.SelectedCompanyID = this.CompanyService.ListFilter[0].ID;
      this.CategoryHRMPhongBanSearch();
    }
    this.Search();
  }

  private FormatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  private SetDefaultDateRange(): void {
    const today = new Date();
    this.FromDate = this.FormatDate(new Date(today.getFullYear(), today.getMonth(), 1));
    this.ToDate = this.FormatDate(today);
  }

  private BuildHistoryFilter(): AnnualLeaveBalanceHistoryFilter {
    return {
      CompanyID: this.SelectedCompanyID,
      FromDate: this.FromDate || undefined,
      ToDate: this.ToDate || undefined,
      DepartmentID: this.SelectedDepartmentID > 0 ? this.SelectedDepartmentID : undefined,
      IsOffice: this.SelectedIsOffice === null ? undefined : this.SelectedIsOffice,
      SearchString: this.SearchString.trim(),
      PageIndex: this.PageIndex,
      PageSize: this.PageSize,
    };
  }

  private DownloadFile(fileBlob: Blob, fileName: string): void {
    const downloadUrl = window.URL.createObjectURL(fileBlob);
    const downloadLink = document.createElement('a');
    downloadLink.href = downloadUrl;
    downloadLink.download = fileName;
    downloadLink.click();
    setTimeout(() => window.URL.revokeObjectURL(downloadUrl), 100);
  }
}
