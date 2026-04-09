import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseInventory } from 'src/app/shared/ERP/WarehouseInventory.model';
import { WarehouseInventoryService } from 'src/app/shared/ERP/WarehouseInventory.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

@Component({
  selector: 'app-warehouse-inventory-company',
  templateUrl: './warehouse-inventory-company.component.html',
  styleUrls: ['./warehouse-inventory-company.component.css']
})
export class WarehouseInventoryCompanyComponent {

  @ViewChild('WarehouseInventorySort') WarehouseInventorySort: MatSort;
  @ViewChild('WarehouseInventoryPaginator') WarehouseInventoryPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseInventoryService: WarehouseInventoryService,
    public CompanyService: CompanyService,
    public MaterialService: MaterialService,
    public CategoryDepartmentService: CategoryDepartmentService,

  ) {
    this.CompanySearch();
    this.ComponentGetYearList();
    this.ComponentGetMonthList();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {

  }
  CompanySearch() {
    this.WarehouseInventoryService.IsShowLoading = true;
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      res => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.WarehouseInventoryService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
        }
        this.CompanyChange();
      },
      err => {
      },
      () => {
        this.WarehouseInventoryService.IsShowLoading = false;
      }
    );
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  ComponentGetYearList() {
    this.WarehouseInventoryService.ComponentGetYearList();
  }
  ComponentGetMonthList() {
    this.WarehouseInventoryService.ComponentGetMonthList();
  }
  CategoryDepartmentSearch() {
  }
  WarehouseInventorySearch() {
    if (this.WarehouseInventoryService.BaseParameter.SearchString.length > 0) {
      this.WarehouseInventoryService.BaseParameter.SearchString = this.WarehouseInventoryService.BaseParameter.SearchString.trim();
      this.WarehouseInventoryService.BaseParameter.SearchString.toLocaleLowerCase();
      let List0 = this.WarehouseInventoryService.List.filter(o => o.SortOrder == environment.InitializationNumber);
      let List = this.WarehouseInventoryService.List.filter(o => (o.Code && o.Code.length > 0 && o.Code.includes(this.WarehouseInventoryService.BaseParameter.SearchString))
        || (o.ParentName && o.ParentName.length > 0 && o.ParentName.includes(this.WarehouseInventoryService.BaseParameter.SearchString))
      );
      if (List0) {
        if (List0.length > 0) {
          List.push(List0[0]);
        }
      }
      List = List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      this.WarehouseInventoryService.DataSource = new MatTableDataSource(List);
      this.WarehouseInventoryService.DataSource.sort = this.WarehouseInventorySort;
      this.WarehouseInventoryService.DataSource.paginator = this.WarehouseInventoryPaginator;
    }
    else {
      this.WarehouseInventoryService.IsShowLoading = true;
      this.WarehouseInventoryService.GetByCompanyIDAndYearAndMonthToListAsync().subscribe(
        res => {
          this.WarehouseInventoryService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          if (this.WarehouseInventoryService.List) {
            if (this.WarehouseInventoryService.List.length > 0) {
              if (this.WarehouseInventoryService.BaseParameter.Year == environment.InitializationNumber) {
                this.WarehouseInventoryService.DisplayColumns000 = this.WarehouseInventoryService.DisplayColumns003;

                this.WarehouseInventoryService.BaseParameter.BaseModel.ID = environment.InitializationNumber;
                this.WarehouseInventoryService.BaseParameter.BaseModel.SortOrder = environment.InitializationNumber;
                this.WarehouseInventoryService.BaseParameter.BaseModel.QuantityInput00 = 100000000;
                this.WarehouseInventoryService.BaseParameter.BaseModel.QuantityOutput00 = 100000000;
                this.WarehouseInventoryService.BaseParameter.BaseModel.Quantity00 = 100000000;
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input01 = "Before 2019";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input02 = "2019";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input03 = "2020";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input04 = "2021";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input05 = "2022";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input06 = "2023";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input07 = "2024";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input08 = "2025";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input09 = "2026";
                this.WarehouseInventoryService.List.push(this.WarehouseInventoryService.BaseParameter.BaseModel);

              }
              else {
                if (this.WarehouseInventoryService.BaseParameter.Month == environment.InitializationNumber) {
                  this.WarehouseInventoryService.DisplayColumns000 = this.WarehouseInventoryService.DisplayColumns002;
                }
                else {
                  this.WarehouseInventoryService.DisplayColumns000 = this.WarehouseInventoryService.DisplayColumns001;
                }
              }
              this.WarehouseInventoryService.List = this.WarehouseInventoryService.List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            }
          }
          this.WarehouseInventoryService.DataSource = new MatTableDataSource(this.WarehouseInventoryService.List);
          this.WarehouseInventoryService.DataSource.sort = this.WarehouseInventorySort;
          this.WarehouseInventoryService.DataSource.paginator = this.WarehouseInventoryPaginator;
        },
        err => {
        },
        () => {
          this.WarehouseInventoryService.IsShowLoading = false;
        }
      );
    }
  }
  MaterialModal(element: WarehouseInventory) {
    this.WarehouseInventoryService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = element.ParentID;
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
        this.WarehouseInventoryService.IsShowLoading = false;
      }
    );
  }
  CreateAutoAsync() {
    this.WarehouseInventoryService.IsShowLoading = true;
    this.WarehouseInventoryService.BaseParameter.CategoryDepartmentID = 0;
    this.WarehouseInventoryService.CreateAutoAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.Synchronizing);
        this.WarehouseInventorySearch();
      },
      err => {
      },
      () => {
        this.WarehouseInventoryService.IsShowLoading = false;
      }
    );
  }
}

