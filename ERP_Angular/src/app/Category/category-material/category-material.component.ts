import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryMaterial } from 'src/app/shared/ERP/CategoryMaterial.model';
import { CategoryMaterialService } from 'src/app/shared/ERP/CategoryMaterial.service';

@Component({
  selector: 'app-category-material',
  templateUrl: './category-material.component.html',
  styleUrls: ['./category-material.component.css']
})
export class CategoryMaterialComponent {

  @ViewChild('CategoryMaterialSort') CategoryMaterialSort: MatSort;
  @ViewChild('CategoryMaterialPaginator') CategoryMaterialPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryMaterialService: CategoryMaterialService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryMaterialSearch();
  }
  CategoryMaterialSearch() {
    this.CategoryMaterialService.SearchAll(this.CategoryMaterialSort, this.CategoryMaterialPaginator);
  }
  CategoryMaterialSave(element: CategoryMaterial) {
    this.CategoryMaterialService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryMaterialService.ComponentSaveAll(this.CategoryMaterialSort, this.CategoryMaterialPaginator));
  }
  CategoryMaterialDelete(element: CategoryMaterial) {
    this.CategoryMaterialService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryMaterialService.ComponentDeleteAll(this.CategoryMaterialSort, this.CategoryMaterialPaginator));
  }
}
