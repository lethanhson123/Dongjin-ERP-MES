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

import { ProductionOrderFile } from 'src/app/shared/ERP/ProductionOrderFile.model';
import { ProductionOrderFileService } from 'src/app/shared/ERP/ProductionOrderFile.service';

import { ProductionOrderDetail } from 'src/app/shared/ERP/ProductionOrderDetail.model';
import { ProductionOrderDetailService } from 'src/app/shared/ERP/ProductionOrderDetail.service';

import { ProductionOrderProductionPlan } from 'src/app/shared/ERP/ProductionOrderProductionPlan.model';
import { ProductionOrderProductionPlanService } from 'src/app/shared/ERP/ProductionOrderProductionPlan.service';

import { ProductionOrderProductionPlanMaterial } from 'src/app/shared/ERP/ProductionOrderProductionPlanMaterial.model';
import { ProductionOrderProductionPlanMaterialService } from 'src/app/shared/ERP/ProductionOrderProductionPlanMaterial.service';

import { ProductionOrderMaterial } from 'src/app/shared/ERP/ProductionOrderMaterial.model';
import { ProductionOrderMaterialService } from 'src/app/shared/ERP/ProductionOrderMaterial.service';

import { ProductionOrderBOM } from 'src/app/shared/ERP/ProductionOrderBOM.model';
import { ProductionOrderBOMService } from 'src/app/shared/ERP/ProductionOrderBOM.service';

import { ProductionOrderBOMDetail } from 'src/app/shared/ERP/ProductionOrderBOMDetail.model';
import { ProductionOrderBOMDetailService } from 'src/app/shared/ERP/ProductionOrderBOMDetail.service';

import { ProductionOrderOutputSchedule } from 'src/app/shared/ERP/ProductionOrderOutputSchedule.model';
import { ProductionOrderOutputScheduleService } from 'src/app/shared/ERP/ProductionOrderOutputSchedule.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';


import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

import { WarehouseRequest } from 'src/app/shared/ERP/WarehouseRequest.model';
import { WarehouseRequestService } from 'src/app/shared/ERP/WarehouseRequest.service';

import { WarehouseInput } from 'src/app/shared/ERP/WarehouseInput.model';
import { WarehouseInputService } from 'src/app/shared/ERP/WarehouseInput.service';

import { WarehouseOutput } from 'src/app/shared/ERP/WarehouseOutput.model';
import { WarehouseOutputService } from 'src/app/shared/ERP/WarehouseOutput.service';

@Component({
  selector: 'app-production-order-info-backup',
  templateUrl: './production-order-info-backup.component.html',
  styleUrls: ['./production-order-info-backup.component.css']
})
export class ProductionOrderInfoBackupComponent {

@ViewChild('ProductionOrderDetailSort') ProductionOrderDetailSort: MatSort;
  @ViewChild('ProductionOrderDetailPaginator') ProductionOrderDetailPaginator: MatPaginator;

  @ViewChild('ProductionOrderBOMSort') ProductionOrderBOMSort: MatSort;
  @ViewChild('ProductionOrderBOMPaginator') ProductionOrderBOMPaginator: MatPaginator;

  @ViewChild('ProductionOrderBOMDetailSort') ProductionOrderBOMDetailSort: MatSort;
  @ViewChild('ProductionOrderBOMDetailPaginator') ProductionOrderBOMDetailPaginator: MatPaginator;

  @ViewChild('ProductionOrderMaterialSort') ProductionOrderMaterialSort: MatSort;
  @ViewChild('ProductionOrderMaterialPaginator') ProductionOrderMaterialPaginator: MatPaginator;

  @ViewChild('ProductionOrderProductionPlanSort') ProductionOrderProductionPlanSort: MatSort;
  @ViewChild('ProductionOrderProductionPlanPaginator') ProductionOrderProductionPlanPaginator: MatPaginator;

  @ViewChild('ProductionOrderProductionPlanMaterialSort') ProductionOrderProductionPlanMaterialSort: MatSort;
  @ViewChild('ProductionOrderProductionPlanMaterialPaginator') ProductionOrderProductionPlanMaterialPaginator: MatPaginator;

  @ViewChild('ProductionOrderFileSort') ProductionOrderFileSort: MatSort;
  @ViewChild('ProductionOrderFilePaginator') ProductionOrderFilePaginator: MatPaginator;

