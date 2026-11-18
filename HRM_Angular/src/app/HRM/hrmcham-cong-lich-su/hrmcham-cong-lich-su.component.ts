import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { HRMChamCongLichSu } from 'src/app/shared/ERP/HRMChamCongLichSu.model';
import { HRMChamCongLichSuService } from 'src/app/shared/ERP/HRMChamCongLichSu.service';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';
import { HRMChamCongLichSuModalComponent } from '../hrmcham-cong-lich-su-modal/hrmcham-cong-lich-su-modal.component';

@Component({
  selector: 'app-hrmcham-cong-lich-su',
  templateUrl: './hrmcham-cong-lich-su.component.html',
  styleUrls: ['./hrmcham-cong-lich-su.component.css'],
})
export class HRMChamCongLichSuComponent implements OnInit {
  @ViewChild('HRMChamCongLichSuSort') HRMChamCongLichSuSort: MatSort;
  @ViewChild('HRMChamCongLichSuPaginator') HRMChamCongLichSuPaginator: MatPaginator;
  @ViewChild('HRMChamCongLichSuSortFilter') HRMChamCongLichSuSortFilter: MatSort;
  @ViewChild('HRMChamCongLichSuPaginatorFilter') HRMChamCongLichSuPaginatorFilter: MatPaginator;
  @ViewChild('HRMChamCongLichSuSort01') HRMChamCongLichSuSort01: MatSort;
  @ViewChild('HRMChamCongLichSuPaginator01') HRMChamCongLichSuPaginator01: MatPaginator;
  @ViewChild('HRMChamCongLichSuSort02') HRMChamCongLichSuSort02: MatSort;
  @ViewChild('HRMChamCongLichSuPaginator02') HRMChamCongLichSuPaginator02: MatPaginator;
  @ViewChild('HRMChamCongLichSuSort004') HRMChamCongLichSuSort004: MatSort;
  @ViewChild('HRMChamCongLichSuPaginator004') HRMChamCongLichSuPaginator004: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,
    public HRMChamCongLichSuService: HRMChamCongLichSuService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
    public CompanyService: CompanyService,
  ) {
    this.HRMChamCongLichSuService.BaseParameter.CompanyID = 16;
    this.CompanySearch();
  }

  ngOnInit(): void {
    this.HRMChamCongLichSuService.BaseParameter.CategoryHRMPhongBanID = 0;
  }

  ngAfterViewInit(): void { 
    this.HRMChamCongLichSuService.SummaryDataSource.sort = this.HRMChamCongLichSuSort004;
    this.HRMChamCongLichSuService.SummaryDataSource.paginator = this.HRMChamCongLichSuPaginator004;
  }

  DateBegin(value) { this.HRMChamCongLichSuService.BaseParameter.DateBegin = new Date(value); }
  DateEnd(value) { this.HRMChamCongLichSuService.BaseParameter.DateEnd = new Date(value); }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.HRMChamCongLichSuService);
    this.CategoryHRMPhongBanSearch();
  }
  CompanyChange() { this.CategoryHRMPhongBanSearch(); }
  CategoryHRMPhongBanSearch() {
    this.CategoryHRMPhongBanService.BaseParameter.CompanyID = this.HRMChamCongLichSuService.BaseParameter.CompanyID;
    this.CategoryHRMPhongBanService.BaseParameter.Active = true;
    this.CategoryHRMPhongBanService.ComponentGetByCompanyIDAndActiveToList(this.HRMChamCongLichSuService);
  }

  HRMChamCongLichSuAdd(ID: number) {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.BaseParameter.ID = ID;
    this.HRMChamCongLichSuService.GetByIDAsync().subscribe(
      res => {
        this.HRMChamCongLichSuService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.HRMChamCongLichSuService.BaseParameter.BaseModel.CompanyID = this.HRMChamCongLichSuService.BaseParameter.CompanyID;
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        const dialog = this.Dialog.open(HRMChamCongLichSuModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => this.HRMChamCongLichSuSearch());
      },
      () => { },
      () => { this.HRMChamCongLichSuService.IsShowLoading = false; },
    );
  }

  HRMChamCongLichSuDeviceSync() {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.GetHRMMayChamCongToHRMChamCongLichSuViaZK().subscribe(
      () => this.HRMChamCongLichSuSearch(),
      () => { this.NotificationService.warn(environment.SaveNotSuccess); this.HRMChamCongLichSuService.IsShowLoading = false; },
      () => { this.HRMChamCongLichSuService.IsShowLoading = false; },
    );
  }

  HRMChamCongLichSuSearch() {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.GetByCompanyID_CategoryHRMPhongBanID_DateBegin_DateEnd_SearchStringToSummaryAsync().subscribe(
      res => {
        const response = res as BaseResult;
        this.HRMChamCongLichSuService.SummaryList = response.ListSummary || [];
        this.HRMChamCongLichSuService.SummaryDataSource = new MatTableDataSource(this.HRMChamCongLichSuService.SummaryList);
        this.HRMChamCongLichSuService.SummaryDataSource.sort = this.HRMChamCongLichSuSort004;
        this.HRMChamCongLichSuService.SummaryDataSource.paginator = this.HRMChamCongLichSuPaginator004;
        this.HRMChamCongLichSuService.List = response.List;
        this.HRMChamCongLichSuService.DataSource = new MatTableDataSource(this.HRMChamCongLichSuService.List);
        this.HRMChamCongLichSuService.DataSource.sort = this.HRMChamCongLichSuSort;
        this.HRMChamCongLichSuService.DataSource.paginator = this.HRMChamCongLichSuPaginator;
        this.HRMChamCongLichSuService.ListFilter = response.ListFilter;
        this.HRMChamCongLichSuService.DataSourceFilter = new MatTableDataSource(this.HRMChamCongLichSuService.ListFilter);
        this.HRMChamCongLichSuService.DataSourceFilter.sort = this.HRMChamCongLichSuSortFilter;
        this.HRMChamCongLichSuService.DataSourceFilter.paginator = this.HRMChamCongLichSuPaginatorFilter;
        this.HRMChamCongLichSuService.ListParent = response.ListParent;
        this.HRMChamCongLichSuService.DataSource01 = new MatTableDataSource(this.HRMChamCongLichSuService.ListParent);
        this.HRMChamCongLichSuService.DataSource01.sort = this.HRMChamCongLichSuSort01;
        this.HRMChamCongLichSuService.DataSource01.paginator = this.HRMChamCongLichSuPaginator01;
        this.HRMChamCongLichSuService.ListChild = response.ListChild;
        this.HRMChamCongLichSuService.DataSource02 = new MatTableDataSource(this.HRMChamCongLichSuService.ListChild);
        this.HRMChamCongLichSuService.DataSource02.sort = this.HRMChamCongLichSuSort02;
        this.HRMChamCongLichSuService.DataSource02.paginator = this.HRMChamCongLichSuPaginator02;
      },
      () => { },
      () => { this.HRMChamCongLichSuService.IsShowLoading = false; },
    );
  }

  HRMChamCongLichSuDelete(element: HRMChamCongLichSu) {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.BaseParameter.ID = element.ID;
    this.HRMChamCongLichSuService.RemoveAsync().subscribe(
      () => { this.HRMChamCongLichSuSearch(); this.NotificationService.warn(environment.DeleteSuccess); },
      () => this.NotificationService.warn(environment.DeleteNotSuccess),
      () => { this.HRMChamCongLichSuService.IsShowLoading = false; },
    );
  }

  HRMChamCongLichSuExcel() {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.GetByCompanyID_CategoryHRMPhongBanID_DateBegin_DateEnd_SearchStringToExcelAsync().subscribe(
      res => window.open((res as BaseResult).Message, '_blank'),
      () => this.NotificationService.warn(environment.SaveNotSuccess),
      () => { this.HRMChamCongLichSuService.IsShowLoading = false; },
    );
  }

  HRMChamCongLichSuSync() {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.CreateAutoAsync().subscribe(
      () => { this.HRMChamCongLichSuSearch(); this.NotificationService.warn(environment.SaveSuccess); },
      () => this.NotificationService.warn(environment.SaveNotSuccess),
      () => { this.HRMChamCongLichSuService.IsShowLoading = false; },
    );
  }
}
