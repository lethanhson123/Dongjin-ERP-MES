import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryFamily } from 'src/app/shared/ERP/CategoryFamily.model';
import { CategoryFamilyService } from 'src/app/shared/ERP/CategoryFamily.service';

@Component({
  selector: 'app-category-family',
  templateUrl: './category-family.component.html',
  styleUrls: ['./category-family.component.css']
})
export class CategoryFamilyComponent {

  @ViewChild('CategoryFamilySort') CategoryFamilySort: MatSort;
  @ViewChild('CategoryFamilyPaginator') CategoryFamilyPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryFamilyService: CategoryFamilyService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryFamilySearch();
  }
  CategoryFamilySearch() {
    this.CategoryFamilyService.SearchAll(this.CategoryFamilySort, this.CategoryFamilyPaginator);
  }
  CategoryFamilySave(element: CategoryFamily) {
    this.CategoryFamilyService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryFamilyService.ComponentSaveAll(this.CategoryFamilySort, this.CategoryFamilyPaginator));
  }
  CategoryFamilyDelete(element: CategoryFamily) {
    this.CategoryFamilyService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryFamilyService.ComponentDeleteAll(this.CategoryFamilySort, this.CategoryFamilyPaginator));
  }
}

