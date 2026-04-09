import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { ProductionOrder } from 'src/app/shared/ERP/ProductionOrder.model';
import { ProductionOrderService } from 'src/app/shared/ERP/ProductionOrder.service';

@Component({
  selector: 'app-production-order-admin',
  templateUrl: './production-order-admin.component.html',
  styleUrls: ['./production-order-admin.component.css']
})
export class ProductionOrderAdminComponent {

@ViewChild('ProductionOrderSort') ProductionOrderSort: MatSort;
  @ViewChild('ProductionOrderPaginator') ProductionOrderPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ProductionOrderService: ProductionOrderService,

  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.ProductionOrderSearch();
  }

  ProductionOrderSearch() {
    this.ProductionOrderService.IsShowLoading = true;
    this.ProductionOrderService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.ProductionOrderService.BaseParameter.Active = true;
    this.ProductionOrderService.BaseParameter.IsComplete = false;
    this.ProductionOrderService.GetByMembershipIDToListAsync().subscribe(
      res => {
        this.ProductionOrderService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder < b.SortOrder ? 1 : -1));
        this.ProductionOrderService.DataSource = new MatTableDataSource(this.ProductionOrderService.List);
        this.ProductionOrderService.DataSource.sort = this.ProductionOrderSort;
        this.ProductionOrderService.DataSource.paginator = this.ProductionOrderPaginator;
      },
      err => {
      },
      () => {
        this.ProductionOrderService.IsShowLoading = false;
      }
    );
  }
  ProductionOrderDelete(element: ProductionOrder) {
    this.ProductionOrderService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.ProductionOrderService.ComponentDelete(this.ProductionOrderSort, this.ProductionOrderPaginator));
  }

}
