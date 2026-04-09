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

import { WarehouseStock } from 'src/app/shared/ERP/WarehouseStock.model';
import { WarehouseStockService } from 'src/app/shared/ERP/WarehouseStock.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

import { ProductionOrder } from 'src/app/shared/ERP/ProductionOrder.model';
import { ProductionOrderService } from 'src/app/shared/ERP/ProductionOrder.service';

@Component({
  selector: 'app-warehouse-stock-by-invoice',
  templateUrl: './warehouse-stock-by-invoice.component.html',
  styleUrls: ['./warehouse-stock-by-invoice.component.css']
})
export class WarehouseStockByInvoiceComponent {

  @ViewChild('WarehouseStockSort') WarehouseStockSort: MatSort;
  @ViewChild('WarehouseStockPaginator') WarehouseStockPaginator: MatPaginator;

  URLTemplate: string;

  PageSize: number = environment.PageSize;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseStockService: WarehouseStockService,
    public CompanyService: CompanyService,
    public MaterialService: MaterialService,
    public CategoryDepartmentService: CategoryDepartmentService,

    public ProductionOrderService: ProductionOrderService,

  ) {
    this.URLTemplate = this.WarehouseStockService.APIRootURL + "Download/WarehouseStock.xlsx";
    this.WarehouseStockService.BaseParameter.Action = 2;
    this.CompanySearch();
    this.ComponentGetYearList();
    this.ComponentGetMonthList();
    this.ComponentGetDayList();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {

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
          this.WarehouseStockService.List = (res as BaseResult).List;
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
  MaterialModal(element: WarehouseStock) {
    this.WarehouseStockService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = element.ParentID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        const dialog = this.Dialog.open(MaterialModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => {

        });
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

    let filename = this.CompanyService.BaseParameter.BaseModel.Code + "-" + this.WarehouseStockService.BaseParameter.Year + "-" + this.WarehouseStockService.BaseParameter.Month + "-" + this.WarehouseStockService.BaseParameter.Day + "-StockByInvoice.xlsx";
    XLSX.writeFile(wb, filename);
  }
}
