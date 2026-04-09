import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseOutputDetailBarcodeMaterial } from 'src/app/shared/ERP/WarehouseOutputDetailBarcodeMaterial.model';
import { WarehouseOutputDetailBarcodeMaterialService } from 'src/app/shared/ERP/WarehouseOutputDetailBarcodeMaterial.service';

@Component({
  selector: 'app-warehouse-output-detail-barcode-material-modal',
  templateUrl: './warehouse-output-detail-barcode-material-modal.component.html',
  styleUrls: ['./warehouse-output-detail-barcode-material-modal.component.css']
})
export class WarehouseOutputDetailBarcodeMaterialModalComponent implements OnInit {

  @ViewChild('WarehouseOutputDetailBarcodeMaterialSortFilter') WarehouseOutputDetailBarcodeMaterialSortFilter: MatSort;
  @ViewChild('WarehouseOutputDetailBarcodeMaterialPaginatorFilter') WarehouseOutputDetailBarcodeMaterialPaginatorFilter: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<WarehouseOutputDetailBarcodeMaterialModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseOutputDetailBarcodeMaterialService: WarehouseOutputDetailBarcodeMaterialService,

  ) {

  }
  ngOnInit(): void {
  }
  ngAfterViewInit() {
    this.WarehouseOutputDetailBarcodeMaterialSearch();
  }
  Close() {
    this.DialogRef.close();
  }


  WarehouseOutputDetailBarcodeMaterialSearch() {
    if (this.WarehouseOutputDetailBarcodeMaterialService.BaseParameter.SearchString.length > 0) {
      this.WarehouseOutputDetailBarcodeMaterialService.BaseParameter.SearchString = this.WarehouseOutputDetailBarcodeMaterialService.BaseParameter.SearchString.trim();
      if (this.WarehouseOutputDetailBarcodeMaterialService.DataSource) {
        this.WarehouseOutputDetailBarcodeMaterialService.DataSource.filter = this.WarehouseOutputDetailBarcodeMaterialService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseOutputDetailBarcodeMaterialService.IsShowLoading = true;
      this.WarehouseOutputDetailBarcodeMaterialService.GetByWarehouseOutputDetailBarcodeIDToListAsync().subscribe(
        res => {
          this.WarehouseOutputDetailBarcodeMaterialService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.Quantity < b.Quantity ? 1 : -1));
          this.WarehouseOutputDetailBarcodeMaterialService.DataSourceFilter = new MatTableDataSource(this.WarehouseOutputDetailBarcodeMaterialService.ListFilter);
          this.WarehouseOutputDetailBarcodeMaterialService.DataSourceFilter.sort = this.WarehouseOutputDetailBarcodeMaterialSortFilter;
          this.WarehouseOutputDetailBarcodeMaterialService.DataSourceFilter.paginator = this.WarehouseOutputDetailBarcodeMaterialPaginatorFilter;
        },
        err => {
        },
        () => {
          this.WarehouseOutputDetailBarcodeMaterialService.IsShowLoading = false;
        }
      );
    }
  }
}