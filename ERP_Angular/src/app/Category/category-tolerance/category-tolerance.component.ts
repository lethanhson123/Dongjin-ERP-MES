import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { CategoryTolerance } from 'src/app/shared/ERP/CategoryTolerance.model';
import { CategoryToleranceService } from 'src/app/shared/ERP/CategoryTolerance.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-category-tolerance',
  templateUrl: './category-tolerance.component.html',
  styleUrls: ['./category-tolerance.component.css']
})
export class CategoryToleranceComponent {

@ViewChild('CategoryToleranceSort') CategoryToleranceSort: MatSort;
  @ViewChild('CategoryTolerancePaginator') CategoryTolerancePaginator: MatPaginator;

   URLTemplate: string;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryToleranceService: CategoryToleranceService,
    public CompanyService: CompanyService,

  ) {
    this.URLTemplate = this.CategoryToleranceService.APIRootURL + "Download/CategoryTolerance.xlsx";
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryToleranceSearch();
  }
  CategoryToleranceFileChange(event, files: FileList) {
    if (files) {
      this.CategoryToleranceService.FileToUpload = files;
      this.CategoryToleranceService.BaseParameter.Event = event;
    }
  }
  CompanySearch() {
    this.CategoryToleranceService.IsShowLoading = true;
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.GetByActiveToListAsync().subscribe(
      res => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter) {
          if (this.CompanyService.ListFilter.length > 0) {
            this.CategoryToleranceService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          }
        }
      },
      err => {
      },
      () => {
        this.CategoryToleranceService.IsShowLoading = false;
      }
    );
  }
  CategoryToleranceSearch() {
    if (this.CategoryToleranceService.BaseParameter.SearchString.length > 0) {
      this.CategoryToleranceService.BaseParameter.SearchString = this.CategoryToleranceService.BaseParameter.SearchString.trim();
      if (this.CategoryToleranceService.DataSource) {
        this.CategoryToleranceService.DataSource.filter = this.CategoryToleranceService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.CategoryToleranceService.IsShowLoading = true;
      this.CategoryToleranceService.GetByCompanyIDAndEmptyToListAsync().subscribe(
        res => {
          this.CategoryToleranceService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.CategoryToleranceService.DataSource = new MatTableDataSource(this.CategoryToleranceService.List);
          this.CategoryToleranceService.DataSource.sort = this.CategoryToleranceSort;
          this.CategoryToleranceService.DataSource.paginator = this.CategoryTolerancePaginator;
        },
        err => {
        },
        () => {
          this.CategoryToleranceService.IsShowLoading = false;
        }
      );
    }
  }
  CategoryToleranceSave(element: CategoryTolerance) {
    this.CategoryToleranceService.IsShowLoading = true;
    element.CompanyID = this.CategoryToleranceService.BaseParameter.CompanyID;
    this.CategoryToleranceService.BaseParameter.BaseModel = element;
    this.CategoryToleranceService.SaveAsync().subscribe(
      res => {
        this.CategoryToleranceSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategoryToleranceService.IsShowLoading = false;
      }
    );
  }
  CategoryToleranceSaveAndUploadFile() {
    this.CategoryToleranceService.IsShowLoading = true;
    this.CategoryToleranceService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.CategoryToleranceSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategoryToleranceService.IsShowLoading = false;
      }
    );
  }
  CategoryToleranceDelete(element: CategoryTolerance) {
    if (confirm(environment.DeleteConfirm)) {
      this.CategoryToleranceService.IsShowLoading = true;
      this.CategoryToleranceService.BaseParameter.ID = element.ID;
      this.CategoryToleranceService.RemoveAsync().subscribe(
        res => {
          this.CategoryToleranceSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.CategoryToleranceService.IsShowLoading = false;
        }
      );
      return environment.DeleteSuccess;
    }
  }
}

