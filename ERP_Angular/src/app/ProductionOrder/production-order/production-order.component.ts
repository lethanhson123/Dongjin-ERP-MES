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

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-production-order',
  templateUrl: './production-order.component.html',
  styleUrls: ['./production-order.component.css']
})
export class ProductionOrderComponent {

  @ViewChild('ProductionOrderSort') ProductionOrderSort: MatSort;
  @ViewChild('ProductionOrderPaginator') ProductionOrderPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ProductionOrderService: ProductionOrderService,
    public CompanyService: CompanyService,

  ) { }

  ngOnInit(): void {
    this.CompanySearch();
    this.ComponentGetYearList();
    this.ComponentGetMonthList();
  }
  ngAfterViewInit() {
    this.ProductionOrderSearch();
  }
  DateBegin(value) {
    this.ProductionOrderService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.ProductionOrderService.BaseParameter.DateEnd = new Date(value);
  }
  ComponentGetYearList() {
    this.ProductionOrderService.ComponentGetYearList();
  }
  ComponentGetMonthList() {
    this.ProductionOrderService.ComponentGetMonthList();
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.ProductionOrderService);
  }
  ProductionOrderSearch() {
    this.ProductionOrderService.IsShowLoading = true;    
    this.ProductionOrderService.GetByCompanyID_DateBegin_DateEnd_SearchString_ActionToListAsync().subscribe(
      res => {
        this.ProductionOrderService.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));
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
