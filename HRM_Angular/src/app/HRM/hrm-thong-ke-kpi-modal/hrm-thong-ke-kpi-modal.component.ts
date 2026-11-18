import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { HRMThongKeKPIService } from 'src/app/shared/ERP/HRMThongKeKPI.service';
import { HRMThongKeKPIFileService } from 'src/app/shared/ERP/HRMThongKeKPIFile.service';
import { HRMThongKeKPIFile } from 'src/app/shared/ERP/HRMThongKeKPIFile.model';

import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';
import { CategoryStatusService } from 'src/app/shared/ERP/CategoryStatus.service';

@Component({
  selector: 'app-hrm-thong-ke-kpi-modal',
  templateUrl: './hrm-thong-ke-kpi-modal.component.html',
  styleUrls: ['./hrm-thong-ke-kpi-modal.component.css']
})
export class HrmThongKeKpiModalComponent implements OnInit {
  @ViewChild('HRMThongKeKPIFileSort') HRMThongKeKPIFileSort: MatSort;
  @ViewChild('HRMThongKeKPIFilePaginator') HRMThongKeKPIFilePaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<HrmThongKeKpiModalComponent>,
    public NotificationService: NotificationService,
    public HRMThongKeKPIService: HRMThongKeKPIService,
    public HRMThongKeKPIFileService: HRMThongKeKPIFileService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public CategoryStatusService: CategoryStatusService,
  ) {
    if (this.HRMThongKeKPIService.BaseParameter.BaseModel.ID == 0) {
      this.HRMThongKeKPIService.BaseParameter.BaseModel.CreateUserCode = localStorage.getItem(environment.UserName);
      this.HRMThongKeKPIService.BaseParameter.BaseModel.CreateUserName = localStorage.getItem(environment.FullName);
      this.HRMThongKeKPIService.BaseParameter.BaseModel.CreateUserID = Number(localStorage.getItem(environment.UserID));
    }
    this.CompanySearch();
    this.CategoryDepartmentSearch();
    this.CategoryStatusSearch();
  }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.HRMThongKeKPIFileSearch();
  }

  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.HRMThongKeKPIService);
  }

  CategoryDepartmentSearch() {
    this.HRMThongKeKPIService.IsShowLoading = true;
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.HRMThongKeKPIService.BaseParameter.BaseModel.CompanyID;
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.GetByCompanyIDAndActiveToListAsync().subscribe(
      (res) => {
        this.CategoryDepartmentService.ListParent = (res as any).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      },
      (err) => { },
      () => {
        this.HRMThongKeKPIService.IsShowLoading = false;
      },
    );
  }

  CategoryStatusSearch() {
    this.HRMThongKeKPIService.IsShowLoading = true;
    this.CategoryStatusService.BaseParameter.Active = true;
    this.CategoryStatusService.GetByActiveToListAsync().subscribe(
      (res) => {
        this.CategoryStatusService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.HRMThongKeKPIService.BaseParameter.BaseModel.ID == 0 && this.CategoryStatusService.ListFilter.length > 0) {
          this.HRMThongKeKPIService.BaseParameter.BaseModel.CategoryStatusID = this.CategoryStatusService.ListFilter[0].ID;
        }
      },
      (err) => { },
      () => {
        this.HRMThongKeKPIService.IsShowLoading = false;
      },
    );
  }

  HRMThongKeKPISave() {
    this.HRMThongKeKPIService.IsShowLoading = true;
    this.HRMThongKeKPIService.SaveAsync().subscribe(
      (res) => {
        this.HRMThongKeKPIService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;

        if (this.HRMThongKeKPIService.BaseParameter.BaseModel.ID > 0) {
          if (this.HRMThongKeKPIFileService.FileToUpload && this.HRMThongKeKPIFileService.FileToUpload.length > 0) {
            this.HRMThongKeKPIFileService.BaseParameter.BaseModel.ParentID = this.HRMThongKeKPIService.BaseParameter.BaseModel.ID;
            this.HRMThongKeKPIFileService.SaveAndUploadFilesAsync().subscribe(
              () => {
                this.HRMThongKeKPIFileService.FileToUpload = null;
                this.HRMThongKeKPIService.FileToUpload = null;
                if (this.HRMThongKeKPIFileService.BaseParameter.Event) {
                  this.HRMThongKeKPIFileService.BaseParameter.Event.target.value = '';
                  this.HRMThongKeKPIFileService.BaseParameter.Event = null;
                }
                this.HRMThongKeKPIFileSearch();
              },
              (err) => {
                this.NotificationService.warn(environment.UploadNotSuccess);
              },
              () => {
                this.HRMThongKeKPIService.IsShowLoading = false;
              },
            );
          }
        }
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMThongKeKPIService.IsShowLoading = false;
      },
    );
  }

  HRMThongKeKPIClose() {
    this.DialogRef.close();
  }

  HRMThongKeKPIChange(event, files: FileList) {
    if (files) {
      this.HRMThongKeKPIService.FileToUpload = files;
      this.HRMThongKeKPIService.BaseParameter.Event = event;
      this.HRMThongKeKPIFileService.FileToUpload = files;
      this.HRMThongKeKPIFileService.BaseParameter.Event = event;
    }
  }

  HRMThongKeKPIFileChange(event, files: FileList) {
    this.HRMThongKeKPIService.FileToUpload = files;
    this.HRMThongKeKPIService.BaseParameter.Event = event;
  }

  HRMThongKeKPIFileSearch() {
    if (this.HRMThongKeKPIService.BaseParameter.BaseModel.ID > 0) {
      if (this.HRMThongKeKPIFileService.BaseParameter.SearchString.length > 0) {
        this.HRMThongKeKPIFileService.BaseParameter.SearchString = this.HRMThongKeKPIFileService.BaseParameter.SearchString.trim();
        if (this.HRMThongKeKPIFileService.DataSource) {
          this.HRMThongKeKPIFileService.DataSource.filter = this.HRMThongKeKPIFileService.BaseParameter.SearchString.toLowerCase();
        }
      } else {
        this.HRMThongKeKPIFileService.BaseParameter.ParentID = this.HRMThongKeKPIService.BaseParameter.BaseModel.ID;
        this.HRMThongKeKPIService.IsShowLoading = true;
        this.HRMThongKeKPIFileService.GetByParentIDToListAsync().subscribe(
          (res) => {
            this.HRMThongKeKPIFileService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.HRMThongKeKPIFileService.DataSource = new MatTableDataSource(this.HRMThongKeKPIFileService.List);
            this.HRMThongKeKPIFileService.DataSource.sort = this.HRMThongKeKPIFileSort;
            this.HRMThongKeKPIFileService.DataSource.paginator = this.HRMThongKeKPIFilePaginator;
          },
          (err) => { },
          () => {
            this.HRMThongKeKPIService.IsShowLoading = false;
          },
        );
      }
    } else {
      if (!this.HRMThongKeKPIFileService.DataSource)
        this.HRMThongKeKPIFileService.DataSource = new MatTableDataSource<HRMThongKeKPIFile>();
    }
  }

  HRMThongKeKPIFileDelete(element: HRMThongKeKPIFile) {
    this.HRMThongKeKPIFileService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.HRMThongKeKPIService.IsShowLoading = true;
      this.HRMThongKeKPIFileService.RemoveAsync().subscribe(
        (res) => {
          this.HRMThongKeKPIFileSearch();
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
  }
}
