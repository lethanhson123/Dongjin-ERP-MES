import { Component, OnInit, ViewChild } from '@angular/core';
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

import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';
import { HRMChamCongLichSuModalComponent } from '../hrmcham-cong-lich-su-modal/hrmcham-cong-lich-su-modal.component';

@Component({
  selector: 'app-hrmcham-cong-lich-su002',
  templateUrl: './hrmcham-cong-lich-su002.component.html',
  styleUrls: ['./hrmcham-cong-lich-su002.component.css']
})
export class HRMChamCongLichSu002Component implements OnInit {

  @ViewChild('HRMChamCongLichSuSort') HRMChamCongLichSuSort: MatSort;
  @ViewChild('HRMChamCongLichSuPaginator') HRMChamCongLichSuPaginator: MatPaginator;

  @ViewChild('HRMChamCongLichSuSortFilter') HRMChamCongLichSuSortFilter: MatSort;
  @ViewChild('HRMChamCongLichSuPaginatorFilter') HRMChamCongLichSuPaginatorFilter: MatPaginator;

  @ViewChild('HRMChamCongLichSuSort01') HRMChamCongLichSuSort01: MatSort;
  @ViewChild('HRMChamCongLichSuPaginator01') HRMChamCongLichSuPaginator01: MatPaginator;

  @ViewChild('HRMChamCongLichSuSort02') HRMChamCongLichSuSort02: MatSort;
  @ViewChild('HRMChamCongLichSuPaginator02') HRMChamCongLichSuPaginator02: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public HRMChamCongLichSuService: HRMChamCongLichSuService,

    public CategoryDepartmentService: CategoryDepartmentService,
    public CompanyService: CompanyService,
  ) {
    this.HRMChamCongLichSuService.BaseParameter.CompanyID = 16;
    this.CompanySearch();
  }

  ngOnInit(): void {
    this.HRMChamCongLichSuService.BaseParameter.CategoryDepartmentID = 0;
  }
  ngAfterViewInit() {}
  DateBegin(value) {
    this.HRMChamCongLichSuService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.HRMChamCongLichSuService.BaseParameter.DateEnd = new Date(value);
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.HRMChamCongLichSuService);
    this.CategoryDepartmentSearch();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.HRMChamCongLichSuService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.HRMChamCongLichSuService);
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.HRMChamCongLichSuService);
  }
  HRMChamCongLichSuAdd(ID: number) {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.BaseParameter.ID = ID;
    this.HRMChamCongLichSuService.GetByIDAsync().subscribe(
      (res) => {
        this.HRMChamCongLichSuService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.HRMChamCongLichSuService.BaseParameter.BaseModel.CompanyID = this.HRMChamCongLichSuService.BaseParameter.CompanyID;
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        const dialog = this.Dialog.open(HRMChamCongLichSuModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => {
          this.HRMChamCongLichSuSearch();
        });
      },
      (err) => {},
      () => {
        this.HRMChamCongLichSuService.IsShowLoading = false;
      },
    );
  }
  HRMChamCongLichSuSearch() {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.GetByCompanyID_CategoryHRMPhongBanID_DateBegin_DateEnd_SearchStringToListAsync().subscribe(
      (res) => {
        this.HRMChamCongLichSuService.List = (res as BaseResult).List;
        this.HRMChamCongLichSuService.DataSource = new MatTableDataSource(this.HRMChamCongLichSuService.List);
        this.HRMChamCongLichSuService.DataSource.sort = this.HRMChamCongLichSuSort;
        this.HRMChamCongLichSuService.DataSource.paginator = this.HRMChamCongLichSuPaginator;

        this.HRMChamCongLichSuService.ListFilter = (res as BaseResult).ListFilter;
        this.HRMChamCongLichSuService.DataSourceFilter = new MatTableDataSource(this.HRMChamCongLichSuService.ListFilter);
        this.HRMChamCongLichSuService.DataSourceFilter.sort = this.HRMChamCongLichSuSortFilter;
        this.HRMChamCongLichSuService.DataSourceFilter.paginator = this.HRMChamCongLichSuPaginatorFilter;

        this.HRMChamCongLichSuService.ListParent = (res as BaseResult).ListParent;
        this.HRMChamCongLichSuService.DataSource01 = new MatTableDataSource(this.HRMChamCongLichSuService.ListParent);
        this.HRMChamCongLichSuService.DataSource01.sort = this.HRMChamCongLichSuSort01;
        this.HRMChamCongLichSuService.DataSource01.paginator = this.HRMChamCongLichSuPaginator01;

        this.HRMChamCongLichSuService.ListChild = (res as BaseResult).ListChild;
        this.HRMChamCongLichSuService.DataSource02 = new MatTableDataSource(this.HRMChamCongLichSuService.ListChild);
        this.HRMChamCongLichSuService.DataSource02.sort = this.HRMChamCongLichSuSort02;
        this.HRMChamCongLichSuService.DataSource02.paginator = this.HRMChamCongLichSuPaginator02;
      },
      (err) => {},
      () => {
        this.HRMChamCongLichSuService.IsShowLoading = false;
      },
    );
  }
  HRMChamCongLichSuDelete(element: HRMChamCongLichSu) {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.BaseParameter.ID = element.ID;
    this.HRMChamCongLichSuService.RemoveAsync().subscribe(
      (res) => {
        this.HRMChamCongLichSuSearch();
        this.NotificationService.warn(environment.DeleteSuccess);
      },
      (err) => {
        this.NotificationService.warn(environment.DeleteNotSuccess);
      },
      () => {
        this.HRMChamCongLichSuService.IsShowLoading = false;
      },
    );
  }
  HRMChamCongLichSuExcel() {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.GetByCompanyID_CategoryHRMPhongBanID_DateBegin_DateEnd_SearchStringToExcelAsync().subscribe(
      (res) => {
        let url = (res as BaseResult).Message;
        window.open(url, '_blank');
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMChamCongLichSuService.IsShowLoading = false;
      },
    );
  }
  HRMChamCongLichSuSync() {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.CreateAutoAsync().subscribe(
      (res) => {
        this.HRMChamCongLichSuSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMChamCongLichSuService.IsShowLoading = false;
      },
    );
  }
}