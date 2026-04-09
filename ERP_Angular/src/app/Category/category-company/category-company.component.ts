import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryCompany } from 'src/app/shared/ERP/CategoryCompany.model';
import { CategoryCompanyService } from 'src/app/shared/ERP/CategoryCompany.service';

@Component({
  selector: 'app-category-company',
  templateUrl: './category-company.component.html',
  styleUrls: ['./category-company.component.css']
})
export class CategoryCompanyComponent {

  @ViewChild('CategoryCompanySort') CategoryCompanySort: MatSort;
  @ViewChild('CategoryCompanyPaginator') CategoryCompanyPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryCompanyService: CategoryCompanyService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryCompanySearch();
  }
  CategoryCompanySearch() {
    this.CategoryCompanyService.SearchAll(this.CategoryCompanySort, this.CategoryCompanyPaginator);
  }
  CategoryCompanySave(element: CategoryCompany) {
    this.CategoryCompanyService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryCompanyService.ComponentSaveAll(this.CategoryCompanySort, this.CategoryCompanyPaginator));
  }
  CategoryCompanyDelete(element: CategoryCompany) {
    this.CategoryCompanyService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryCompanyService.ComponentDeleteAll(this.CategoryCompanySort, this.CategoryCompanyPaginator));
  }
}
