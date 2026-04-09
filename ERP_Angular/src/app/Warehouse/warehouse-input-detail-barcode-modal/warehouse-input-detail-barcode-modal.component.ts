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

import { WarehouseOutputDetailBarcode } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.model';
import { WarehouseOutputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.service';

import { WarehouseInputDetailBarcodeMaterial } from 'src/app/shared/ERP/WarehouseInputDetailBarcodeMaterial.model';
import { WarehouseInputDetailBarcodeMaterialService } from 'src/app/shared/ERP/WarehouseInputDetailBarcodeMaterial.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';
import { WarehouseOutputDetailBarcodeHistoryModalComponent } from '../warehouse-output-detail-barcode-history-modal/warehouse-output-detail-barcode-history-modal.component';
import { WarehouseInputDetailBarcodeMaterialModalComponent } from '../warehouse-input-detail-barcode-material-modal/warehouse-input-detail-barcode-material-modal.component';

@Component({
  selector: 'app-warehouse-input-detail-barcode-modal',
  templateUrl: './warehouse-input-detail-barcode-modal.component.html',
  styleUrls: ['./warehouse-input-detail-barcode-modal.component.css']
})
export class WarehouseInputDetailBarcodeModalComponent {

  @ViewChild('WarehouseInputDetailBarcodeSortFilter') WarehouseInputDetailBarcodeSortFilter: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginatorFilter') WarehouseInputDetailBarcodePaginatorFilter: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<WarehouseInputDetailBarcodeModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,


    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,
    public WarehouseOutputDetailBarcodeService: WarehouseOutputDetailBarcodeService,
    public WarehouseInputDetailBarcodeMaterialService: WarehouseInputDetailBarcodeMaterialService,
    public MaterialService: MaterialService,
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
  MaterialModal(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
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
        this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputDetailBarcodeModal(element: WarehouseInputDetailBarcode) {
    this.WarehouseOutputDetailBarcodeService.BaseParameter.Barcode = element.Barcode;
    this.WarehouseOutputDetailBarcodeService.BaseParameter.Active = true;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseOutputDetailBarcodeHistoryModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
  WarehouseInputDetailBarcodeSearch() {
    if (this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.length > 0) {
      this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.trim();
      if (this.WarehouseInputDetailBarcodeService.DataSource) {
        this.WarehouseInputDetailBarcodeService.DataSource.filter = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
      this.WarehouseInputDetailBarcodeService.GetByParentID_MaterialIDToListAsync().subscribe(
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
  WarehouseInputDetailBarcodeDelete(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
      this.WarehouseInputDetailBarcodeService.RemoveAsync().subscribe(
        res => {
          this.WarehouseInputDetailBarcodeSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputDetailBarcodeSave(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailBarcodeService.SaveAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodeMaterialModal(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputDetailBarcodeMaterialService.BaseParameter.GeneralID = element.ID;
    //this.WarehouseInputDetailBarcodeMaterialService.BaseParameter.GeneralID = 1841744;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseInputDetailBarcodeMaterialModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
}
