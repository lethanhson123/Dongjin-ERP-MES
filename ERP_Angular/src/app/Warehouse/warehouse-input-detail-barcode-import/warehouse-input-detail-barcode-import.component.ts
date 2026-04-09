import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseInput } from 'src/app/shared/ERP/WarehouseInput.model';
import { WarehouseInputService } from 'src/app/shared/ERP/WarehouseInput.service';

import { WarehouseInputDetail } from 'src/app/shared/ERP/WarehouseInputDetail.model';
import { WarehouseInputDetailService } from 'src/app/shared/ERP/WarehouseInputDetail.service';

import { WarehouseInputDetailBarcode } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.model';
import { WarehouseInputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.service';

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';

import { InvoiceInput } from 'src/app/shared/ERP/InvoiceInput.model';
import { InvoiceInputService } from 'src/app/shared/ERP/InvoiceInput.service';

@Component({
  selector: 'app-warehouse-input-detail-barcode-import',
  templateUrl: './warehouse-input-detail-barcode-import.component.html',
  styleUrls: ['./warehouse-input-detail-barcode-import.component.css']
})
export class WarehouseInputDetailBarcodeImportComponent {

  @ViewChild('WarehouseInputSort') WarehouseInputSort: MatSort;
  @ViewChild('WarehouseInputPaginator') WarehouseInputPaginator: MatPaginator;

  @ViewChild('WarehouseInputDetailSort') WarehouseInputDetailSort: MatSort;
  @ViewChild('WarehouseInputDetailPaginator') WarehouseInputDetailPaginator: MatPaginator;

  @ViewChild('WarehouseInputDetailBarcodeSort') WarehouseInputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginator') WarehouseInputDetailBarcodePaginator: MatPaginator;

  @ViewChild('InvoiceInputSort') InvoiceInputSort: MatSort;
  @ViewChild('InvoiceInputPaginator') InvoiceInputPaginator: MatPaginator;

  WarehouseInputDetailBarcodeActiveCount: number = environment.InitializationNumber;

  IsBarcodeView: boolean = false;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseInputService: WarehouseInputService,
    public WarehouseInputDetailService: WarehouseInputDetailService,
    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,

    public MembershipService: MembershipService,

    public InvoiceInputService: InvoiceInputService,
  ) {
    this.WarehouseInputDetailService.BaseParameter.Active = false;
    this.MembershipSearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.WarehouseInputSearch();
    this.InvoiceInputSearch();
  }
  InvoiceInputSearch() {
    this.WarehouseInputService.IsShowLoading = true;
    this.InvoiceInputService.BaseParameter.Active = true;
    this.InvoiceInputService.BaseParameter.IsComplete = true;
    this.InvoiceInputService.GetByActive_IsFutureToListAsync().subscribe(
      res => {
        this.InvoiceInputService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder < b.SortOrder ? 1 : -1));
        this.InvoiceInputService.DataSource = new MatTableDataSource(this.InvoiceInputService.List);
        this.InvoiceInputService.DataSource.sort = this.WarehouseInputSort;
        this.InvoiceInputService.DataSource.paginator = this.WarehouseInputPaginator;

      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  MembershipSearch() {
    this.MembershipService.BaseParameter.CategoryDepartmentID = environment.DepartmentID;
    this.MembershipService.BaseParameter.Active = true;
    this.MembershipService.ComponentGetByCategoryDepartmentID_ActiveToListAsync(this.WarehouseInputService);
  }
  WarehouseInputScan() {
    this.WarehouseInputService.IsShowLoading = true;

    this.WarehouseInputService.BaseParameter.ID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputService.BaseParameter.UpdateUserID = Number(localStorage.getItem(environment.UserID));
    this.WarehouseInputService.GetByBarcodeAsync().subscribe(
      res => {
        this.WarehouseInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseInputDetailBarcodeSearch(this.WarehouseInputService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
      }
    );


    let Barcode = this.WarehouseInputService.BaseParameter.SearchString;
    let BarcodeArray = Barcode.split('$');
    let Index = Number(BarcodeArray[BarcodeArray.length - 1]);
    let List = this.WarehouseInputDetailBarcodeService.List.filter(o => o.Barcode == Barcode);
    if (Index == 0) {
      Barcode = BarcodeArray[0];
      List = this.WarehouseInputDetailBarcodeService.List.filter(o => o.Barcode.includes(Barcode));
    }
    if (List && List.length > 0) {
      for (let i = 0; i < List.length; i++) {
        List[i].Active = true;
      }
      this.WarehouseInputDetailBarcodeActiveCount = List.length;
      this.NotificationService.warn(environment.ScanSuccess);
      let audio = new Audio("/Media/Success.wav");
      audio.play();
    }
    else {
      this.NotificationService.warn(environment.ScanNotSuccess);
      let audio = new Audio("/Media/SuccessNot.wav");
      audio.play();
    }
    this.WarehouseInputService.BaseParameter.SearchString = environment.InitializationString;
    this.WarehouseInputService.IsShowLoading = false;
  }
  WarehouseInputSearch() {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.WarehouseInputService.BaseParameter.Active = true;
    this.WarehouseInputService.BaseParameter.IsComplete = false;
    this.WarehouseInputService.GetByMembershipID_Active_IsCompleteToListAsync().subscribe(
      res => {
        this.WarehouseInputService.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));
        for (let i = 0; i < this.WarehouseInputService.List.length; i++) {
          this.WarehouseInputService.List[i].Root = false;
        }
        this.WarehouseInputService.DataSource = new MatTableDataSource(this.WarehouseInputService.List);
        this.WarehouseInputService.DataSource.sort = this.WarehouseInputSort;
        this.WarehouseInputService.DataSource.paginator = this.WarehouseInputPaginator;
      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputSave(element: WarehouseInput) {
    //this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputService.BaseParameter.BaseModel = element;
    this.WarehouseInputService.SaveAsync().subscribe(
      res => {
        this.WarehouseInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseInputSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        //this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputChange(element: WarehouseInput) {
    this.WarehouseInputDetailSearch(element);
    this.WarehouseInputDetailBarcodeSearch(element);
  }
  WarehouseInputDetailSearch(element: WarehouseInput) {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputDetailService.GetByParentIDToListAsync().subscribe(
      res => {
        this.WarehouseInputDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.WarehouseInputDetailService.DataSource = new MatTableDataSource(this.WarehouseInputDetailService.List);
        this.WarehouseInputDetailService.DataSource.sort = this.WarehouseInputDetailSort;
        this.WarehouseInputDetailService.DataSource.paginator = this.WarehouseInputDetailPaginator;

        if (this.WarehouseInputDetailService.List && this.WarehouseInputDetailService.List.length > 0) {
          this.WarehouseInputDetailBarcodeService.BaseParameter.ListID = [];
          for (let i = 0; i < this.WarehouseInputDetailService.List.length; i++) {
            if (this.WarehouseInputDetailService.List[i].Active == true) {
              this.WarehouseInputDetailBarcodeService.BaseParameter.ListID.push(this.WarehouseInputDetailService.List[i].ID);
            }
          }
        }
      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodeSearch(element: WarehouseInput) {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailBarcodeService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputDetailBarcodeService.GetByParentIDToListAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.List);
        this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
        this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;
        this.WarehouseInputDetailBarcodeCheck();
      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodeCheck() {
    if (this.WarehouseInputDetailBarcodeService.List) {
      if (this.WarehouseInputDetailBarcodeService.List.length > 0) {
        this.WarehouseInputDetailBarcodeActiveCount = environment.InitializationNumber;
        let IsCheck = true;
        for (let i = 0; i < this.WarehouseInputDetailBarcodeService.List.length; i++) {
          if (this.WarehouseInputDetailBarcodeService.List[i].Active == true) {
            this.WarehouseInputDetailBarcodeActiveCount = this.WarehouseInputDetailBarcodeActiveCount + 1;
          }
          else {
            IsCheck = false;
          }
        }
        if (IsCheck == true) {
          this.WarehouseInputSearch();
        }
      }
    }
  }
  WarehouseInputDetailBarcodePrintByParentID(element: WarehouseInput) {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.BaseParameter.ParentID = element.ID;
    this.WarehouseInputDetailBarcodeService.PrintByParentIDAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseInputDetailBarcodeService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailPrint(element: WarehouseInputDetail) {    
    this.WarehouseInputDetailBarcodeService.BaseParameter.ID = element.ID;
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.PrintByWarehouseInputDetailIDAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseInputDetailBarcodeService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodePrint(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputService.IsShowLoading = true;
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
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailSync(element: WarehouseInput) {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputDetailService.SaveListAndSyncWarehouseInputDetailBarcodeAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeSearch(this.WarehouseInputService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailActiveAllChange() {
    this.WarehouseInputDetailService.BaseParameter.ListID = [];
    for (let i = 0; i < this.WarehouseInputDetailService.List.length; i++) {
      if (this.WarehouseInputDetailService.List[i].ID > 0) {
        this.WarehouseInputDetailService.List[i].Active = this.WarehouseInputDetailService.BaseParameter.Active;
        if (this.WarehouseInputDetailService.List[i].Active == true) {
          this.WarehouseInputDetailBarcodeService.BaseParameter.ListID.push(this.WarehouseInputDetailService.List[i].ID);
        }
      }
    }
  }
  WarehouseInputDetailActiveChange(element: WarehouseInputDetail) {
    if (element.ID > 0) {
      if (element.Active == true) {
        this.WarehouseInputDetailBarcodeService.BaseParameter.ListID.push(element.ID);
      }
      else {
        this.WarehouseInputDetailBarcodeService.BaseParameter.ListID = this.WarehouseInputDetailBarcodeService.BaseParameter.ListID.filter(o => o != element.ID);
      }
    }
  }
}