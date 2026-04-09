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

import { WarehouseOutputDetail } from 'src/app/shared/ERP/WarehouseOutputDetail.model';
import { WarehouseOutputDetailService } from 'src/app/shared/ERP/WarehouseOutputDetail.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

@Component({
  selector: 'app-warehouse-output-detail',
  templateUrl: './warehouse-output-detail.component.html',
  styleUrls: ['./warehouse-output-detail.component.css']
})
export class WarehouseOutputDetailComponent {

  @ViewChild('WarehouseOutputDetailSort') WarehouseOutputDetailSort: MatSort;
  @ViewChild('WarehouseOutputDetailPaginator') WarehouseOutputDetailPaginator: MatPaginator;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseOutputDetailService: WarehouseOutputDetailService,
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
    this.WarehouseOutputDetailSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.ComponentGetByActiveToListAsync(this.WarehouseOutputDetailService);
  }
  ComponentGetYearList() {
    this.WarehouseOutputDetailService.ComponentGetYearList();
  }
  ComponentGetMonthList() {
    this.WarehouseOutputDetailService.ComponentGetMonthList();
  }
  ComponentGetDayList() {
    this.WarehouseOutputDetailService.ComponentGetDayList();
  }
  WarehouseOutputDetailSearch() {
    this.WarehouseOutputDetailService.IsShowLoading = true;
    this.WarehouseOutputDetailService.GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync().subscribe(
      res => {
        this.WarehouseOutputDetailService.List = (res as BaseResult).List;
        this.WarehouseOutputDetailService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailService.List);
        this.WarehouseOutputDetailService.DataSource.sort = this.WarehouseOutputDetailSort;
        this.WarehouseOutputDetailService.DataSource.paginator = this.WarehouseOutputDetailPaginator;
      },
      err => {
      },
      () => {
        this.WarehouseOutputDetailService.IsShowLoading = false;
      }
    );
  }
  MaterialModal(element: WarehouseOutputDetail) {
    this.WarehouseOutputDetailService.IsShowLoading = true;
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
        this.WarehouseOutputDetailService.IsShowLoading = false;
      }
    );
  }
}