  @ViewChild('ProductionOrderOutputScheduleSort') ProductionOrderOutputScheduleSort: MatSort;
  @ViewChild('ProductionOrderOutputSchedulePaginator') ProductionOrderOutputSchedulePaginator: MatPaginator;

  @ViewChild('WarehouseRequestSort') WarehouseRequestSort: MatSort;
  @ViewChild('WarehouseRequestPaginator') WarehouseRequestPaginator: MatPaginator;

  @ViewChild('WarehouseInputSort') WarehouseInputSort: MatSort;
  @ViewChild('WarehouseInputPaginator') WarehouseInputPaginator: MatPaginator;

  @ViewChild('WarehouseOutputSort') WarehouseOutputSort: MatSort;
  @ViewChild('WarehouseOutputPaginator') WarehouseOutputPaginator: MatPaginator;

  URLTemplate: string = environment.APIRootURL + "Download/ProductionOrder.xlsx";

  URLTemplateProductionOrderProductionPlan: string = environment.APIRootURL + "Download/ProductionOrderProductionPlan.xlsx";
  URLTemplateProductionOrderProductionPlanMaterial: string = environment.APIRootURL + "Download/ProductionOrderProductionPlanMaterial.xlsx";

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ProductionOrderService: ProductionOrderService,
    public ProductionOrderFileService: ProductionOrderFileService,
    public ProductionOrderDetailService: ProductionOrderDetailService,
    public ProductionOrderProductionPlanService: ProductionOrderProductionPlanService,
    public ProductionOrderProductionPlanMaterialService: ProductionOrderProductionPlanMaterialService,
    public ProductionOrderMaterialService: ProductionOrderMaterialService,
    public ProductionOrderBOMService: ProductionOrderBOMService,
    public ProductionOrderBOMDetailService: ProductionOrderBOMDetailService,
    public ProductionOrderOutputScheduleService: ProductionOrderOutputScheduleService,

    public WarehouseRequestService: WarehouseRequestService,
    public WarehouseInputService: WarehouseInputService,
    public WarehouseOutputService: WarehouseOutputService,

