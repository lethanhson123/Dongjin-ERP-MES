import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseInput } from 'src/app/shared/ERP/WarehouseInput.model';
import { WarehouseInputService } from 'src/app/shared/ERP/WarehouseInput.service';

import { WarehouseInputDetailBarcode } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.model';
import { WarehouseInputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { InvoiceInputModalComponent } from 'src/app/Invoice/invoice-input-modal/invoice-input-modal.component';
import { InvoiceInput } from 'src/app/shared/ERP/InvoiceInput.model';
import { InvoiceInputService } from 'src/app/shared/ERP/InvoiceInput.service';
import { WarehouseInputModalComponent } from '../warehouse-input-modal/warehouse-input-modal.component';

@Component({
  selector: 'app-warehouse-input',
  templateUrl: './warehouse-input.component.html',
  styleUrls: ['./warehouse-input.component.css']
})
export class WarehouseInputComponent {

  @ViewChild('WarehouseInputSort') WarehouseInputSort: MatSort;
  @ViewChild('WarehouseInputPaginator') WarehouseInputPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseInputService: WarehouseInputService,
    public InvoiceInputService: InvoiceInputService,
    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,

  ) {
    this.WarehouseInputService.BaseParameter.Page = -1;
    this.WarehouseInputService.BaseParameter.PageSize = 100;
    this.WarehouseInputService.BaseParameter.Action = 1;
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    //this.WarehouseInputSearch();
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.WarehouseInputService);    
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.WarehouseInputService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.WarehouseInputService);    
  }
  DateBegin(value) {
    this.WarehouseInputService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.WarehouseInputService.BaseParameter.DateEnd = new Date(value);
  }
  WarehouseInputSearch() {    
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputService.GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync().subscribe(
      res => {
        this.WarehouseInputService.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));
        this.WarehouseInputService.DataSource = new MatTableDataSource(this.WarehouseInputService.List);
        this.WarehouseInputService.DataSource.sort = this.WarehouseInputSort;
        this.WarehouseInputService.DataSource.paginator = this.WarehouseInputPaginator;
      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDelete(element: WarehouseInput) {
    this.WarehouseInputService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.WarehouseInputService.ComponentDelete(this.WarehouseInputSort, this.WarehouseInputPaginator));
  }
  WarehouseInputModal(element: WarehouseInput) {
    this.WarehouseInputService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseInputModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
      this.WarehouseInputSearch();
    });
  }
  WarehouseInputAdd() {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputService.BaseParameter.ID = environment.InitializationNumber;
    this.WarehouseInputService.GetByIDAsync().subscribe(
      res => {
        this.WarehouseInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseInputModal(this.WarehouseInputService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  InvoiceInputModal(element: WarehouseInput) {
    this.WarehouseInputService.IsShowLoading = true;
    this.InvoiceInputService.BaseParameter.ID = element.InvoiceInputID;
    this.InvoiceInputService.GetByIDAsync().subscribe(
      res => {
        this.InvoiceInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        dialogConfig.data = { ID: 0 };
        const dialog = this.Dialog.open(InvoiceInputModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => {
        });
      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  CreateAutoAsync() {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputService.SyncFromMESByCompanyID_CategoryDepartmentIDAsync().subscribe(
      res => {
        this.WarehouseInputSearch();
      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
}
