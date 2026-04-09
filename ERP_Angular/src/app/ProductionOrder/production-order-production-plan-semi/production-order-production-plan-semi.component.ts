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

import { ProductionOrderProductionPlan } from 'src/app/shared/ERP/ProductionOrderProductionPlan.model';
import { ProductionOrderProductionPlanService } from 'src/app/shared/ERP/ProductionOrderProductionPlan.service';

import { ProductionOrderProductionPlanSemi } from 'src/app/shared/ERP/ProductionOrderProductionPlanSemi.model';
import { ProductionOrderProductionPlanSemiService } from 'src/app/shared/ERP/ProductionOrderProductionPlanSemi.service';

import { ProductionOrderCuttingOrder } from 'src/app/shared/ERP/ProductionOrderCuttingOrder.model';
import { ProductionOrderCuttingOrderService } from 'src/app/shared/ERP/ProductionOrderCuttingOrder.service';

import { ProductionOrderSPSTOrder } from 'src/app/shared/ERP/ProductionOrderSPSTOrder.model';
import { ProductionOrderSPSTOrderService } from 'src/app/shared/ERP/ProductionOrderSPSTOrder.service';

import { ProductionOrderProductionPlanBackup } from 'src/app/shared/ERP/ProductionOrderProductionPlanBackup.model';
import { ProductionOrderProductionPlanBackupService } from 'src/app/shared/ERP/ProductionOrderProductionPlanBackup.service';


import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';


import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';
import { ProductionOrderProductionPlanBackupModalComponent } from '../production-order-production-plan-backup-modal/production-order-production-plan-backup-modal.component';
import { ProductionOrderProductionPlanBackupComponent } from '../production-order-production-plan-backup/production-order-production-plan-backup.component';




@Component({
  selector: 'app-production-order-production-plan-semi',
  templateUrl: './production-order-production-plan-semi.component.html',
  styleUrls: ['./production-order-production-plan-semi.component.css']
})
export class ProductionOrderProductionPlanSemiComponent {

  @ViewChild('ProductionOrderProductionPlanSort') ProductionOrderProductionPlanSort: MatSort;
  @ViewChild('ProductionOrderProductionPlanPaginator') ProductionOrderProductionPlanPaginator: MatPaginator;

  @ViewChild('ProductionOrderProductionPlanSemiSort') ProductionOrderProductionPlanSemiSort: MatSort;
  @ViewChild('ProductionOrderProductionPlanSemiPaginator') ProductionOrderProductionPlanSemiPaginator: MatPaginator;

  @ViewChild('ProductionOrderCuttingOrderSort') ProductionOrderCuttingOrderSort: MatSort;
  @ViewChild('ProductionOrderCuttingOrderPaginator') ProductionOrderCuttingOrderPaginator: MatPaginator;

  @ViewChild('ProductionOrderSPSTOrderSort') ProductionOrderSPSTOrderSort: MatSort;
  @ViewChild('ProductionOrderSPSTOrderPaginator') ProductionOrderSPSTOrderPaginator: MatPaginator;

  URLTemplateProductionOrderProductionPlan: string = environment.APIProductionOrderRootURL + "Download/ProductionOrderProductionPlan2026.xlsx";


  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ProductionOrderService: ProductionOrderService,
    public ProductionOrderProductionPlanService: ProductionOrderProductionPlanService,
    public ProductionOrderProductionPlanSemiService: ProductionOrderProductionPlanSemiService,
    public ProductionOrderCuttingOrderService: ProductionOrderCuttingOrderService,
    public ProductionOrderSPSTOrderService: ProductionOrderSPSTOrderService,
    public ProductionOrderProductionPlanBackupService: ProductionOrderProductionPlanBackupService,

