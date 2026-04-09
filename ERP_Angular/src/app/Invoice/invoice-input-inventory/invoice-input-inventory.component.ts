import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { InvoiceInputInventory } from 'src/app/shared/ERP/InvoiceInputInventory.model';
import { InvoiceInputInventoryService } from 'src/app/shared/ERP/InvoiceInputInventory.service';


import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

@Component({
  selector: 'app-invoice-input-inventory',
  templateUrl: './invoice-input-inventory.component.html',
  styleUrls: ['./invoice-input-inventory.component.css']
})
export class InvoiceInputInventoryComponent {

  @ViewChild('InvoiceInputInventorySort') InvoiceInputInventorySort: MatSort;
  @ViewChild('InvoiceInputInventoryPaginator') InvoiceInputInventoryPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public InvoiceInputInventoryService: InvoiceInputInventoryService,
    public MaterialService: MaterialService,

  ) {
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.InvoiceInputInventorySearch();
  }

  InvoiceInputInventorySearch() {
    if (this.InvoiceInputInventoryService.BaseParameter.SearchString.length > 0) {
      this.InvoiceInputInventoryService.BaseParameter.SearchString = this.InvoiceInputInventoryService.BaseParameter.SearchString.trim();
      this.InvoiceInputInventoryService.BaseParameter.SearchString.toLocaleLowerCase();
      if (this.InvoiceInputInventoryService.DataSource) {
        this.InvoiceInputInventoryService.DataSource.filter = this.InvoiceInputInventoryService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.InvoiceInputInventoryService.BaseParameter.CategoryDepartmentID = environment.DepartmentID;
      this.InvoiceInputInventoryService.BaseParameter.Year = environment.InitializationNumber;
      this.InvoiceInputInventoryService.BaseParameter.Month = environment.InitializationNumber;
      this.InvoiceInputInventoryService.IsShowLoading = true;
      this.InvoiceInputInventoryService.GetByCategoryDepartmentIDAndYearAndMonthToListAsync().subscribe(
        res => {
          this.InvoiceInputInventoryService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.InvoiceInputInventoryService.DataSource = new MatTableDataSource(this.InvoiceInputInventoryService.List);
          this.InvoiceInputInventoryService.DataSource.sort = this.InvoiceInputInventorySort;
          this.InvoiceInputInventoryService.DataSource.paginator = this.InvoiceInputInventoryPaginator;
        },
        err => {
        },
        () => {
          this.InvoiceInputInventoryService.IsShowLoading = false;
        }
      );
    }
  }
  MaterialModal(element: InvoiceInputInventory) {
    this.InvoiceInputInventoryService.IsShowLoading = true;
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
        this.InvoiceInputInventoryService.IsShowLoading = false;
      }
    );
  }
  CreateAutoAsync() {
    this.NotificationService.warn(environment.Synchronizing);
    this.InvoiceInputInventoryService.CreateAutoAsync().subscribe(
      res => {
        this.InvoiceInputInventorySearch();
      },
      err => {
      },
      () => {
      }
    );
  }
}
