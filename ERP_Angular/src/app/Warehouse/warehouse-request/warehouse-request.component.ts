import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseRequest } from 'src/app/shared/ERP/WarehouseRequest.model';
import { WarehouseRequestService } from 'src/app/shared/ERP/WarehouseRequest.service';
import { WarehouseRequestModalComponent } from '../warehouse-request-modal/warehouse-request-modal.component';


@Component({
  selector: 'app-warehouse-request',
  templateUrl: './warehouse-request.component.html',
  styleUrls: ['./warehouse-request.component.css']
})
export class WarehouseRequestComponent {

  @ViewChild('WarehouseRequestSort') WarehouseRequestSort: MatSort;
  @ViewChild('WarehouseRequestPaginator') WarehouseRequestPaginator: MatPaginator;

  @ViewChild('WarehouseRequestSort1') WarehouseRequestSort1: MatSort;
  @ViewChild('WarehouseRequestPaginator1') WarehouseRequestPaginator1: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseRequestService: WarehouseRequestService,
  ) {
    this.WarehouseRequestService.BaseParameter.IsComplete = false;
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.WarehouseRequestSearch();
    this.StartTimer();
  }
  StartTimer() {
    setInterval(() => {
      this.WarehouseRequestSearch();
    }, environment.Timer)
  }
  DateBegin(value) {
    this.WarehouseRequestService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.WarehouseRequestService.BaseParameter.DateEnd = new Date(value);
  }
  WarehouseRequestSearch() {
    if (this.WarehouseRequestService.BaseParameter.SearchString && this.WarehouseRequestService.BaseParameter.SearchString.length > 0) {
      this.WarehouseRequestService.BaseParameter.SearchString = this.WarehouseRequestService.BaseParameter.SearchString.trim();
      if (this.WarehouseRequestService.DataSource) {
        this.WarehouseRequestService.DataSource.filter = this.WarehouseRequestService.BaseParameter.SearchString.toLowerCase();
      }
      if (this.WarehouseRequestService.DataSourceFilter) {
        this.WarehouseRequestService.DataSourceFilter.filter = this.WarehouseRequestService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseRequestService.IsShowLoading = true;
      this.WarehouseRequestService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
      this.WarehouseRequestService.GetByMembershipID_DateBegin_DateEndToListAsync().subscribe(
        res => {
          this.WarehouseRequestService.List = (res as BaseResult).List.sort((a, b) => (a.ID < b.ID ? 1 : -1));

          if (this.WarehouseRequestService.BaseParameter.IsComplete == true) {
            this.WarehouseRequestService.ListFilter = this.WarehouseRequestService.List.filter(o => o.Description == "MES");
            this.WarehouseRequestService.ListFilter01 = this.WarehouseRequestService.List.filter(o => o.Description != "MES");
          }
          else {
            this.WarehouseRequestService.ListFilter = this.WarehouseRequestService.List.filter(o => o.Description == "MES" && o.IsManagerSupplier != true);
            this.WarehouseRequestService.ListFilter01 = this.WarehouseRequestService.List.filter(o => o.Description != "MES" && o.IsManagerSupplier != true);
          }

          this.WarehouseRequestService.ListFilter = this.WarehouseRequestService.ListFilter.sort((a, b) => (a.ID < b.ID ? 1 : -1));
          this.WarehouseRequestService.ListFilter01 = this.WarehouseRequestService.ListFilter01.sort((a, b) => (a.ID < b.ID ? 1 : -1));

          this.WarehouseRequestService.DataSource = new MatTableDataSource(this.WarehouseRequestService.ListFilter);
          this.WarehouseRequestService.DataSource.sort = this.WarehouseRequestSort;
          this.WarehouseRequestService.DataSource.paginator = this.WarehouseRequestPaginator;

          this.WarehouseRequestService.DataSourceFilter = new MatTableDataSource(this.WarehouseRequestService.ListFilter01);
          this.WarehouseRequestService.DataSourceFilter.sort = this.WarehouseRequestSort1;
          this.WarehouseRequestService.DataSourceFilter.paginator = this.WarehouseRequestPaginator1;

          let audio = new Audio("/Media/Success.wav");
          audio.play();
        },
        err => {
        },
        () => {
          this.WarehouseRequestService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseRequestDelete(element: WarehouseRequest) {
    this.WarehouseRequestService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.WarehouseRequestService.ComponentDelete(this.WarehouseRequestSort, this.WarehouseRequestPaginator));
  }
  WarehouseRequestModal(element: WarehouseRequest) {
    this.WarehouseRequestService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseRequestModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
      this.WarehouseRequestSearch();
    });
  }
  WarehouseRequestAdd() {
    this.WarehouseRequestService.IsShowLoading = true;
    this.WarehouseRequestService.BaseParameter.ID = environment.InitializationNumber;
    this.WarehouseRequestService.GetByIDAsync().subscribe(
      res => {
        this.WarehouseRequestService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseRequestModal(this.WarehouseRequestService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.WarehouseRequestService.IsShowLoading = false;
      }
    );
  }
}

