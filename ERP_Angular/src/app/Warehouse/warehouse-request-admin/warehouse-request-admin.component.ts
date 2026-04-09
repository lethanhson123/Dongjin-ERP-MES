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
  selector: 'app-warehouse-request-admin',
  templateUrl: './warehouse-request-admin.component.html',
  styleUrls: ['./warehouse-request-admin.component.css']
})
export class WarehouseRequestAdminComponent {

  @ViewChild('WarehouseRequestSort') WarehouseRequestSort: MatSort;
  @ViewChild('WarehouseRequestPaginator') WarehouseRequestPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseRequestService: WarehouseRequestService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,
  ) {
    this.WarehouseRequestService.BaseParameter.Action = 1;
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    //this.WarehouseRequestSearch();
  }
  DateBegin(value) {
    this.WarehouseRequestService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.WarehouseRequestService.BaseParameter.DateEnd = new Date(value);
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
  }
  WarehouseRequestSearch() {
    this.WarehouseRequestService.IsShowLoading = true;
    this.WarehouseRequestService.GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync().subscribe(
      res => {
        this.WarehouseRequestService.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));
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
  WarehouseRequestDelete(element: WarehouseRequest) {
    this.WarehouseRequestService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.WarehouseRequestService.ComponentDelete(this.WarehouseRequestSort, this.WarehouseRequestPaginator));
  }
  WarehouseRequestModal(element: WarehouseRequest) {
    this.WarehouseRequestService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseRequestModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
      this.WarehouseRequestSearch();
    });
  }
  WarehouseRequestAdd() {
    this.WarehouseRequestService.IsShowLoading = true;
    this.WarehouseRequestService.BaseParameter.ID = environment.InitializationNumber;
    this.WarehouseRequestService.GetByIDAsync().subscribe(
      res => {
        this.WarehouseRequestService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseRequestModal(this.WarehouseRequestService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.WarehouseRequestService.IsShowLoading = false;
      }
    );
  }
}
