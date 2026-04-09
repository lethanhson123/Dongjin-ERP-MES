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


@Component({
  selector: 'app-warehouse-input-detail-scan',
  templateUrl: './warehouse-input-detail-scan.component.html',
  styleUrls: ['./warehouse-input-detail-scan.component.css']
})
export class WarehouseInputDetailScanComponent {


  @ViewChild('WarehouseInputSort') WarehouseInputSort: MatSort;
  @ViewChild('WarehouseInputPaginator') WarehouseInputPaginator: MatPaginator;

  @ViewChild('WarehouseInputDetailSort') WarehouseInputDetailSort: MatSort;
  @ViewChild('WarehouseInputDetailPaginator') WarehouseInputDetailPaginator: MatPaginator;

  @ViewChild('WarehouseInputDetailBarcodeSort') WarehouseInputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginator') WarehouseInputDetailBarcodePaginator: MatPaginator;

  WarehouseInputDetailBarcodeActiveCount: number = environment.InitializationNumber;


  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseInputService: WarehouseInputService,
    public WarehouseInputDetailService: WarehouseInputDetailService,
    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,

    public MembershipService: MembershipService,
  ) {
    this.WarehouseInputService.List = [];
    this.WarehouseInputDetailService.List = [];
    this.WarehouseInputDetailBarcodeService.List = [];
    this.MembershipSearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.WarehouseInputSearch();
  }
  MembershipSearch() {
    this.MembershipService.BaseParameter.CategoryDepartmentID = environment.DepartmentID;
    this.MembershipService.BaseParameter.Active = true;
    this.MembershipService.ComponentGetByCategoryDepartmentID_ActiveToListAsync(this.WarehouseInputService);
  }
  WarehouseInputScan() {

    let IsCheck = false;
    if (this.WarehouseInputDetailBarcodeService.List) {
      if (this.WarehouseInputDetailBarcodeService.List.length > 0) {
        for (let i = 0; i < this.WarehouseInputDetailBarcodeService.List.length; i++) {
          if (this.WarehouseInputDetailBarcodeService.List[i].Barcode == this.WarehouseInputService.BaseParameter.SearchString) {
            IsCheck = true;
            this.WarehouseInputDetailBarcodeService.List[i].Active = IsCheck;
            this.WarehouseInputDetailBarcodeService.List[i].DateScan = new Date();
            i = this.WarehouseInputDetailBarcodeService.List.length;
          }
        }
        this.WarehouseInputDetailBarcodeCheck();
        if (IsCheck == true) {
          this.NotificationService.warn(environment.ScanSuccess);
          let audio = new Audio("/Media/Success.wav");
          audio.play();
        }
        else {
          this.NotificationService.warn(environment.ScanNotSuccess);
          let audio = new Audio("/Media/SuccessNot.wav");
          audio.play();
        }
      }
    }

    this.WarehouseInputService.BaseParameter.ID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputService.BaseParameter.UpdateUserID = Number(localStorage.getItem(environment.UserID));
    this.WarehouseInputService.GetByBarcodeAsync().subscribe(
      res => {
        this.WarehouseInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        //this.WarehouseInputDetailBarcodeSearch(this.WarehouseInputService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {

      }
    );
    this.WarehouseInputService.BaseParameter.SearchString = environment.InitializationString;
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
        this.NotificationService.warn(environment.SaveSuccess);
        this.WarehouseInputSearch();
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        //this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodeSearch(element: WarehouseInput) {    
    this.WarehouseInputDetailBarcodeService.List = [];
    this.WarehouseInputService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailBarcodeService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputService.IsShowLoading = true;
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
}