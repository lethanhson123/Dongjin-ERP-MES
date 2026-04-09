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

@Component({
  selector: 'app-warehouse-check-djm',
  templateUrl: './warehouse-check-djm.component.html',
  styleUrls: ['./warehouse-check-djm.component.css']
})
export class WarehouseCheckDJMComponent {

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

  ) {
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild = [];
    this.SearchString.nativeElement.focus();
  }
  WarehouseInputDetailBarcodeSearchString() {
    if (this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString && this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.length > 0) {
      this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.trim();
    }
    let audio = new Audio("/Media/Success.wav");
    audio.play();
    this.SearchStringFilter.nativeElement.focus();
  }
  WarehouseInputDetailBarcodeCheck() {
    this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter.trim();
    let ListCheck = this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild.filter(o => o.Barcode == this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter);
    if (ListCheck && ListCheck.length > 0) {
      let audio = new Audio("/Media/SuccessNot.wav");
      audio.play();
      this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter = environment.InitializationString;
      this.WarehouseInputDetailBarcodeService.BaseParameter.PageSize = null;
      this.SearchStringFilter.nativeElement.focus();
    }
    else {
      this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
      this.WarehouseInputDetailBarcodeService.BaseParameter.CompanyID = 17;
      this.WarehouseInputDetailBarcodeService.BaseParameter.Barcode = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter;
      this.WarehouseInputDetailBarcodeService.GetByBarcodeFromtmbrcdAsync().subscribe(
        res => {
          this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
          this.WarehouseInputDetailBarcodeService.BaseParameter.PageSize = this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel.Quantity;
          this.WarehouseInputDetailBarcodeSearchStringFilter();
        },
        err => {
        },
        () => {
          this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputDetailBarcodeSearchStringFilter() {
    let IsCheck = true;
    if (this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter && this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter.length > 0) {
      this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter.trim();
      if (this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter == this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString) {
        this.Save();
        this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter = environment.InitializationString;
        this.SearchString.nativeElement.focus();
        IsCheck = false;
      }
      else {
        if (IsCheck == true) {
          let WarehouseInputDetailBarcode: WarehouseInputDetailBarcode;
          WarehouseInputDetailBarcode = {
          };
          WarehouseInputDetailBarcode.UpdateUserID = Number(localStorage.getItem(environment.UserID));
          WarehouseInputDetailBarcode.ParentID = 66;
          WarehouseInputDetailBarcode.Active = true;
          WarehouseInputDetailBarcode.IsSync = true;
          WarehouseInputDetailBarcode.CategoryLocationName = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString;
          WarehouseInputDetailBarcode.Barcode = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter;
          WarehouseInputDetailBarcode.Quantity = this.WarehouseInputDetailBarcodeService.BaseParameter.PageSize;
          WarehouseInputDetailBarcode.UpdateDate = this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel.UpdateDate;
          WarehouseInputDetailBarcode.UpdateUserCode = this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel.UpdateUserCode;
          WarehouseInputDetailBarcode.UpdateUserName = this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel.UpdateUserName;
          WarehouseInputDetailBarcode.Quantity02 = this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel.Quantity02;
          WarehouseInputDetailBarcode.UpdateDate02 = this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel.UpdateDate02;
          WarehouseInputDetailBarcode.UpdateUserCode02 = this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel.UpdateUserCode02;
          WarehouseInputDetailBarcode.UpdateUserName02 = this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel.UpdateUserName02;
          WarehouseInputDetailBarcode.Quantity03 = this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel.Quantity03;
          WarehouseInputDetailBarcode.UpdateDate03 = this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel.UpdateDate03;
          WarehouseInputDetailBarcode.UpdateUserCode03 = this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel.UpdateUserCode03;
          WarehouseInputDetailBarcode.UpdateUserName03 = this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel.UpdateUserName03;


          WarehouseInputDetailBarcode.ProductID = 1;
          if (this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild && this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild.length > 0) {
            WarehouseInputDetailBarcode.ProductID = this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild.length + 1;
          }

          this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild.push(WarehouseInputDetailBarcode);

          // let audio = new Audio("/Media/Success.wav");
          // audio.play();

          this.NotificationService.SoundCallByCount(this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild.length);

          this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild = this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild.sort((a, b) => (a.ProductID < b.ProductID ? 1 : -1));

          this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.BaseParameter.ListChild);
          this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
          this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;

          this.WarehouseInputDetailBarcodeService.BaseParameter.SearchStringFilter = environment.InitializationString;
          this.WarehouseInputDetailBarcodeService.BaseParameter.PageSize = null;
          this.SearchStringFilter.nativeElement.focus();
        }
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
    for (let i = 0; i < this.WarehouseInputDetailBarcodeService.BaseParameter.List.length; i++) {
      this.WarehouseInputDetailBarcodeService.BaseParameter.List[i].Active = true;
    }
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
