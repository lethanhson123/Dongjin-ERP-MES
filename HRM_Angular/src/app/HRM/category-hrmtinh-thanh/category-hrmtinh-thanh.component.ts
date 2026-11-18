import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryHRMTinhThanhService } from 'src/app/shared/ERP/CategoryHRMTinhThanh.service';
import { CategoryHRMTinhThanh } from 'src/app/shared/ERP/CategoryHRMTinhThanh.model';


@Component({
  selector: 'app-category-hrmtinh-thanh',
  templateUrl: './category-hrmtinh-thanh.component.html',
  styleUrls: ['./category-hrmtinh-thanh.component.css']
})
export class CategoryHRMTinhThanhComponent implements OnInit {
@ViewChild('CategoryHRMTinhThanhSort') CategoryHRMTinhThanhSort: MatSort;
  @ViewChild('CategoryHRMTinhThanhPaginator') CategoryHRMTinhThanhPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryHRMTinhThanhService: CategoryHRMTinhThanhService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryHRMTinhThanhSearch();
  }

  CategoryHRMTinhThanhSearch() {
    this.CategoryHRMTinhThanhService.SearchAll(this.CategoryHRMTinhThanhSort, this.CategoryHRMTinhThanhPaginator);
  }
  CategoryHRMTinhThanhSave(element: CategoryHRMTinhThanh) {
    this.CategoryHRMTinhThanhService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryHRMTinhThanhService.ComponentSaveAll(this.CategoryHRMTinhThanhSort, this.CategoryHRMTinhThanhPaginator));
  }
  CategoryHRMTinhThanhDelete(element: CategoryHRMTinhThanh) {
    this.CategoryHRMTinhThanhService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryHRMTinhThanhService.ComponentDeleteAll(this.CategoryHRMTinhThanhSort, this.CategoryHRMTinhThanhPaginator));
  }
}
