import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';

import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { HRMQuanLyCongViec } from 'src/app/shared/ERP/HRMQuanLyCongViec.model';
import { HRMQuanLyCongViecService } from 'src/app/shared/ERP/HRMQuanLyCongViec.service';
import { HRMQuanLyCongViecFileService } from 'src/app/shared/ERP/HRMQuanLyCongViecFile.service';

import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';
import { CategoryStatusService } from 'src/app/shared/ERP/CategoryStatus.service';


import { HrmQuanLyCongViecModalComponent } from '../hrm-quan-ly-cong-viec-modal/hrm-quan-ly-cong-viec-modal.component';

@Component({
  selector: 'app-hrm-quan-ly-cong-viec001',
  templateUrl: './hrm-quan-ly-cong-viec001.component.html',
  styleUrls: ['./hrm-quan-ly-cong-viec001.component.css']
})
export class HrmQuanLyCongViec001Component implements OnInit {

 @ViewChild('HRMQuanLyCongViecSort') HRMQuanLyCongViecSort: MatSort;
  @ViewChild('HRMQuanLyCongViecPaginator') HRMQuanLyCongViecPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public HRMQuanLyCongViecService: HRMQuanLyCongViecService,
    public HRMQuanLyCongViecFileService: HRMQuanLyCongViecFileService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public CategoryStatusService: CategoryStatusService,
  ) {
    this.HRMQuanLyCongViecService.BaseParameter.CompanyID = 16;
    this.HRMQuanLyCongViecService.BaseParameter.CategoryDepartmentID = 0;
    this.HRMQuanLyCongViecService.BaseParameter.CategoryStatusID = 0;
    this.HRMQuanLyCongViecService.BaseParameter.SearchString = localStorage.getItem(environment.UserName);

    this.CompanySearch();

  }

  ngOnInit(): void {

  }

  ngAfterViewInit() {
    this.HRMQuanLyCongViecSearch();
  }

  DateBegin(value) {
    this.HRMQuanLyCongViecService.BaseParameter.DateBegin = new Date(value);
  }

  DateEnd(value) {
    this.HRMQuanLyCongViecService.BaseParameter.DateEnd = new Date(value);
  }

  HRMQuanLyCongViecAdd(ID: number) {
    this.HRMQuanLyCongViecService.IsShowLoading = true;
    this.HRMQuanLyCongViecService.BaseParameter.ID = ID;
    this.HRMQuanLyCongViecService.GetByIDAsync().subscribe(
      (res) => {
        this.HRMQuanLyCongViecService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.HRMQuanLyCongViecService.BaseParameter.BaseModel.CompanyID = this.HRMQuanLyCongViecService.BaseParameter.CompanyID;
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        const dialog = this.Dialog.open(HrmQuanLyCongViecModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => {
          this.HRMQuanLyCongViecSearch();
        });
      },
      (err) => {},
      () => {
        this.HRMQuanLyCongViecService.IsShowLoading = false;
      },
    );
  }

  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.HRMQuanLyCongViecService);
    this.CompanyChange();
  }

  CompanyChange() {
    this.CategoryDepartmentSearch();
    this.CategoryStatusSearch();
  }

  CategoryStatusSearch() {
    this.CategoryStatusService.BaseParameter.Active = true;
    this.CategoryStatusService.GetByActiveToListAsync().subscribe(
      (res) => {
        this.CategoryStatusService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      },
      (err) => { },
    );
  }

  CategoryDepartmentSearch() {
    this.HRMQuanLyCongViecService.IsShowLoading = true;
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.HRMQuanLyCongViecService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.GetByCompanyIDAndActiveToListAsync().subscribe(
      (res) => {
        this.CategoryDepartmentService.ListParent = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));       
      },
      (err) => {

      },
      () => {
        this.HRMQuanLyCongViecService.IsShowLoading = false;
      }
    );
  }

  HRMQuanLyCongViecSearch() {
    this.HRMQuanLyCongViecService.IsShowLoading = true;
    this.HRMQuanLyCongViecService.GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync().subscribe(
      (res) => {
        this.HRMQuanLyCongViecService.List = (res as BaseResult).List;
        this.HRMQuanLyCongViecFileService.GetAllToListAsync().subscribe(
          (resFile) => {
            const files = (resFile as BaseResult).List;
            this.HRMQuanLyCongViecService.List.forEach(item => {
              item.Files = files.filter(f => f.ParentID == item.ID);
            });
            this.HRMQuanLyCongViecService.DataSource = new MatTableDataSource(this.HRMQuanLyCongViecService.List);
            this.HRMQuanLyCongViecService.DataSource.sort = this.HRMQuanLyCongViecSort;
            this.HRMQuanLyCongViecService.DataSource.paginator = this.HRMQuanLyCongViecPaginator;
          },
          (errFile) => {
            this.HRMQuanLyCongViecService.DataSource = new MatTableDataSource(this.HRMQuanLyCongViecService.List);
            this.HRMQuanLyCongViecService.DataSource.sort = this.HRMQuanLyCongViecSort;
            this.HRMQuanLyCongViecService.DataSource.paginator = this.HRMQuanLyCongViecPaginator;
          },
        );
      },
      (err) => { },
      () => {
        this.HRMQuanLyCongViecService.IsShowLoading = false;
      },
    );
  }

  HRMQuanLyCongViecExcel() {
    this.HRMQuanLyCongViecService.IsShowLoading = true;
    this.HRMQuanLyCongViecService.GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToExcelAsync().subscribe(
      (res) => {
        let url = (res as BaseResult).Message;
        window.open(url, '_blank');
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMQuanLyCongViecService.IsShowLoading = false;
      },
    );
  }


}