    public MaterialService: MaterialService,
    public CompanyService: CompanyService,
  ) {
    this.CompanySearch();

    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this.ProductionOrderService.BaseParameter.ID = Number(this.ActiveRouter.snapshot.params.ID);
        this.ProductionOrderSearch();
      }
    });
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  ProductionOrderProductionPlanChange(event, files: FileList) {
    if (files) {
      this.ProductionOrderProductionPlanService.FileToUpload = files;
      this.ProductionOrderProductionPlanService.BaseParameter.Event = event;
    }
  }
  ProductionOrderProductionPlanMaterialChange(event, files: FileList) {
    if (files) {
      this.ProductionOrderProductionPlanMaterialService.FileToUpload = files;
      this.ProductionOrderProductionPlanMaterialService.BaseParameter.Event = event;
    }
  }
  ProductionOrderMaterialChange(event, files: FileList) {
    if (files) {
      this.ProductionOrderMaterialService.FileToUpload = files;
      this.ProductionOrderMaterialService.BaseParameter.Event = event;
    }
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.ProductionOrderService);
  }
  Date(value) {
    this.ProductionOrderService.BaseParameter.BaseModel.Date = new Date(value);
  }
  DateEnd(value) {
    this.ProductionOrderService.BaseParameter.BaseModel.DateEnd = new Date(value);
  }
  // ProductionOrderProductionPlanDate01(value,element: ProductionOrderProductionPlan) {
  //   element.Date01 = new Date(value);
  // }
  ProductionOrderChange(event, files: FileList) {
    if (files) {
      this.ProductionOrderService.FileToUpload = files;
      this.ProductionOrderService.BaseParameter.Event = event;
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
  ProductionOrderSearch() {
    //this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderService.GetByIDAsync().subscribe(
      res => {
        this.ProductionOrderService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;

        this.ProductionOrderService.FileToUpload = null;
        this.ProductionOrderProductionPlanService.FileToUpload = null;
        this.ProductionOrderProductionPlanMaterialService.FileToUpload = null;
        this.ProductionOrderMaterialService.FileToUpload = null;

        this.ProductionOrderService.BaseParameter.SearchString = environment.InitializationString;
        this.ProductionOrderFileService.BaseParameter.SearchString = environment.InitializationString;
        this.ProductionOrderDetailService.BaseParameter.SearchString = environment.InitializationString;
        this.ProductionOrderProductionPlanService.BaseParameter.SearchString = environment.InitializationString;
        this.ProductionOrderBOMService.BaseParameter.SearchString = environment.InitializationString;
        this.ProductionOrderBOMDetailService.BaseParameter.SearchString = environment.InitializationString;
        this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString = environment.InitializationString;
        this.ProductionOrderMaterialService.BaseParameter.SearchString = environment.InitializationString;
        this.ProductionOrderOutputScheduleService.BaseParameter.SearchString = environment.InitializationString;

        this.ProductionOrderFileSearch();
        this.ProductionOrderDetailSearch();
        this.ProductionOrderProductionPlanSearch();
        this.ProductionOrderBOMSearch();
        this.ProductionOrderBOMDetailSearch();
        this.ProductionOrderProductionPlanMaterialSearch();
        this.ProductionOrderMaterialSearch();
        this.ProductionOrderOutputScheduleSearch();
        this.WarehouseRequestSearch();
        this.WarehouseInputSearch();
        this.WarehouseOutputSearch();
      },
      err => {
      },
      () => {
      }
    );
  }
  Add(ID: number) {
    this.Router.navigateByUrl("ProductionOrderInfo/" + ID);
    this.ProductionOrderService.FileToUpload = null;
    this.ProductionOrderService.BaseParameter.ID = ID;
    this.ProductionOrderSearch();
  }
  Save() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderService.BaseParameter.ID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
    this.ProductionOrderService.SaveAndUploadFilesAsync().subscribe(
      res => {
        this.ProductionOrderService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.ProductionOrderService.FileToUpload = null;
        this.ProductionOrderService.BaseParameter.Event = null;
        this.Add(this.ProductionOrderService.BaseParameter.BaseModel.ID);
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
  ProductionOrderFileSearch() {
    this.ProductionOrderFileService.List = [];
    if (this.ProductionOrderService.BaseParameter.BaseModel.ID > 0) {
      if (this.ProductionOrderFileService.BaseParameter.SearchString && this.ProductionOrderFileService.BaseParameter.SearchString.length > 0) {
        this.ProductionOrderFileService.BaseParameter.SearchString = this.ProductionOrderFileService.BaseParameter.SearchString.trim();
        if (this.ProductionOrderFileService.DataSource) {
          this.ProductionOrderFileService.DataSource.filter = this.ProductionOrderFileService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.ProductionOrderFileService.BaseParameter.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
        this.ProductionOrderService.IsShowLoading = true;
        this.ProductionOrderFileService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.ProductionOrderFileService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.ProductionOrderFileService.DataSource = new MatTableDataSource(this.ProductionOrderFileService.List);
            this.ProductionOrderFileService.DataSource.sort = this.ProductionOrderFileSort;
            this.ProductionOrderFileService.DataSource.paginator = this.ProductionOrderFilePaginator;
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
      this.ProductionOrderFileService.List = [];
      this.ProductionOrderFileService.DataSource = new MatTableDataSource(this.ProductionOrderFileService.List);
      this.ProductionOrderFileService.DataSource.sort = this.ProductionOrderFileSort;
      this.ProductionOrderFileService.DataSource.paginator = this.ProductionOrderFilePaginator;
    }
  }
  ProductionOrderFileDelete(element: ProductionOrderFile) {
    this.ProductionOrderFileService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.ProductionOrderService.IsShowLoading = true;
      this.ProductionOrderFileService.RemoveAsync().subscribe(
        res => {
          this.ProductionOrderFileSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.ProductionOrderService.IsShowLoading = false;
        }
      );
    }
  }
  ProductionOrderFileSave(element: ProductionOrderFile) {
    this.ProductionOrderService.IsShowLoading = true;
    element.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
    this.ProductionOrderFileService.BaseParameter.BaseModel = element;
    this.ProductionOrderFileService.SaveAndUploadFilesAsync().subscribe(
      res => {
        this.ProductionOrderFileSearch();
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
  ProductionOrderDetailSearch() {
    this.ProductionOrderDetailService.List = [];
    if (this.ProductionOrderService.BaseParameter.BaseModel.ID > 0) {
      if (this.ProductionOrderDetailService.BaseParameter.SearchString && this.ProductionOrderDetailService.BaseParameter.SearchString.length > 0) {
        this.ProductionOrderDetailService.BaseParameter.SearchString = this.ProductionOrderDetailService.BaseParameter.SearchString.trim();
        if (this.ProductionOrderDetailService.DataSource) {
          this.ProductionOrderDetailService.DataSource.filter = this.ProductionOrderDetailService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.ProductionOrderDetailService.BaseParameter.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
        this.ProductionOrderService.IsShowLoading = true;
        this.ProductionOrderDetailService.GetByParentIDToListAsync().subscribe(
          res => {
            this.ProductionOrderDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.ProductionOrderDetailService.DataSource = new MatTableDataSource(this.ProductionOrderDetailService.List);
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
    this.ProductionOrderProductionPlanService.List = [];
    if (this.ProductionOrderService.BaseParameter.BaseModel.ID > 0) {
      if (this.ProductionOrderProductionPlanService.BaseParameter.SearchString && this.ProductionOrderProductionPlanService.BaseParameter.SearchString.length > 0) {
        this.ProductionOrderProductionPlanService.BaseParameter.SearchString = this.ProductionOrderProductionPlanService.BaseParameter.SearchString.trim();
        if (this.ProductionOrderProductionPlanService.DataSource) {
          this.ProductionOrderProductionPlanService.DataSource.filter = this.ProductionOrderProductionPlanService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.ProductionOrderProductionPlanService.BaseParameter.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
        this.ProductionOrderService.IsShowLoading = true;
        this.ProductionOrderProductionPlanService.GetByParentIDToListAsync().subscribe(
          res => {
            this.ProductionOrderProductionPlanService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.ProductionOrderProductionPlanService.DataSource = new MatTableDataSource(this.ProductionOrderProductionPlanService.List);
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
    element.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
    this.ProductionOrderProductionPlanService.BaseParameter.BaseModel = element;
    this.ProductionOrderProductionPlanService.SaveAsync().subscribe(
      res => {
        this.ProductionOrderProductionPlanSearch();
        this.ProductionOrderProductionPlanMaterialSearch();
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
        this.ProductionOrderProductionPlanMaterialSearch();
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

  ProductionOrderBOMSearch() {
    this.ProductionOrderBOMService.List = [];
    if (this.ProductionOrderService.BaseParameter.BaseModel.ID > 0) {
      if (this.ProductionOrderBOMService.BaseParameter.SearchString && this.ProductionOrderBOMService.BaseParameter.SearchString.length > 0) {
        this.ProductionOrderBOMService.BaseParameter.SearchString = this.ProductionOrderBOMService.BaseParameter.SearchString.trim();
        if (this.ProductionOrderBOMService.DataSource) {
          this.ProductionOrderBOMService.DataSource.filter = this.ProductionOrderBOMService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.ProductionOrderBOMService.BaseParameter.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
        this.ProductionOrderService.IsShowLoading = true;
        this.ProductionOrderBOMService.GetByParentIDToListAsync().subscribe(
          res => {
            this.ProductionOrderBOMService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.ProductionOrderBOMService.DataSource = new MatTableDataSource(this.ProductionOrderBOMService.List);
            this.ProductionOrderBOMService.DataSource.sort = this.ProductionOrderBOMSort;
            this.ProductionOrderBOMService.DataSource.paginator = this.ProductionOrderBOMPaginator;
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
      this.ProductionOrderBOMService.List = [];
      this.ProductionOrderBOMService.DataSource = new MatTableDataSource(this.ProductionOrderBOMService.List);
      this.ProductionOrderBOMService.DataSource.sort = this.ProductionOrderBOMSort;
      this.ProductionOrderBOMService.DataSource.paginator = this.ProductionOrderBOMPaginator;
    }
  }
  ProductionOrderBOMDetailSearch() {
    this.ProductionOrderBOMDetailService.List = [];
    if (this.ProductionOrderService.BaseParameter.BaseModel.ID > 0) {
      if (this.ProductionOrderBOMDetailService.BaseParameter.SearchString && this.ProductionOrderBOMDetailService.BaseParameter.SearchString.length > 0) {
        this.ProductionOrderBOMDetailService.BaseParameter.SearchString = this.ProductionOrderBOMDetailService.BaseParameter.SearchString.trim();
        if (this.ProductionOrderBOMDetailService.DataSource) {
          this.ProductionOrderBOMDetailService.DataSource.filter = this.ProductionOrderBOMDetailService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.ProductionOrderBOMDetailService.BaseParameter.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
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
    else {
      this.ProductionOrderBOMDetailService.List = [];
      this.ProductionOrderBOMDetailService.DataSource = new MatTableDataSource(this.ProductionOrderBOMDetailService.List);
      this.ProductionOrderBOMDetailService.DataSource.sort = this.ProductionOrderBOMDetailSort;
      this.ProductionOrderBOMDetailService.DataSource.paginator = this.ProductionOrderBOMDetailPaginator;
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
    this.ProductionOrderProductionPlanMaterialService.List = [];
    if (this.ProductionOrderService.BaseParameter.BaseModel.ID > 0) {
      if (this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString && this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString.length > 0) {
        this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString = this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString.trim();
        // if (this.ProductionOrderProductionPlanMaterialService.DataSource) {
        //   this.ProductionOrderProductionPlanMaterialService.DataSource.filter = this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString.toLowerCase();
        // }
        if (this.ProductionOrderProductionPlanMaterialService.List) {
          if (this.ProductionOrderProductionPlanMaterialService.List.length > 0) {
            let List0 = this.ProductionOrderProductionPlanMaterialService.List.filter(o => o.MaterialID == environment.InitializationNumber);
            let List = this.ProductionOrderProductionPlanMaterialService.List.filter(o => (o.MaterialID > 0 && o.MaterialCode && o.MaterialCode.length > 0 && o.MaterialCode.includes(this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString))
              || (o.MaterialID01 > 0 && o.MaterialCode01 && o.MaterialCode01.length > 0 && o.MaterialCode01.includes(this.ProductionOrderProductionPlanMaterialService.BaseParameter.SearchString))
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
        this.ProductionOrderProductionPlanMaterialService.BaseParameter.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
        this.ProductionOrderService.IsShowLoading = true;
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
    else {
      this.ProductionOrderProductionPlanMaterialService.ListFilter = [];
      this.ProductionOrderProductionPlanMaterialService.DataSource = new MatTableDataSource(this.ProductionOrderProductionPlanMaterialService.ListFilter);
      this.ProductionOrderProductionPlanMaterialService.DataSource.sort = this.ProductionOrderProductionPlanMaterialSort;
      this.ProductionOrderProductionPlanMaterialService.DataSource.paginator = this.ProductionOrderProductionPlanMaterialPaginator;
    }
  }
  ProductionOrderProductionPlanMaterialSaveList() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderProductionPlanMaterialService.BaseParameter.List = this.ProductionOrderProductionPlanMaterialService.List;
    this.ProductionOrderProductionPlanMaterialService.SaveListAsync().subscribe(
      res => {
        this.ProductionOrderProductionPlanMaterialSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        //this.ProductionOrderService.IsShowLoading = false;
      }
    );
  }
  ProductionOrderProductionPlanMaterialSaveAndUploadFile() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderProductionPlanMaterialService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.ProductionOrderProductionPlanMaterialService.FileToUpload = null;
        this.ProductionOrderProductionPlanMaterialService.BaseParameter.Event = null;
        this.ProductionOrderProductionPlanMaterialSearch();
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
    this.ProductionOrderMaterialService.List = [];
    if (this.ProductionOrderService.BaseParameter.BaseModel.ID > 0) {
      if (this.ProductionOrderMaterialService.BaseParameter.SearchString && this.ProductionOrderMaterialService.BaseParameter.SearchString.length > 0) {
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
        this.ProductionOrderMaterialService.BaseParameter.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
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
    else {
      this.ProductionOrderMaterialService.ListFilter = [];
      this.ProductionOrderMaterialService.DataSource = new MatTableDataSource(this.ProductionOrderMaterialService.ListFilter);
      this.ProductionOrderMaterialService.DataSource.sort = this.ProductionOrderMaterialSort;
      this.ProductionOrderMaterialService.DataSource.paginator = this.ProductionOrderMaterialPaginator;
    }
  }
  ProductionOrderMaterialSaveList() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderMaterialService.BaseParameter.List = this.ProductionOrderMaterialService.List;
    this.ProductionOrderMaterialService.SaveListAsync().subscribe(
      res => {
        this.ProductionOrderMaterialSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        //this.ProductionOrderService.IsShowLoading = false;
      }
    );
  }
  ProductionOrderMaterialSaveAndUploadFile() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderMaterialService.SaveAndUploadFileAsync().subscribe(
      res => {
         this.ProductionOrderMaterialService.FileToUpload = null;
        this.ProductionOrderMaterialService.BaseParameter.Event = null;
        this.ProductionOrderMaterialSearch();
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
    this.ProductionOrderOutputScheduleService.List = [];
    if (this.ProductionOrderService.BaseParameter.BaseModel.ID > 0) {
      if (this.ProductionOrderOutputScheduleService.BaseParameter.SearchString && this.ProductionOrderOutputScheduleService.BaseParameter.SearchString.length > 0) {
        this.ProductionOrderOutputScheduleService.BaseParameter.SearchString = this.ProductionOrderOutputScheduleService.BaseParameter.SearchString.trim();
        if (this.ProductionOrderOutputScheduleService.DataSource) {
          this.ProductionOrderOutputScheduleService.DataSource.filter = this.ProductionOrderOutputScheduleService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.ProductionOrderOutputScheduleService.BaseParameter.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
        this.ProductionOrderService.IsShowLoading = true;
        this.ProductionOrderOutputScheduleService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.ProductionOrderOutputScheduleService.List = (res as BaseResult).List.sort((a, b) => (a.CreateDate > b.CreateDate ? 1 : -1));
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

  WarehouseOutputSearch() {
    this.WarehouseOutputService.List = [];
    if (this.ProductionOrderService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseOutputService.BaseParameter.SearchString && this.WarehouseOutputService.BaseParameter.SearchString.length > 0) {
        this.WarehouseOutputService.BaseParameter.SearchString = this.WarehouseOutputService.BaseParameter.SearchString.trim();
        if (this.WarehouseOutputService.DataSource) {
          this.WarehouseOutputService.DataSource.filter = this.WarehouseOutputService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseOutputService.IsShowLoading = true;
        this.WarehouseOutputService.BaseParameter.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
        this.WarehouseOutputService.GetByParentIDToListAsync().subscribe(
          res => {
            this.WarehouseOutputService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder < b.SortOrder ? 1 : -1));
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
    }
  }
  WarehouseInputSearch() {
    this.WarehouseInputService.List = [];
    if (this.ProductionOrderService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseInputService.BaseParameter.SearchString && this.WarehouseInputService.BaseParameter.SearchString.length > 0) {
        this.WarehouseInputService.BaseParameter.SearchString = this.WarehouseInputService.BaseParameter.SearchString.trim();
        if (this.WarehouseInputService.DataSource) {
          this.WarehouseInputService.DataSource.filter = this.WarehouseInputService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseInputService.IsShowLoading = true;
        this.WarehouseInputService.BaseParameter.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
        this.WarehouseInputService.GetByParentIDToListAsync().subscribe(
          res => {
            this.WarehouseInputService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder < b.SortOrder ? 1 : -1));
            this.WarehouseInputService.DataSource = new MatTableDataSource(this.WarehouseInputService.List);
            this.WarehouseInputService.DataSource.sort = this.WarehouseInputSort;
            this.WarehouseInputService.DataSource.paginator = this.WarehouseInputPaginator;
          },
          err => {
          },
          () => {
            this.WarehouseInputService.IsShowLoading = false;
          }
        );
      }
    }
  }
  WarehouseRequestSearch() {
    this.WarehouseRequestService.List = [];
    if (this.ProductionOrderService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseRequestService.BaseParameter.SearchString && this.WarehouseRequestService.BaseParameter.SearchString.length > 0) {
        this.WarehouseRequestService.BaseParameter.SearchString = this.WarehouseRequestService.BaseParameter.SearchString.trim();
        if (this.WarehouseRequestService.DataSource) {
          this.WarehouseRequestService.DataSource.filter = this.WarehouseRequestService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseRequestService.IsShowLoading = true;
        this.WarehouseRequestService.BaseParameter.ParentID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
        this.WarehouseRequestService.GetByParentIDToListAsync().subscribe(
          res => {
            this.WarehouseRequestService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder < b.SortOrder ? 1 : -1));
            this.WarehouseRequestService.DataSource = new MatTableDataSource(this.WarehouseRequestService.List);
            this.WarehouseRequestService.DataSource.sort = this.WarehouseRequestSort;
            this.WarehouseRequestService.DataSource.paginator = this.WarehouseRequestPaginator;
          },
          err => {
          },
          () => {
            this.WarehouseRequestService.IsShowLoading = false;
          }
        );
      }
    }
  }
}

