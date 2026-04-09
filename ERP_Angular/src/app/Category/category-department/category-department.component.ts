import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-category-department',
  templateUrl: './category-department.component.html',
  styleUrls: ['./category-department.component.css']
})
export class CategoryDepartmentComponent {

  @ViewChild('CategoryDepartmentSort') CategoryDepartmentSort: MatSort;
  @ViewChild('CategoryDepartmentPaginator') CategoryDepartmentPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryDepartmentService: CategoryDepartmentService,
    public CompanyService: CompanyService,
  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {

  }
  CompanySearch() {
    this.CategoryDepartmentService.IsShowLoading = true;
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      res => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.CategoryDepartmentService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
        }
        this.CategoryDepartmentSearch();
      },
      err => {
      },
      () => {
        this.CategoryDepartmentService.IsShowLoading = false;
      }
    );
  }
  CategoryDepartmentSearch() {
    if (this.CategoryDepartmentService.BaseParameter.SearchString.length > 0) {
      this.CategoryDepartmentService.BaseParameter.SearchString = this.CategoryDepartmentService.BaseParameter.SearchString.trim();
      if (this.CategoryDepartmentService.DataSource) {
        this.CategoryDepartmentService.DataSource.filter = this.CategoryDepartmentService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.CategoryDepartmentService.IsShowLoading = true;
      this.CategoryDepartmentService.GetByCompanyIDAndEmptyToListAsync().subscribe(
        res => {
          this.CategoryDepartmentService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.CategoryDepartmentService.DataSource = new MatTableDataSource(this.CategoryDepartmentService.List);
          this.CategoryDepartmentService.DataSource.sort = this.CategoryDepartmentSort;
          this.CategoryDepartmentService.DataSource.paginator = this.CategoryDepartmentPaginator;
        },
        err => {
        },
        () => {
          this.CategoryDepartmentService.IsShowLoading = false;
        }
      );
    }
  }
  CategoryDepartmentSave(element: CategoryDepartment) {
    this.CategoryDepartmentService.IsShowLoading = true;
    this.CategoryDepartmentService.BaseParameter.BaseModel = element;
    this.CategoryDepartmentService.SaveAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
        this.CategoryDepartmentSearch();
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategoryDepartmentService.IsShowLoading = false;
      }
    );
  }
  CategoryDepartmentDelete(element: CategoryDepartment) {
    if (confirm(environment.DeleteConfirm)) {
      this.CategoryDepartmentService.IsShowLoading = true;
      this.CategoryDepartmentService.BaseParameter.ID = element.ID;
      this.CategoryDepartmentService.RemoveAsync().subscribe(
        res => {
          this.NotificationService.warn(environment.DeleteSuccess);
          this.CategoryDepartmentSearch();
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.CategoryDepartmentService.IsShowLoading = false;
        }
      );
    }
  }
  CreateAutoAsync() {
    this.CategoryDepartmentService.IsShowLoading = true;
    this.CategoryDepartmentService.CreateAutoAsync().subscribe(
      res => {
        this.CategoryDepartmentSearch();
      },
      err => {
      },
      () => {
        this.CategoryDepartmentService.IsShowLoading = false;
      }
    );
  }
}

