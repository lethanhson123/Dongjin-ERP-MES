import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

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
  selector: 'app-warehouse-inventory-invoice',
  templateUrl: './warehouse-inventory-invoice.component.html',
  styleUrls: ['./warehouse-inventory-invoice.component.css']
})
export class WarehouseInventoryInvoiceComponent implements OnInit {

  @ViewChild('WarehouseInventorySort') WarehouseInventorySort: MatSort;
  @ViewChild('WarehouseInventoryPaginator') WarehouseInventoryPaginator: MatPaginator;

  URLTemplate: string = environment.APIRootURL + "Download/WarehouseInventory.xlsx";

  PageSize: number = environment.PageSize;

  Begin: number = 0;
  Input: number = 0;
  Output: number = 0;
  Stock: number = 0;
  End: number = 0;


  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseInventoryService: WarehouseInventoryService,
    public CompanyService: CompanyService,
    public MaterialService: MaterialService,
    public CategoryDepartmentService: CategoryDepartmentService,

  ) {
    this.WarehouseInventoryService.BaseParameter.Active = false;
    this.CompanySearch();
    this.ComponentGetYearList();
    this.ComponentGetMonthList();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {

  }
  FileChange(event, files: FileList) {
    if (files) {
      this.WarehouseInventoryService.FileToUpload = files;
      this.WarehouseInventoryService.BaseParameter.Event = event;
    }
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
    this.WarehouseInventoryService.IsShowLoading = true;
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.WarehouseInventoryService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.GetByMembershipID_CompanyID_ActiveToListAsync().subscribe(
      res => {
        this.CategoryDepartmentService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CategoryDepartmentService.ListFilter && this.CategoryDepartmentService.ListFilter.length > 0) {
          if (this.CategoryDepartmentService.BaseParameter.CompanyID == environment.CompanyIDDJG) {
            this.WarehouseInventoryService.BaseParameter.CategoryDepartmentID = environment.DepartmentID;
          }
          else {
            this.WarehouseInventoryService.BaseParameter.CategoryDepartmentID = this.CategoryDepartmentService.ListFilter[0].ID;
          }
        }
        //this.WarehouseInventorySearch();
      },
      err => {
      },
      () => {
        this.WarehouseInventoryService.IsShowLoading = false;
      }
    );
  }
  SaveAndUploadFile() {
    this.WarehouseInventoryService.IsShowLoading = true;
    this.WarehouseInventoryService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.WarehouseInventoryService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.WarehouseInventoryService.FileToUpload = null;
        this.WarehouseInventoryService.BaseParameter.Event = null;
        this.NotificationService.warn(environment.SaveSuccess);
        this.WarehouseInventorySearch();
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInventoryService.IsShowLoading = false;
      }
    );
  }
  WarehouseInventorySearch() {
    this.Begin = 0;
    this.Input = 0;
    this.Output = 0;
    this.Stock = 0;
    this.End = 0;
    this.WarehouseInventoryService.IsShowLoading = true;
    this.WarehouseInventoryService.BaseParameter.Action = 100;
    this.WarehouseInventoryService.GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndActionAndSearchStringToListAsync().subscribe(
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
              this.WarehouseInventoryService.BaseParameter.BaseModel.Input01 = "2026";
              this.WarehouseInventoryService.BaseParameter.BaseModel.Input02 = "2027";
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


        for (let i = 0; i < this.WarehouseInventoryService.List.length; i++) {
          this.Begin = this.Begin + this.WarehouseInventoryService.List[i].QuantityBegin;
          this.Input = this.Input + this.WarehouseInventoryService.List[i].QuantityInput00;
          this.Output = this.Output + this.WarehouseInventoryService.List[i].QuantityOutput00;
          this.Stock = this.Stock + this.WarehouseInventoryService.List[i].Quantity00;
          this.End = this.End + this.WarehouseInventoryService.List[i].QuantityEnd;
        }
      },
      err => {
      },
      () => {
        this.WarehouseInventoryService.IsShowLoading = false;
      }
    );

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
    this.WarehouseInventoryService.SyncInvoiceByCategoryDepartmentIDAndYearAndMonthAsync().subscribe(
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
  @ViewChild("TABLE") table: ElementRef;
  WarehouseInventoryExcel() {

    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");

    let ListCategoryDepartment = this.CategoryDepartmentService.ListFilter.filter(o => o.ID == this.WarehouseInventoryService.BaseParameter.CategoryDepartmentID);
    if (ListCategoryDepartment && ListCategoryDepartment.length > 0) {
      this.CategoryDepartmentService.BaseParameter.BaseModel = ListCategoryDepartment[0];
    }
    let filename = this.CategoryDepartmentService.BaseParameter.BaseModel.Code + "-" + this.WarehouseInventoryService.BaseParameter.Year + "-" + this.WarehouseInventoryService.BaseParameter.Month + "-WarehouseInventoryByInvoice.xlsx";
    XLSX.writeFile(wb, filename);

  }
}