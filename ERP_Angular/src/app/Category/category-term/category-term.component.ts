import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { CategoryTerm } from 'src/app/shared/ERP/CategoryTerm.model';
import { CategoryTermService } from 'src/app/shared/ERP/CategoryTerm.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

@Component({
  selector: 'app-category-term',
  templateUrl: './category-term.component.html',
  styleUrls: ['./category-term.component.css']
})
export class CategoryTermComponent {

  @ViewChild('CategoryTermSort') CategoryTermSort: MatSort;
  @ViewChild('CategoryTermPaginator') CategoryTermPaginator: MatPaginator;

  URLTemplate: string;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryTermService: CategoryTermService,
    public CompanyService: CompanyService,

  ) {
    this.URLTemplate = this.CategoryTermService.APIRootURL + "Download/CategoryTerm.xlsx";

    this.CategoryTermService.BaseParameter.Active = true;
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryTermSearch();
  }
  CategoryTermFileChange(event, files: FileList) {
    if (files) {
      this.CategoryTermService.FileToUpload = files;
      this.CategoryTermService.BaseParameter.Event = event;
    }
  }
  CompanySearch() {
    this.CategoryTermService.IsShowLoading = true;
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.GetByActiveToListAsync().subscribe(
      res => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter) {
          if (this.CompanyService.ListFilter.length > 0) {
            this.CategoryTermService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          }
        }
      },
      err => {
      },
      () => {
        this.CategoryTermService.IsShowLoading = false;
      }
    );
  }
  CategoryTermSearch() {
    if (this.CategoryTermService.BaseParameter.SearchString.length > 0) {
      this.CategoryTermService.BaseParameter.SearchString = this.CategoryTermService.BaseParameter.SearchString.trim();
      if (this.CategoryTermService.DataSource) {
        this.CategoryTermService.DataSource.filter = this.CategoryTermService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.CategoryTermService.IsShowLoading = true;
      this.CategoryTermService.GetByCompanyIDAndEmptyToListAsync().subscribe(
        res => {
          this.CategoryTermService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.CategoryTermService.DataSource = new MatTableDataSource(this.CategoryTermService.List);
          this.CategoryTermService.DataSource.sort = this.CategoryTermSort;
          this.CategoryTermService.DataSource.paginator = this.CategoryTermPaginator;
        },
        err => {
        },
        () => {
          this.CategoryTermService.IsShowLoading = false;
        }
      );
    }
  }
  CategoryTermSave(element: CategoryTerm) {
    this.CategoryTermService.IsShowLoading = true;
    element.CompanyID = this.CategoryTermService.BaseParameter.CompanyID;
    this.CategoryTermService.BaseParameter.BaseModel = element;
    this.CategoryTermService.SaveAsync().subscribe(
      res => {
        this.CategoryTermSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategoryTermService.IsShowLoading = false;
      }
    );
  }
  CategoryTermSaveAndUploadFile() {
    this.CategoryTermService.IsShowLoading = true;
    this.CategoryTermService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.CategoryTermSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategoryTermService.IsShowLoading = false;
      }
    );
  }
  CategoryTermDelete(element: CategoryTerm) {
    if (confirm(environment.DeleteConfirm)) {
      this.CategoryTermService.IsShowLoading = true;
      this.CategoryTermService.BaseParameter.ID = element.ID;
      this.CategoryTermService.RemoveAsync().subscribe(
        res => {
          this.CategoryTermSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.CategoryTermService.IsShowLoading = false;
        }
      );
      return environment.DeleteSuccess;
    }
  }
  @ViewChild("TABLE") table: ElementRef;
  CategoryTermExcel() {
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");
    let filename = "CategoryTerm.xlsx";
    XLSX.writeFile(wb, filename);
  }
}

