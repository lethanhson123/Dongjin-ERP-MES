import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseOutput } from 'src/app/shared/ERP/WarehouseOutput.model';
import { WarehouseOutputService } from 'src/app/shared/ERP/WarehouseOutput.service';

import { WarehouseOutputDetail } from 'src/app/shared/ERP/WarehouseOutputDetail.model';
import { WarehouseOutputDetailService } from 'src/app/shared/ERP/WarehouseOutputDetail.service';

import { WarehouseOutputDetailBarcode } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.model';
import { WarehouseOutputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.service';

import { WarehouseInputDetailBarcode } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.model';
import { WarehouseInputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.service';

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';

@Component({
  selector: 'app-warehouse-output-return',
  templateUrl: './warehouse-output-return.component.html',
  styleUrls: ['./warehouse-output-return.component.css']
})
export class WarehouseOutputReturnComponent {

  @ViewChild('WarehouseOutputSort') WarehouseOutputSort: MatSort;
  @ViewChild('WarehouseOutputPaginator') WarehouseOutputPaginator: MatPaginator;

  @ViewChild('WarehouseOutputDetailSort') WarehouseOutputDetailSort: MatSort;
  @ViewChild('WarehouseOutputDetailPaginator') WarehouseOutputDetailPaginator: MatPaginator;

  @ViewChild('WarehouseOutputDetailBarcodeSort') WarehouseOutputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseOutputDetailBarcodePaginator') WarehouseOutputDetailBarcodePaginator: MatPaginator;

  WarehouseOutputDetailBarcodeActiveCount: number = environment.InitializationNumber;

  IsBarcodeView: boolean = true;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseOutputService: WarehouseOutputService,
    public WarehouseOutputDetailService: WarehouseOutputDetailService,
    public WarehouseOutputDetailBarcodeService: WarehouseOutputDetailBarcodeService,
    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,

    public MembershipService: MembershipService,
  ) {
    this.WarehouseOutputService.List=[];
    this.WarehouseOutputDetailService.List=[];
    this.WarehouseOutputDetailBarcodeService.List=[];
    this.MembershipSearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.WarehouseOutputSearch();    
  }
  StartTimer() {
    setInterval(() => {
      this.WarehouseOutputSearch();
    }, 60000)
  }
  MembershipSearch() {
    this.MembershipService.BaseParameter.CategoryDepartmentID = Number(localStorage.getItem(environment.CategoryDepartmentID));
    this.MembershipService.BaseParameter.CategoryPositionID = environment.CategoryPositionID;
    this.MembershipService.BaseParameter.Active = true;
    this.MembershipService.ComponentGetByCategoryDepartmentID_CategoryPositionID_ActiveToListAsync(this.WarehouseOutputService);
    //this.MembershipService.ComponentGetByCategoryDepartmentID_ActiveToListAsync(this.WarehouseOutputService);
  }
  WarehouseOutputScan() {
    //this.WarehouseOutputService.IsShowLoading = true;
    let IsCheck = false;
    if (this.WarehouseOutputDetailBarcodeService.List) {
      if (this.WarehouseOutputDetailBarcodeService.List.length > 0) {
        for (let i = 0; i < this.WarehouseOutputDetailBarcodeService.List.length; i++) {
          if (this.WarehouseOutputDetailBarcodeService.List[i].Barcode == this.WarehouseOutputService.BaseParameter.SearchString) {
            IsCheck = true;
            this.WarehouseOutputDetailBarcodeService.List[i].Active = IsCheck;
            this.WarehouseOutputDetailBarcodeService.List[i].DateScan = new Date();
            i = this.WarehouseOutputDetailBarcodeService.List.length;
          }
        }
        this.WarehouseOutputDetailBarcodeCheck();
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


    this.WarehouseOutputService.BaseParameter.ID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
    this.WarehouseOutputService.BaseParameter.UpdateUserID = Number(localStorage.getItem(environment.UserID));
    this.WarehouseOutputService.GetByBarcodeAsync().subscribe(
      res => {
        this.WarehouseOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        //this.WarehouseOutputDetailBarcodeSearch(this.WarehouseOutputService.BaseParameter.BaseModel);
        // this.WarehouseOutputService.BaseResult = (res as BaseResult);
        // if (this.WarehouseOutputService.BaseResult.IsCheck == true) {
        //   this.NotificationService.warn(environment.ScanNotSuccess);
        //   let audio = new Audio("/Media/SuccessNot.wav");
        //   audio.play();
        // }
        // else {
        //   this.NotificationService.warn(environment.ScanSuccess);
        //   let audio = new Audio("/Media/Success.wav");
        //   audio.play();
        // }
      },
      err => {
      },
      () => {
        //this.WarehouseOutputService.IsShowLoading = false;
      }
    );
    this.WarehouseOutputService.BaseParameter.SearchString = environment.InitializationString;
  }
  WarehouseOutputSearch() {
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.WarehouseOutputService.BaseParameter.Active = true;
    this.WarehouseOutputService.BaseParameter.IsComplete = false;
    this.WarehouseOutputService.BaseParameter.Action = 1;
    this.WarehouseOutputService.GetByMembershipID_Active_SearchStringToListAsync().subscribe(
      res => {
        this.WarehouseOutputService.List = (res as BaseResult).List.sort((a, b) => (a.Date > b.Date ? 1 : -1));
        for (let i = 0; i < this.WarehouseOutputService.List.length; i++) {
          this.WarehouseOutputService.List[i].Root = false;
        }
        this.WarehouseOutputService.DataSource = new MatTableDataSource(this.WarehouseOutputService.List);
        this.WarehouseOutputService.DataSource.sort = this.WarehouseOutputSort;
        this.WarehouseOutputService.DataSource.paginator = this.WarehouseOutputPaginator;
      },
      err => {
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputSave(element: WarehouseOutput) {
    //this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputService.BaseParameter.BaseModel = element;
    this.WarehouseOutputService.SaveAsync().subscribe(
      res => {
        this.WarehouseOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseOutputSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        //this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputPrint(element: WarehouseOutput) {
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputService.BaseParameter.ID = element.ID;
    this.WarehouseOutputService.BaseParameter.Active = element.IsComplete;
    this.WarehouseOutputService.PrintAsync().subscribe(
      res => {
        this.WarehouseOutputService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseOutputService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputChange(element: WarehouseOutput) {
    this.WarehouseOutputDetailBarcodeSearch(element);
    this.WarehouseOutputDetailSearch(element);
  }
  WarehouseOutputDetailSearch(element: WarehouseOutput) {
    this.WarehouseOutputDetailService.List=[];
    this.WarehouseOutputDetailService.BaseParameter.ParentID = element.ID;
    this.WarehouseOutputDetailService.GetByParentIDToListAsync().subscribe(
      res => {
        this.WarehouseOutputDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.WarehouseOutputDetailService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailService.List);
        this.WarehouseOutputDetailService.DataSource.sort = this.WarehouseOutputDetailSort;
        this.WarehouseOutputDetailService.DataSource.paginator = this.WarehouseOutputDetailPaginator;
      },
      err => {
      },
      () => {
        //this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputDetailBarcodeSearch(element: WarehouseOutput) {
    this.WarehouseOutputDetailBarcodeService.List=[];
    this.WarehouseOutputService.BaseParameter.BaseModel = element;
    this.WarehouseOutputDetailBarcodeService.BaseParameter.ParentID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputDetailBarcodeService.GetByParentIDToListAsync().subscribe(
      res => {
        this.WarehouseOutputDetailBarcodeService.List = (res as BaseResult).List.sort((a, b) => (a.CategoryLocationName > b.CategoryLocationName ? 1 : -1));
        this.WarehouseOutputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailBarcodeService.List);
        this.WarehouseOutputDetailBarcodeService.DataSource.sort = this.WarehouseOutputDetailBarcodeSort;
        this.WarehouseOutputDetailBarcodeService.DataSource.paginator = this.WarehouseOutputDetailBarcodePaginator;
        this.WarehouseOutputDetailBarcodeCheck();
      },
      err => {
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputDetailBarcodeSearchSub() {
    if (this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString.length > 0) {
      this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString.trim();
      if (this.WarehouseOutputDetailBarcodeService.DataSource) {
        this.WarehouseOutputDetailBarcodeService.DataSource.filter = this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
    }
  }
  WarehouseInputDetailBarcodePrint(element: WarehouseOutputDetailBarcode) {
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = element.Barcode;
    this.WarehouseInputDetailBarcodeService.PrintByBarcodeAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseInputDetailBarcodeService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputDetailBarcodeCheck() {
    if (this.WarehouseOutputDetailBarcodeService.List) {
      if (this.WarehouseOutputDetailBarcodeService.List.length > 0) {
        this.WarehouseOutputDetailBarcodeActiveCount = environment.InitializationNumber;
        let IsCheck = true;
        for (let i = 0; i < this.WarehouseOutputDetailBarcodeService.List.length; i++) {
          if (this.WarehouseOutputDetailBarcodeService.List[i].Active == true) {
            this.WarehouseOutputDetailBarcodeActiveCount = this.WarehouseOutputDetailBarcodeActiveCount + 1;
          }
          else {
            IsCheck = false;
          }
        }
        if (IsCheck == true) {
          this.WarehouseOutputSearch();
        }
      }
    }
  }
  WarehouseOutputDetailBarcodeSave(element: WarehouseOutputDetailBarcode) {
    this.WarehouseOutputService.IsShowLoading = true;
    element.ParentID = this.WarehouseOutputDetailBarcodeService.BaseParameter.BaseModel.ID;
    this.WarehouseOutputDetailBarcodeService.BaseParameter.BaseModel = element;
    this.WarehouseOutputDetailBarcodeService.SaveAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
}
