import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryHRMDanTocService } from 'src/app/shared/ERP/CategoryHRMDanToc.service';
import { CategoryHRMDanToc } from 'src/app/shared/ERP/CategoryHRMDanToc.model';


@Component({
  selector: 'app-category-hrmdan-toc',
  templateUrl: './category-hrmdan-toc.component.html',
  styleUrls: ['./category-hrmdan-toc.component.css']
})
export class CategoryHRMDanTocComponent implements OnInit {
@ViewChild('CategoryHRMDanTocSort') CategoryHRMDanTocSort: MatSort;
  @ViewChild('CategoryHRMDanTocPaginator') CategoryHRMDanTocPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryHRMDanTocService: CategoryHRMDanTocService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryHRMDanTocSearch();
  }

  CategoryHRMDanTocSearch() {
    this.CategoryHRMDanTocService.SearchAll(this.CategoryHRMDanTocSort, this.CategoryHRMDanTocPaginator);
  }
  CategoryHRMDanTocSave(element: CategoryHRMDanToc) {
    this.CategoryHRMDanTocService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryHRMDanTocService.ComponentSaveAll(this.CategoryHRMDanTocSort, this.CategoryHRMDanTocPaginator));
  }
  CategoryHRMDanTocDelete(element: CategoryHRMDanToc) {
    this.CategoryHRMDanTocService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryHRMDanTocService.ComponentDeleteAll(this.CategoryHRMDanTocSort, this.CategoryHRMDanTocPaginator));
  }
}
