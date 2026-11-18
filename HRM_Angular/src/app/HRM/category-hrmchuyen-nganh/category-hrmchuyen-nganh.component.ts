import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryHRMChuyenNganhService } from 'src/app/shared/ERP/CategoryHRMChuyenNganh.service';
import { CategoryHRMChuyenNganh } from 'src/app/shared/ERP/CategoryHRMChuyenNganh.model';


@Component({
  selector: 'app-category-hrmchuyen-nganh',
  templateUrl: './category-hrmchuyen-nganh.component.html',
  styleUrls: ['./category-hrmchuyen-nganh.component.css']
})
export class CategoryHRMChuyenNganhComponent implements OnInit {

  @ViewChild('CategoryHRMChuyenNganhSort') CategoryHRMChuyenNganhSort: MatSort;
  @ViewChild('CategoryHRMChuyenNganhPaginator') CategoryHRMChuyenNganhPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryHRMChuyenNganhService: CategoryHRMChuyenNganhService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryHRMChuyenNganhSearch();
  }

  CategoryHRMChuyenNganhSearch() {
    this.CategoryHRMChuyenNganhService.SearchAll(this.CategoryHRMChuyenNganhSort, this.CategoryHRMChuyenNganhPaginator);
  }
  CategoryHRMChuyenNganhSave(element: CategoryHRMChuyenNganh) {
    this.CategoryHRMChuyenNganhService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryHRMChuyenNganhService.ComponentSaveAll(this.CategoryHRMChuyenNganhSort, this.CategoryHRMChuyenNganhPaginator));
  }
  CategoryHRMChuyenNganhDelete(element: CategoryHRMChuyenNganh) {
    this.CategoryHRMChuyenNganhService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryHRMChuyenNganhService.ComponentDeleteAll(this.CategoryHRMChuyenNganhSort, this.CategoryHRMChuyenNganhPaginator));
  }
}
