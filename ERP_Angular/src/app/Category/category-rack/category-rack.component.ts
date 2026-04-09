import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { CategoryRack } from 'src/app/shared/ERP/CategoryRack.model';
import { CategoryRackService } from 'src/app/shared/ERP/CategoryRack.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-category-rack',
  templateUrl: './category-rack.component.html',
  styleUrls: ['./category-rack.component.css']
})
export class CategoryRackComponent {

  @ViewChild('CategoryRackSort') CategoryRackSort: MatSort;
  @ViewChild('CategoryRackPaginator') CategoryRackPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryRackService: CategoryRackService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,

  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryRackSearch();
  }
  CompanySearch() {
    this.CategoryRackService.IsShowLoading = true;
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.GetByActiveToListAsync().subscribe(
      res => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter) {
          if (this.CompanyService.ListFilter.length > 0) {
            this.CategoryRackService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
            this.CategoryDepartmentSearch();
          }
        }
      },
      err => {
      },
      () => {
        this.CategoryRackService.IsShowLoading = false;
      }
    );
  }
  CategoryDepartmentSearch() {
    this.CategoryRackService.IsShowLoading = true;
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.CategoryRackService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.GetByCompanyIDAndActiveToListAsync().subscribe(
      res => {
        this.CategoryDepartmentService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      },
      err => {
      },
      () => {
        this.CategoryRackService.IsShowLoading = false;
      }
    );
  }
  CategoryRackSearch() {
    if (this.CategoryRackService.BaseParameter.SearchString.length > 0) {
      this.CategoryRackService.BaseParameter.SearchString = this.CategoryRackService.BaseParameter.SearchString.trim();
      if (this.CategoryRackService.DataSource) {
        this.CategoryRackService.DataSource.filter = this.CategoryRackService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.CategoryRackService.IsShowLoading = true;
      this.CategoryRackService.GetByCompanyIDAndEmptyToListAsync().subscribe(
        res => {
          this.CategoryRackService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.CategoryRackService.DataSource = new MatTableDataSource(this.CategoryRackService.List);
          this.CategoryRackService.DataSource.sort = this.CategoryRackSort;
          this.CategoryRackService.DataSource.paginator = this.CategoryRackPaginator;
        },
        err => {
        },
        () => {
          this.CategoryRackService.IsShowLoading = false;
        }
      );
    }
  }
  CategoryRackSave(element: CategoryRack) {
    this.CategoryRackService.IsShowLoading = true;
    element.CompanyID = this.CategoryRackService.BaseParameter.CompanyID;
    this.CategoryRackService.BaseParameter.BaseModel = element;
    this.CategoryRackService.SaveAsync().subscribe(
      res => {
        this.CategoryRackSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategoryRackService.IsShowLoading = false;
      }
    );
  }
  CategoryRackDelete(element: CategoryRack) {
    if (confirm(environment.DeleteConfirm)) {
      this.CategoryRackService.IsShowLoading = true;
      this.CategoryRackService.BaseParameter.ID = element.ID;
      this.CategoryRackService.RemoveAsync().subscribe(
        res => {
          this.CategoryRackSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.CategoryRackService.IsShowLoading = false;
        }
      );
      return environment.DeleteSuccess;
    }
  }
}
