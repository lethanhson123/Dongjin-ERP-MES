import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
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
  selector: 'app-category-location',
  templateUrl: './category-location.component.html',
  styleUrls: ['./category-location.component.css']
})
export class CategoryLocationComponent implements OnInit {

  @ViewChild('WarehouseInputDetailBarcodeSort') WarehouseInputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginator') WarehouseInputDetailBarcodePaginator: MatPaginator;

   @ViewChild('WarehouseInputDetailBarcodeSortFilter') WarehouseInputDetailBarcodeSortFilter: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginatorFilter') WarehouseInputDetailBarcodePaginatorFilter: MatPaginator;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,

  ) {
    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {        
        this.WarehouseInputDetailBarcodeService.BaseParameter.CategoryDepartmentID = this.ActiveRouter.snapshot.params.CategoryDepartmentID;
        this.WarehouseInputDetailBarcodeService.BaseParameter.Name = this.ActiveRouter.snapshot.params.Name;
        this.WarehouseInputDetailBarcodeSearch();
      }
    });
  }

  ngOnInit(): void {
  }
  ngAfterViewInit() {
  }

  WarehouseInputDetailBarcodeSearch() {
    this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.GetStockByCategoryLocationToListAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.List = (res as BaseResult).List;
        this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.List);
        this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
        this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;  
        
        this.WarehouseInputDetailBarcodeService.ListFilter = (res as BaseResult).ListFilter;
        this.WarehouseInputDetailBarcodeService.DataSourceFilter = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.ListFilter);
        this.WarehouseInputDetailBarcodeService.DataSourceFilter.sort = this.WarehouseInputDetailBarcodeSort;
        this.WarehouseInputDetailBarcodeService.DataSourceFilter.paginator = this.WarehouseInputDetailBarcodePaginator;  
        console.log(this.WarehouseInputDetailBarcodeService.ListFilter);
      },      
      err => {
      },
      () => {
        this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }

}
