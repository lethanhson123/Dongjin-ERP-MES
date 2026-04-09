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
  selector: 'app-warehouse-input-detail-barcode-compare-part-no',
  templateUrl: './warehouse-input-detail-barcode-compare-part-no.component.html',
  styleUrls: ['./warehouse-input-detail-barcode-compare-part-no.component.css']
})
export class WarehouseInputDetailBarcodeComparePartNoComponent {

  @ViewChild('SearchString') SearchString!: ElementRef;

  @ViewChild('WarehouseInputDetailBarcodeSort') WarehouseInputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginator') WarehouseInputDetailBarcodePaginator: MatPaginator;

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
  WarehouseInputDetailBarcodeSearch() {
    this.WarehouseInputDetailBarcodeService.List = [];
    if (this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.length > 0) {
      this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.trim();
      if (this.WarehouseInputDetailBarcodeService.DataSource) {
        this.WarehouseInputDetailBarcodeService.DataSource.filter = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
      this.WarehouseInputDetailBarcodeService.GetPARTNOCompareMESAndERPToListAsync().subscribe(
        res => {
          this.WarehouseInputDetailBarcodeService.List = (res as BaseResult).List;
          this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.List);
          this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
          this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;
          this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = environment.InitializationString;
          this.SearchString.nativeElement.focus();

          this.WarehouseInputDetailBarcodeService.BaseParameter.Count = 0;
          for (let i = 0; i < this.WarehouseInputDetailBarcodeService.List.length; i++) {
            this.WarehouseInputDetailBarcodeService.BaseParameter.Count = this.WarehouseInputDetailBarcodeService.BaseParameter.Count + this.WarehouseInputDetailBarcodeService.List[i].PKG_QTYActual;
          }
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

    let filename = this.CompanyService.BaseParameter.BaseModel.Name + "-" + this.CategoryDepartmentService.BaseParameter.BaseModel.Name + "-Inventory.xlsx";
    XLSX.writeFile(wb, filename);
  }
}
