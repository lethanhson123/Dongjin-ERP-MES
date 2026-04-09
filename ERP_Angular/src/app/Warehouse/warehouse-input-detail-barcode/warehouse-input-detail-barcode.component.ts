import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

import { WarehouseInputDetailBarcode } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.model';
import { WarehouseInputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.service';

import { WarehouseOutputDetailBarcode } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.model';
import { WarehouseOutputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';
import { WarehouseOutputDetailBarcodeHistoryModalComponent } from '../warehouse-output-detail-barcode-history-modal/warehouse-output-detail-barcode-history-modal.component';

@Component({
  selector: 'app-warehouse-input-detail-barcode',
  templateUrl: './warehouse-input-detail-barcode.component.html',
  styleUrls: ['./warehouse-input-detail-barcode.component.css']
})
export class WarehouseInputDetailBarcodeComponent {

  @ViewChild('SearchString') SearchString!: ElementRef;

  @ViewChild('WarehouseInputDetailBarcodeSort') WarehouseInputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginator') WarehouseInputDetailBarcodePaginator: MatPaginator;

  Quantity: number = 0;
  Output: number = 0;
  Stock: number = 0;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,


    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,
    public WarehouseOutputDetailBarcodeService: WarehouseOutputDetailBarcodeService,
    public MaterialService: MaterialService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,
  ) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.Active = true;
    this.WarehouseInputDetailBarcodeService.BaseParameter.IsComplete = true;
    this.WarehouseInputDetailBarcodeService.List = [];
    this.ComponentGetYearList();
    this.ComponentGetMonthList();
    this.ComponentGetDayList();
    this.CompanySearch();

    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = this.ActiveRouter.snapshot.params.SearchString;
        //this.WarehouseInputDetailBarcodeSearch();
      }
    });
  }
  ngOnInit(): void {
  }
  ngAfterViewInit() {
    //this.WarehouseInputDetailBarcodeSearch();
  }
  DateBegin(value) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.DateEnd = new Date(value);
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.WarehouseInputDetailBarcodeService);
    this.CompanyChange();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.WarehouseInputDetailBarcodeService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.WarehouseInputDetailBarcodeService);
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.WarehouseInputDetailBarcodeService);
  }
  ComponentGetYearList() {
    this.WarehouseInputDetailBarcodeService.ComponentGetYearList();
  }
  ComponentGetMonthList() {
    this.WarehouseInputDetailBarcodeService.ComponentGetMonthList();
  }
  ComponentGetDayList() {
    this.WarehouseInputDetailBarcodeService.ComponentGetDayList();
  }
  MaterialModal(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = element.MaterialID;
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
        this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodeSearch() {
    this.Quantity = 0;
    this.Output = 0;
    this.Stock = 0;
    this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
    if (this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString == null) {
      this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = environment.InitializationString;
    }
    this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.trim();
    this.WarehouseInputDetailBarcodeService.GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_FIFO_StockToListAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.List = (res as BaseResult).List;
        this.WarehouseInputDetailBarcodeService.ListFilter = this.WarehouseInputDetailBarcodeService.List.filter(o => o.IsStock != true);

        let ListWarehouseInputDetailBarcodeStock = this.WarehouseInputDetailBarcodeService.List.filter(o => o.IsStock == true);
        if (ListWarehouseInputDetailBarcodeStock && ListWarehouseInputDetailBarcodeStock.length > 0) {
          this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel = ListWarehouseInputDetailBarcodeStock[0];
        }
        // if (this.WarehouseInputDetailBarcodeService.BaseParameter.Active == true) {
        //   this.WarehouseInputDetailBarcodeService.ListFilter = this.WarehouseInputDetailBarcodeService.List.filter(o => o.Year == this.WarehouseInputDetailBarcodeService.BaseParameter.Year && o.Month == this.WarehouseInputDetailBarcodeService.BaseParameter.Month && o.Day == this.WarehouseInputDetailBarcodeService.BaseParameter.Day);
        // }
        //this.WarehouseInputDetailBarcodeService.ListFilter = this.WarehouseInputDetailBarcodeService.ListFilter.sort((a, b) => (a.DateScan > b.DateScan ? 1 : -1));

        this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.ListFilter);
        this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
        this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;
        this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = environment.InitializationString;
        this.SearchString.nativeElement.focus();


        for (let i = 0; i < this.WarehouseInputDetailBarcodeService.ListFilter.length; i++) {
          if (this.WarehouseInputDetailBarcodeService.ListFilter[i].Active == true) {
            this.Quantity = this.Quantity + this.WarehouseInputDetailBarcodeService.ListFilter[i].Quantity;
            this.Output = this.Output + this.WarehouseInputDetailBarcodeService.ListFilter[i].QuantityOutput;
            this.Stock = this.Stock + this.WarehouseInputDetailBarcodeService.ListFilter[i].QuantityInventory;
          }
        }
      },
      err => {
      },
      () => {
        this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputDetailBarcodeModal(element: WarehouseInputDetailBarcode) {
    this.WarehouseOutputDetailBarcodeService.BaseParameter.ParentID = element.ParentID;
    this.WarehouseOutputDetailBarcodeService.BaseParameter.Barcode = element.Barcode;
    this.WarehouseOutputDetailBarcodeService.BaseParameter.Active = true;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseOutputDetailBarcodeHistoryModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
  WarehouseInputDetailBarcodeSave(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
    element.Quantity = element.QuantityOutput + element.QuantityInventory;
    this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailBarcodeService.SaveAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodePrint(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.BaseParameter.ID = element.ID;
    this.WarehouseInputDetailBarcodeService.PrintAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseInputDetailBarcodeService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
  @ViewChild("TABLE") table: ElementRef;
  WarehouseInputDetailBarcodeCompareExcel() {
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");



    let filename = "Barcode.xlsx";
    XLSX.writeFile(wb, filename);
  }
  WarehouseInputDetailBarcodeAutoSync() {
    this.WarehouseInputDetailBarcodeService.IsShowLoading = true;    
    this.WarehouseInputDetailBarcodeService.AutoSyncAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeSearch();
      },
      err => {        
      },
      () => {
        this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
}
