import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseRequest } from 'src/app/shared/ERP/WarehouseRequest.model';
import { WarehouseRequestService } from 'src/app/shared/ERP/WarehouseRequest.service';
import { WarehouseRequestModalComponent } from '../warehouse-request-modal/warehouse-request-modal.component';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-warehouse-request-by-category-department',
  templateUrl: './warehouse-request-by-category-department.component.html',
  styleUrls: ['./warehouse-request-by-category-department.component.css']
})
export class WarehouseRequestByCategoryDepartmentComponent {

  @ViewChild('WarehouseRequestSort') WarehouseRequestSort: MatSort;
  @ViewChild('WarehouseRequestPaginator') WarehouseRequestPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseRequestService: WarehouseRequestService,

    public CategoryDepartmentService: CategoryDepartmentService,
    public CompanyService: CompanyService,
  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.WarehouseRequestSearch();
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.WarehouseRequestService);
    this.CompanyChange();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.WarehouseRequestService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.WarehouseRequestService);
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.WarehouseRequestService);
  }
  WarehouseRequestSearch() {
    this.WarehouseRequestService.IsShowLoading = true;
    this.WarehouseRequestService.GetByCategoryDepartmentID_SearchStringToListAsync().subscribe(
      res => {
        this.WarehouseRequestService.List = (res as BaseResult).List;
        this.WarehouseRequestService.DataSource = new MatTableDataSource(this.WarehouseRequestService.List);
        this.WarehouseRequestService.DataSource.sort = this.WarehouseRequestSort;
        this.WarehouseRequestService.DataSource.paginator = this.WarehouseRequestPaginator;
      },
      err => {
      },
      () => {
        this.WarehouseRequestService.IsShowLoading = false;
      }
    );
  }
}
