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
  selector: 'app-hrm-quan-ly-cong-viec',
  templateUrl: './hrm-quan-ly-cong-viec.component.html',
  styleUrls: ['./hrm-quan-ly-cong-viec.component.css'],
})
export class HrmQuanLyCongViecComponent implements OnInit {
  @ViewChild('HRMQuanLyCongViecSort') HRMQuanLyCongViecSort: MatSort;
  @ViewChild('HRMQuanLyCongViecPaginator') HRMQuanLyCongViecPaginator: MatPaginator;

  @ViewChild('HRMQuanLyCongViecSortFilter') HRMQuanLyCongViecSortFilter: MatSort;
  @ViewChild('HRMQuanLyCongViecPaginatorFilter') HRMQuanLyCongViecPaginatorFilter: MatPaginator;

  @ViewChild('HRMQuanLyCongViecSort01') HRMQuanLyCongViecSort01: MatSort;
  @ViewChild('HRMQuanLyCongViecPaginator01') HRMQuanLyCongViecPaginator01: MatPaginator;

  @ViewChild('HRMQuanLyCongViecSort02') HRMQuanLyCongViecSort02: MatSort;
  @ViewChild('HRMQuanLyCongViecPaginator02') HRMQuanLyCongViecPaginator02: MatPaginator;

  @ViewChild('HRMQuanLyCongViecSort03') HRMQuanLyCongViecSort03: MatSort;
  @ViewChild('HRMQuanLyCongViecPaginator03') HRMQuanLyCongViecPaginator03: MatPaginator;

  @ViewChild('HRMQuanLyCongViecSort04') HRMQuanLyCongViecSort04: MatSort;
  @ViewChild('HRMQuanLyCongViecPaginator04') HRMQuanLyCongViecPaginator04: MatPaginator;

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

    this.CompanySearch();
    this.CategoryStatusSearch();
  }

  ngOnInit(): void {}

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
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      (res) => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.HRMQuanLyCongViecService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          this.CompanyChange();
          this.HRMQuanLyCongViecSearch();
        }
      },
      (err) => {},
      () => {},
    );
  }

  CompanyChange() {
    this.CategoryDepartmentSearch();
  }

  CategoryStatusSearch() {
    this.CategoryStatusService.BaseParameter.Active = true;
    this.CategoryStatusService.GetByActiveToListAsync().subscribe(
      (res) => {
        this.CategoryStatusService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      },
      (err) => {},
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
      (err) => {},
      () => {
        this.HRMQuanLyCongViecService.IsShowLoading = false;
      },
    );
  }

  HRMQuanLyCongViecSearch() {
    this.HRMQuanLyCongViecService.IsShowLoading = true;
    this.HRMQuanLyCongViecService.GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync().subscribe(
      (res) => {
        this.HRMQuanLyCongViecService.List = (res as BaseResult).List;

        this.HRMQuanLyCongViecService.ListFilter = (res as BaseResult).ListFilter;
        this.HRMQuanLyCongViecService.DataSourceFilter = new MatTableDataSource(this.HRMQuanLyCongViecService.ListFilter);
        this.HRMQuanLyCongViecService.DataSourceFilter.sort = this.HRMQuanLyCongViecSortFilter;
        this.HRMQuanLyCongViecService.DataSourceFilter.paginator = this.HRMQuanLyCongViecPaginatorFilter;

        this.HRMQuanLyCongViecService.ListChild = (res as BaseResult).ListChild;
        this.HRMQuanLyCongViecService.DataSource01 = new MatTableDataSource(this.HRMQuanLyCongViecService.ListChild);
        this.HRMQuanLyCongViecService.DataSource01.sort = this.HRMQuanLyCongViecSort01;
        this.HRMQuanLyCongViecService.DataSource01.paginator = this.HRMQuanLyCongViecPaginator01;

        this.HRMQuanLyCongViecService.ListParent = (res as BaseResult).ListParent;
        this.HRMQuanLyCongViecService.DataSource02 = new MatTableDataSource(this.HRMQuanLyCongViecService.ListParent);
        this.HRMQuanLyCongViecService.DataSource02.sort = this.HRMQuanLyCongViecSort02;
        this.HRMQuanLyCongViecService.DataSource02.paginator = this.HRMQuanLyCongViecPaginator02;

        this.HRMQuanLyCongViecService.List02 = (res as BaseResult).List02;
        this.HRMQuanLyCongViecService.DataSource03 = new MatTableDataSource(this.HRMQuanLyCongViecService.List02);
        this.HRMQuanLyCongViecService.DataSource03.sort = this.HRMQuanLyCongViecSort03;
        this.HRMQuanLyCongViecService.DataSource03.paginator = this.HRMQuanLyCongViecPaginator03;

        this.HRMQuanLyCongViecService.List03 = (res as BaseResult).List03;
        this.HRMQuanLyCongViecService.DataSource04 = new MatTableDataSource(this.HRMQuanLyCongViecService.List03);
        this.HRMQuanLyCongViecService.DataSource04.sort = this.HRMQuanLyCongViecSort04;
        this.HRMQuanLyCongViecService.DataSource04.paginator = this.HRMQuanLyCongViecPaginator04;

        this.HRMQuanLyCongViecFileService.GetAllToListAsync().subscribe(
          (resFile) => {
            const files = (resFile as BaseResult).List;
            this.HRMQuanLyCongViecService.List.forEach((item) => {
              item.Files = files.filter((f) => f.ParentID == item.ID);
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
      (err) => {},
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

  HRMQuanLyCongViecDelete(element) {
    this.HRMQuanLyCongViecService.IsShowLoading = true;
    this.HRMQuanLyCongViecService.BaseParameter.ID = element.ID;
    this.HRMQuanLyCongViecService.RemoveAsync().subscribe(
      (res) => {
        if ((res as BaseResult).StatusCode == 403) {
          this.NotificationService.warn((res as BaseResult).Message);
        } else {
          this.HRMQuanLyCongViecSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        }
      },
      (err) => {
        this.NotificationService.warn(environment.DeleteNotSuccess);
      },
      () => {
        this.HRMQuanLyCongViecService.IsShowLoading = false;
      },
    );
  }

  getFileName(fileUrl: string): string {
    if (!fileUrl) return '';
    const name = fileUrl.split('/').pop() || '';
    return name.split('.').slice(0, -1).join('.');
  }

  getFileIcon(fileUrl: string): string {
    if (!fileUrl) return 'attach_file';
    if (fileUrl.endsWith('.pdf')) return 'picture_as_pdf';
    if (fileUrl.endsWith('.xlsx') || fileUrl.endsWith('.xls')) return 'description';
    if (fileUrl.endsWith('.docx') || fileUrl.endsWith('.doc')) return 'article';
    return 'attach_file';
  }

  getFileColor(fileUrl: string): string {
    if (!fileUrl) return '#757575';
    if (fileUrl.endsWith('.pdf')) return '#e53935';
    if (fileUrl.endsWith('.xlsx') || fileUrl.endsWith('.xls')) return '#43a047';
    if (fileUrl.endsWith('.docx') || fileUrl.endsWith('.doc')) return '#1e88e5';
    return '#757575';
  }
}
