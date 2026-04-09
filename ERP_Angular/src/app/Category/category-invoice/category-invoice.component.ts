import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryInvoice } from 'src/app/shared/ERP/CategoryInvoice.model';
import { CategoryInvoiceService } from 'src/app/shared/ERP/CategoryInvoice.service';

@Component({
  selector: 'app-category-invoice',
  templateUrl: './category-invoice.component.html',
  styleUrls: ['./category-invoice.component.css']
})
export class CategoryInvoiceComponent {

 @ViewChild('CategoryInvoiceSort') CategoryInvoiceSort: MatSort;
  @ViewChild('CategoryInvoicePaginator') CategoryInvoicePaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryInvoiceService: CategoryInvoiceService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryInvoiceSearch();
  }
  CategoryInvoiceSearch() {
    this.CategoryInvoiceService.SearchAll(this.CategoryInvoiceSort, this.CategoryInvoicePaginator);
  }
  CategoryInvoiceSave(element: CategoryInvoice) {
    this.CategoryInvoiceService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryInvoiceService.ComponentSaveAll(this.CategoryInvoiceSort, this.CategoryInvoicePaginator));
  }
  CategoryInvoiceDelete(element: CategoryInvoice) {
    this.CategoryInvoiceService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryInvoiceService.ComponentDeleteAll(this.CategoryInvoiceSort, this.CategoryInvoicePaginator));
  }
}
