import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Traceability } from 'src/app/shared/ERP/Traceability.model';
import { TraceabilityService } from 'src/app/shared/ERP/Traceability.service';

import { TraceabilityDetail } from 'src/app/shared/ERP/TraceabilityDetail.model';
import { TraceabilityDetailService } from 'src/app/shared/ERP/TraceabilityDetail.service';

@Component({
  selector: 'app-barcode',
  templateUrl: './barcode.component.html',
  styleUrls: ['./barcode.component.css']
})
export class BarcodeComponent implements OnInit {

  @ViewChild('TraceabilityDetailSort01') TraceabilityDetailSort01: MatSort;
  @ViewChild('TraceabilityDetailPaginator01') TraceabilityDetailPaginator01: MatPaginator;

  @ViewChild('TraceabilityDetailSort021') TraceabilityDetailSort021: MatSort;
  @ViewChild('TraceabilityDetailPaginator021') TraceabilityDetailPaginator021: MatPaginator;

  @ViewChild('TraceabilityDetailSort02') TraceabilityDetailSort02: MatSort;
  @ViewChild('TraceabilityDetailPaginator02') TraceabilityDetailPaginator02: MatPaginator;

  @ViewChild('TraceabilityDetailSort03') TraceabilityDetailSort03: MatSort;
  @ViewChild('TraceabilityDetailPaginator03') TraceabilityDetailPaginator03: MatPaginator;

  @ViewChild('TraceabilityDetailSort04') TraceabilityDetailSort04: MatSort;
  @ViewChild('TraceabilityDetailPaginator04') TraceabilityDetailPaginator04: MatPaginator;

  @ViewChild('TraceabilityDetailSort05') TraceabilityDetailSort05: MatSort;
  @ViewChild('TraceabilityDetailPaginator05') TraceabilityDetailPaginator05: MatPaginator;

  @ViewChild('TraceabilityDetailSort06') TraceabilityDetailSort06: MatSort;
  @ViewChild('TraceabilityDetailPaginator06') TraceabilityDetailPaginator06: MatPaginator;

