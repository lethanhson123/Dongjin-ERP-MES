import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryMenu } from 'src/app/shared/ERP/CategoryMenu.model';
import { CategoryMenuService } from 'src/app/shared/ERP/CategoryMenu.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';


@Component({
  selector: 'app-category-menu',
  templateUrl: './category-menu.component.html',
  styleUrls: ['./category-menu.component.css']
})
export class CategoryMenuComponent {

  @ViewChild('CategoryMenuSort') CategoryMenuSort: MatSort;
  @ViewChild('CategoryMenuPaginator') CategoryMenuPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryMenuService: CategoryMenuService,

  ) {

  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryMenuSearch();    
  }  
  CategoryMenuSearch() {
    this.CategoryMenuService.SearchAll(this.CategoryMenuSort, this.CategoryMenuPaginator);
  }
  CategoryMenuSave(element: CategoryMenu) {
    this.CategoryMenuService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryMenuService.ComponentSaveAll(this.CategoryMenuSort, this.CategoryMenuPaginator));    
  }
  CategoryMenuDelete(element: CategoryMenu) {
    this.CategoryMenuService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryMenuService.ComponentDeleteAll(this.CategoryMenuSort, this.CategoryMenuPaginator));    
  }
}
