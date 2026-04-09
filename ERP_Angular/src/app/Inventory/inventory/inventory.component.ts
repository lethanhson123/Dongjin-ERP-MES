import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Inventory } from 'src/app/shared/ERP/Inventory.model';
import { InventoryService } from 'src/app/shared/ERP/Inventory.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})
export class InventoryComponent {

  @ViewChild('InventorySort') InventorySort: MatSort;
  @ViewChild('InventoryPaginator') InventoryPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public InventoryService: InventoryService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,

  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    //this.InventorySearch();
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.InventoryService);
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.InventoryService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.InventoryService);
  }
  DateBegin(value) {
    this.InventoryService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.InventoryService.BaseParameter.DateEnd = new Date(value);
  }
  InventorySearch() {
    this.InventoryService.IsShowLoading = true;
    this.InventoryService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));

    this.InventoryService.GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync().subscribe(
      res => {
        this.InventoryService.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));
        this.InventoryService.DataSource = new MatTableDataSource(this.InventoryService.List);
        this.InventoryService.DataSource.sort = this.InventorySort;
        this.InventoryService.DataSource.paginator = this.InventoryPaginator;
      },
      err => {
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
}
