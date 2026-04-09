import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryConfig } from 'src/app/shared/ERP/CategoryConfig.model';
import { CategoryConfigService } from 'src/app/shared/ERP/CategoryConfig.service';

@Component({
  selector: 'app-category-config',
  templateUrl: './category-config.component.html',
  styleUrls: ['./category-config.component.css']
})
export class CategoryConfigComponent implements OnInit {

@ViewChild('CategoryConfigSort') CategoryConfigSort: MatSort;
  @ViewChild('CategoryConfigPaginator') CategoryConfigPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryConfigService: CategoryConfigService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryConfigSearch();
  }
  CategoryConfigSearch() {
    this.CategoryConfigService.SearchAll(this.CategoryConfigSort, this.CategoryConfigPaginator);
  }
  CategoryConfigSave(element: CategoryConfig) {
    this.CategoryConfigService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryConfigService.ComponentSaveAll(this.CategoryConfigSort, this.CategoryConfigPaginator));
  }
  CategoryConfigDelete(element: CategoryConfig) {
    this.CategoryConfigService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryConfigService.ComponentDeleteAll(this.CategoryConfigSort, this.CategoryConfigPaginator));
  }
}

