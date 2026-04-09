import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategorySystem } from 'src/app/shared/ERP/CategorySystem.model';
import { CategorySystemService } from 'src/app/shared/ERP/CategorySystem.service';

@Component({
  selector: 'app-category-system',
  templateUrl: './category-system.component.html',
  styleUrls: ['./category-system.component.css']
})
export class CategorySystemComponent implements OnInit {

@ViewChild('CategorySystemSort') CategorySystemSort: MatSort;
  @ViewChild('CategorySystemPaginator') CategorySystemPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategorySystemService: CategorySystemService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategorySystemSearch();
  }
  CategorySystemSearch() {
    this.CategorySystemService.SearchAll(this.CategorySystemSort, this.CategorySystemPaginator);
  }
  CategorySystemSave(element: CategorySystem) {
    this.CategorySystemService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategorySystemService.ComponentSaveAll(this.CategorySystemSort, this.CategorySystemPaginator));
  }
  CategorySystemDelete(element: CategorySystem) {
    this.CategorySystemService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategorySystemService.ComponentDeleteAll(this.CategorySystemSort, this.CategorySystemPaginator));
  }
}