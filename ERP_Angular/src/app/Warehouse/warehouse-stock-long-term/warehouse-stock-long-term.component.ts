import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
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

import { Report } from 'src/app/shared/ERP/Report.model';
import { ReportService } from 'src/app/shared/ERP/Report.service';

import { ReportDetail } from 'src/app/shared/ERP/ReportDetail.model';
import { ReportDetailService } from 'src/app/shared/ERP/ReportDetail.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

@Component({
  selector: 'app-warehouse-stock-long-term',
  templateUrl: './warehouse-stock-long-term.component.html',
  styleUrls: ['./warehouse-stock-long-term.component.css']
})
export class WarehouseStockLongTermComponent {

  @ViewChild('ReportDetailSort') ReportDetailSort: MatSort;
  @ViewChild('ReportDetailPaginator') ReportDetailPaginator: MatPaginator;

  @ViewChild('ReportDetailFilterSort') ReportDetailFilterSort: MatSort;
  @ViewChild('ReportDetailFilterPaginator') ReportDetailFilterPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ReportService: ReportService,
    public ReportDetailService: ReportDetailService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,

  ) {
    this.CompanySearch();

  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {

  }

  CompanySearch() {
    this.ReportService.IsShowLoading = true;
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      res => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.ReportService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
        }
        this.CompanyChange();
      },
      err => {
      },
      () => {
        this.ReportService.IsShowLoading = false;
      }
    );
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }

  CategoryDepartmentSearch() {
    this.ReportService.IsShowLoading = true;
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.ReportService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.GetByMembershipID_CompanyID_ActiveToListAsync().subscribe(
      res => {
        this.CategoryDepartmentService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CategoryDepartmentService.ListFilter && this.CategoryDepartmentService.ListFilter.length > 0) {
          if (this.CategoryDepartmentService.BaseParameter.CompanyID == environment.CompanyIDDJG) {
            this.ReportService.BaseParameter.CategoryDepartmentID = environment.DepartmentID;
          }
          else {
            this.ReportService.BaseParameter.CategoryDepartmentID = this.CategoryDepartmentService.ListFilter[0].ID;
          }
        }
      },
      err => {
      },
      () => {
        this.ReportService.IsShowLoading = false;
      }
    );
  }
  ReportSearch() {
    this.ReportService.IsShowLoading = true;
    this.ReportService.GetByWarehouseStockLongTermAsync().subscribe(
      res => {
        this.ReportService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        if (this.ReportService.BaseParameter.BaseModel) {
          if (this.ReportService.BaseParameter.BaseModel.ID > 0) {
            if (this.ReportService.BaseParameter.SearchString.length > 0) {
              this.ReportService.BaseParameter.SearchString = this.ReportService.BaseParameter.SearchString.trim();
              if (this.ReportDetailService.DataSource) {
                this.ReportDetailService.DataSource.filter = this.ReportService.BaseParameter.SearchString.toLowerCase();
              }
              if (this.ReportDetailService.DataSourceFilter) {
                this.ReportDetailService.DataSourceFilter.filter = this.ReportService.BaseParameter.SearchString.toLowerCase();
              }
              this.ReportService.IsShowLoading = false;
            }
            else {

              this.ReportDetailService.BaseParameter.ParentID = this.ReportService.BaseParameter.BaseModel.ID;
              this.ReportService.IsShowLoading = true;
              this.ReportDetailService.GetWarehouseStockLongTermByParentIDToListAsync().subscribe(
                res => {
                  this.ReportDetailService.List = (res as BaseResult).List;
                  this.ReportDetailService.DataSource = new MatTableDataSource(this.ReportDetailService.List);
                  this.ReportDetailService.DataSource.sort = this.ReportDetailSort;
                  this.ReportDetailService.DataSource.paginator = this.ReportDetailPaginator;
                },
                err => {
                },
                () => {
                  //this.ReportService.IsShowLoading = false;
                }
              );
              this.ReportService.IsShowLoading = true;
              this.ReportDetailService.GetWarehouseStockLongTerm1000ByParentIDToListAsync().subscribe(
                res => {
                  this.ReportDetailService.ListFilter = (res as BaseResult).List;
                  this.ReportDetailService.DataSourceFilter = new MatTableDataSource(this.ReportDetailService.ListFilter);
                  this.ReportDetailService.DataSourceFilter.sort = this.ReportDetailFilterSort;
                  this.ReportDetailService.DataSourceFilter.paginator = this.ReportDetailFilterPaginator;
                },
                err => {
                },
                () => {
                  this.ReportService.IsShowLoading = false;
                }
              );
            }
          }
        }
        else {
          this.ReportService.IsShowLoading = false;
        }
      },
      err => {
      },
      () => {
        //this.ReportService.IsShowLoading = false;
      }
    );
  }
  ReportSync() {
    this.ReportService.IsShowLoading = true;
    this.ReportService.SyncByWarehouseStockLongTermAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.Synchronizing);
        this.ReportSearch();
      },
      err => {
      },
      () => {
        this.ReportService.IsShowLoading = false;
      }
    );
  }
  @ViewChild("TABLE") table: ElementRef;
  @ViewChild("TABLE1000") table1000: ElementRef;
  ReportExcel() {

    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");

    let ListCompany = this.CompanyService.ListFilter.filter(o => o.ID == this.ReportService.BaseParameter.CompanyID);
    if (ListCompany && ListCompany.length > 0) {
      this.CompanyService.BaseParameter.BaseModel = ListCompany[0];
    }

    let ListCategoryDepartment = this.CategoryDepartmentService.ListFilter.filter(o => o.ID == this.ReportService.BaseParameter.CategoryDepartmentID);
    if (ListCategoryDepartment && ListCategoryDepartment.length > 0) {
      this.CategoryDepartmentService.BaseParameter.BaseModel = ListCategoryDepartment[0];
    }
    let Code = this.CompanyService.BaseParameter.BaseModel.Code + "-" + this.CategoryDepartmentService.BaseParameter.BaseModel.Code;
    let filename = Code + "-ReportLongTermPartNo.xlsx";
    XLSX.writeFile(wb, filename);

    const ws1000: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table1000.nativeElement);
    const wb1000: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb1000, ws1000, "Sheet1");


    filename = Code + "-ReportLongTermBarcode.xlsx";
    XLSX.writeFile(wb1000, filename);
  }
}
