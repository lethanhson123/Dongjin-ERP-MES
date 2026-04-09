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

import { WarehouseInputDetail } from 'src/app/shared/ERP/WarehouseInputDetail.model';
import { WarehouseInputDetailService } from 'src/app/shared/ERP/WarehouseInputDetail.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';


@Component({
  selector: 'app-warehouse-input-detail',
  templateUrl: './warehouse-input-detail.component.html',
  styleUrls: ['./warehouse-input-detail.component.css']
})
export class WarehouseInputDetailComponent {

  @ViewChild('WarehouseInputDetailSort') WarehouseInputDetailSort: MatSort;
  @ViewChild('WarehouseInputDetailPaginator') WarehouseInputDetailPaginator: MatPaginator;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseInputDetailService: WarehouseInputDetailService,
    public MaterialService: MaterialService,
    public CategoryDepartmentService: CategoryDepartmentService,
  ) {
    this.ComponentGetYearList();
    this.ComponentGetMonthList();
    this.ComponentGetDayList();
    this.CategoryDepartmentSearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.WarehouseInputDetailSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.ComponentGetByActiveToListAsync(this.WarehouseInputDetailService);
  }
  ComponentGetYearList() {
    this.WarehouseInputDetailService.ComponentGetYearList();
  }
  ComponentGetMonthList() {
    this.WarehouseInputDetailService.ComponentGetMonthList();
  }
  ComponentGetDayList() {
    this.WarehouseInputDetailService.ComponentGetDayList();
  }
  WarehouseInputDetailSearch() {
    this.WarehouseInputDetailService.IsShowLoading = true;
    this.WarehouseInputDetailService.GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync().subscribe(
      res => {
        this.WarehouseInputDetailService.List = (res as BaseResult).List;
        this.WarehouseInputDetailService.DataSource = new MatTableDataSource(this.WarehouseInputDetailService.List);
        this.WarehouseInputDetailService.DataSource.sort = this.WarehouseInputDetailSort;
        this.WarehouseInputDetailService.DataSource.paginator = this.WarehouseInputDetailPaginator;
      },
      err => {
      },
      () => {
        this.WarehouseInputDetailService.IsShowLoading = false;
      }
    );
  }
  MaterialModal(element: WarehouseInputDetail) {
    this.WarehouseInputDetailService.IsShowLoading = true;
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
        this.WarehouseInputDetailService.IsShowLoading = false;
      }
    );
  }
}
