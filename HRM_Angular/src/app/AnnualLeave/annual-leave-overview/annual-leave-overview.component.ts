import { AfterViewInit, Component } from '@angular/core';
import { MAT_DATE_FORMATS } from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { NotificationService } from 'src/app/shared/Notification.service';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';
import { LanguageService } from 'src/app/shared/Language.service';
import { AnnualLeaveBalanceService } from 'src/app/shared/RD/AnnualLeaveBalance.service';
import {
  AnnualLeaveBalanceSyncApiResult,
  AnnualLeaveBalanceSyncFilter,
} from 'src/app/shared/RD/AnnualLeaveBalance.model';
import { AnnualLeaveOverview, AnnualLeaveOverviewFilter } from 'src/app/shared/RD/AnnualLeaveOverview.model';
import { AnnualLeaveOverviewService } from 'src/app/shared/RD/AnnualLeaveOverview.service';

const ANNUAL_LEAVE_OVERVIEW_DATE_FORMATS = {
  parse: {
    dateInput: null,
  },
  display: {
    dateInput: { year: 'numeric' },
    monthYearLabel: { year: 'numeric' },
    dateA11yLabel: { year: 'numeric', month: 'long', day: 'numeric' },
    monthYearA11yLabel: { year: 'numeric', month: 'long' },
  },
};

@Component({
  selector: 'app-annual-leave-overview',
  templateUrl: './annual-leave-overview.component.html',
  styleUrls: ['./annual-leave-overview.component.css'],
  providers: [{ provide: MAT_DATE_FORMATS, useValue: ANNUAL_LEAVE_OVERVIEW_DATE_FORMATS }],
})
export class AnnualLeaveOverviewComponent implements AfterViewInit {
  DisplayColumns: string[] = [
    'No',
    'EmployeeCode',
    'EmployeeName',
    'DepartmentName',
    'OpeningDate',
    'OpeningBalance',
    'MonthlyAccruedDays',
    'SeniorityLeaveDays',
    'UsedDays',
    'ManualAdjustmentDays',
    'CurrentBalance',
  ];

  CurrentYear = new Date().getFullYear();
  SelectedYear = this.CurrentYear;
  SelectedYearDate = new Date(this.CurrentYear, 0, 1);
  SelectedCompanyID = 0;
  SelectedDepartmentID = 0;
  SelectedIsOffice: boolean | null = true;
  SearchString = '';
  IsLoading = true;
  IsSyncing = false;
  IsExporting = false;
  NoDataKey = 'hrm-nghi-phep.overview.messages.noData';
  PageIndex = 0;
  PageSize = 20;
  TotalCount = 0;
  SortBy = 'UsedDays';
  SortDirection: 'asc' | 'desc' = 'desc';
  DataSource = new MatTableDataSource<AnnualLeaveOverview>([]);

