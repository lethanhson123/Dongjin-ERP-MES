import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseInputDetailBarcode } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.model';
import { WarehouseInputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.service';

@Component({
  selector: 'app-production-tracking-lp',
  templateUrl: './production-tracking-lp.component.html',
  styleUrls: ['./production-tracking-lp.component.css']
})
export class ProductionTrackingLPComponent {

@ViewChild('WarehouseInputDetailBarcodeSortFilter') WarehouseInputDetailBarcodeSortFilter: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginatorFilter') WarehouseInputDetailBarcodePaginatorFilter: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<ProductionTrackingLPComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,


    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,
  ) {

  }
  ngOnInit(): void {
  }
  ngAfterViewInit() {
    this.WarehouseInputDetailBarcodeSearch();
  }
  Close() {
    this.DialogRef.close();
  }
  WarehouseInputDetailBarcodeSearch() {
    this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.GetByLP_SearchStringToListAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.DateScan > b.DateScan ? 1 : -1));
        this.WarehouseInputDetailBarcodeService.DataSourceFilter = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.ListFilter);
        this.WarehouseInputDetailBarcodeService.DataSourceFilter.sort = this.WarehouseInputDetailBarcodeSortFilter;
        this.WarehouseInputDetailBarcodeService.DataSourceFilter.paginator = this.WarehouseInputDetailBarcodePaginatorFilter;
      },
      err => {
      },
      () => {
        this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
}