    public MaterialService: MaterialService,

  ) {
    this.ProductionOrderProductionPlanSemiService.BaseParameter.Active001 = false;
    this.ProductionOrderProductionPlanService.BaseParameter.IsShow = true;
    this.ProductionOrderProductionPlanSemiService.BaseParameter.IsShow = true;
    this.ProductionOrderCuttingOrderService.BaseParameter.IsShow = true;
    this.ProductionOrderSPSTOrderService.BaseParameter.IsShow = true;

    this.ProductionOrderProductionPlanService.BaseParameter.Active = true;
    this.ProductionOrderCuttingOrderService.BaseParameter.Active = true;

    this.ProductionOrderProductionPlanSemiService.BaseParameter.Active = true;
    this.ProductionOrderProductionPlanSemiService.BaseParameter.IsComplete = false;

    this.ProductionOrderSearch();

  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  ProductionOrderChange() {
    this.ProductionOrderProductionPlanSearch();
    this.ProductionOrderProductionPlanSemiSearch();
    this.ProductionOrderCuttingOrderSearch();
    this.ProductionOrderSPSTOrderSearch();
  }


  ProductionOrderCuttingOrderDate(value) {
    this.ProductionOrderCuttingOrderService.BaseParameter.Date = new Date(value);
  }
  ProductionOrderSPSTOrderDate(value) {
    this.ProductionOrderSPSTOrderService.BaseParameter.Date = new Date(value);
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
  ProductionOrderProductionPlanBackupSearch() {
    this.ProductionOrderProductionPlanBackupService.BaseParameter.ParentID = this.ProductionOrderProductionPlanSemiService.BaseParameter.ParentID;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(ProductionOrderProductionPlanBackupModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
  ProductionOrderProductionPlanBackupCuttingSearch() {
    this.ProductionOrderProductionPlanBackupService.BaseParameter.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(ProductionOrderProductionPlanBackupComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
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
            this.ProductionOrderProductionPlanSemiService.BaseParameter.ParentID = this.ProductionOrderService.ListFilter[0].ID;
            this.ProductionOrderChange();
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
  ProductionOrderProductionPlanChange(event, files: FileList) {
    if (files) {
      this.ProductionOrderProductionPlanService.FileToUpload = files;
      this.ProductionOrderProductionPlanService.BaseParameter.Event = event;
    }
  }
  ProductionOrderProductionPlanSync() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderProductionPlanService.SyncQuantityToQuantityCutAsync().subscribe(
      res => {
        this.ProductionOrderProductionPlanSearch();
        this.ProductionOrderProductionPlanSemiSearch();
      },
      err => {
      },
      () => {
        this.ProductionOrderService.IsShowLoading = false;
      }
    );
  }
  ProductionOrderProductionPlanSearch() {

    if (this.ProductionOrderProductionPlanService.BaseParameter.SearchString && this.ProductionOrderProductionPlanService.BaseParameter.SearchString.length > 0) {
      this.ProductionOrderProductionPlanService.BaseParameter.SearchString = this.ProductionOrderProductionPlanService.BaseParameter.SearchString.trim();
      // if (this.ProductionOrderProductionPlanService.DataSource) {
      //   this.ProductionOrderProductionPlanService.DataSource.filter = this.ProductionOrderProductionPlanService.BaseParameter.SearchString.toLowerCase();
      // }

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
      this.ProductionOrderProductionPlanService.List = [];
      this.ProductionOrderProductionPlanService.BaseParameter.ParentID = this.ProductionOrderProductionPlanSemiService.BaseParameter.ParentID;
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
  ProductionOrderProductionPlanSave(element: ProductionOrderProductionPlan) {
    this.ProductionOrderService.IsShowLoading = true;
    element.ParentID = this.ProductionOrderProductionPlanSemiService.BaseParameter.ParentID;
    this.ProductionOrderProductionPlanService.BaseParameter.BaseModel = element;
    this.ProductionOrderProductionPlanService.SaveAsync().subscribe(
      res => {
        this.ProductionOrderProductionPlanSearch();
        this.ProductionOrderProductionPlanSemiSearch();
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
  ProductionOrderProductionPlanSaveAndUploadFile() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderProductionPlanService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.ProductionOrderProductionPlanService.FileToUpload = null;
        this.ProductionOrderProductionPlanService.BaseParameter.Event = null;
        this.ProductionOrderProductionPlanSearch();
        this.ProductionOrderProductionPlanSemiSearch();
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
  ProductionOrderProductionPlanSaveCutAndUploadFile() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderProductionPlanService.SaveQuantityCutAndUploadFileAsync().subscribe(
      res => {
        this.ProductionOrderProductionPlanService.FileToUpload = null;
        this.ProductionOrderProductionPlanService.BaseParameter.Event = null;
        this.ProductionOrderProductionPlanSearch();
        this.ProductionOrderProductionPlanSemiSearch();
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
  ProductionOrderProductionPlanSemiSync() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderProductionPlanSemiService.SyncByQuantityActualAsync().subscribe(
      res => {
        this.ProductionOrderProductionPlanSemiSearch();
      },
      err => {
      },
      () => {
        this.ProductionOrderService.IsShowLoading = false;
      }
    );
  }
  ProductionOrderProductionPlanSemiActiveAllChange() {
    for (let i = 0; i < this.ProductionOrderProductionPlanSemiService.ListFilter.length; i++) {
      this.ProductionOrderProductionPlanSemiService.ListFilter[i].Active = this.ProductionOrderProductionPlanSemiService.BaseParameter.Active001;
    }
  }
  ProductionOrderProductionPlanSemiSearch() {
    if (this.ProductionOrderProductionPlanSemiService.BaseParameter.SearchString && this.ProductionOrderProductionPlanSemiService.BaseParameter.SearchString.length > 0) {
      this.ProductionOrderProductionPlanSemiService.BaseParameter.SearchString = this.ProductionOrderProductionPlanSemiService.BaseParameter.SearchString.trim();
      let List = this.ProductionOrderProductionPlanSemiService.List.filter(o => o.SortOrder == 1);
      this.ProductionOrderProductionPlanSemiService.ListFilter = this.ProductionOrderProductionPlanSemiService.List.filter(o => o.SortOrder > 1 && (o.MaterialCode == this.ProductionOrderProductionPlanSemiService.BaseParameter.SearchString || o.MaterialCode01 == this.ProductionOrderProductionPlanSemiService.BaseParameter.SearchString));
      if (List && List.length > 0) {
        this.ProductionOrderProductionPlanSemiService.ListFilter.push(List[0]);
      }
      this.ProductionOrderProductionPlanSemiService.ListFilter = this.ProductionOrderProductionPlanSemiService.ListFilter.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      this.ProductionOrderProductionPlanSemiService.DataSource = new MatTableDataSource(this.ProductionOrderProductionPlanSemiService.ListFilter);
      this.ProductionOrderProductionPlanSemiService.DataSource.sort = this.ProductionOrderProductionPlanSemiSort;
      this.ProductionOrderProductionPlanSemiService.DataSource.paginator = this.ProductionOrderProductionPlanSemiPaginator;
    }
    else {
      this.ProductionOrderService.IsShowLoading = true;
      this.ProductionOrderProductionPlanSemiService.List = [];
      this.ProductionOrderProductionPlanSemiService.BaseParameter.ParentID = this.ProductionOrderProductionPlanSemiService.BaseParameter.ParentID;
      this.ProductionOrderProductionPlanSemiService.GetByParentIDToListAsync().subscribe(
        res => {
          this.ProductionOrderProductionPlanSemiService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.ProductionOrderProductionPlanSemiService.ListFilter = this.ProductionOrderProductionPlanSemiService.List;
          this.ProductionOrderProductionPlanSemiService.DataSource = new MatTableDataSource(this.ProductionOrderProductionPlanSemiService.ListFilter);
          this.ProductionOrderProductionPlanSemiService.DataSource.sort = this.ProductionOrderProductionPlanSemiSort;
          this.ProductionOrderProductionPlanSemiService.DataSource.paginator = this.ProductionOrderProductionPlanSemiPaginator;
        },
        err => {
        },
        () => {
          this.ProductionOrderService.IsShowLoading = false;
        }
      );
    }
  }
  ProductionOrderProductionPlanSemiSave(element: ProductionOrderProductionPlanSemi) {
    this.ProductionOrderService.IsShowLoading = true;
    element.ParentID = this.ProductionOrderProductionPlanSemiService.BaseParameter.ParentID;
    this.ProductionOrderProductionPlanSemiService.BaseParameter.BaseModel = element;
    this.ProductionOrderProductionPlanSemiService.SaveAsync().subscribe(
      res => {
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
  ProductionOrderCuttingOrderSearch() {


    if (this.ProductionOrderCuttingOrderService.BaseParameter.SearchString && this.ProductionOrderCuttingOrderService.BaseParameter.SearchString.length > 0) {
      this.ProductionOrderCuttingOrderService.BaseParameter.SearchString = this.ProductionOrderCuttingOrderService.BaseParameter.SearchString.trim();
      if (this.ProductionOrderCuttingOrderService.DataSource) {
        this.ProductionOrderCuttingOrderService.DataSource.filter = this.ProductionOrderCuttingOrderService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.ProductionOrderCuttingOrderService.List = [];
      this.ProductionOrderCuttingOrderService.BaseParameter.ParentID = this.ProductionOrderProductionPlanSemiService.BaseParameter.ParentID;
      this.ProductionOrderService.IsShowLoading = true;
      this.ProductionOrderCuttingOrderService.GetByParentID_DateToListAsync().subscribe(
        res => {
          this.ProductionOrderCuttingOrderService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.ProductionOrderCuttingOrderService.DataSource = new MatTableDataSource(this.ProductionOrderCuttingOrderService.List);
          this.ProductionOrderCuttingOrderService.DataSource.sort = this.ProductionOrderCuttingOrderSort;
          this.ProductionOrderCuttingOrderService.DataSource.paginator = this.ProductionOrderCuttingOrderPaginator;
        },
        err => {
        },
        () => {
          this.ProductionOrderService.IsShowLoading = false;
        }
      );
    }

  }
  ProductionOrderCuttingOrderSave(element: ProductionOrderCuttingOrder) {
    this.ProductionOrderService.IsShowLoading = true;
    element.ParentID = this.ProductionOrderProductionPlanSemiService.BaseParameter.ParentID;
    this.ProductionOrderCuttingOrderService.BaseParameter.BaseModel = element;
    this.ProductionOrderCuttingOrderService.SaveAsync().subscribe(
      res => {
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
  ProductionOrderCuttingOrderSync() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderCuttingOrderService.SyncByParentID_DateToListAsync().subscribe(
      res => {
        this.ProductionOrderCuttingOrderSearch();
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
  ProductionOrderCuttingOrderDownload() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderCuttingOrderService.ExportToExcelAsync().subscribe(
      res => {
        let url = (res as BaseResult).Message;
        window.open(url, "_blank");
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.ProductionOrderService.IsShowLoading = false;
      }
    );
  }
  ProductionOrderCuttingOrderSyncMES() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderCuttingOrderService.SyncMESByParentID_DateToListAsync().subscribe(
      res => {
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
  ProductionOrderSPSTOrderSearch() {


    if (this.ProductionOrderSPSTOrderService.BaseParameter.SearchString && this.ProductionOrderSPSTOrderService.BaseParameter.SearchString.length > 0) {
      this.ProductionOrderSPSTOrderService.BaseParameter.SearchString = this.ProductionOrderSPSTOrderService.BaseParameter.SearchString.trim();
      if (this.ProductionOrderSPSTOrderService.DataSource) {
        this.ProductionOrderSPSTOrderService.DataSource.filter = this.ProductionOrderSPSTOrderService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.ProductionOrderSPSTOrderService.List = [];
      this.ProductionOrderSPSTOrderService.BaseParameter.ParentID = this.ProductionOrderProductionPlanSemiService.BaseParameter.ParentID;
      this.ProductionOrderService.IsShowLoading = true;
      this.ProductionOrderSPSTOrderService.GetByParentID_DateToListAsync().subscribe(
        res => {
          this.ProductionOrderSPSTOrderService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.ProductionOrderSPSTOrderService.DataSource = new MatTableDataSource(this.ProductionOrderSPSTOrderService.List);
          this.ProductionOrderSPSTOrderService.DataSource.sort = this.ProductionOrderSPSTOrderSort;
          this.ProductionOrderSPSTOrderService.DataSource.paginator = this.ProductionOrderSPSTOrderPaginator;
        },
        err => {
        },
        () => {
          this.ProductionOrderService.IsShowLoading = false;
        }
      );
    }

  }
  ProductionOrderSPSTOrderSave(element: ProductionOrderSPSTOrder) {
    this.ProductionOrderService.IsShowLoading = true;
    element.ParentID = this.ProductionOrderProductionPlanSemiService.BaseParameter.ParentID;
    this.ProductionOrderSPSTOrderService.BaseParameter.BaseModel = element;
    this.ProductionOrderSPSTOrderService.SaveAsync().subscribe(
      res => {
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
  ProductionOrderSPSTOrderSync() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderSPSTOrderService.SyncByParentID_DateToListAsync().subscribe(
      res => {
        this.ProductionOrderSPSTOrderSearch();
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
  ProductionOrderSPSTOrderDownload() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderSPSTOrderService.ExportToExcelAsync().subscribe(
      res => {
        let url = (res as BaseResult).Message;
        window.open(url, "_blank");
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.ProductionOrderService.IsShowLoading = false;
      }
    );

  }
  ProductionOrderSPSTOrderSyncMES() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderSPSTOrderService.SyncMESByParentID_DateToListAsync().subscribe(
      res => {
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
  ProductionOrderProductionPlanExcel() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderProductionPlanService.ExportByParentIDToExcelAsync().subscribe(
      res => {
        this.ProductionOrderProductionPlanService.BaseResult = (res as BaseResult);
        window.open(this.ProductionOrderProductionPlanService.BaseResult.Message, "_blank");
      },
      err => {
      },
      () => {
        this.ProductionOrderService.IsShowLoading = false;
      }
    );
  }
}
