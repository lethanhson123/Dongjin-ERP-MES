import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryLayer } from 'src/app/shared/ERP/CategoryLayer.model';
import { CategoryLayerService } from 'src/app/shared/ERP/CategoryLayer.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-category-layer',
  templateUrl: './category-layer.component.html',
  styleUrls: ['./category-layer.component.css']
})
export class CategoryLayerComponent {

  @ViewChild('CategoryLayerSort') CategoryLayerSort: MatSort;
  @ViewChild('CategoryLayerPaginator') CategoryLayerPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryLayerService: CategoryLayerService,
    public CompanyService: CompanyService,
  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryLayerSearch();
  }
  CompanySearch() {
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.ComponentGetByActiveToListAsync(this.CategoryLayerService);
  }
  CategoryLayerSearch() {
    this.CategoryLayerService.SearchAll(this.CategoryLayerSort, this.CategoryLayerPaginator);
  }
  CategoryLayerSave(element: CategoryLayer) {
    this.CategoryLayerService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryLayerService.ComponentSaveAll(this.CategoryLayerSort, this.CategoryLayerPaginator));
  }
  CategoryLayerDelete(element: CategoryLayer) {
    this.CategoryLayerService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryLayerService.ComponentDeleteAll(this.CategoryLayerSort, this.CategoryLayerPaginator));
  }

}