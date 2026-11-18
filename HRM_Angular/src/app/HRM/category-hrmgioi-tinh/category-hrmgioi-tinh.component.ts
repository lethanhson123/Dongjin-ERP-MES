import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryHRMGioiTinh } from 'src/app/shared/ERP/CategoryHRMGioiTinh.model';
import { CategoryHRMGioiTinhService } from 'src/app/shared/ERP/CategoryHRMGioiTinh.service';

@Component({
  selector: 'app-category-hrmgioi-tinh',
  templateUrl: './category-hrmgioi-tinh.component.html',
  styleUrls: ['./category-hrmgioi-tinh.component.css']
})
export class CategoryHRMGioiTinhComponent implements OnInit {

  @ViewChild('CategoryHRMGioiTinhSort') CategoryHRMGioiTinhSort: MatSort;
  @ViewChild('CategoryHRMGioiTinhPaginator') CategoryHRMGioiTinhPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryHRMGioiTinhService: CategoryHRMGioiTinhService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryHRMGioiTinhSearch();
  }

  CategoryHRMGioiTinhSearch() {
    this.CategoryHRMGioiTinhService.SearchAll(this.CategoryHRMGioiTinhSort, this.CategoryHRMGioiTinhPaginator);
  }
  CategoryHRMGioiTinhSave(element: CategoryHRMGioiTinh) {
    this.CategoryHRMGioiTinhService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryHRMGioiTinhService.ComponentSaveAll(this.CategoryHRMGioiTinhSort, this.CategoryHRMGioiTinhPaginator));
  }
  CategoryHRMGioiTinhDelete(element: CategoryHRMGioiTinh) {
    this.CategoryHRMGioiTinhService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryHRMGioiTinhService.ComponentDeleteAll(this.CategoryHRMGioiTinhSort, this.CategoryHRMGioiTinhPaginator));
  }
}
