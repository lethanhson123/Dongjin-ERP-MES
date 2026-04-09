import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Report } from 'src/app/shared/ERP/Report.model';
import { ReportService } from 'src/app/shared/ERP/Report.service';

import { ReportDetail } from 'src/app/shared/ERP/ReportDetail.model';
import { ReportDetailService } from 'src/app/shared/ERP/ReportDetail.service';

import { WarehouseInputDetailBarcode } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.model';
import { WarehouseInputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { ProductionTrackingKOMAXComponent } from '../production-tracking-komax/production-tracking-komax.component';
import { ProductionTrackingLPComponent } from '../production-tracking-lp/production-tracking-lp.component';
import { ProductionTrackingSPSTComponent } from '../production-tracking-spst/production-tracking-spst.component';
import { ProductionTrackingSHIELDWIREComponent } from '../production-tracking-shieldwire/production-tracking-shieldwire.component';
import { ProductionTrackingHookRackModalComponent } from '../production-tracking-hook-rack-modal/production-tracking-hook-rack-modal.component';

@Component({
  selector: 'app-production-tracking',
  templateUrl: './production-tracking.component.html',
  styleUrls: ['./production-tracking.component.css']
})
export class ProductionTrackingComponent {

  @ViewChild('ReportDetailSort') ReportDetailSort: MatSort;
  @ViewChild('ReportDetailPaginator') ReportDetailPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ReportService: ReportService,
    public ReportDetailService: ReportDetailService,
    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,
    public CompanyService: CompanyService,

  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.ReportService);

  }
  ReportSearch() {
    this.ReportService.IsShowLoading = true;
    if (this.ReportService.BaseParameter.SearchStringFilter01 && this.ReportService.BaseParameter.SearchStringFilter01.length > 0) {
      this.ReportService.BaseParameter.SearchStringFilter01 = this.ReportService.BaseParameter.SearchStringFilter01.trim();
    }
    if (this.ReportService.BaseParameter.SearchStringFilter && this.ReportService.BaseParameter.SearchStringFilter.length > 0) {
      this.ReportService.BaseParameter.SearchStringFilter = this.ReportService.BaseParameter.SearchStringFilter.trim();
    }
    if (this.ReportService.BaseParameter.SearchString && this.ReportService.BaseParameter.SearchString.length > 0) {
      this.ReportService.BaseParameter.SearchString = this.ReportService.BaseParameter.SearchString.trim();
    }

    this.ReportService.GetByProductionTrackingToListAsync().subscribe(
      res => {
        this.ReportService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        if (this.ReportService.BaseParameter.BaseModel) {
          if (this.ReportService.BaseParameter.BaseModel.ID) {
            this.ReportDetailService.BaseParameter.ParentID = this.ReportService.BaseParameter.BaseModel.ID;
            this.ReportDetailService.GetByParentIDToListAsync().subscribe(
              res => {
                this.ReportDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder < b.SortOrder ? 1 : -1));
                this.ReportDetailService.DataSource = new MatTableDataSource(this.ReportDetailService.List);
                this.ReportDetailService.DataSource.sort = this.ReportDetailSort;
                this.ReportDetailService.DataSource.paginator = this.ReportDetailPaginator;
              },
              err => {
              },
              () => {
                this.ReportService.IsShowLoading = false;
              }
            );
          }
        }
      },
      err => {
      },
      () => {
      }
    );
  }
  ProductionTrackingHOOKRACKModalOpen(element: ReportDetail) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.CompanyID = this.ReportService.BaseParameter.CompanyID;
    this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = element.Code;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(ProductionTrackingHookRackModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
  ProductionTrackingKOMAXModalOpen(element: ReportDetail) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.CompanyID = this.ReportService.BaseParameter.CompanyID;
    this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = element.Code;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(ProductionTrackingKOMAXComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
  ProductionTrackingSHIELDWIREModalOpen(element: ReportDetail) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.CompanyID = this.ReportService.BaseParameter.CompanyID;
    this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = element.Code;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(ProductionTrackingSHIELDWIREComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
  ProductionTrackingLPModalOpen(element: ReportDetail) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.CompanyID = this.ReportService.BaseParameter.CompanyID;
    this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = element.Code;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(ProductionTrackingLPComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }

  ProductionTrackingSPSTModalOpen(element: ReportDetail) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.CompanyID = this.ReportService.BaseParameter.CompanyID;
    this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = element.Code;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(ProductionTrackingSPSTComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
}
