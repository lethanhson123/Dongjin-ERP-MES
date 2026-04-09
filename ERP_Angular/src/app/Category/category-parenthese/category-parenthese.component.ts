import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { CategoryParenthese } from 'src/app/shared/ERP/CategoryParenthese.model';
import { CategoryParentheseService } from 'src/app/shared/ERP/CategoryParenthese.service';



import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-category-parenthese',
  templateUrl: './category-parenthese.component.html',
  styleUrls: ['./category-parenthese.component.css']
})
export class CategoryParentheseComponent {

  @ViewChild('CategoryParentheseSort') CategoryParentheseSort: MatSort;
  @ViewChild('CategoryParenthesePaginator') CategoryParenthesePaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryParentheseService: CategoryParentheseService,
    public CompanyService: CompanyService,
    
  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryParentheseSearch();
  }
  CompanySearch() {
    this.CategoryParentheseService.IsShowLoading = true;
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.GetByActiveToListAsync().subscribe(
      res => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter) {
          if (this.CompanyService.ListFilter.length > 0) {
            this.CategoryParentheseService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
           
          }
        }
      },
      err => {
      },
      () => {
        this.CategoryParentheseService.IsShowLoading = false;
      }
    );
  }  
  CategoryParentheseSearch() {
    if (this.CategoryParentheseService.BaseParameter.SearchString.length > 0) {
      this.CategoryParentheseService.BaseParameter.SearchString = this.CategoryParentheseService.BaseParameter.SearchString.trim();
      if (this.CategoryParentheseService.DataSource) {
        this.CategoryParentheseService.DataSource.filter = this.CategoryParentheseService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.CategoryParentheseService.IsShowLoading = true;
      this.CategoryParentheseService.GetByCompanyIDAndEmptyToListAsync().subscribe(
        res => {
          this.CategoryParentheseService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.CategoryParentheseService.DataSource = new MatTableDataSource(this.CategoryParentheseService.List);
          this.CategoryParentheseService.DataSource.sort = this.CategoryParentheseSort;
          this.CategoryParentheseService.DataSource.paginator = this.CategoryParenthesePaginator;
        },
        err => {
        },
        () => {
          this.CategoryParentheseService.IsShowLoading = false;
        }
      );
    }
  }
  CategoryParentheseSave(element: CategoryParenthese) {
    this.CategoryParentheseService.IsShowLoading = true;
    element.CompanyID = this.CategoryParentheseService.BaseParameter.CompanyID;
    this.CategoryParentheseService.BaseParameter.BaseModel = element;
    this.CategoryParentheseService.SaveAsync().subscribe(
      res => {
        this.CategoryParentheseSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategoryParentheseService.IsShowLoading = false;
      }
    );
  }
  CategoryParentheseDelete(element: CategoryParenthese) {
    if (confirm(environment.DeleteConfirm)) {
      this.CategoryParentheseService.IsShowLoading = true;
      this.CategoryParentheseService.BaseParameter.ID = element.ID;
      this.CategoryParentheseService.RemoveAsync().subscribe(
        res => {
          this.CategoryParentheseSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.CategoryParentheseService.IsShowLoading = false;
        }
      );
      return environment.DeleteSuccess;
    }
  }
}

