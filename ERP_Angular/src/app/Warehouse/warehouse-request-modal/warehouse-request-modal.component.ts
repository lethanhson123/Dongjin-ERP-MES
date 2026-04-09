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

import { WarehouseRequest } from 'src/app/shared/ERP/WarehouseRequest.model';
import { WarehouseRequestService } from 'src/app/shared/ERP/WarehouseRequest.service';

import { WarehouseRequestDetail } from 'src/app/shared/ERP/WarehouseRequestDetail.model';
import { WarehouseRequestDetailService } from 'src/app/shared/ERP/WarehouseRequestDetail.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';


import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { CategoryUnit } from 'src/app/shared/ERP/CategoryUnit.model';
import { CategoryUnitService } from 'src/app/shared/ERP/CategoryUnit.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

@Component({
  selector: 'app-warehouse-request-modal',
  templateUrl: './warehouse-request-modal.component.html',
  styleUrls: ['./warehouse-request-modal.component.css']
})
export class WarehouseRequestModalComponent {

  @ViewChild('WarehouseRequestDetailSort') WarehouseRequestDetailSort: MatSort;
  @ViewChild('WarehouseRequestDetailPaginator') WarehouseRequestDetailPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<WarehouseRequestModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseRequestService: WarehouseRequestService,
    public WarehouseRequestDetailService: WarehouseRequestDetailService,

    public CategoryDepartmentService: CategoryDepartmentService,
    public MaterialService: MaterialService,
    public CategoryUnitService: CategoryUnitService,

 
  ) {
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    
    this.CategoryUnitSearch();
    this.MaterialSearch();
    this.CategoryDepartmentSearch();
    this.WarehouseRequestDetailSearch();
  }
  Close() {
    this.DialogRef.close();
  }
  
  CategoryUnitSearch() {
    this.CategoryUnitService.ComponentGetAllToListAsync(this.WarehouseRequestService);
  }
  MaterialSearch() {
    this.MaterialService.ComponentGetAllToListAsync(this.WarehouseRequestService);
  }
  MaterialFilter(searchString: string) {
    this.MaterialService.Filter(searchString);
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.ComponentGetAllToListAsync(this.WarehouseRequestService);
  }
  Date(value) {
    this.WarehouseRequestService.BaseParameter.BaseModel.Date = new Date(value);
  }
  WarehouseRequestFileNameChange(files: FileList) {
    if (files) {
      this.WarehouseRequestService.FileToUpload = files;
      this.WarehouseRequestService.File = files.item(0);
    }
  }
  Save() {
    this.WarehouseRequestService.IsShowLoading = true;
    this.WarehouseRequestService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.WarehouseRequestService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseRequestDetailSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseRequestService.IsShowLoading = false;
      }
    );
  }
  WarehouseRequestDetailSearch() {
    if (this.WarehouseRequestDetailService.BaseParameter.SearchString.length > 0) {
      this.WarehouseRequestDetailService.BaseParameter.SearchString = this.WarehouseRequestDetailService.BaseParameter.SearchString.trim();
      if (this.WarehouseRequestDetailService.DataSource) {
        this.WarehouseRequestDetailService.DataSource.filter = this.WarehouseRequestDetailService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseRequestDetailService.BaseParameter.ParentID = this.WarehouseRequestService.BaseParameter.BaseModel.ID;
      this.WarehouseRequestService.IsShowLoading = true;
      this.WarehouseRequestDetailService.GetByParentIDAndEmptyToListAsync().subscribe(
        res => {
          this.WarehouseRequestDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.WarehouseRequestDetailService.DataSource = new MatTableDataSource(this.WarehouseRequestDetailService.List);
          this.WarehouseRequestDetailService.DataSource.sort = this.WarehouseRequestDetailSort;
          this.WarehouseRequestDetailService.DataSource.paginator = this.WarehouseRequestDetailPaginator;
        },
        err => {
        },
        () => {
          this.WarehouseRequestService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseRequestDetailDelete(element: WarehouseRequestDetail) {
    this.WarehouseRequestService.IsShowLoading = true;
    this.WarehouseRequestDetailService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseRequestService.IsShowLoading = true;
      this.WarehouseRequestDetailService.RemoveAsync().subscribe(
        res => {
          this.WarehouseRequestDetailSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseRequestService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseRequestDetailSave(element: WarehouseRequestDetail) {
    this.WarehouseRequestService.IsShowLoading = true;
    element.ParentID = this.WarehouseRequestService.BaseParameter.BaseModel.ID;
    this.WarehouseRequestDetailService.BaseParameter.BaseModel = element;
    this.WarehouseRequestDetailService.SaveAsync().subscribe(
      res => {
        this.WarehouseRequestDetailSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseRequestService.IsShowLoading = false;
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
  MaterialModal(element: WarehouseRequestDetail) {
    this.WarehouseRequestService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = element.MaterialID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.WarehouseRequestService.IsShowLoading = false;
      }
    );
  }
  PlannedOrderModal() {
  }
}
