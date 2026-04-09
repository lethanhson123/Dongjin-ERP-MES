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

import { WarehouseInput } from 'src/app/shared/ERP/WarehouseInput.model';
import { WarehouseInputService } from 'src/app/shared/ERP/WarehouseInput.service';

import { WarehouseInputDetail } from 'src/app/shared/ERP/WarehouseInputDetail.model';
import { WarehouseInputDetailService } from 'src/app/shared/ERP/WarehouseInputDetail.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';


import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { InvoiceInput } from 'src/app/shared/ERP/InvoiceInput.model';
import { InvoiceInputService } from 'src/app/shared/ERP/InvoiceInput.service';

import { CategoryUnit } from 'src/app/shared/ERP/CategoryUnit.model';
import { CategoryUnitService } from 'src/app/shared/ERP/CategoryUnit.service';
import { InvoiceInputModalComponent } from 'src/app/Invoice/invoice-input-modal/invoice-input-modal.component';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

@Component({
  selector: 'app-warehouse-input-modal',
  templateUrl: './warehouse-input-modal.component.html',
  styleUrls: ['./warehouse-input-modal.component.css']
})
export class WarehouseInputModalComponent {

  @ViewChild('WarehouseInputDetailSort') WarehouseInputDetailSort: MatSort;
  @ViewChild('WarehouseInputDetailPaginator') WarehouseInputDetailPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<WarehouseInputModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseInputService: WarehouseInputService,
    public WarehouseInputDetailService: WarehouseInputDetailService,

    public CategoryDepartmentService: CategoryDepartmentService,
    public MaterialService: MaterialService,
    public CategoryUnitService: CategoryUnitService,

    public InvoiceInputService: InvoiceInputService,
  ) {
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.InvoiceInputSearch();
    this.CategoryUnitSearch();
    this.MaterialSearch();
    this.CategoryDepartmentSearch();
    this.WarehouseInputDetailSearch();
  }
  Close() {
    this.DialogRef.close();
  }
  InvoiceInputSearch() {
    this.InvoiceInputService.ComponentGetAllToListAsync(this.WarehouseInputService);
  }
  CategoryUnitSearch() {
    this.CategoryUnitService.ComponentGetAllToListAsync(this.WarehouseInputService);
  }
  MaterialSearch() {
    this.MaterialService.ComponentGetAllToListAsync(this.WarehouseInputService);
  }
  MaterialFilter(searchString: string) {
    this.MaterialService.Filter(searchString);
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.ComponentGetByActiveToListAsync(this.WarehouseInputService);
  }
  Date(value) {
    this.WarehouseInputService.BaseParameter.BaseModel.Date = new Date(value);
  }
  WarehouseInputFileNameChange(files: FileList) {
    if (files) {
      this.WarehouseInputService.FileToUpload = files;
      this.WarehouseInputService.File = files.item(0);
    }
  }
  Save() {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.WarehouseInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseInputDetailSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailSearch() {
    if (this.WarehouseInputDetailService.BaseParameter.SearchString.length > 0) {
      this.WarehouseInputDetailService.BaseParameter.SearchString = this.WarehouseInputDetailService.BaseParameter.SearchString.trim();
      if (this.WarehouseInputDetailService.DataSource) {
        this.WarehouseInputDetailService.DataSource.filter = this.WarehouseInputDetailService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseInputDetailService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
      this.WarehouseInputService.IsShowLoading = true;
      this.WarehouseInputDetailService.GetByParentIDAndEmptyToListAsync().subscribe(
        res => {
          this.WarehouseInputDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.WarehouseInputDetailService.DataSource = new MatTableDataSource(this.WarehouseInputDetailService.List);
          this.WarehouseInputDetailService.DataSource.sort = this.WarehouseInputDetailSort;
          this.WarehouseInputDetailService.DataSource.paginator = this.WarehouseInputDetailPaginator;
        },
        err => {
        },
        () => {
          this.WarehouseInputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputDetailDelete(element: WarehouseInputDetail) {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseInputService.IsShowLoading = true;
      this.WarehouseInputDetailService.RemoveAsync().subscribe(
        res => {
          this.WarehouseInputDetailSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseInputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputDetailSave(element: WarehouseInputDetail) {
    this.WarehouseInputService.IsShowLoading = true;
    element.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputDetailService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailService.SaveAsync().subscribe(
      res => {
        this.WarehouseInputDetailSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  MaterialModalOpen(element: Material) {
    this.MaterialService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(MaterialModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {

    });
  }
  MaterialModal(element: WarehouseInputDetail) {
    this.WarehouseInputService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = element.MaterialID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  InvoiceInputModal() {
    this.WarehouseInputService.IsShowLoading = true;
    this.InvoiceInputService.BaseParameter.ID = this.WarehouseInputService.BaseParameter.BaseModel.InvoiceInputID;
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
}
