import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
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

import { WarehouseOutputDetailBarcode } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.model';
import { WarehouseOutputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.service';

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';

@Component({
  selector: 'app-warehouse-output-detail-barcode-find',
  templateUrl: './warehouse-output-detail-barcode-find.component.html',
  styleUrls: ['./warehouse-output-detail-barcode-find.component.css']
})
export class WarehouseOutputDetailBarcodeFindComponent {

  @ViewChild('WarehouseOutputDetailBarcodeSort') WarehouseOutputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseOutputDetailBarcodePaginator') WarehouseOutputDetailBarcodePaginator: MatPaginator;

  WarehouseOutputDetailBarcodeActiveCount: number = environment.InitializationNumber;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseOutputService: WarehouseOutputService,
    public WarehouseOutputDetailBarcodeService: WarehouseOutputDetailBarcodeService,

    public MembershipService: MembershipService,
  ) {

  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.WarehouseOutputService.BaseParameter.ID = Number(this.ActiveRouter.snapshot.params.ID);
    this.WarehouseOutputSearch();
  }

  WarehouseOutputScan() {
    //this.WarehouseOutputService.IsShowLoading = true;
    let IsCheck = false;
    if (this.WarehouseOutputDetailBarcodeService.List) {
      if (this.WarehouseOutputDetailBarcodeService.List.length > 0) {
        this.WarehouseOutputService.BaseParameter.SearchString = this.WarehouseOutputService.BaseParameter.SearchString.trim();        
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
    this.WarehouseOutputService.GetByBarcodeNoFIFOAsync().subscribe(
      res => {
        this.WarehouseOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        //this.WarehouseOutputDetailBarcodeSearch();

        //this.WarehouseOutputService.BaseParameter.SearchString = environment.InitializationString;        
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
    this.WarehouseOutputService.GetByIDAsync().subscribe(
      res => {
        this.WarehouseOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseOutputDetailBarcodeSearch();
      },
      err => {
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputDetailBarcodeSearch() {
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputDetailBarcodeService.BaseParameter.ParentID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
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
  WarehouseOutputDetailBarcodeCheck() {
    if (this.WarehouseOutputDetailBarcodeService.List) {
      if (this.WarehouseOutputDetailBarcodeService.List.length > 0) {
        this.WarehouseOutputDetailBarcodeActiveCount = environment.InitializationNumber;        
        for (let i = 0; i < this.WarehouseOutputDetailBarcodeService.List.length; i++) {
          if (this.WarehouseOutputDetailBarcodeService.List[i].Active == true) {
            this.WarehouseOutputDetailBarcodeActiveCount = this.WarehouseOutputDetailBarcodeActiveCount + 1;
          }          
        }        
      }
    }
  }
}
