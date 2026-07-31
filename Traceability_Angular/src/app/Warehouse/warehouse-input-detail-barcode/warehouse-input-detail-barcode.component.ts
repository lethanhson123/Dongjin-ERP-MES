import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

import { WarehouseInputDetailBarcode } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.model';
import { WarehouseInputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.service';


@Component({
  selector: 'app-warehouse-input-detail-barcode',
  templateUrl: './warehouse-input-detail-barcode.component.html',
  styleUrls: ['./warehouse-input-detail-barcode.component.css']
})
export class WarehouseInputDetailBarcodeComponent {



  @ViewChild('WarehouseInputDetailBarcodeSort') WarehouseInputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginator') WarehouseInputDetailBarcodePaginator: MatPaginator;



  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,


    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,

  ) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.Active = true;
    this.WarehouseInputDetailBarcodeService.BaseParameter.IsComplete = true;
    this.WarehouseInputDetailBarcodeService.List = [];

    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this.WarehouseInputDetailBarcodeService.BaseParameter.ID = this.ActiveRouter.snapshot.params.ID;        
        this.WarehouseInputDetailBarcodeSearch();
      }
    });
  }
  ngOnInit(): void {
  }
  ngAfterViewInit() {
    //this.WarehouseInputDetailBarcodeSearch();
  }

  WarehouseInputDetailBarcodeSearch() {
    this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
    if (this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString == null) {
      this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = environment.InitializationString;
    }
    this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.trim();
    this.WarehouseInputDetailBarcodeService.GetByIDToListAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.List = (res as BaseResult).List;
        this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.List);
        this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
        this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;
        this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = environment.InitializationString;
      },
      err => {
      },
      () => {
        this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }

}
