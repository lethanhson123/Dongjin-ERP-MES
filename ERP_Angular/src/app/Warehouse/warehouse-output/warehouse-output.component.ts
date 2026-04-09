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
import { WarehouseOutputModalComponent } from '../warehouse-output-modal/warehouse-output-modal.component';


@Component({
  selector: 'app-warehouse-output',
  templateUrl: './warehouse-output.component.html',
  styleUrls: ['./warehouse-output.component.css']
})
export class WarehouseOutputComponent {

  @ViewChild('WarehouseOutputSort') WarehouseOutputSort: MatSort;
  @ViewChild('WarehouseOutputPaginator') WarehouseOutputPaginator: MatPaginator;

  @ViewChild('WarehouseOutputSort1') WarehouseOutputSort1: MatSort;
  @ViewChild('WarehouseOutputPaginator1') WarehouseOutputPaginator1: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseOutputService: WarehouseOutputService,
  ) {
    this.WarehouseOutputService.BaseParameter.IsComplete = false;
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.WarehouseOutputSearch();
  }
  DateBegin(value) {
    this.WarehouseOutputService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.WarehouseOutputService.BaseParameter.DateEnd = new Date(value);
  }
  WarehouseOutputSearch() {
    if (this.WarehouseOutputService.BaseParameter.SearchString && this.WarehouseOutputService.BaseParameter.SearchString.length > 0) {
      this.WarehouseOutputService.BaseParameter.SearchString = this.WarehouseOutputService.BaseParameter.SearchString.trim();
      if (this.WarehouseOutputService.DataSource) {
        this.WarehouseOutputService.DataSource.filter = this.WarehouseOutputService.BaseParameter.SearchString.toLowerCase();
      }
      if (this.WarehouseOutputService.DataSourceFilter) {
        this.WarehouseOutputService.DataSourceFilter.filter = this.WarehouseOutputService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseOutputService.IsShowLoading = true;
      this.WarehouseOutputService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
      this.WarehouseOutputService.GetByMembershipID_DateBegin_DateEndToListAsync().subscribe(
        res => {
          this.WarehouseOutputService.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));

          if (this.WarehouseOutputService.BaseParameter.IsComplete == true) {
            this.WarehouseOutputService.ListFilter = this.WarehouseOutputService.List.filter(o => o.Description == "MES");
            this.WarehouseOutputService.ListFilter01 = this.WarehouseOutputService.List.filter(o => o.Description != "MES");
          }
          else {
            this.WarehouseOutputService.ListFilter = this.WarehouseOutputService.List.filter(o => o.Description == "MES" && o.IsComplete != true);
            this.WarehouseOutputService.ListFilter01 = this.WarehouseOutputService.List.filter(o => o.Description != "MES" && o.IsComplete != true);
          }

          this.WarehouseOutputService.ListFilter = this.WarehouseOutputService.ListFilter.sort((a, b) => (a.ID < b.ID ? 1 : -1));
          this.WarehouseOutputService.ListFilter01 = this.WarehouseOutputService.ListFilter01.sort((a, b) => (a.ID < b.ID ? 1 : -1));

          this.WarehouseOutputService.DataSource = new MatTableDataSource(this.WarehouseOutputService.ListFilter);
          this.WarehouseOutputService.DataSource.sort = this.WarehouseOutputSort;
          this.WarehouseOutputService.DataSource.paginator = this.WarehouseOutputPaginator;

          this.WarehouseOutputService.DataSourceFilter = new MatTableDataSource(this.WarehouseOutputService.ListFilter01);
          this.WarehouseOutputService.DataSourceFilter.sort = this.WarehouseOutputSort1;
          this.WarehouseOutputService.DataSourceFilter.paginator = this.WarehouseOutputPaginator1;

        },
        err => {
        },
        () => {
          this.WarehouseOutputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseOutputDelete(element: WarehouseOutput) {
    this.WarehouseOutputService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.WarehouseOutputService.ComponentDelete(this.WarehouseOutputSort, this.WarehouseOutputPaginator));
  }
  WarehouseOutputModal(element: WarehouseOutput) {
    this.WarehouseOutputService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseOutputModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
      this.WarehouseOutputSearch();
    });
  }
  WarehouseOutputAdd() {
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputService.BaseParameter.ID = environment.InitializationNumber;
    this.WarehouseOutputService.GetByIDAsync().subscribe(
      res => {
        this.WarehouseOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseOutputModal(this.WarehouseOutputService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
}

