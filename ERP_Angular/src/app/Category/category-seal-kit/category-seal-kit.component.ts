import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { CategorySealKit } from 'src/app/shared/ERP/CategorySealKit.model';
import { CategorySealKitService } from 'src/app/shared/ERP/CategorySealKit.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-category-seal-kit',
  templateUrl: './category-seal-kit.component.html',
  styleUrls: ['./category-seal-kit.component.css']
})
export class CategorySealKitComponent implements OnInit {

  @ViewChild('CategorySealKitSort') CategorySealKitSort: MatSort;
  @ViewChild('CategorySealKitPaginator') CategorySealKitPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategorySealKitService: CategorySealKitService,
    public CompanyService: CompanyService,

  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategorySealKitSearch();
  }
  CompanySearch() {
    this.CategorySealKitService.IsShowLoading = true;
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.GetByActiveToListAsync().subscribe(
      res => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter) {
          if (this.CompanyService.ListFilter.length > 0) {
            this.CategorySealKitService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
            this.CategorySealKitSearch();
          }
        }
      },
      err => {
      },
      () => {
        this.CategorySealKitService.IsShowLoading = false;
      }
    );
  }
  CategorySealKitSearch() {
    if (this.CategorySealKitService.BaseParameter.SearchString.length > 0) {
      this.CategorySealKitService.BaseParameter.SearchString = this.CategorySealKitService.BaseParameter.SearchString.trim();
      if (this.CategorySealKitService.DataSource) {
        this.CategorySealKitService.DataSource.filter = this.CategorySealKitService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.CategorySealKitService.IsShowLoading = true;
      this.CategorySealKitService.GetByCompanyIDAndEmptyToListAsync().subscribe(
        res => {
          this.CategorySealKitService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.CategorySealKitService.DataSource = new MatTableDataSource(this.CategorySealKitService.List);
          this.CategorySealKitService.DataSource.sort = this.CategorySealKitSort;
          this.CategorySealKitService.DataSource.paginator = this.CategorySealKitPaginator;
        },
        err => {
        },
        () => {
          this.CategorySealKitService.IsShowLoading = false;
        }
      );
    }
  }
  CategorySealKitSave(element: CategorySealKit) {
    this.CategorySealKitService.IsShowLoading = true;
    element.CompanyID = this.CategorySealKitService.BaseParameter.CompanyID;
    this.CategorySealKitService.BaseParameter.BaseModel = element;
    this.CategorySealKitService.SaveAsync().subscribe(
      res => {
        this.CategorySealKitSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategorySealKitService.IsShowLoading = false;
      }
    );
  }
  CategorySealKitDelete(element: CategorySealKit) {
    if (confirm(environment.DeleteConfirm)) {
      this.CategorySealKitService.IsShowLoading = true;
      this.CategorySealKitService.BaseParameter.ID = element.ID;
      this.CategorySealKitService.RemoveAsync().subscribe(
        res => {
          this.CategorySealKitSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.CategorySealKitService.IsShowLoading = false;
        }
      );
      return environment.DeleteSuccess;
    }
  }
}