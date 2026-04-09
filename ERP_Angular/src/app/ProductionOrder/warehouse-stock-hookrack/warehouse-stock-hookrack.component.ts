import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

import { WarehouseStock } from 'src/app/shared/ERP/WarehouseStock.model';
import { WarehouseStockService } from 'src/app/shared/ERP/WarehouseStock.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-warehouse-stock-hookrack',
  templateUrl: './warehouse-stock-hookrack.component.html',
  styleUrls: ['./warehouse-stock-hookrack.component.css']
})
export class WarehouseStockHOOKRACKComponent {

  @ViewChild('WarehouseStockSort') WarehouseStockSort: MatSort;
  @ViewChild('WarehouseStockPaginator') WarehouseStockPaginator: MatPaginator;

  URLTemplate: string;

  PageSize: number = environment.PageSize;

  constructor(
    public Dialog: MatDialog,
    public Sanitizer: DomSanitizer,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseStockService: WarehouseStockService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public CompanyService: CompanyService,

  ) {
    this.WarehouseStockService.BaseParameter.Active = true;
    this.URLTemplate = this.WarehouseStockService.APIRootURL + "Download/WarehouseStock.xlsx";
    this.WarehouseStockService.BaseParameter.Action = 4;
    this.CompanySearch();
    this.ComponentGetYearList();
    this.ComponentGetMonthList();
    this.ComponentGetDayList();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {

  }
  HTMLTrusted(value: string) {
    return this.Sanitizer.bypassSecurityTrustHtml(value);
  }
  FileChange(event, files: FileList) {
    if (files) {
      this.WarehouseStockService.FileToUpload = files;
      this.WarehouseStockService.BaseParameter.Event = event;
    }
  }
  CompanySearch() {
    this.WarehouseStockService.IsShowLoading = true;
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      res => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.WarehouseStockService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
        }
        this.CompanyChange();
      },
      err => {
      },
      () => {
        this.WarehouseStockService.IsShowLoading = false;
      }
    );
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  ComponentGetYearList() {
    this.WarehouseStockService.ComponentGetYearList();
  }
  ComponentGetMonthList() {
    this.WarehouseStockService.ComponentGetMonthList();
  }
  ComponentGetDayList() {
    this.WarehouseStockService.ComponentGetDayList();
  }
  CategoryDepartmentSearch() {
    this.WarehouseStockService.IsShowLoading = true;
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.WarehouseStockService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.GetByMembershipID_CompanyID_ActiveToListAsync().subscribe(
      res => {
        this.CategoryDepartmentService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.CategoryDepartmentService.ListFilter = this.CategoryDepartmentService.ListFilter.filter(o => o.Code.includes("Warehouse") || o.Code.includes("FinishGoods") || o.Code.includes("HOOKRACK"));
        if (this.CategoryDepartmentService.ListFilter && this.CategoryDepartmentService.ListFilter.length > 0) {
          if (this.CategoryDepartmentService.BaseParameter.CompanyID == environment.CompanyIDDJG) {
            this.WarehouseStockService.BaseParameter.CategoryDepartmentID = environment.DepartmentID;
          }
          else {
            this.WarehouseStockService.BaseParameter.CategoryDepartmentID = this.CategoryDepartmentService.ListFilter[0].ID;
          }
        }
      },
      err => {
      },
      () => {
        this.WarehouseStockService.IsShowLoading = false;
      }
    );
  }
  WarehouseStockSearch() {
    if (this.WarehouseStockService.BaseParameter.SearchString.length > 0) {
      this.WarehouseStockService.BaseParameter.SearchString = this.WarehouseStockService.BaseParameter.SearchString.trim();
      if (this.WarehouseStockService.DataSource) {
        this.WarehouseStockService.DataSource.filter = this.WarehouseStockService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseStockService.IsShowLoading = true;
      this.WarehouseStockService.GetByCompanyIDAndCategoryDepartmentIDAndYearAndMonthAndDayAndActionToListAsync().subscribe(
        res => {
          this.WarehouseStockService.List = (res as BaseResult).List.sort((a, b) => (a.QuantityStock < b.QuantityStock ? 1 : -1));
          this.WarehouseStockService.DataSource = new MatTableDataSource(this.WarehouseStockService.List);
          this.WarehouseStockService.DataSource.sort = this.WarehouseStockSort;
          this.WarehouseStockService.DataSource.paginator = this.WarehouseStockPaginator;
        },
        err => {
        },
        () => {
          this.WarehouseStockService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseStockSync() {
    this.WarehouseStockService.IsShowLoading = true;
    this.WarehouseStockService.SyncAsync().subscribe(
      res => {
        this.WarehouseStockSearch();
      },
      err => {
      },
      () => {
        this.WarehouseStockService.IsShowLoading = false;
      }
    );
  }

  @ViewChild("TABLE") table: ElementRef;
  WarehouseStockExcel() {

    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");

    let ListCompany = this.CompanyService.ListFilter.filter(o => o.ID == this.WarehouseStockService.BaseParameter.CompanyID);
    if (ListCompany && ListCompany.length > 0) {
      this.CompanyService.BaseParameter.BaseModel = ListCompany[0];
    }

    let ListCategoryDepartment = this.CategoryDepartmentService.ListFilter.filter(o => o.ID == this.WarehouseStockService.BaseParameter.CategoryDepartmentID);
    if (ListCategoryDepartment && ListCategoryDepartment.length > 0) {
      this.CategoryDepartmentService.BaseParameter.BaseModel = ListCategoryDepartment[0];
    }
    let filename = this.CompanyService.BaseParameter.BaseModel.Code + "-" + this.CategoryDepartmentService.BaseParameter.BaseModel.Code + "-WarehouseStock.xlsx";
    XLSX.writeFile(wb, filename);
  }
}