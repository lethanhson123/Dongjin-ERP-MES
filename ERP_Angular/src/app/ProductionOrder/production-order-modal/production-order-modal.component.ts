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

import { ProductionOrderBOM } from 'src/app/shared/ERP/ProductionOrderBOM.model';
import { ProductionOrderBOMService } from 'src/app/shared/ERP/ProductionOrderBOM.service';

import { ProductionOrderBOMDetail } from 'src/app/shared/ERP/ProductionOrderBOMDetail.model';
import { ProductionOrderBOMDetailService } from 'src/app/shared/ERP/ProductionOrderBOMDetail.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

@Component({
  selector: 'app-production-order-modal',
  templateUrl: './production-order-modal.component.html',
  styleUrls: ['./production-order-modal.component.css']
})
export class ProductionOrderModalComponent {

  URLTemplate: string = environment.APIRootURL + "Download/ProductionOrder2026.xlsx";

  constructor(

    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<ProductionOrderModalComponent>,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ProductionOrderService: ProductionOrderService,
    public ProductionOrderFileService: ProductionOrderFileService,
    public ProductionOrderDetailService: ProductionOrderDetailService,
    public ProductionOrderProductionPlanService: ProductionOrderProductionPlanService,
    public ProductionOrderProductionPlanMaterialService: ProductionOrderProductionPlanMaterialService,
    public ProductionOrderBOMService: ProductionOrderBOMService,
    public ProductionOrderBOMDetailService: ProductionOrderBOMDetailService,


    public MaterialService: MaterialService,
    public CompanyService: CompanyService,
  ) {
    this.CompanySearch();
  }
  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  Close() {
    this.DialogRef.close();
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
  ProductionOrderChange(event, files: FileList) {
    if (files) {
      this.ProductionOrderService.FileToUpload = files;
      this.ProductionOrderService.BaseParameter.Event = event;
    }
  }
  Save() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderService.BaseParameter.ID = this.ProductionOrderService.BaseParameter.BaseModel.ID;
    this.ProductionOrderService.SaveAndUploadFilesAsync().subscribe(
      res => {
        this.ProductionOrderService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.ProductionOrderService.FileToUpload = null;
        this.ProductionOrderService.BaseParameter.Event = null;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.ProductionOrderService.IsShowLoading = false;
      }
    );
    this.StartTimer();
  }
  StartTimer() {
    let Timer1 = setInterval(() => {
      this.ProductionOrderService.IsShowLoading = false;
      this.NotificationService.warn(environment.SaveSuccess);
      clearInterval(Timer1);
    }, 20000)
  }
}
