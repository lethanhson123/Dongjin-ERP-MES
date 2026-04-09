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

import { ProductionOrderDetail } from 'src/app/shared/ERP/ProductionOrderDetail.model';
import { ProductionOrderDetailService } from 'src/app/shared/ERP/ProductionOrderDetail.service';

import { ProductionOrderProductionPlan } from 'src/app/shared/ERP/ProductionOrderProductionPlan.model';
import { ProductionOrderProductionPlanService } from 'src/app/shared/ERP/ProductionOrderProductionPlan.service';

import { ProductionOrderOutputSchedule } from 'src/app/shared/ERP/ProductionOrderOutputSchedule.model';
import { ProductionOrderOutputScheduleService } from 'src/app/shared/ERP/ProductionOrderOutputSchedule.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';
import { ProductionOrderModalComponent } from '../production-order-modal/production-order-modal.component';


@Component({
  selector: 'app-production-order-production-plan',
  templateUrl: './production-order-production-plan.component.html',
  styleUrls: ['./production-order-production-plan.component.css']
})
export class ProductionOrderProductionPlanComponent {

  @ViewChild('ProductionOrderDetailSort') ProductionOrderDetailSort: MatSort;
  @ViewChild('ProductionOrderDetailPaginator') ProductionOrderDetailPaginator: MatPaginator;

  @ViewChild('ProductionOrderProductionPlanSort') ProductionOrderProductionPlanSort: MatSort;
  @ViewChild('ProductionOrderProductionPlanPaginator') ProductionOrderProductionPlanPaginator: MatPaginator;

  @ViewChild('ProductionOrderOutputScheduleSort') ProductionOrderOutputScheduleSort: MatSort;
  @ViewChild('ProductionOrderOutputSchedulePaginator') ProductionOrderOutputSchedulePaginator: MatPaginator;

  URLTemplateProductionOrderProductionPlan: string = environment.APIRootURL + "Download/ProductionOrderProductionPlan2026.xlsx";

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ProductionOrderService: ProductionOrderService,
    public ProductionOrderDetailService: ProductionOrderDetailService,
    public ProductionOrderProductionPlanService: ProductionOrderProductionPlanService,
    public ProductionOrderOutputScheduleService: ProductionOrderOutputScheduleService,

