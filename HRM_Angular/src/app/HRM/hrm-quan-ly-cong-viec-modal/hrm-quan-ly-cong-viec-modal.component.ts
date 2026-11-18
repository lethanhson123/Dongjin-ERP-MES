import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { HRMQuanLyCongViecService } from 'src/app/shared/ERP/HRMQuanLyCongViec.service';
import { HRMQuanLyCongViecFileService } from 'src/app/shared/ERP/HRMQuanLyCongViecFile.service';

import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';
import { CategoryPositionService } from 'src/app/shared/ERP/CategoryPosition.service';
import { CategoryStatusService } from 'src/app/shared/ERP/CategoryStatus.service';
import { CategoryHRMTaiLieuService } from 'src/app/shared/ERP/CategoryHRMTaiLieu.service';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';
import { MatTableDataSource } from '@angular/material/table';
import { HRMQuanLyCongViecFile } from 'src/app/shared/ERP/HRMQuanLyCongViecFile.model';

@Component({
  selector: 'app-hrm-quan-ly-cong-viec-modal',
  templateUrl: './hrm-quan-ly-cong-viec-modal.component.html',
  styleUrls: ['./hrm-quan-ly-cong-viec-modal.component.css'],
})
export class HrmQuanLyCongViecModalComponent implements OnInit {
  @ViewChild('HRMQuanLyCongViecSort') HRMQuanLyCongViecSort: MatSort;
  @ViewChild('HRMQuanLyCongViecPaginator') HRMQuanLyCongViecPaginator: MatPaginator;
  @ViewChild('HRMQuanLyCongViecFileSort') HRMQuanLyCongViecFileSort: MatSort;
  @ViewChild('HRMQuanLyCongViecFilePaginator') HRMQuanLyCongViecFilePaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<HrmQuanLyCongViecModalComponent>,
    public NotificationService: NotificationService,
    public HRMQuanLyCongViecService: HRMQuanLyCongViecService,
    public HRMQuanLyCongViecFileService: HRMQuanLyCongViecFileService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public CategoryPositionService: CategoryPositionService,
    public CategoryStatusService: CategoryStatusService,
    public CategoryHRMTaiLieuService: CategoryHRMTaiLieuService,
    public MembershipService: MembershipService,
  ) {
    if (this.HRMQuanLyCongViecService.BaseParameter.BaseModel.ID == 0) {
      this.HRMQuanLyCongViecService.BaseParameter.BaseModel.CreateUserCode = localStorage.getItem(environment.UserName);
      this.HRMQuanLyCongViecService.BaseParameter.BaseModel.CreateUserName = localStorage.getItem(environment.FullName);
      this.HRMQuanLyCongViecService.BaseParameter.BaseModel.CreateUserID = Number(localStorage.getItem(environment.UserID));
    }
    this.CompanySearch();
    this.MembershipSearch();

    this.CategoryDepartmentSearch();
    this.CategoryPositionSearch();
    this.CategoryStatusSearch();
  }

  ngOnInit(): void {}

  ngAfterViewInit(): void {
    this.HRMQuanLyCongViecFileSearch();
  }

  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.HRMQuanLyCongViecService);
    this.CompanyChange();
  }

  CategoryDepartmentSearch() {
    this.CategoryStatusService.IsShowLoading = true;
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.HRMQuanLyCongViecService.BaseParameter.BaseModel.CompanyID;
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.GetByCompanyIDAndActiveToListAsync().subscribe(
      (res) => {
        this.CategoryDepartmentService.ListParent = (res as any).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      },
      (err) => {},
      () => {
        this.CategoryStatusService.IsShowLoading = false;
      },
    );
  }

  CompanyChange() {
    this.CategoryDepartmentSearch();
  }

  CategoryPositionSearch() {
    this.CategoryPositionService.ComponentGetByActiveToListAsync(this.HRMQuanLyCongViecService);
  }

  CategoryStatusSearch() {
    this.CategoryStatusService.IsShowLoading = true;
    this.CategoryStatusService.BaseParameter.Active = true;
    this.CategoryStatusService.GetByActiveToListAsync().subscribe(
      (res) => {
        this.CategoryStatusService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CategoryStatusService.ListFilter.length > 0) {
          this.HRMQuanLyCongViecService.BaseParameter.BaseModel.CategoryStatusID = this.CategoryStatusService.ListFilter[0].ID;
        }
      },
      (err) => {},
      () => {
        this.CategoryStatusService.IsShowLoading = false;
      },
    );
  }

  MembershipSearch() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.BaseParameter.CompanyID = this.HRMQuanLyCongViecService.BaseParameter.CompanyID;
    this.MembershipService.GetByCompanyIDAndActiveToListAsync().subscribe(
      (res) => {
        this.MembershipService.BaseResult = res as BaseResult;
        this.MembershipService.ListParent = this.MembershipService.BaseResult.List.sort((a, b) => (a.UserName > b.UserName ? 1 : -1));
        this.MembershipService.ListChild = this.MembershipService.ListParent;
      },
      (err) => {},
      () => {
        this.MembershipService.IsShowLoading = false;
      },
    );
  }

  MembershipFilter(SearchString: string) {
    this.MembershipService.ListChild = this.MembershipService.ListParent;
    if (this.HRMQuanLyCongViecService.BaseParameter.BaseModel.CategoryDepartmentID > 0) {
      this.MembershipService.ListChild = this.MembershipService.ListChild.filter((o) => o.CategoryDepartmentID == this.HRMQuanLyCongViecService.BaseParameter.BaseModel.CategoryDepartmentID);
    }
    if (this.MembershipService.BaseParameter.SearchString.length > 0) {
      this.MembershipService.ListChild = this.MembershipService.ListChild.filter((o) =>
        this.MembershipService.BaseParameter.SearchString.trim().toLowerCase().split(/\s+/).every((keyword) =>
          ((o.UserName || '') + ' ' + (o.Name || '') + ' ' + (o.CategoryDepartmentName || '') + ' ' + (o.Search || '')).toLowerCase().includes(keyword)
        )
      );
    }
  }

  HRMQuanLyCongViecSave() {
    this.HRMQuanLyCongViecService.IsShowLoading = true;
    this.HRMQuanLyCongViecService.SaveAsync().subscribe(
      (res) => {
        this.HRMQuanLyCongViecService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;

        if (this.HRMQuanLyCongViecService.BaseParameter.BaseModel.ID > 0) {
          this.HRMQuanLyCongViecFileService.BaseParameter.ParentID = this.HRMQuanLyCongViecService.BaseParameter.BaseModel.ID;
          this.HRMQuanLyCongViecFileService.SaveAndUploadFilesAsync().subscribe(
            () => {
              this.HRMQuanLyCongViecFileService.FileToUpload = null;
                this.HRMQuanLyCongViecFileService.FileToUpload = null;
                if (this.HRMQuanLyCongViecFileService.BaseParameter.Event) {
                  this.HRMQuanLyCongViecFileService.BaseParameter.Event.target.value = '';
                  this.HRMQuanLyCongViecFileService.BaseParameter.Event = null;
                }
                this.HRMQuanLyCongViecFileSearch();
              this.HRMQuanLyCongViecFileSearch();
            },
            (err) => {
              this.NotificationService.warn(environment.UploadNotSuccess);
            },
            () => {
              this.HRMQuanLyCongViecService.IsShowLoading = false;
            },
          );
        }
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMQuanLyCongViecService.IsShowLoading = false;
      },
    );
  }

  HRMQuanLyCongViecDateBegin(value) {
    this.HRMQuanLyCongViecService.BaseParameter.BaseModel.DateBegin = new Date(value);
  }

  HRMQuanLyCongViecDateEnd(value) {
    this.HRMQuanLyCongViecService.BaseParameter.BaseModel.DateEnd = new Date(value);
  }

  HRMQuanLyCongViecClose() {
    this.DialogRef.close();
  }

  HRMQuanLyCongViecChange(event, files: FileList) {
    if (files) {
      this.HRMQuanLyCongViecService.FileToUpload = files;
      this.HRMQuanLyCongViecService.BaseParameter.Event = event;
      this.HRMQuanLyCongViecFileService.FileToUpload = files;
      this.HRMQuanLyCongViecFileService.BaseParameter.Event = event;
    }
  }

  HRMQuanLyCongViecFileChange(event, files: FileList) {
    this.HRMQuanLyCongViecService.FileToUpload = files;
    this.HRMQuanLyCongViecService.BaseParameter.Event = event;
  }

  HRMQuanLyCongViecFileSearch() {
    if (this.HRMQuanLyCongViecService.BaseParameter.BaseModel.ID > 0) {
      if (this.HRMQuanLyCongViecFileService.BaseParameter.SearchString.length > 0) {
        this.HRMQuanLyCongViecFileService.BaseParameter.SearchString = this.HRMQuanLyCongViecFileService.BaseParameter.SearchString.trim();
        if (this.HRMQuanLyCongViecFileService.DataSource) {
          this.HRMQuanLyCongViecFileService.DataSource.filter = this.HRMQuanLyCongViecFileService.BaseParameter.SearchString.toLowerCase();
        }
      } else {
        this.HRMQuanLyCongViecFileService.BaseParameter.ParentID = this.HRMQuanLyCongViecService.BaseParameter.BaseModel.ID;
        this.HRMQuanLyCongViecService.IsShowLoading = true;
        this.HRMQuanLyCongViecFileService.GetByParentIDToListAsync().subscribe(
          (res) => {
            this.HRMQuanLyCongViecFileService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.HRMQuanLyCongViecFileService.DataSource = new MatTableDataSource(this.HRMQuanLyCongViecFileService.List);
            this.HRMQuanLyCongViecFileService.DataSource.sort = this.HRMQuanLyCongViecFileSort;
            this.HRMQuanLyCongViecFileService.DataSource.paginator = this.HRMQuanLyCongViecFilePaginator;
          },
          (err) => {},
          () => {
            this.HRMQuanLyCongViecService.IsShowLoading = false;
          },
        );
      }
    } else {
      this.HRMQuanLyCongViecFileService.DataSource.data = [{ ID: 0 }];
    }
  }

  HRMQuanLyCongViecFileDelete(element: HRMQuanLyCongViecFile) {
    this.HRMQuanLyCongViecFileService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.HRMQuanLyCongViecService.IsShowLoading = true;
      this.HRMQuanLyCongViecFileService.RemoveAsync().subscribe(
        (res) => {
          this.HRMQuanLyCongViecFileSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        (err) => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.HRMQuanLyCongViecService.IsShowLoading = false;
        },
      );
    }
  }
}
