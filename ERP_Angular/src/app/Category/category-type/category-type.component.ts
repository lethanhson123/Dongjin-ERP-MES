import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryType } from 'src/app/shared/ERP/CategoryType.model';
import { CategoryTypeService } from 'src/app/shared/ERP/CategoryType.service';

@Component({
  selector: 'app-category-type',
  templateUrl: './category-type.component.html',
  styleUrls: ['./category-type.component.css']
})
export class CategoryTypeComponent implements OnInit {

 @ViewChild('CategoryTypeSort') CategoryTypeSort: MatSort;
  @ViewChild('CategoryTypePaginator') CategoryTypePaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryTypeService: CategoryTypeService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryTypeSearch();
  }
  CategoryTypeSearch() {
    this.CategoryTypeService.SearchAll(this.CategoryTypeSort, this.CategoryTypePaginator);
  }
  CategoryTypeSave(element: CategoryType) {
    this.CategoryTypeService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryTypeService.ComponentSaveAll(this.CategoryTypeSort, this.CategoryTypePaginator));
  }
  CategoryTypeDelete(element: CategoryType) {
    this.CategoryTypeService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryTypeService.ComponentDeleteAll(this.CategoryTypeSort, this.CategoryTypePaginator));
  }
}
