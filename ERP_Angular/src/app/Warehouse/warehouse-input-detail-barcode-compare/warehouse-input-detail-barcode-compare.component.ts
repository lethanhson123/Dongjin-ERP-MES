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
  selector: 'app-warehouse-input-detail-barcode-compare',
  templateUrl: './warehouse-input-detail-barcode-compare.component.html',
  styleUrls: ['./warehouse-input-detail-barcode-compare.component.css']
})
export class WarehouseInputDetailBarcodeCompareComponent {

  @ViewChild('SearchString') SearchString!: ElementRef;

  @ViewChild('WarehouseInputDetailBarcodeSort') WarehouseInputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginator') WarehouseInputDetailBarcodePaginator: MatPaginator;

  TotalMES: number = 0;
  TotalERP: number = 0;
  TotalCompare: number = 0;

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
    this.WarehouseInputDetailBarcodeService.List = [];
    this.CompanySearch();
  }
  ngOnInit(): void {
  }
  ngAfterViewInit() {

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
  WarehouseInputDetailBarcodeSum() {
    this.TotalCompare = 0;
    this.TotalERP = 0;
    this.TotalMES = 0;
    for (let i = 0; i < this.WarehouseInputDetailBarcodeService.ListChild.length; i++) {
      this.TotalCompare = this.TotalCompare + this.WarehouseInputDetailBarcodeService.ListChild[i].PKG_QTYActual;
      this.TotalMES = this.TotalMES + this.WarehouseInputDetailBarcodeService.ListChild[i].QuantityMES;
      this.TotalERP = this.TotalERP + this.WarehouseInputDetailBarcodeService.ListChild[i].QuantityInventory;
    }
  }
  WarehouseInputDetailBarcodeSearch() {
    this.WarehouseInputDetailBarcodeService.List = [];
    if (this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.length > 0) {
      this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.trim();

      this.WarehouseInputDetailBarcodeService.ListChild = this.WarehouseInputDetailBarcodeService.ListParent.filter(o => o.MaterialName == this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString);
      this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.ListChild);
      this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
      this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;

      this.WarehouseInputDetailBarcodeSum();
    }
    else {
      this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
      this.WarehouseInputDetailBarcodeService.GetCompareMESAndERPToListAsync().subscribe(
        res => {
          this.WarehouseInputDetailBarcodeService.ListParent = (res as BaseResult).List;
          this.WarehouseInputDetailBarcodeService.ListChild = this.WarehouseInputDetailBarcodeService.ListParent;
          this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.ListChild);
          this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
          this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;
          this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = environment.InitializationString;
          this.SearchString.nativeElement.focus();

          this.WarehouseInputDetailBarcodeSum();


        },
        err => {
        },
        () => {
          this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
        }
      );
    }

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
    this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailBarcodeService.SaveAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeSearch();
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
  WarehouseInputDetailBarcodeSaveList() {
    this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.BaseParameter.List = this.WarehouseInputDetailBarcodeService.List;
    for (let i = 0; i < this.WarehouseInputDetailBarcodeService.BaseParameter.List.length; i++) {
      //this.WarehouseInputDetailBarcodeService.BaseParameter.List[i].Quantity = null;
      this.WarehouseInputDetailBarcodeService.BaseParameter.List[i].Active = true;
      if (this.WarehouseInputDetailBarcodeService.BaseParameter.List[i].ParentID == null) {
        this.WarehouseInputDetailBarcodeService.BaseParameter.List[i].ParentID = 39;
      }
    }
    this.WarehouseInputDetailBarcodeService.SaveListAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = environment.InitializationString;
        this.WarehouseInputDetailBarcodeSearch();
      },
      err => {
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

    let ListCategoryDepartment = this.CategoryDepartmentService.ListFilter.filter(o => o.ID == this.WarehouseInputDetailBarcodeService.BaseParameter.CategoryDepartmentID);
    if (ListCategoryDepartment && ListCategoryDepartment.length > 0) {
      this.CategoryDepartmentService.BaseParameter.BaseModel = ListCategoryDepartment[0];
    }
    let ListCompany = this.CompanyService.ListFilter.filter(o => o.ID == this.WarehouseInputDetailBarcodeService.BaseParameter.CompanyID);
    if (ListCompany && ListCompany.length > 0) {
      this.CompanyService.BaseParameter.BaseModel = ListCompany[0];
    }

    let filename = this.CompanyService.BaseParameter.BaseModel.Name + "-" + this.CategoryDepartmentService.BaseParameter.BaseModel.Name + "-Barcode-Inventory.xlsx";
    XLSX.writeFile(wb, filename);
  }
}

