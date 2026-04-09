import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryLevel } from 'src/app/shared/ERP/CategoryLevel.model';
import { CategoryLevelService } from 'src/app/shared/ERP/CategoryLevel.service';

@Component({
  selector: 'app-category-level',
  templateUrl: './category-level.component.html',
  styleUrls: ['./category-level.component.css']
})
export class CategoryLevelComponent implements OnInit {

@ViewChild('CategoryLevelSort') CategoryLevelSort: MatSort;
  @ViewChild('CategoryLevelPaginator') CategoryLevelPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryLevelService: CategoryLevelService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryLevelSearch();
  }
  CategoryLevelSearch() {
    this.CategoryLevelService.SearchAll(this.CategoryLevelSort, this.CategoryLevelPaginator);
  }
  CategoryLevelSave(element: CategoryLevel) {
    this.CategoryLevelService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryLevelService.ComponentSaveAll(this.CategoryLevelSort, this.CategoryLevelPaginator));
  }
  CategoryLevelDelete(element: CategoryLevel) {
    this.CategoryLevelService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryLevelService.ComponentDeleteAll(this.CategoryLevelSort, this.CategoryLevelPaginator));
  }
}
