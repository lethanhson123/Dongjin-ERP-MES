import { Component, OnInit, ViewChild } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets, Chart, ChartConfiguration, ChartData } from 'chart.js';
import { Color, Label, SingleDataSet, monkeyPatchChartJsLegend, monkeyPatchChartJsTooltip } from 'ng2-charts';

import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { ProductionOrder } from 'src/app/shared/ERP/ProductionOrder.model';
import { ProductionOrderService } from 'src/app/shared/ERP/ProductionOrder.service';

import { ProductionOrderBOM } from 'src/app/shared/ERP/ProductionOrderBOM.model';
import { ProductionOrderBOMService } from 'src/app/shared/ERP/ProductionOrderBOM.service';

import { ProductionOrderBOMDetail } from 'src/app/shared/ERP/ProductionOrderBOMDetail.model';
import { ProductionOrderBOMDetailService } from 'src/app/shared/ERP/ProductionOrderBOMDetail.service';


import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';


@Component({
  selector: 'app-production-order-bom',
  templateUrl: './production-order-bom.component.html',
  styleUrls: ['./production-order-bom.component.css']
})
export class ProductionOrderBOMComponent {

  @ViewChild('ProductionOrderBOMSort') ProductionOrderBOMSort: MatSort;
  @ViewChild('ProductionOrderBOMPaginator') ProductionOrderBOMPaginator: MatPaginator;

  @ViewChild('ProductionOrderBOMDetailSort') ProductionOrderBOMDetailSort: MatSort;
  @ViewChild('ProductionOrderBOMDetailPaginator') ProductionOrderBOMDetailPaginator: MatPaginator;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ProductionOrderService: ProductionOrderService,
    public ProductionOrderBOMService: ProductionOrderBOMService,
    public ProductionOrderBOMDetailService: ProductionOrderBOMDetailService,

    public MaterialService: MaterialService,

  ) {
  }
  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.ProductionOrderSearch();
  }
  MaterialModalOpen(element: Material) {
    this.MaterialService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(MaterialModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {

    });
  }
  MaterialModal(ID: number) {
    this.ProductionOrderService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = ID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.ProductionOrderService.IsShowLoading = false;
      }
    );
  }

  ProductionOrderSearch() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.ProductionOrderService.BaseParameter.Active = true;
    this.ProductionOrderService.BaseParameter.IsComplete = false;
    this.ProductionOrderService.GetByMembershipID_Active_IsCompleteToListAsync().subscribe(
      res => {
        this.ProductionOrderService.ListFilter = (res as BaseResult).List;
        if (this.ProductionOrderService.ListFilter) {
          if (this.ProductionOrderService.ListFilter.length > 0) {
            this.ProductionOrderBOMService.BaseParameter.ParentID = this.ProductionOrderService.ListFilter[0].ID;
          }
        }
      },
      err => {
      },
      () => {
        this.ProductionOrderService.IsShowLoading = false;
      }
    );
  }
  ProductionOrderBOMSearch() {
    if (this.ProductionOrderBOMService.BaseParameter.SearchString.length > 0) {
      this.ProductionOrderBOMService.BaseParameter.SearchString = this.ProductionOrderBOMService.BaseParameter.SearchString.trim();
      if (this.ProductionOrderBOMService.DataSource) {
        this.ProductionOrderBOMService.DataSource.filter = this.ProductionOrderBOMService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.ProductionOrderBOMService.BaseParameter.ParentID = this.ProductionOrderBOMService.BaseParameter.ParentID;
      this.ProductionOrderService.IsShowLoading = true;
      this.ProductionOrderBOMService.GetByParentIDToListAsync().subscribe(
        res => {
          this.ProductionOrderBOMService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.ProductionOrderBOMService.DataSource = new MatTableDataSource(this.ProductionOrderBOMService.List);
          this.ProductionOrderBOMService.DataSource.sort = this.ProductionOrderBOMSort;
          this.ProductionOrderBOMService.DataSource.paginator = this.ProductionOrderBOMPaginator;
          this.ProductionOrderBOMDetailSearch();
        },
        err => {
        },
        () => {
          this.ProductionOrderService.IsShowLoading = false;
        }
      );
    }
  }
  ProductionOrderBOMDetailSearch() {
    if (this.ProductionOrderBOMDetailService.BaseParameter.SearchString.length > 0) {
      this.ProductionOrderBOMDetailService.BaseParameter.SearchString = this.ProductionOrderBOMDetailService.BaseParameter.SearchString.trim();
      if (this.ProductionOrderBOMDetailService.DataSource) {
        this.ProductionOrderBOMDetailService.DataSource.filter = this.ProductionOrderBOMDetailService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.ProductionOrderBOMDetailService.BaseParameter.ParentID = this.ProductionOrderBOMService.BaseParameter.ParentID;
      this.ProductionOrderService.IsShowLoading = true;
      this.ProductionOrderBOMDetailService.GetByParentIDToListAsync().subscribe(
        res => {
          this.ProductionOrderBOMDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.ProductionOrderBOMDetailService.DataSource = new MatTableDataSource(this.ProductionOrderBOMDetailService.List);
          this.ProductionOrderBOMDetailService.DataSource.sort = this.ProductionOrderBOMDetailSort;
          this.ProductionOrderBOMDetailService.DataSource.paginator = this.ProductionOrderBOMDetailPaginator;
        },
        err => {
        },
        () => {
          this.ProductionOrderService.IsShowLoading = false;
        }
      );
    }
  }

}

