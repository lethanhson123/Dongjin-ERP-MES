import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryPosition } from 'src/app/shared/ERP/CategoryPosition.model';
import { CategoryPositionService } from 'src/app/shared/ERP/CategoryPosition.service';

@Component({
  selector: 'app-category-position',
  templateUrl: './category-position.component.html',
  styleUrls: ['./category-position.component.css']
})
export class CategoryPositionComponent {

 @ViewChild('CategoryPositionSort') CategoryPositionSort: MatSort;
  @ViewChild('CategoryPositionPaginator') CategoryPositionPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryPositionService: CategoryPositionService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryPositionSearch();
  }
  CategoryPositionSearch() {
    this.CategoryPositionService.SearchAll(this.CategoryPositionSort, this.CategoryPositionPaginator);
  }
  CategoryPositionSave(element: CategoryPosition) {
    this.CategoryPositionService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryPositionService.ComponentSaveAll(this.CategoryPositionSort, this.CategoryPositionPaginator));
  }
  CategoryPositionDelete(element: CategoryPosition) {
    this.CategoryPositionService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryPositionService.ComponentDeleteAll(this.CategoryPositionSort, this.CategoryPositionPaginator));
  }
}
