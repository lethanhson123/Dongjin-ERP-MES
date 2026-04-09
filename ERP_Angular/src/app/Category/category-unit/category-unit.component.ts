import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryUnit } from 'src/app/shared/ERP/CategoryUnit.model';
import { CategoryUnitService } from 'src/app/shared/ERP/CategoryUnit.service';

@Component({
  selector: 'app-category-unit',
  templateUrl: './category-unit.component.html',
  styleUrls: ['./category-unit.component.css']
})
export class CategoryUnitComponent {

  @ViewChild('CategoryUnitSort') CategoryUnitSort: MatSort;
  @ViewChild('CategoryUnitPaginator') CategoryUnitPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryUnitService: CategoryUnitService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryUnitSearch();
  }
  CategoryUnitSearch() {
    this.CategoryUnitService.SearchAll(this.CategoryUnitSort, this.CategoryUnitPaginator);
  }
  CategoryUnitSave(element: CategoryUnit) {
    this.CategoryUnitService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryUnitService.ComponentSaveAll(this.CategoryUnitSort, this.CategoryUnitPaginator));
  }
  CategoryUnitDelete(element: CategoryUnit) {
    this.CategoryUnitService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryUnitService.ComponentDeleteAll(this.CategoryUnitSort, this.CategoryUnitPaginator));
  }
}
