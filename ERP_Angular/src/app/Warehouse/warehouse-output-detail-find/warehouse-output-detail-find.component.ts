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

import { WarehouseOutputDetail } from 'src/app/shared/ERP/WarehouseOutputDetail.model';
import { WarehouseOutputDetailService } from 'src/app/shared/ERP/WarehouseOutputDetail.service';

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';

@Component({
  selector: 'app-warehouse-output-detail-find',
  templateUrl: './warehouse-output-detail-find.component.html',
  styleUrls: ['./warehouse-output-detail-find.component.css']
})
export class WarehouseOutputDetailFindComponent {

  @ViewChild('WarehouseOutputDetailSort') WarehouseOutputDetailSort: MatSort;
  @ViewChild('WarehouseOutputDetailPaginator') WarehouseOutputDetailPaginator: MatPaginator;


  constructor(
    public ActiveRouter: ActivatedRoute,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseOutputService: WarehouseOutputService,
    public WarehouseOutputDetailService: WarehouseOutputDetailService,

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
    this.WarehouseOutputService.BaseParameter.SearchString = environment.InitializationString;
    this.WarehouseOutputService.BaseParameter.ID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
    this.WarehouseOutputService.GetByBarcodeNoFIFOAsync().subscribe(
      res => {
        this.WarehouseOutputDetailSearch();
      },
      err => {
      },
      () => {
        //this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputSearch() {
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputService.GetByIDAsync().subscribe(
      res => {
        this.WarehouseOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseOutputDetailSearch();

        this.WarehouseOutputService.BaseResult = (res as BaseResult);
        if (this.WarehouseOutputService.BaseResult.IsCheck == true) {
          this.NotificationService.warn(environment.ScanNotSuccess);
          let audio = new Audio("/Media/SuccessNot.wav");
          audio.play();
        }
        else {
          this.NotificationService.warn(environment.ScanSuccess);
          let audio = new Audio("/Media/Success.wav");
          audio.play();
        }
        
      },
      err => {
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputDetailSearch() {
    this.WarehouseOutputDetailService.BaseParameter.ParentID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputDetailService.GetByParentIDToListAsync().subscribe(
      res => {
        this.WarehouseOutputDetailService.List = (res as BaseResult).List.sort((a, b) => (a.MaterialID > b.MaterialID ? 1 : -1));
        this.WarehouseOutputDetailService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailService.List);
        this.WarehouseOutputDetailService.DataSource.sort = this.WarehouseOutputDetailSort;
        this.WarehouseOutputDetailService.DataSource.paginator = this.WarehouseOutputDetailPaginator;
      },
      err => {
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
}