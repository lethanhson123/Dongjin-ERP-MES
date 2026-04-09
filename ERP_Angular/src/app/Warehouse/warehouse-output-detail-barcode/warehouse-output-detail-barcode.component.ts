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

import { WarehouseOutput } from 'src/app/shared/ERP/WarehouseOutput.model';
import { WarehouseOutputService } from 'src/app/shared/ERP/WarehouseOutput.service';

import { WarehouseOutputDetailBarcode } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.model';
import { WarehouseOutputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

@Component({
  selector: 'app-warehouse-output-detail-barcode',
  templateUrl: './warehouse-output-detail-barcode.component.html',
  styleUrls: ['./warehouse-output-detail-barcode.component.css']
})
export class WarehouseOutputDetailBarcodeComponent {

  @ViewChild('SearchString') SearchString!: ElementRef;

  @ViewChild('WarehouseOutputDetailBarcodeSort') WarehouseOutputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseOutputDetailBarcodePaginator') WarehouseOutputDetailBarcodePaginator: MatPaginator;

  Count: number = 0;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseOutputService: WarehouseOutputService,
    public WarehouseOutputDetailBarcodeService: WarehouseOutputDetailBarcodeService,
    public MaterialService: MaterialService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,
  ) {
    this.WarehouseOutputDetailBarcodeService.BaseParameter.Active = true;
    this.ComponentGetYearList();
    this.ComponentGetMonthList();
    this.ComponentGetDayList();
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    //this.WarehouseOutputDetailBarcodeSearch();
  }
  DateBegin(value) {
    this.WarehouseOutputDetailBarcodeService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.WarehouseOutputDetailBarcodeService.BaseParameter.DateEnd = new Date(value);
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.WarehouseOutputDetailBarcodeService);
    this.CompanyChange();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.WarehouseOutputDetailBarcodeService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.WarehouseOutputDetailBarcodeService);
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.WarehouseOutputDetailBarcodeService);
  }
  ComponentGetYearList() {
    this.WarehouseOutputDetailBarcodeService.ComponentGetYearList();
  }
  ComponentGetMonthList() {
    this.WarehouseOutputDetailBarcodeService.ComponentGetMonthList();
  }
  ComponentGetDayList() {
    this.WarehouseOutputDetailBarcodeService.ComponentGetDayList();
  }
  WarehouseOutputDetailBarcodeSearch() {
    this.Count = 0;
    if (this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString == null) {
      this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString = environment.InitializationString;
    }
    this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString.trim();
    this.WarehouseOutputDetailBarcodeService.IsShowLoading = true;
    this.WarehouseOutputDetailBarcodeService.GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_ActiveToListAsync().subscribe(
      res => {
        this.WarehouseOutputDetailBarcodeService.List = (res as BaseResult).List;
        //this.WarehouseOutputDetailBarcodeService.List = (res as BaseResult).List.sort((a, b) => (a.DateScan > b.DateScan ? 1 : -1));       
        this.WarehouseOutputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailBarcodeService.List);
        this.WarehouseOutputDetailBarcodeService.DataSource.sort = this.WarehouseOutputDetailBarcodeSort;
        this.WarehouseOutputDetailBarcodeService.DataSource.paginator = this.WarehouseOutputDetailBarcodePaginator;

        this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString = environment.InitializationString;
        this.SearchString.nativeElement.focus();
        for (let i = 0; i < this.WarehouseOutputDetailBarcodeService.List.length; i++) {
          if (this.WarehouseOutputDetailBarcodeService.List[i].Active == true) {
            this.Count = this.Count + this.WarehouseOutputDetailBarcodeService.List[i].Quantity;
          }
        }
      },
      err => {
      },
      () => {
        this.WarehouseOutputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
  MaterialModal(element: WarehouseOutputDetailBarcode) {
    this.WarehouseOutputDetailBarcodeService.IsShowLoading = true;
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
        this.WarehouseOutputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }

  @ViewChild("TABLE") table: ElementRef;
  WarehouseOutputDetailBarcodeExcel() {
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");

    let ListCategoryDepartment = this.CategoryDepartmentService.ListFilter.filter(o => o.ID == this.WarehouseOutputDetailBarcodeService.BaseParameter.CategoryDepartmentID);
    if (ListCategoryDepartment && ListCategoryDepartment.length > 0) {
      this.CategoryDepartmentService.BaseParameter.BaseModel = ListCategoryDepartment[0];
    }
    let ListCompany = this.CompanyService.ListFilter.filter(o => o.ID == this.WarehouseOutputDetailBarcodeService.BaseParameter.CompanyID);
    if (ListCompany && ListCompany.length > 0) {
      this.CompanyService.BaseParameter.BaseModel = ListCompany[0];
    }

    let filename = this.CompanyService.BaseParameter.BaseModel.Name + "-" + this.CategoryDepartmentService.BaseParameter.BaseModel.Name + "-" + this.WarehouseOutputDetailBarcodeService.BaseParameter.Year + "-" + this.WarehouseOutputDetailBarcodeService.BaseParameter.Month + "-" + this.WarehouseOutputDetailBarcodeService.BaseParameter.Day + "-WarehouseOutput.xlsx";
    XLSX.writeFile(wb, filename);
  }
  WarehouseOutputDetailBarcodeAutoSync() {
    this.WarehouseOutputDetailBarcodeService.IsShowLoading = true;
    this.WarehouseOutputDetailBarcodeService.AutoSyncAsync().subscribe(
      res => {
        this.WarehouseOutputDetailBarcodeSearch();
      },
      err => {
      },
      () => {
        this.WarehouseOutputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputPrint() {
    this.WarehouseOutputDetailBarcodeService.IsShowLoading = true;
    this.WarehouseOutputService.BaseParameter.CategoryDepartmentID = this.WarehouseOutputDetailBarcodeService.BaseParameter.CategoryDepartmentID;
    this.WarehouseOutputService.BaseParameter.DateBegin = this.WarehouseOutputDetailBarcodeService.BaseParameter.DateBegin;
    this.WarehouseOutputService.BaseParameter.DateEnd = this.WarehouseOutputDetailBarcodeService.BaseParameter.DateEnd;
    this.WarehouseOutputService.PrintSumAsync().subscribe(
      res => {
        this.WarehouseOutputService.BaseResult = (res as BaseResult);        
        window.open(this.WarehouseOutputService.BaseResult.Message, '_blank').focus();
        window.open(this.WarehouseOutputService.BaseResult.Note, '_blank').focus();        
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseOutputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
}