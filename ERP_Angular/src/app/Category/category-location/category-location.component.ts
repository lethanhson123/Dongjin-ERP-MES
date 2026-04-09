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

import { CategoryRack } from 'src/app/shared/ERP/CategoryRack.model';
import { CategoryRackService } from 'src/app/shared/ERP/CategoryRack.service';

import { CategoryLocation } from 'src/app/shared/ERP/CategoryLocation.model';
import { CategoryLocationService } from 'src/app/shared/ERP/CategoryLocation.service';
import { CategoryLocationModalComponent } from '../category-location-modal/category-location-modal.component';

@Component({
  selector: 'app-category-location',
  templateUrl: './category-location.component.html',
  styleUrls: ['./category-location.component.css']
})
export class CategoryLocationComponent {

  @ViewChild('CategoryLocationSort') CategoryLocationSort: MatSort;
  @ViewChild('CategoryLocationPaginator') CategoryLocationPaginator: MatPaginator;

  URLTemplate: string;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryLocationService: CategoryLocationService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public CategoryRackService: CategoryRackService,
    public CompanyService: CompanyService,
  ) {
    this.URLTemplate = this.CategoryLocationService.APIRootURL + "Download/CategoryLocation.xlsx";
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  CategoryLocationChange(event, files: FileList) {
    if (files) {
      this.CategoryLocationService.FileToUpload = files;
      this.CategoryLocationService.BaseParameter.Event = event;
    }
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.CategoryLocationService);
    this.CompanyChange();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.CategoryLocationService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.CategoryLocationService);
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.CategoryLocationService);
  }
  CategoryRackSearch() {
    this.CategoryRackService.BaseParameter.Active = true;
    this.CategoryRackService.BaseParameter.CompanyID = this.CategoryLocationService.BaseParameter.CompanyID;
    this.CategoryRackService.BaseParameter.ParentID = this.CategoryLocationService.BaseParameter.CategoryDepartmentID;
    this.CategoryRackService.ComponentGetByParentIDAndCompanyIDAndActiveToListAsync(this.CategoryLocationService);
  }
  CategoryDepartmentChange() {
    this.CategoryRackSearch();
  }
  CategoryLocationSearch() {
    if (this.CategoryLocationService.BaseParameter.SearchString.length > 0) {
      this.CategoryLocationService.BaseParameter.SearchString = this.CategoryLocationService.BaseParameter.SearchString.trim();
      if (this.CategoryLocationService.DataSource) {
        this.CategoryLocationService.DataSource.filter = this.CategoryLocationService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.CategoryLocationService.IsShowLoading = true;
      this.CategoryLocationService.GetByCategoryDepartmentIDToListAsync().subscribe(
        res => {
          this.CategoryLocationService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.CategoryLocationService.DataSource = new MatTableDataSource(this.CategoryLocationService.List);
          this.CategoryLocationService.DataSource.sort = this.CategoryLocationSort;
          this.CategoryLocationService.DataSource.paginator = this.CategoryLocationPaginator;
        },
        err => {
        },
        () => {
          this.CategoryLocationService.IsShowLoading = false;
        }
      );
    }
  }
  Save(element: CategoryLocation) {
    this.CategoryLocationService.IsShowLoading = true;
    this.CategoryLocationService.BaseParameter.BaseModel = element;
    this.CategoryLocationService.SaveAsync().subscribe(
      res => {
        this.CategoryLocationService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategoryLocationService.IsShowLoading = false;
      }
    );
  }
  Delete(element: CategoryLocation) {
    this.CategoryLocationService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryLocationService.ComponentDeleteAll(this.CategoryLocationSort, this.CategoryLocationPaginator));
  }
  CategoryLocationModal(element: CategoryLocation) {
    this.CategoryLocationService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(CategoryLocationModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
      //this.CategoryLocationSearch();
    });
  }
  Add() {
    this.CategoryLocationService.IsShowLoading = true;
    this.CategoryLocationService.BaseParameter.ID = environment.InitializationNumber;
    this.CategoryLocationService.GetByIDAsync().subscribe(
      res => {
        this.CategoryLocationService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.CategoryLocationModal(this.CategoryLocationService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.CategoryLocationService.IsShowLoading = false;
      }
    );
  }
  Print(element: CategoryLocation) {
    this.CategoryLocationService.IsShowLoading = true;
    this.CategoryLocationService.BaseParameter.BaseModel = element;
    this.CategoryLocationService.PrintAsync().subscribe(
      res => {
        this.CategoryLocationService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.CategoryLocationService.BaseResult.Message);
      },
      err => {
      },
      () => {
        this.CategoryLocationService.IsShowLoading = false;
      }
    );
  }
  PrintByCategoryDepartment() {
    if (this.CategoryLocationService.BaseParameter.ParentID > 0) {
      this.PrintByParentID();
    }
    else {
      this.CategoryLocationService.IsShowLoading = true;
      this.CategoryLocationService.PrintByCategoryDepartmentIDAsync().subscribe(
        res => {
          this.CategoryLocationService.BaseResult = (res as BaseResult);
          this.NotificationService.OpenWindowByURL(this.CategoryLocationService.BaseResult.Message);
        },
        err => {
        },
        () => {
          this.CategoryLocationService.IsShowLoading = false;
        }
      );
    }
  }
  PrintByParentID() {
    this.CategoryLocationService.IsShowLoading = true;
    this.CategoryLocationService.PrintByParentIDAsync().subscribe(
      res => {
        this.CategoryLocationService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.CategoryLocationService.BaseResult.Message);
      },
      err => {
      },
      () => {
        this.CategoryLocationService.IsShowLoading = false;
      }
    );
  }
  SaveAndUploadFileAsync() {
    this.CategoryLocationService.IsShowLoading = true;
    this.CategoryLocationService.SaveAndUploadFileAsync().subscribe(
      res => {

        this.CategoryLocationService.FileToUpload = null;
        this.CategoryLocationService.BaseParameter.Event = null;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategoryLocationService.IsShowLoading = false;
      }
    );
  }
}
