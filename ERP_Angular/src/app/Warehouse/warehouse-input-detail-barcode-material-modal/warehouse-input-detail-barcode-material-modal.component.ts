import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseInputDetailBarcodeMaterial } from 'src/app/shared/ERP/WarehouseInputDetailBarcodeMaterial.model';
import { WarehouseInputDetailBarcodeMaterialService } from 'src/app/shared/ERP/WarehouseInputDetailBarcodeMaterial.service';

@Component({
  selector: 'app-warehouse-input-detail-barcode-material-modal',
  templateUrl: './warehouse-input-detail-barcode-material-modal.component.html',
  styleUrls: ['./warehouse-input-detail-barcode-material-modal.component.css']
})
export class WarehouseInputDetailBarcodeMaterialModalComponent {
  @ViewChild('WarehouseInputDetailBarcodeMaterialSortFilter') WarehouseInputDetailBarcodeMaterialSortFilter: MatSort;
  @ViewChild('WarehouseInputDetailBarcodeMaterialPaginatorFilter') WarehouseInputDetailBarcodeMaterialPaginatorFilter: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<WarehouseInputDetailBarcodeMaterialModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseInputDetailBarcodeMaterialService: WarehouseInputDetailBarcodeMaterialService,

  ) {

  }
  ngOnInit(): void {
  }
  ngAfterViewInit() {
    this.WarehouseInputDetailBarcodeMaterialSearch();
  }
  Close() {
    this.DialogRef.close();
  }


  WarehouseInputDetailBarcodeMaterialSearch() {
    if (this.WarehouseInputDetailBarcodeMaterialService.BaseParameter.SearchString.length > 0) {
      this.WarehouseInputDetailBarcodeMaterialService.BaseParameter.SearchString = this.WarehouseInputDetailBarcodeMaterialService.BaseParameter.SearchString.trim();
      if (this.WarehouseInputDetailBarcodeMaterialService.DataSource) {
        this.WarehouseInputDetailBarcodeMaterialService.DataSource.filter = this.WarehouseInputDetailBarcodeMaterialService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseInputDetailBarcodeMaterialService.IsShowLoading = true;
      this.WarehouseInputDetailBarcodeMaterialService.GetByWarehouseInputDetailBarcodeIDToListAsync().subscribe(
        res => {
          this.WarehouseInputDetailBarcodeMaterialService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.Quantity < b.Quantity ? 1 : -1));
          this.WarehouseInputDetailBarcodeMaterialService.DataSourceFilter = new MatTableDataSource(this.WarehouseInputDetailBarcodeMaterialService.ListFilter);
          this.WarehouseInputDetailBarcodeMaterialService.DataSourceFilter.sort = this.WarehouseInputDetailBarcodeMaterialSortFilter;
          this.WarehouseInputDetailBarcodeMaterialService.DataSourceFilter.paginator = this.WarehouseInputDetailBarcodeMaterialPaginatorFilter;
        },
        err => {
        },
        () => {
          this.WarehouseInputDetailBarcodeMaterialService.IsShowLoading = false;
        }
      );
    }
  }
}