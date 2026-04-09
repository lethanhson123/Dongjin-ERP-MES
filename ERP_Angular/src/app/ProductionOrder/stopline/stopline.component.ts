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

import { ProductionOrderProductionPlanMaterial } from 'src/app/shared/ERP/ProductionOrderProductionPlanMaterial.model';
import { ProductionOrderProductionPlanMaterialService } from 'src/app/shared/ERP/ProductionOrderProductionPlanMaterial.service';

import { ProductionOrderMaterial } from 'src/app/shared/ERP/ProductionOrderMaterial.model';
import { ProductionOrderMaterialService } from 'src/app/shared/ERP/ProductionOrderMaterial.service';

import { ProductionOrderProductionPlan } from 'src/app/shared/ERP/ProductionOrderProductionPlan.model';
import { ProductionOrderProductionPlanService } from 'src/app/shared/ERP/ProductionOrderProductionPlan.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';
import { ProductionOrderModalComponent } from '../production-order-modal/production-order-modal.component';

@Component({
  selector: 'app-stopline',
  templateUrl: './stopline.component.html',
  styleUrls: ['./stopline.component.css']
})
export class STOPLINEComponent {

  @ViewChild('ProductionOrderMaterialSort') ProductionOrderMaterialSort: MatSort;
  @ViewChild('ProductionOrderMaterialPaginator') ProductionOrderMaterialPaginator: MatPaginator;

  @ViewChild('ProductionOrderProductionPlanMaterialSort') ProductionOrderProductionPlanMaterialSort: MatSort;
  @ViewChild('ProductionOrderProductionPlanMaterialPaginator') ProductionOrderProductionPlanMaterialPaginator: MatPaginator;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ProductionOrderService: ProductionOrderService,
    public ProductionOrderProductionPlanMaterialService: ProductionOrderProductionPlanMaterialService,
    public ProductionOrderMaterialService: ProductionOrderMaterialService,
    public ProductionOrderProductionPlanService: ProductionOrderProductionPlanService,

