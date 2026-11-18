import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryHRMChucDanh } from 'src/app/shared/ERP/CategoryHRMChucDanh.model';
import { CategoryHRMChucDanhService } from 'src/app/shared/ERP/CategoryHRMChucDanh.service';

@Component({
  selector: 'app-category-hrmchuc-danh',
  templateUrl: './category-hrmchuc-danh.component.html',
  styleUrls: ['./category-hrmchuc-danh.component.css']
})
export class CategoryHRMChucDanhComponent implements OnInit {

 @ViewChild('CategoryHRMChucDanhSort') CategoryHRMChucDanhSort: MatSort;
  @ViewChild('CategoryHRMChucDanhPaginator') CategoryHRMChucDanhPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryHRMChucDanhService: CategoryHRMChucDanhService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryHRMChucDanhSearch();
  }

  CategoryHRMChucDanhSearch() {
    this.CategoryHRMChucDanhService.SearchAll(this.CategoryHRMChucDanhSort, this.CategoryHRMChucDanhPaginator);
  }
  CategoryHRMChucDanhSave(element: CategoryHRMChucDanh) {
    this.CategoryHRMChucDanhService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryHRMChucDanhService.ComponentSaveAll(this.CategoryHRMChucDanhSort, this.CategoryHRMChucDanhPaginator));
  }
  CategoryHRMChucDanhDelete(element: CategoryHRMChucDanh) {
    this.CategoryHRMChucDanhService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryHRMChucDanhService.ComponentDeleteAll(this.CategoryHRMChucDanhSort, this.CategoryHRMChucDanhPaginator));
  }
}