    public MaterialService: MaterialService,

  ) {
  }
  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.ProductionOrderSearch();
  }
  ProductionOrderProductionPlanChange(event, files: FileList) {
    if (files) {
      this.ProductionOrderProductionPlanService.FileToUpload = files;
      this.ProductionOrderProductionPlanService.BaseParameter.Event = event;
    }
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
  ProductionOrderModal(ID: number) {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderService.BaseParameter.ID = ID;
    this.ProductionOrderService.GetByIDAsync().subscribe(
      res => {
        this.ProductionOrderService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        const dialog = this.Dialog.open(ProductionOrderModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => {
          this.ProductionOrderSearch();
        });
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
            this.ProductionOrderDetailService.BaseParameter.ParentID = this.ProductionOrderService.ListFilter[0].ID;
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
  ProductionOrderChange() {
    this.ProductionOrderDetailSearch();
    this.ProductionOrderProductionPlanSearch();
    this.ProductionOrderOutputScheduleSearch();
  }
  ProductionOrderDetailSearch() {
    if (this.ProductionOrderDetailService.BaseParameter.ParentID > 0) {
      if (this.ProductionOrderDetailService.BaseParameter.SearchString.length > 0) {
        this.ProductionOrderDetailService.BaseParameter.SearchString = this.ProductionOrderDetailService.BaseParameter.SearchString.trim();
        let List = this.ProductionOrderDetailService.List.filter(o => o.SortOrder == 1);
        this.ProductionOrderDetailService.ListFilter = this.ProductionOrderDetailService.List.filter(o => o.SortOrder > 1 && (o.MaterialCode == this.ProductionOrderDetailService.BaseParameter.SearchString || o.BOMECN == this.ProductionOrderDetailService.BaseParameter.SearchString));
        if (List && List.length > 0) {
          this.ProductionOrderDetailService.ListFilter.push(List[0]);
        }
        this.ProductionOrderDetailService.ListFilter = this.ProductionOrderDetailService.ListFilter.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.ProductionOrderDetailService.DataSource = new MatTableDataSource(this.ProductionOrderDetailService.ListFilter);
        this.ProductionOrderDetailService.DataSource.sort = this.ProductionOrderDetailSort;
        this.ProductionOrderDetailService.DataSource.paginator = this.ProductionOrderDetailPaginator;
      }
      else {
        this.ProductionOrderService.IsShowLoading = true;
        this.ProductionOrderDetailService.GetByParentIDToListAsync().subscribe(
          res => {
            this.ProductionOrderDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.ProductionOrderDetailService.ListFilter = this.ProductionOrderDetailService.List;
            this.ProductionOrderDetailService.DataSource = new MatTableDataSource(this.ProductionOrderDetailService.ListFilter);
            this.ProductionOrderDetailService.DataSource.sort = this.ProductionOrderDetailSort;
            this.ProductionOrderDetailService.DataSource.paginator = this.ProductionOrderDetailPaginator;
          },
          err => {
          },
          () => {
            this.ProductionOrderService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.ProductionOrderDetailService.List = [];
      this.ProductionOrderDetailService.DataSource = new MatTableDataSource(this.ProductionOrderDetailService.List);
      this.ProductionOrderDetailService.DataSource.sort = this.ProductionOrderDetailSort;
      this.ProductionOrderDetailService.DataSource.paginator = this.ProductionOrderDetailPaginator;
    }
  }
  ProductionOrderProductionPlanSearch() {
    if (this.ProductionOrderDetailService.BaseParameter.ParentID > 0) {
      if (this.ProductionOrderProductionPlanService.BaseParameter.SearchString.length > 0) {
        this.ProductionOrderProductionPlanService.BaseParameter.SearchString = this.ProductionOrderProductionPlanService.BaseParameter.SearchString.trim();
        let List = this.ProductionOrderProductionPlanService.List.filter(o => o.SortOrder == 1);
        this.ProductionOrderProductionPlanService.ListFilter = this.ProductionOrderProductionPlanService.List.filter(o => o.SortOrder > 1 && (o.MaterialCode == this.ProductionOrderProductionPlanService.BaseParameter.SearchString));
        if (List && List.length > 0) {
          this.ProductionOrderProductionPlanService.ListFilter.push(List[0]);
        }
        this.ProductionOrderProductionPlanService.ListFilter = this.ProductionOrderProductionPlanService.ListFilter.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.ProductionOrderProductionPlanService.DataSource = new MatTableDataSource(this.ProductionOrderProductionPlanService.ListFilter);
        this.ProductionOrderProductionPlanService.DataSource.sort = this.ProductionOrderProductionPlanSort;
        this.ProductionOrderProductionPlanService.DataSource.paginator = this.ProductionOrderProductionPlanPaginator;
      }
      else {
        this.ProductionOrderProductionPlanService.BaseParameter.ParentID = this.ProductionOrderDetailService.BaseParameter.ParentID;
        this.ProductionOrderService.IsShowLoading = true;
        this.ProductionOrderProductionPlanService.GetByParentIDToListAsync().subscribe(
          res => {
            this.ProductionOrderProductionPlanService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.ProductionOrderProductionPlanService.ListFilter = this.ProductionOrderProductionPlanService.List;
            this.ProductionOrderProductionPlanService.DataSource = new MatTableDataSource(this.ProductionOrderProductionPlanService.ListFilter);
            this.ProductionOrderProductionPlanService.DataSource.sort = this.ProductionOrderProductionPlanSort;
            this.ProductionOrderProductionPlanService.DataSource.paginator = this.ProductionOrderProductionPlanPaginator;
          },
          err => {
          },
          () => {
            this.ProductionOrderService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.ProductionOrderProductionPlanService.List = [];
      this.ProductionOrderProductionPlanService.DataSource = new MatTableDataSource(this.ProductionOrderProductionPlanService.List);
      this.ProductionOrderProductionPlanService.DataSource.sort = this.ProductionOrderProductionPlanSort;
      this.ProductionOrderProductionPlanService.DataSource.paginator = this.ProductionOrderProductionPlanPaginator;
    }
  }
  ProductionOrderProductionPlanSave(element: ProductionOrderProductionPlan) {
    this.ProductionOrderService.IsShowLoading = true;
    element.ParentID = this.ProductionOrderDetailService.BaseParameter.ParentID;
    this.ProductionOrderProductionPlanService.BaseParameter.BaseModel = element;
    this.ProductionOrderProductionPlanService.SaveAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
        this.ProductionOrderProductionPlanSearch();
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.ProductionOrderService.IsShowLoading = false;
      }
    );
  }
  ProductionOrderProductionPlanSaveAndUploadFile() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderProductionPlanService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.ProductionOrderProductionPlanService.FileToUpload = null;
        this.ProductionOrderProductionPlanService.BaseParameter.Event = null;
        this.ProductionOrderProductionPlanSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.ProductionOrderService.IsShowLoading = false;
      }
    );
  }

  ProductionOrderOutputScheduleSearch() {
    if (this.ProductionOrderDetailService.BaseParameter.ParentID > 0) {
      if (this.ProductionOrderOutputScheduleService.BaseParameter.SearchString.length > 0) {
        this.ProductionOrderOutputScheduleService.BaseParameter.SearchString = this.ProductionOrderOutputScheduleService.BaseParameter.SearchString.trim();
        if (this.ProductionOrderOutputScheduleService.DataSource) {
          this.ProductionOrderOutputScheduleService.DataSource.filter = this.ProductionOrderOutputScheduleService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.ProductionOrderOutputScheduleService.BaseParameter.ParentID = this.ProductionOrderDetailService.BaseParameter.ParentID;
        this.ProductionOrderService.IsShowLoading = true;
        this.ProductionOrderOutputScheduleService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.ProductionOrderOutputScheduleService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.ProductionOrderOutputScheduleService.DataSource = new MatTableDataSource(this.ProductionOrderOutputScheduleService.List);
            this.ProductionOrderOutputScheduleService.DataSource.sort = this.ProductionOrderOutputScheduleSort;
            this.ProductionOrderOutputScheduleService.DataSource.paginator = this.ProductionOrderOutputSchedulePaginator;
          },
          err => {
          },
          () => {
            this.ProductionOrderService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.ProductionOrderOutputScheduleService.List = [];
      this.ProductionOrderOutputScheduleService.DataSource = new MatTableDataSource(this.ProductionOrderOutputScheduleService.List);
      this.ProductionOrderOutputScheduleService.DataSource.sort = this.ProductionOrderOutputScheduleSort;
      this.ProductionOrderOutputScheduleService.DataSource.paginator = this.ProductionOrderOutputSchedulePaginator;
    }
  }
  ProductionOrderOutputScheduleSave(element: ProductionOrderOutputSchedule) {
    this.ProductionOrderService.IsShowLoading = true;
    // this.NotificationService.warn(environment.SaveSuccess);
    // this.ProductionOrderOutputScheduleSearch();
    element.ParentID = this.ProductionOrderDetailService.BaseParameter.ParentID;
    this.ProductionOrderOutputScheduleService.BaseParameter.BaseModel = element;
    this.ProductionOrderOutputScheduleService.SaveAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
        this.ProductionOrderOutputScheduleSearch();
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.ProductionOrderService.IsShowLoading = false;
      }
    );
  }
  ProductionOrderOutputScheduleBegin(value, element: ProductionOrderOutputSchedule) {
    element.Begin = new Date(value);
  }
  ProductionOrderOutputScheduleEnd(value, element: ProductionOrderOutputSchedule) {
    element.End = new Date(value);
  }
}