  constructor(
    public AnnualLeaveOverviewService: AnnualLeaveOverviewService,
    public AnnualLeaveBalanceService: AnnualLeaveBalanceService,
    public CompanyService: CompanyService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
    public LanguageService: LanguageService,
    public NotificationService: NotificationService,
  ) {}

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.CompanySearch();
    });
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

        if (this.CompanyService.ListFilter.length > 0) {
          this.SelectedCompanyID = this.CompanyService.ListFilter[0].ID;
          this.CategoryHRMPhongBanSearch();
          this.Search();
        } else {
          this.DataSource.data = [];
          this.TotalCount = 0;
          this.NoDataKey = 'hrm-nghi-phep.overview.messages.noCompany';
          this.IsLoading = false;
        }
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
    this.Search(false);
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

        this.CategoryHRMPhongBanService.ListFilter = (result.List || [])
          .sort((firstDepartment, secondDepartment) => firstDepartment.Name > secondDepartment.Name ? 1 : -1);
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

    this.DataSource.data = [];
    this.TotalCount = 0;
    this.IsLoading = true;
    const filter: AnnualLeaveOverviewFilter = {
      CompanyID: this.SelectedCompanyID,
      Year: this.SelectedYear,
      DepartmentID: this.SelectedDepartmentID > 0 ? this.SelectedDepartmentID : undefined,
      IsOffice: this.SelectedIsOffice === null ? undefined : this.SelectedIsOffice,
      SearchString: this.SearchString.trim() || undefined,
      PageIndex: this.PageIndex,
      PageSize: this.PageSize,
      SortBy: this.SortBy,
      SortDirection: this.SortDirection,
    };

    this.AnnualLeaveOverviewService.GetPagedListAsync(filter).subscribe(
      response => {
        const result = response as BaseResult;
        if (result.StatusCode && result.StatusCode >= 400) {
          this.NoDataKey = 'hrm-nghi-phep.overview.messages.loadOverviewFailed';
          this.NotificationService.warn(
            this.LanguageService.TranslateService.instant('hrm-nghi-phep.overview.messages.loadOverviewFailed'),
          );
          return;
        }

        this.DataSource.data = (result.List || []) as AnnualLeaveOverview[];
        this.TotalCount = result.Count || 0;
        this.NoDataKey = 'hrm-nghi-phep.overview.messages.noData';
      },
      () => {
        this.DataSource.data = [];
        this.TotalCount = 0;
        this.NoDataKey = 'hrm-nghi-phep.overview.messages.loadOverviewFailed';
        this.NotificationService.warn(
          this.LanguageService.TranslateService.instant('hrm-nghi-phep.overview.messages.loadOverviewFailed'),
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

  SortChange(sort: Sort): void {
    if (!sort.direction) {
      this.SortBy = 'UsedDays';
      this.SortDirection = 'desc';
    } else {
      this.SortBy = sort.active;
      this.SortDirection = sort.direction;
    }
    this.Search();
  }

  ExportExcel(): void {
    if (this.SelectedCompanyID <= 0 || this.IsExporting) {
      return;
    }

    this.IsExporting = true;
    this.AnnualLeaveOverviewService.ExportExcelAsync(this.BuildFilter()).subscribe(
      file => this.DownloadFile(file.Blob, file.FileName),
      () => {
        this.IsExporting = false;
        this.NotificationService.warn(
          this.LanguageService.TranslateService.instant('hrm-nghi-phep.requests.messages.exportFailed'),
        );
      },
      () => this.IsExporting = false,
    );
  }

  private BuildFilter(): AnnualLeaveOverviewFilter {
    return {
      CompanyID: this.SelectedCompanyID,
      Year: this.SelectedYear,
      DepartmentID: this.SelectedDepartmentID > 0 ? this.SelectedDepartmentID : undefined,
      IsOffice: this.SelectedIsOffice === null ? undefined : this.SelectedIsOffice,
      SearchString: this.SearchString.trim() || undefined,
      PageIndex: this.PageIndex,
      PageSize: this.PageSize,
      SortBy: this.SortBy,
      SortDirection: this.SortDirection,
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

  YearSelected(selectedDate: Date, datepicker: MatDatepicker<Date>): void {
    this.SelectedYear = selectedDate.getFullYear();
    this.SelectedYearDate = new Date(this.SelectedYear, 0, 1);
    datepicker.close();
    this.Search();
  }

  ResetFilters(): void {
    this.SelectedYear = this.CurrentYear;
    this.SelectedYearDate = new Date(this.CurrentYear, 0, 1);
    this.PageSize = 20;
    this.SelectedIsOffice = true;
    this.SelectedDepartmentID = 0;
    this.SearchString = '';
    this.SortBy = 'UsedDays';
    this.SortDirection = 'desc';

    if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
      this.SelectedCompanyID = this.CompanyService.ListFilter[0].ID;
      this.CategoryHRMPhongBanSearch();
    }

    this.Search();
  }

  SyncBalance(): void {
    if (this.SelectedCompanyID <= 0 || this.IsSyncing) {
      return;
    }

    this.IsSyncing = true;
    const filter: AnnualLeaveBalanceSyncFilter = {
      Year: this.SelectedYear,
      CompanyID: this.SelectedCompanyID,
    };

    this.AnnualLeaveBalanceService.SyncBalanceAsync(filter).subscribe(
      response => {
        const result = response as AnnualLeaveBalanceSyncApiResult;
        if (result.StatusCode && result.StatusCode >= 400) {
          this.NotificationService.warn(
            result.Message || this.LanguageService.TranslateService.instant(
              'hrm-nghi-phep.overview.messages.syncFailed',
            ),
          );
          return;
        }

        const syncResult = result.BaseModel;
        this.NotificationService.success(this.LanguageService.TranslateService.instant(
          'hrm-nghi-phep.overview.messages.syncSuccess',
          {
            total: syncResult?.TotalEmployeesSynced || 0,
            monthly: syncResult?.MonthlyAccrualsAdded || 0,
            seniority: syncResult?.SeniorityAccrualsAdded || 0,
          },
        ));
        this.Search(false);
      },
      error => {
        this.NotificationService.warn(
          error?.error?.Message || this.LanguageService.TranslateService.instant(
            'hrm-nghi-phep.overview.messages.syncFailed',
          ),
        );
        this.IsSyncing = false;
      },
      () => this.IsSyncing = false,
    );
  }
}
