import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

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
  selector: 'app-production-tracking2026',
  templateUrl: './production-tracking2026.component.html',
  styleUrls: ['./production-tracking2026.component.css']
})
export class ProductionTracking2026Component implements OnInit {

  @ViewChild('ReportDetailSort') ReportDetailSort: MatSort;
  @ViewChild('ReportDetailPaginator') ReportDetailPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ReportService: ReportService,
    public ReportDetailService: ReportDetailService,
    public CompanyService: CompanyService,

  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  ReportDate(value) {
    this.ReportService.BaseParameter.Date = new Date(value);
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.ReportService);

  }
  ReportSearch() {
    this.ReportService.IsShowLoading = true;
    this.ReportService.GetByProductionTracking2026Async().subscribe(
      res => {
        this.ReportService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        if (this.ReportService.BaseParameter.BaseModel) {
          if (this.ReportService.BaseParameter.BaseModel.ID > 0) {
            this.ReportService.IsShowLoading = true;
            this.ReportDetailService.BaseParameter.ParentID = this.ReportService.BaseParameter.BaseModel.ID;
            this.ReportDetailService.BaseParameter.Name = this.ReportService.BaseParameter.Name;
            this.ReportDetailService.BaseParameter.Code = this.ReportService.BaseParameter.Code;
            this.ReportDetailService.BaseParameter.Display = this.ReportService.BaseParameter.Display;
            this.ReportDetailService.GetProductionTracking2026ByParentIDToListAsync().subscribe(
              res => {
                this.ReportDetailService.List = (res as BaseResult).List;
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
        else {
          this.ReportService.IsShowLoading = false;
        }
      },
      err => {
      },
      () => {
        //this.ReportService.IsShowLoading = false;
      }
    );
  }
  ReportSync() {
    this.ReportService.IsShowLoading = true;
    this.ReportService.SyncByProductionTracking2026Async().subscribe(
      res => {
        this.NotificationService.warn(environment.Synchronizing);
        this.ReportSearch();
      },
      err => {
      },
      () => {
        this.ReportService.IsShowLoading = false;
      }
    );
  }
  ProductionTrackingHOOKRACKModalOpen(element: ReportDetail) {

  }
  ProductionTrackingKOMAXModalOpen(element: ReportDetail) {

  }
  ProductionTrackingSHIELDWIREModalOpen(element: ReportDetail) {

  }
  ProductionTrackingLPModalOpen(element: ReportDetail) {

  }

  ProductionTrackingSPSTModalOpen(element: ReportDetail) {

  }

  @ViewChild("TABLE") table: ElementRef;
  ReportExcel() {
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");
    let filename = "ProductionTracking.xlsx";
    XLSX.writeFile(wb, filename);
  }
}