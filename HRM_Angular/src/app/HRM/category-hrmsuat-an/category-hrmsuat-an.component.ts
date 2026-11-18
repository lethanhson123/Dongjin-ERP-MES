import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryHRMSuatAn } from 'src/app/shared/ERP/CategoryHRMSuatAn.model';
import { CategoryHRMSuatAnService } from 'src/app/shared/ERP/CategoryHRMSuatAn.service';


@Component({
  selector: 'app-category-hrmsuat-an',
  templateUrl: './category-hrmsuat-an.component.html',
  styleUrls: ['./category-hrmsuat-an.component.css']
})
export class CategoryHRMSuatAnComponent implements OnInit {

  @ViewChild('CategoryHRMSuatAnSort') CategoryHRMSuatAnSort: MatSort;
  @ViewChild('CategoryHRMSuatAnPaginator') CategoryHRMSuatAnPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,
    public CategoryHRMSuatAnService: CategoryHRMSuatAnService,
  ) { }

  ngOnInit(): void {
  }


  ngAfterViewInit() {
    this.CategoryHRMSuatAnSearch();
  }
  CategoryHRMSuatAnSearch() {
    this.CategoryHRMSuatAnService.SearchAll(this.CategoryHRMSuatAnSort, this.CategoryHRMSuatAnPaginator);
  }
  CategoryHRMSuatAnSave(element: CategoryHRMSuatAn) {
    this.CategoryHRMSuatAnService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryHRMSuatAnService.ComponentSaveAll(this.CategoryHRMSuatAnSort, this.CategoryHRMSuatAnPaginator));
  }
  CategoryHRMSuatAnDelete(element: CategoryHRMSuatAn) {
    this.CategoryHRMSuatAnService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryHRMSuatAnService.ComponentDeleteAll(this.CategoryHRMSuatAnSort, this.CategoryHRMSuatAnPaginator));
  }
}
