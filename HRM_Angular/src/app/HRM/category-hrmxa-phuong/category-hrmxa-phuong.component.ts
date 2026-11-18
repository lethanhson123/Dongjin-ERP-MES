import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryHRMXaPhuongService } from 'src/app/shared/ERP/CategoryHRMXaPhuong.service';
import { CategoryHRMXaPhuong } from 'src/app/shared/ERP/CategoryHRMXaPhuong.model';


@Component({
  selector: 'app-category-hrmxa-phuong',
  templateUrl: './category-hrmxa-phuong.component.html',
  styleUrls: ['./category-hrmxa-phuong.component.css']
})
export class CategoryHRMXaPhuongComponent implements OnInit {
@ViewChild('CategoryHRMXaPhuongSort') CategoryHRMXaPhuongSort: MatSort;
  @ViewChild('CategoryHRMXaPhuongPaginator') CategoryHRMXaPhuongPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryHRMXaPhuongService: CategoryHRMXaPhuongService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryHRMXaPhuongSearch();
  }

  CategoryHRMXaPhuongSearch() {
    this.CategoryHRMXaPhuongService.SearchAll(this.CategoryHRMXaPhuongSort, this.CategoryHRMXaPhuongPaginator);
  }
  CategoryHRMXaPhuongSave(element: CategoryHRMXaPhuong) {
    this.CategoryHRMXaPhuongService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryHRMXaPhuongService.ComponentSaveAll(this.CategoryHRMXaPhuongSort, this.CategoryHRMXaPhuongPaginator));
  }
  CategoryHRMXaPhuongDelete(element: CategoryHRMXaPhuong) {
    this.CategoryHRMXaPhuongService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryHRMXaPhuongService.ComponentDeleteAll(this.CategoryHRMXaPhuongSort, this.CategoryHRMXaPhuongPaginator));
  }
}
