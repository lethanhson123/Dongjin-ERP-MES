import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryHRMQuocGiaService } from 'src/app/shared/ERP/CategoryHRMQuocGia.service';
import { CategoryHRMQuocGia } from 'src/app/shared/ERP/CategoryHRMQuocGia.model';


@Component({
  selector: 'app-category-hrm-quoc-tich',
  templateUrl: './category-hrmquoc-tich.component.html',
  styleUrls: ['./category-hrmquoc-tich.component.css']
})
export class CategoryHRMQuocTichComponent implements OnInit {
@ViewChild('CategoryHRMQuocGiaSort') CategoryHRMQuocGiaSort: MatSort;
  @ViewChild('CategoryHRMQuocGiaPaginator') CategoryHRMQuocGiaPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryHRMQuocGiaService: CategoryHRMQuocGiaService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryHRMQuocGiaSearch();
  }

  CategoryHRMQuocGiaSearch() {
    this.CategoryHRMQuocGiaService.SearchAll(this.CategoryHRMQuocGiaSort, this.CategoryHRMQuocGiaPaginator);
  }
  CategoryHRMQuocGiaSave(element: CategoryHRMQuocGia) {
    this.CategoryHRMQuocGiaService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryHRMQuocGiaService.ComponentSaveAll(this.CategoryHRMQuocGiaSort, this.CategoryHRMQuocGiaPaginator));
  }
  CategoryHRMQuocGiaDelete(element: CategoryHRMQuocGia) {
    this.CategoryHRMQuocGiaService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryHRMQuocGiaService.ComponentDeleteAll(this.CategoryHRMQuocGiaSort, this.CategoryHRMQuocGiaPaginator));
  }
}
