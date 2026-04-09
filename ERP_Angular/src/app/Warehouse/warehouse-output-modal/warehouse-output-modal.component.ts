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

import { WarehouseOutput } from 'src/app/shared/ERP/WarehouseOutput.model';
import { WarehouseOutputService } from 'src/app/shared/ERP/WarehouseOutput.service';

import { WarehouseOutputDetail } from 'src/app/shared/ERP/WarehouseOutputDetail.model';
import { WarehouseOutputDetailService } from 'src/app/shared/ERP/WarehouseOutputDetail.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';


import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';
import { CategoryUnit } from 'src/app/shared/ERP/CategoryUnit.model';
import { CategoryUnitService } from 'src/app/shared/ERP/CategoryUnit.service';

import { WarehouseRequest } from 'src/app/shared/ERP/WarehouseRequest.model';
import { WarehouseRequestService } from 'src/app/shared/ERP/WarehouseRequest.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

@Component({
  selector: 'app-warehouse-output-modal',
  templateUrl: './warehouse-output-modal.component.html',
  styleUrls: ['./warehouse-output-modal.component.css']
})
export class WarehouseOutputModalComponent {

  @ViewChild('WarehouseOutputDetailSort') WarehouseOutputDetailSort: MatSort;
  @ViewChild('WarehouseOutputDetailPaginator') WarehouseOutputDetailPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<WarehouseOutputModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseOutputService: WarehouseOutputService,
    public WarehouseOutputDetailService: WarehouseOutputDetailService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public MaterialService: MaterialService,
    public CategoryUnitService: CategoryUnitService,
    public WarehouseRequestService: WarehouseRequestService,
  ) {
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.WarehouseRequestSearch();
    this.CategoryUnitSearch();
    this.MaterialSearch();
    this.CategoryDepartmentSearch();
    this.WarehouseOutputDetailSearch();
  }
  Close() {
    this.DialogRef.close();
  }
  WarehouseRequestSearch() {
    this.WarehouseRequestService.ComponentGetAllToListAsync(this.WarehouseOutputService);
  }
  CategoryUnitSearch() {
    this.CategoryUnitService.ComponentGetAllToListAsync(this.WarehouseOutputService);
  }
  MaterialSearch() {
    this.MaterialService.ComponentGetAllToListAsync(this.WarehouseOutputService);
  }
  MaterialFilter(searchString: string) {
    this.MaterialService.Filter(searchString);
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.ComponentGetByActiveToListAsync(this.WarehouseOutputService);
  }
  Date(value) {
    this.WarehouseOutputService.BaseParameter.BaseModel.Date = new Date(value);
  }
  WarehouseOutputFileNameChange(files: FileList) {
    if (files) {
      this.WarehouseOutputService.FileToUpload = files;
      this.WarehouseOutputService.File = files.item(0);
    }
  }
  Save() {
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputService.SaveAsync().subscribe(
      res => {
        this.WarehouseOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseOutputDetailSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputDetailSearch() {
    if (this.WarehouseOutputDetailService.BaseParameter.SearchString.length > 0) {
      this.WarehouseOutputDetailService.BaseParameter.SearchString = this.WarehouseOutputDetailService.BaseParameter.SearchString.trim();
      if (this.WarehouseOutputDetailService.DataSource) {
        this.WarehouseOutputDetailService.DataSource.filter = this.WarehouseOutputDetailService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseOutputDetailService.BaseParameter.ParentID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
      this.WarehouseOutputService.IsShowLoading = true;
      this.WarehouseOutputDetailService.GetByParentIDAndEmptyToListAsync().subscribe(
        res => {
          this.WarehouseOutputDetailService.List = (res as BaseResult).List.sort((a, b) => (a.Quantity < b.Quantity ? 1 : -1));
          this.WarehouseOutputDetailService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailService.List);
          this.WarehouseOutputDetailService.DataSource.sort = this.WarehouseOutputDetailSort;
          this.WarehouseOutputDetailService.DataSource.paginator = this.WarehouseOutputDetailPaginator;
        },
        err => {
        },
        () => {
          this.WarehouseOutputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseOutputDetailDelete(element: WarehouseOutputDetail) {
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputDetailService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseOutputService.IsShowLoading = true;
      this.WarehouseOutputDetailService.RemoveAsync().subscribe(
        res => {
          this.WarehouseOutputDetailSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseOutputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseOutputDetailSave(element: WarehouseOutputDetail) {
    this.WarehouseOutputService.IsShowLoading = true;
    element.ParentID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
    this.WarehouseOutputDetailService.BaseParameter.BaseModel = element;
    this.WarehouseOutputDetailService.SaveAsync().subscribe(
      res => {
        this.WarehouseOutputDetailSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
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
  MaterialModal(element: WarehouseOutputDetail) {
    this.WarehouseOutputService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = element.MaterialID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  MaterialChange(element: WarehouseOutputDetail) {
    let List = this.MaterialService.ListFilter.filter(item => item.ID == element.MaterialID);
    if (List) {
      if (List.length > 0) {
        element.QuantityInvoice = List[0].Quantity;
      }
    }
  }
}