    public MaterialService: MaterialService,

  ) {
  }
  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.ProductionOrderSearch();
  }
  ProductionOrderChange() {
    this.ProductionOrderMaterialSearch();
    this.ProductionOrderProductionPlanMaterialSearch();
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
    // this.ProductionOrderService.IsShowLoading = true;
    // this.MaterialService.BaseParameter.ID = ID;
    // this.MaterialService.GetByIDAsync().subscribe(
    //   res => {
    //     this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
    //     this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
    //   },
    //   err => {
    //   },
    //   () => {
    //     this.ProductionOrderService.IsShowLoading = false;
    //   }
    // );
  }
  ProductionOrderModal() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderService.BaseParameter.ID = environment.InitializationNumber;
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
            this.ProductionOrderMaterialService.BaseParameter.ParentID = this.ProductionOrderService.ListFilter[0].ID;
            this.ProductionOrderProductionPlanMaterialService.BaseParameter.ParentID = this.ProductionOrderService.ListFilter[0].ID;
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
  ProductionOrderMaterialActiveChange() {
    if (this.ProductionOrderMaterialService.BaseParameter.Active == true) {
      this.ProductionOrderMaterialService.ListFilter = this.ProductionOrderMaterialService.List.filter(o => o.SortOrder == 1 || o.Priority == -10);
    }
    else {
      this.ProductionOrderMaterialService.ListFilter = this.ProductionOrderMaterialService.List.filter(o => o.SortOrder == 1 || o.Priority > -10);
    }
    this.ProductionOrderMaterialService.ListFilter = this.ProductionOrderMaterialService.ListFilter.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
    this.ProductionOrderMaterialService.DataSource = new MatTableDataSource(this.ProductionOrderMaterialService.ListFilter);
    this.ProductionOrderMaterialService.DataSource.sort = this.ProductionOrderMaterialSort;
    this.ProductionOrderMaterialService.DataSource.paginator = this.ProductionOrderMaterialPaginator;
  }
  ProductionOrderMaterialSearch() {
    if (this.ProductionOrderMaterialService.BaseParameter.SearchString.length > 0) {
      this.ProductionOrderMaterialService.BaseParameter.SearchString = this.ProductionOrderMaterialService.BaseParameter.SearchString.trim();
      if (this.ProductionOrderMaterialService.List) {
        if (this.ProductionOrderMaterialService.List.length > 0) {
          let List0 = this.ProductionOrderMaterialService.List.filter(o => o.MaterialID == environment.InitializationNumber);
          let List = this.ProductionOrderMaterialService.List.filter(o => (o.MaterialID > 0 && o.MaterialCode && o.MaterialCode.length > 0 && o.MaterialCode.includes(this.ProductionOrderMaterialService.BaseParameter.SearchString))
            || (o.MaterialID01 > 0 && o.MaterialCode01 && o.MaterialCode01.length > 0 && o.MaterialCode01.includes(this.ProductionOrderMaterialService.BaseParameter.SearchString))
          );
          if (List0) {
            if (List0.length > 0) {
              List.push(List0[0]);
            }
          }
          List = List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.ProductionOrderMaterialService.DataSource = new MatTableDataSource(List);
          this.ProductionOrderMaterialService.DataSource.sort = this.ProductionOrderMaterialSort;
          this.ProductionOrderMaterialService.DataSource.paginator = this.ProductionOrderMaterialPaginator;
        }
      }
    }
    else {
      this.ProductionOrderService.IsShowLoading = true;
      this.ProductionOrderMaterialService.GetByParentIDToListAsync().subscribe(
        res => {
          this.ProductionOrderMaterialService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.ProductionOrderMaterialActiveChange();
        },
        err => {
        },
        () => {
          this.ProductionOrderService.IsShowLoading = false;
        }
      );
    }
  }
  ProductionOrderProductionPlanMaterialActiveChange() {
    if (this.ProductionOrderProductionPlanMaterialService.BaseParameter.Active == true) {
      this.ProductionOrderProductionPlanMaterialService.ListFilter = this.ProductionOrderProductionPlanMaterialService.List.filter(o => o.SortOrder == 1 || o.Priority == -10);
    }
    else {
      this.ProductionOrderProductionPlanMaterialService.ListFilter = this.ProductionOrderProductionPlanMaterialService.List.filter(o => o.SortOrder == 1 || o.Priority > -10);
    }
    this.ProductionOrderProductionPlanMaterialService.ListFilter = this.ProductionOrderProductionPlanMaterialService.ListFilter.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
    this.ProductionOrderProductionPlanMaterialService.DataSource = new MatTableDataSource(this.ProductionOrderProductionPlanMaterialService.ListFilter);
    this.ProductionOrderProductionPlanMaterialService.DataSource.sort = this.ProductionOrderProductionPlanMaterialSort;
    this.ProductionOrderProductionPlanMaterialService.DataSource.paginator = this.ProductionOrderProductionPlanMaterialPaginator;
  }
  ProductionOrderProductionPlanMaterialSearch() {
    if (this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString.length > 0) {
      this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString = this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString.trim();
      this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString.toLocaleLowerCase();
      if (this.ProductionOrderProductionPlanMaterialService.List) {
        if (this.ProductionOrderProductionPlanMaterialService.List.length > 0) {
          let List0 = this.ProductionOrderProductionPlanMaterialService.List.filter(o => o.MaterialID == environment.InitializationNumber);
          let List = this.ProductionOrderProductionPlanMaterialService.List.filter(o => (o.MaterialCode && o.MaterialCode.length > 0 && o.MaterialCode.includes(this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString))
            || (o.MaterialCode01 && o.MaterialCode01.length > 0 && o.MaterialCode01.includes(this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString))
          );
          if (List0) {
            if (List0.length > 0) {
              List.push(List0[0]);
            }
          }
          List = List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.ProductionOrderProductionPlanMaterialService.DataSource = new MatTableDataSource(List);
          this.ProductionOrderProductionPlanMaterialService.DataSource.sort = this.ProductionOrderProductionPlanMaterialSort;
          this.ProductionOrderProductionPlanMaterialService.DataSource.paginator = this.ProductionOrderProductionPlanMaterialPaginator;
        }
      }

    }
    else {
      this.ProductionOrderService.IsShowLoading = true;
      this.ProductionOrderProductionPlanMaterialService.BaseParameter.ParentID = this.ProductionOrderMaterialService.BaseParameter.ParentID;
      this.ProductionOrderProductionPlanMaterialService.GetByParentIDToListAsync().subscribe(
        res => {
          this.ProductionOrderProductionPlanMaterialService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.ProductionOrderProductionPlanMaterialActiveChange();
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
