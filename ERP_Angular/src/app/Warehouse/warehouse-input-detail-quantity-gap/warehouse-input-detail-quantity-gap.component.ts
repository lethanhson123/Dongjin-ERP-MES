import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseInputDetail } from 'src/app/shared/ERP/WarehouseInputDetail.model';
import { WarehouseInputDetailService } from 'src/app/shared/ERP/WarehouseInputDetail.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-warehouse-input-detail-quantity-gap',
  templateUrl: './warehouse-input-detail-quantity-gap.component.html',
  styleUrls: ['./warehouse-input-detail-quantity-gap.component.css']
})
export class WarehouseInputDetailQuantityGAPComponent implements OnInit {

  @ViewChild('WarehouseInputDetailSort') WarehouseInputDetailSort: MatSort;
  @ViewChild('WarehouseInputDetailPaginator') WarehouseInputDetailPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseInputDetailService: WarehouseInputDetailService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,

  ) {    
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.WarehouseInputDetailSearch();
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.WarehouseInputDetailService);
    this.CompanyChange();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.WarehouseInputDetailService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.WarehouseInputDetailService);
  }
  WarehouseInputDetailSearch() {
    if (this.WarehouseInputDetailService.BaseParameter.SearchString && this.WarehouseInputDetailService.BaseParameter.SearchString.length > 0) {
      this.WarehouseInputDetailService.BaseParameter.SearchString = this.WarehouseInputDetailService.BaseParameter.SearchString.trim();
      if (this.WarehouseInputDetailService.DataSource) {
        this.WarehouseInputDetailService.DataSource.filter = this.WarehouseInputDetailService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseInputDetailService.IsShowLoading = true;
      this.WarehouseInputDetailService.GetByQuantityGAPToListAsync().subscribe(
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
  }
}