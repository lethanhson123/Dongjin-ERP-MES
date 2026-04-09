import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryDelivery } from 'src/app/shared/ERP/CategoryDelivery.model';
import { CategoryDeliveryService } from 'src/app/shared/ERP/CategoryDelivery.service';

@Component({
  selector: 'app-category-delivery',
  templateUrl: './category-delivery.component.html',
  styleUrls: ['./category-delivery.component.css']
})
export class CategoryDeliveryComponent {

 @ViewChild('CategoryDeliverySort') CategoryDeliverySort: MatSort;
  @ViewChild('CategoryDeliveryPaginator') CategoryDeliveryPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryDeliveryService: CategoryDeliveryService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryDeliverySearch();
  }
  CategoryDeliverySearch() {
    this.CategoryDeliveryService.SearchAll(this.CategoryDeliverySort, this.CategoryDeliveryPaginator);
  }
  CategoryDeliverySave(element: CategoryDelivery) {
    this.CategoryDeliveryService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryDeliveryService.ComponentSaveAll(this.CategoryDeliverySort, this.CategoryDeliveryPaginator));
  }
  CategoryDeliveryDelete(element: CategoryDelivery) {
    this.CategoryDeliveryService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryDeliveryService.ComponentDeleteAll(this.CategoryDeliverySort, this.CategoryDeliveryPaginator));
  }
}
