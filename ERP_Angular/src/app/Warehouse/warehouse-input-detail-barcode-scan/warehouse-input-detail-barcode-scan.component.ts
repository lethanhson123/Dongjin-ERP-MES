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

import { WarehouseInputDetailBarcode } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.model';
import { WarehouseInputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

@Component({
  selector: 'app-warehouse-input-detail-barcode-scan',
  templateUrl: './warehouse-input-detail-barcode-scan.component.html',
  styleUrls: ['./warehouse-input-detail-barcode-scan.component.css']
})
export class WarehouseInputDetailBarcodeScanComponent {

  @ViewChild('WarehouseInputDetailBarcodeSort') WarehouseInputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginator') WarehouseInputDetailBarcodePaginator: MatPaginator;

  @ViewChild('SearchString') SearchString!: ElementRef;
  @ViewChild('SearchStringFilter') SearchStringFilter!: ElementRef;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,
    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,
    public CategoryDepartmentService: CategoryDepartmentService,

  ) {
    this.CategoryDepartmentSearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild = [];
    this.SearchString.nativeElement.focus();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CategoryDepartmentService.GetByMembershipID_ActiveToListAsync().subscribe(
      res => {
        this.CategoryDepartmentService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CategoryDepartmentService.ListFilter && this.CategoryDepartmentService.ListFilter.length > 0) {
          this.WarehouseInputDetailBarcodeService.BaseParameter.CategoryDepartmentID = this.CategoryDepartmentService.ListFilter[0].ID;
          if (this.CategoryDepartmentService.ListFilter.length > 1) {
            let List = this.CategoryDepartmentService.ListFilter.filter(o => o.Code.includes("Warehouse") || o.Code.includes("FinishGoods") || o.Code.includes("HOOK_RACK"));
            if (List && List.length > 0) {
              this.WarehouseInputDetailBarcodeService.BaseParameter.CategoryDepartmentID = List[0].ID;
            }
          }
        }
      },
      err => {
      },
      () => {
      }
    );
  }
  WarehouseInputDetailBarcodeSearchString() {
    if (this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString && this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.length > 0) {
      this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.trim();
    }
    let audio = new Audio("/Media/Success.wav");
    audio.play();
    this.SearchStringFilter.nativeElement.focus();
  }
  WarehouseInputDetailBarcodeSearchStringFilter() {
    if (this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter && this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter.length > 0) {
      if (this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter == this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString) {
        this.Save();
      }
      else {
        let WarehouseInputDetailBarcode: WarehouseInputDetailBarcode;
        WarehouseInputDetailBarcode = {
        };
        WarehouseInputDetailBarcode.UpdateUserID = Number(localStorage.getItem(environment.UserID));
        WarehouseInputDetailBarcode.Active = true;
        WarehouseInputDetailBarcode.IsScan = true;
        WarehouseInputDetailBarcode.CategoryLocationName = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString;
        WarehouseInputDetailBarcode.Barcode = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter;
        WarehouseInputDetailBarcode.PageSize = this.WarehouseInputDetailBarcodeService.BaseParameter.PageSize;
        this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild.push(WarehouseInputDetailBarcode);

        let audio = new Audio("/Media/Success.wav");
        audio.play();

        this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild);
        this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
        this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;

        this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter = environment.InitializationString;
        this.WarehouseInputDetailBarcodeService.BaseParameter.PageSize = null;
        this.SearchStringFilter.nativeElement.focus();
      }
    }
  }
  Save() {
    this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.BaseParameter.List = this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild;
    this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild = [];
    this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild);
    this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
    this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;
    this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = environment.InitializationString;
    this.SearchString.nativeElement.focus();
    this.WarehouseInputDetailBarcodeService.IsShowLoading = false;

    this.WarehouseInputDetailBarcodeService.SaveListAsync().subscribe(
      res => {
      },
      err => {
      },
      () => {
        this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
}
