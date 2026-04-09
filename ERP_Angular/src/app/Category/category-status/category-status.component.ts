import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryStatus } from 'src/app/shared/ERP/CategoryStatus.model';
import { CategoryStatusService } from 'src/app/shared/ERP/CategoryStatus.service';

@Component({
  selector: 'app-category-status',
  templateUrl: './category-status.component.html',
  styleUrls: ['./category-status.component.css']
})
export class CategoryStatusComponent implements OnInit {

@ViewChild('CategoryStatusSort') CategoryStatusSort: MatSort;
  @ViewChild('CategoryStatusPaginator') CategoryStatusPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryStatusService: CategoryStatusService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryStatusSearch();
  }
  CategoryStatusSearch() {
    this.CategoryStatusService.SearchAll(this.CategoryStatusSort, this.CategoryStatusPaginator);
  }
  CategoryStatusSave(element: CategoryStatus) {
    this.CategoryStatusService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryStatusService.ComponentSaveAll(this.CategoryStatusSort, this.CategoryStatusPaginator));
  }
  CategoryStatusDelete(element: CategoryStatus) {
    this.CategoryStatusService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryStatusService.ComponentDeleteAll(this.CategoryStatusSort, this.CategoryStatusPaginator));
  }
}