  @ViewChild('TraceabilityDetailSort07') TraceabilityDetailSort07: MatSort;
  @ViewChild('TraceabilityDetailPaginator07') TraceabilityDetailPaginator07: MatPaginator;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public TraceabilityService: TraceabilityService,
    public TraceabilityDetailService: TraceabilityDetailService,

  ) {
    this.TraceabilityService.BaseParameter.SearchString = this.ActiveRouter.snapshot.params.SearchString;
    this.TraceabilitySearch();
  }

  ngOnInit(): void {
  }
  TraceabilitySearch() {
    if (this.TraceabilityService.BaseParameter.SearchString != null && this.TraceabilityService.BaseParameter.SearchString.length > 0) {
      this.TraceabilityService.IsShowLoading = true;
      this.TraceabilityService.GetBySearchStringAsync().subscribe(
        res => {
          this.TraceabilityService.BaseParameter.SearchString = environment.InitializationString;
          this.TraceabilityService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
          this.TraceabilityDetailSearch();
        },
        err => {
        },
        () => {
          this.TraceabilityService.IsShowLoading = false;
        }
      );
    }
  }
  TraceabilityDetailSearch() {
    this.TraceabilityDetailService.List = [];
    this.TraceabilityDetailService.DataSource05 = new MatTableDataSource(this.TraceabilityDetailService.List);
    this.TraceabilityDetailService.DataSource05.sort = this.TraceabilityDetailSort05;
    this.TraceabilityDetailService.DataSource05.paginator = this.TraceabilityDetailPaginator05;

    this.TraceabilityDetailService.DataSource04 = new MatTableDataSource(this.TraceabilityDetailService.List);
    this.TraceabilityDetailService.DataSource04.sort = this.TraceabilityDetailSort04;
    this.TraceabilityDetailService.DataSource04.paginator = this.TraceabilityDetailPaginator04;

    this.TraceabilityDetailService.DataSource03 = new MatTableDataSource(this.TraceabilityDetailService.List);
    this.TraceabilityDetailService.DataSource03.sort = this.TraceabilityDetailSort03;
    this.TraceabilityDetailService.DataSource03.paginator = this.TraceabilityDetailPaginator03;

    this.TraceabilityDetailService.DataSource02 = new MatTableDataSource(this.TraceabilityDetailService.List);
    this.TraceabilityDetailService.DataSource02.sort = this.TraceabilityDetailSort02;
    this.TraceabilityDetailService.DataSource02.paginator = this.TraceabilityDetailPaginator02;


    this.TraceabilityDetailService.DataSource021 = new MatTableDataSource(this.TraceabilityDetailService.List);
    this.TraceabilityDetailService.DataSource021.sort = this.TraceabilityDetailSort021;
    this.TraceabilityDetailService.DataSource021.paginator = this.TraceabilityDetailPaginator021;

    this.TraceabilityDetailService.DataSource07 = new MatTableDataSource(this.TraceabilityDetailService.List);
    this.TraceabilityDetailService.DataSource07.sort = this.TraceabilityDetailSort07;
    this.TraceabilityDetailService.DataSource07.paginator = this.TraceabilityDetailPaginator07;


    this.TraceabilityDetailService.BaseParameter.ParentID = this.TraceabilityService.BaseParameter.BaseModel.ID;
    if (this.TraceabilityDetailService.BaseParameter.ParentID > 0) {
      this.TraceabilityDetailService.IsShowLoading = true;
      this.TraceabilityDetailService.GetByParentIDAndActiveToListModelTranferAsync().subscribe(
        res => {
          this.TraceabilityDetailService.List = (res as BaseResult).List;
          for (let i = 0; i < this.TraceabilityDetailService.List.length; i++) {
            if (this.TraceabilityDetailService.List[i].TraceabilityDetail != null) {
              switch (this.TraceabilityDetailService.List[i].TraceabilityDetail.Code) {
                case "Material":
                  this.TraceabilityDetailService.DataSource06 = new MatTableDataSource(this.TraceabilityDetailService.List[i].ListWarehouseInputDetailBarcodeMaterial);
                  this.TraceabilityDetailService.DataSource06.sort = this.TraceabilityDetailSort06;
                  this.TraceabilityDetailService.DataSource06.paginator = this.TraceabilityDetailPaginator06;
                  break;
                case "KOMAX":
                  if (this.TraceabilityDetailService.List[i].Listtorderlist != null && this.TraceabilityDetailService.List[i].Listtorderlist.length > 0) {
                    for (let j = 0; j < this.TraceabilityDetailService.List[i].Listtorderinspection.length; j++) {
                      this.TraceabilityDetailService.List[i].Listtorderinspection[j].MC = this.TraceabilityDetailService.List[i].Listtorderlist[0].MC;
                      this.TraceabilityDetailService.List[i].Listtorderinspection[j].MC2 = this.TraceabilityDetailService.List[i].Listtorderlist[0].MC2;
                      this.TraceabilityDetailService.List[i].Listtorderinspection[j].CCH_W1 = this.TraceabilityDetailService.List[i].Listtorderlist[0].CCH_W1;
                      this.TraceabilityDetailService.List[i].Listtorderinspection[j].ICH_W1 = this.TraceabilityDetailService.List[i].Listtorderlist[0].ICH_W1;
                      this.TraceabilityDetailService.List[i].Listtorderinspection[j].CCH_W2 = this.TraceabilityDetailService.List[i].Listtorderlist[0].CCH_W2;
                      this.TraceabilityDetailService.List[i].Listtorderinspection[j].ICH_W2 = this.TraceabilityDetailService.List[i].Listtorderlist[0].ICH_W2;
                    }
                  }
                  this.TraceabilityDetailService.DataSource05 = new MatTableDataSource(this.TraceabilityDetailService.List[i].Listtorderinspection);
                  this.TraceabilityDetailService.DataSource05.sort = this.TraceabilityDetailSort05;
                  this.TraceabilityDetailService.DataSource05.paginator = this.TraceabilityDetailPaginator05;
                  break;
                case "LP":
                  if (this.TraceabilityDetailService.List[i].Listtorderlist_lp != null && this.TraceabilityDetailService.List[i].Listtorderlist_lp.length > 0) {
                    for (let j = 0; j < this.TraceabilityDetailService.List[i].Listtorderinspection_lp.length; j++) {
                      this.TraceabilityDetailService.List[i].Listtorderinspection_lp[j].CCH_W1 = this.TraceabilityDetailService.List[i].Listtorderlist_lp[0].CCH_W1;
                      this.TraceabilityDetailService.List[i].Listtorderinspection_lp[j].ICH_W1 = this.TraceabilityDetailService.List[i].Listtorderlist_lp[0].ICH_W1;
                      this.TraceabilityDetailService.List[i].Listtorderinspection_lp[j].CCH_W2 = this.TraceabilityDetailService.List[i].Listtorderlist_lp[0].CCH_W2;
                      this.TraceabilityDetailService.List[i].Listtorderinspection_lp[j].ICH_W2 = this.TraceabilityDetailService.List[i].Listtorderlist_lp[0].ICH_W2;
                    }
                  }
                  this.TraceabilityDetailService.DataSource04 = new MatTableDataSource(this.TraceabilityDetailService.List[i].Listtorderinspection_lp);
                  this.TraceabilityDetailService.DataSource04.sort = this.TraceabilityDetailSort04;
                  this.TraceabilityDetailService.DataSource04.paginator = this.TraceabilityDetailPaginator04;
                  break;
                case "SW":
                  this.TraceabilityDetailService.DataSource03 = new MatTableDataSource(this.TraceabilityDetailService.List[i].Listtorderinspection_sw);
                  this.TraceabilityDetailService.DataSource03.sort = this.TraceabilityDetailSort03;
                  this.TraceabilityDetailService.DataSource03.paginator = this.TraceabilityDetailPaginator03;
                  break;
                case "SPST":
                  if (this.TraceabilityDetailService.List[i].Listtorderlist_spst != null && this.TraceabilityDetailService.List[i].Listtorderlist_spst.length > 0) {
                    for (let j = 0; j < this.TraceabilityDetailService.List[i].Listtorderinspection_spst.length; j++) {
                      this.TraceabilityDetailService.List[i].Listtorderinspection_spst[j].MC2 = this.TraceabilityDetailService.List[i].Listtorderlist_spst[0].MC;
                      this.TraceabilityDetailService.List[i].Listtorderinspection_spst[j].CREATE_DTM = this.TraceabilityDetailService.List[i].Listtorderlist_spst[0].CREATE_DTM;
                      this.TraceabilityDetailService.List[i].Listtorderinspection_spst[j].CREATE_USER = this.TraceabilityDetailService.List[i].Listtorderlist_spst[0].CREATE_USER;
                    }
                  }
                  this.TraceabilityDetailService.DataSource02 = new MatTableDataSource(this.TraceabilityDetailService.List[i].Listtorderinspection_spst);
                  this.TraceabilityDetailService.DataSource02.sort = this.TraceabilityDetailSort02;
                  this.TraceabilityDetailService.DataSource02.paginator = this.TraceabilityDetailPaginator02;
                  break;
                case "LeadNoSPST":
                  this.TraceabilityDetailService.DataSource021 = new MatTableDataSource(this.TraceabilityDetailService.List[i].ListTraceabilityHistory);
                  this.TraceabilityDetailService.DataSource021.sort = this.TraceabilityDetailSort021;
                  this.TraceabilityDetailService.DataSource021.paginator = this.TraceabilityDetailPaginator021;
                  break;
                case "FinishGoods":
                  this.TraceabilityDetailService.DataSource07 = new MatTableDataSource(this.TraceabilityDetailService.List[i].ListBOM);
                  this.TraceabilityDetailService.DataSource07.sort = this.TraceabilityDetailSort07;
                  this.TraceabilityDetailService.DataSource07.paginator = this.TraceabilityDetailPaginator07;
                  break;
              }
            }
          }
        },
        err => {
        },
        () => {
          this.TraceabilityService.IsShowLoading = false;
        }
      );
    }
  }
  TraceabilityPrint() {
  }
}
