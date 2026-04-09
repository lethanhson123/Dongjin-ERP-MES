import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseOutputDetailBarcode } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.model';
import { WarehouseOutputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

@Component({
  selector: 'app-warehouse-output-detail-barcode-modal',
  templateUrl: './warehouse-output-detail-barcode-modal.component.html',
  styleUrls: ['./warehouse-output-detail-barcode-modal.component.css']
})
export class WarehouseOutputDetailBarcodeModalComponent {

  @ViewChild('WarehouseOutputDetailBarcodeSortFilter') WarehouseOutputDetailBarcodeSortFilter: MatSort;
  @ViewChild('WarehouseOutputDetailBarcodePaginatorFilter') WarehouseOutputDetailBarcodePaginatorFilter: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<WarehouseOutputDetailBarcodeModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,


    public WarehouseOutputDetailBarcodeService: WarehouseOutputDetailBarcodeService,
    public MaterialService: MaterialService,
  ) {

  }
  ngOnInit(): void {
  }
  ngAfterViewInit() {
    this.WarehouseOutputDetailBarcodeSearch();
  }
  Close() {
    this.DialogRef.close();
  }
  MaterialModal(element: WarehouseOutputDetailBarcode) {
    this.WarehouseOutputDetailBarcodeService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = element.MaterialID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        const dialog = this.Dialog.open(MaterialModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => {

        });
      },
      err => {
      },
      () => {
        this.WarehouseOutputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputDetailBarcodeSearch() {
    if (this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString.length > 0) {
      this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString.trim();
      if (this.WarehouseOutputDetailBarcodeService.DataSource) {
        this.WarehouseOutputDetailBarcodeService.DataSource.filter = this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseOutputDetailBarcodeService.IsShowLoading = true;
      this.WarehouseOutputDetailBarcodeService.GetByWarehouseOutputDetailIDToListAsync().subscribe(
        res => {
          this.WarehouseOutputDetailBarcodeService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.Quantity > b.Quantity ? 1 : -1));
          this.WarehouseOutputDetailBarcodeService.DataSourceFilter = new MatTableDataSource(this.WarehouseOutputDetailBarcodeService.ListFilter);
          this.WarehouseOutputDetailBarcodeService.DataSourceFilter.sort = this.WarehouseOutputDetailBarcodeSortFilter;
          this.WarehouseOutputDetailBarcodeService.DataSourceFilter.paginator = this.WarehouseOutputDetailBarcodePaginatorFilter;
        },
        err => {
        },
        () => {
          this.WarehouseOutputDetailBarcodeService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseOutputDetailBarcodeDelete(element: WarehouseOutputDetailBarcode) {
    this.WarehouseOutputDetailBarcodeService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseOutputDetailBarcodeService.IsShowLoading = true;
      this.WarehouseOutputDetailBarcodeService.RemoveAsync().subscribe(
        res => {
          this.WarehouseOutputDetailBarcodeSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseOutputDetailBarcodeService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseOutputDetailBarcodeSave(element: WarehouseOutputDetailBarcode) {
    this.WarehouseOutputDetailBarcodeService.IsShowLoading = true;
    element.ParentID = this.WarehouseOutputDetailBarcodeService.BaseParameter.BaseModel.ID;
    this.WarehouseOutputDetailBarcodeService.BaseParameter.BaseModel = element;
    this.WarehouseOutputDetailBarcodeService.SaveAsync().subscribe(
      res => {
        this.WarehouseOutputDetailBarcodeSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseOutputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
}
