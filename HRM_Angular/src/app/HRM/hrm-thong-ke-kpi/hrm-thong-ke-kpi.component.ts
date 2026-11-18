import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';

import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { HRMThongKeKPI } from 'src/app/shared/ERP/HRMThongKeKPI.model';
import { HRMThongKeKPIService } from 'src/app/shared/ERP/HRMThongKeKPI.service';
import { HRMThongKeKPIFileService } from 'src/app/shared/ERP/HRMThongKeKPIFile.service';

import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';
import { CategoryStatusService } from 'src/app/shared/ERP/CategoryStatus.service';

import { HrmThongKeKpiModalComponent } from '../hrm-thong-ke-kpi-modal/hrm-thong-ke-kpi-modal.component';

@Component({
  selector: 'app-hrm-thong-ke-kpi',
  templateUrl: './hrm-thong-ke-kpi.component.html',
  styleUrls: ['./hrm-thong-ke-kpi.component.css']
})
export class HrmThongKeKPIComponent implements OnInit {

  @ViewChild('HRMThongKeKPISort') HRMThongKeKPISort: MatSort;
  @ViewChild('HRMThongKeKPIPaginator') HRMThongKeKPIPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public HRMThongKeKPIService: HRMThongKeKPIService,
    public HRMThongKeKPIFileService: HRMThongKeKPIFileService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public CategoryStatusService: CategoryStatusService,
  ) {
    this.HRMThongKeKPIService.BaseParameter.CompanyID = 16;
    this.HRMThongKeKPIService.BaseParameter.CategoryDepartmentID = 0;
    this.HRMThongKeKPIService.BaseParameter.CategoryStatusID = 0;

    this.CompanySearch();
    this.CategoryStatusSearch();
  }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    this.HRMThongKeKPISearch();
  }

  CompanySearch() {
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      (res) => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.HRMThongKeKPIService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          this.CompanyChange();
          this.HRMThongKeKPISearch();
        }
      },
      (err) => { },
      () => { },
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
      (err) => { },
      () => {}
    );
  }

  CategoryDepartmentSearch() {
    this.HRMThongKeKPIService.IsShowLoading = true;
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.HRMThongKeKPIService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.GetByCompanyIDAndActiveToListAsync().subscribe(
      (res) => {
        this.CategoryDepartmentService.ListParent = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      },
      (err) => { },
      () => {
        this.HRMThongKeKPIService.IsShowLoading = false;
      },
    );
  }

  CategoryDepartmentChange(value) {
    this.HRMThongKeKPIService.BaseParameter.CategoryDepartmentID = value;
    this.HRMThongKeKPISearch();
  }

  CategoryStatusChange(value) {
    this.HRMThongKeKPIService.BaseParameter.CategoryStatusID = value;
    this.HRMThongKeKPISearch();
  }

  SearchStringChange(value) {
    this.HRMThongKeKPIService.BaseParameter.SearchString = value;
    this.HRMThongKeKPISearch();
  }

  HRMThongKeKPISearch() {
    this.HRMThongKeKPIService.IsShowLoading = true;
    this.HRMThongKeKPIService.GetByCompanyID_CategoryDepartmentID_SearchStringToListAsync().subscribe(
      (res) => {
        this.HRMThongKeKPIService.List = (res as BaseResult).List;
        this.HRMThongKeKPIService.ListFilter = (res as BaseResult).ListFilter;
        this.HRMThongKeKPIService.ListChild = (res as BaseResult).ListChild;
        this.HRMThongKeKPIService.ListParent = (res as BaseResult).ListParent;
        this.HRMThongKeKPIService.List02 = (res as BaseResult).List02;
        this.HRMThongKeKPIService.List03 = (res as BaseResult).List03;

        this.HRMThongKeKPIFileService.GetAllToListAsync().subscribe(
          (resFile) => {
            const files = (resFile as BaseResult).List;
            this.HRMThongKeKPIService.List.forEach((item) => {
              item.Files = files.filter((f) => f.ParentID == item.ID);
            });
            this.HRMThongKeKPIService.DataSource = new MatTableDataSource(this.HRMThongKeKPIService.List);
            this.HRMThongKeKPIService.DataSource.sort = this.HRMThongKeKPISort;
            this.HRMThongKeKPIService.DataSource.paginator = this.HRMThongKeKPIPaginator;
          },
          (errFile) => {
            this.HRMThongKeKPIService.DataSource = new MatTableDataSource(this.HRMThongKeKPIService.List);
            this.HRMThongKeKPIService.DataSource.sort = this.HRMThongKeKPISort;
            this.HRMThongKeKPIService.DataSource.paginator = this.HRMThongKeKPIPaginator;
          },
        );
      },
      (err) => { },
      () => {
        this.HRMThongKeKPIService.IsShowLoading = false;
      },
    );
  }

  HRMThongKeKPIAdd(ID: number) {
    this.HRMThongKeKPIService.IsShowLoading = true;
    this.HRMThongKeKPIService.BaseParameter.ID = ID;
    this.HRMThongKeKPIService.GetByIDAsync().subscribe(
      (res) => {
        this.HRMThongKeKPIService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.HRMThongKeKPIService.BaseParameter.BaseModel.CompanyID = this.HRMThongKeKPIService.BaseParameter.CompanyID;
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        const dialog = this.Dialog.open(HrmThongKeKpiModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => {
          this.HRMThongKeKPISearch();
        });
      },
      (err) => { },
      () => {
        this.HRMThongKeKPIService.IsShowLoading = false;
      },
    );
  }

  HRMThongKeKPIExcel() {
    this.HRMThongKeKPIService.IsShowLoading = true;
    this.HRMThongKeKPIService.GetByCompanyID_CategoryDepartmentID_SearchStringToExcelAsync().subscribe(
      (res) => {
        let url = (res as BaseResult).Message;
        window.open(url, '_blank');
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMThongKeKPIService.IsShowLoading = false;
      },
    );
  }

  HRMThongKeKPIDelete(element) {
    this.HRMThongKeKPIService.IsShowLoading = true;
    this.HRMThongKeKPIService.BaseParameter.ID = element.ID;
    this.HRMThongKeKPIService.RemoveAsync().subscribe(
      (res) => {
        this.HRMThongKeKPISearch();
        this.NotificationService.warn(environment.DeleteSuccess);
      },
      (err) => {
        this.NotificationService.warn(environment.DeleteNotSuccess);
      },
      () => {
        this.HRMThongKeKPIService.IsShowLoading = false;
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
