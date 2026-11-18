import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryHRMTrinhDoService } from 'src/app/shared/ERP/CategoryHRMTrinhDo.service';
import { CategoryHRMTrinhDo } from 'src/app/shared/ERP/CategoryHRMTrinhDo.model';


@Component({
  selector: 'app-category-hrmtrinh-do',
  templateUrl: './category-hrmtrinh-do.component.html',
  styleUrls: ['./category-hrmtrinh-do.component.css']
})
export class CategoryHRMTrinhDoComponent implements OnInit {

  @ViewChild('CategoryHRMTrinhDoSort') CategoryHRMTrinhDoSort: MatSort;
  @ViewChild('CategoryHRMTrinhDoPaginator') CategoryHRMTrinhDoPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryHRMTrinhDoService: CategoryHRMTrinhDoService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryHRMTrinhDoSearch();
  }

  CategoryHRMTrinhDoSearch() {
    this.CategoryHRMTrinhDoService.SearchAll(this.CategoryHRMTrinhDoSort, this.CategoryHRMTrinhDoPaginator);
  }
  CategoryHRMTrinhDoSave(element: CategoryHRMTrinhDo) {
    this.CategoryHRMTrinhDoService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryHRMTrinhDoService.ComponentSaveAll(this.CategoryHRMTrinhDoSort, this.CategoryHRMTrinhDoPaginator));
  }
  CategoryHRMTrinhDoDelete(element: CategoryHRMTrinhDo) {
    this.CategoryHRMTrinhDoService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryHRMTrinhDoService.ComponentDeleteAll(this.CategoryHRMTrinhDoSort, this.CategoryHRMTrinhDoPaginator));
  }
}
