import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseOutput } from 'src/app/shared/ERP/WarehouseOutput.model';
import { WarehouseOutputService } from 'src/app/shared/ERP/WarehouseOutput.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { WarehouseOutputModalComponent } from '../warehouse-output-modal/warehouse-output-modal.component';

@Component({
  selector: 'app-warehouse-output-production',
  templateUrl: './warehouse-output-production.component.html',
  styleUrls: ['./warehouse-output-production.component.css']
})
export class WarehouseOutputProductionComponent implements OnInit {

  @ViewChild('WarehouseOutputSort') WarehouseOutputSort: MatSort;
  @ViewChild('WarehouseOutputPaginator') WarehouseOutputPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseOutputService: WarehouseOutputService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,
  ) {
    this.WarehouseOutputService.BaseParameter.Action = 1;
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.WarehouseOutputService);
    this.CompanyChange();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.WarehouseOutputService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.WarehouseOutputService);
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.WarehouseOutputService);
  }
  WarehouseOutputSearch() {
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputService.GetByCompanyID_CategoryDepartmentID_Action_SearchStringToListAsync().subscribe(
      res => {
        this.WarehouseOutputService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder < b.SortOrder ? 1 : -1));
        this.WarehouseOutputService.DataSource = new MatTableDataSource(this.WarehouseOutputService.List);
        this.WarehouseOutputService.DataSource.sort = this.WarehouseOutputSort;
        this.WarehouseOutputService.DataSource.paginator = this.WarehouseOutputPaginator;
      },
      err => {
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputDelete(element: WarehouseOutput) {
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseOutputService.IsShowLoading = true;
      this.WarehouseOutputService.BaseParameter.ID = element.ID;
      this.WarehouseOutputService.RemoveAsync().subscribe(
        res => {
          this.WarehouseOutputSearch();
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
  WarehouseOutputModal(element: WarehouseOutput) {
    this.WarehouseOutputService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseOutputModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
      this.WarehouseOutputSearch();
    });
  }
  WarehouseOutputAdd() {
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputService.BaseParameter.ID = environment.InitializationNumber;
    this.WarehouseOutputService.GetByIDAsync().subscribe(
      res => {
        this.WarehouseOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseOutputModal(this.WarehouseOutputService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
   CreateAutoAsync() {
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputService.SyncFromMESByCompanyID_CategoryDepartmentIDAsync().subscribe(
      res => {
        this.WarehouseOutputSearch();
      },
      err => {
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
}