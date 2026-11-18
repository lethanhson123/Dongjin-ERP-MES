import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryHRMCaLamViecService } from 'src/app/shared/ERP/CategoryHRMCaLamViec.service';
import { CategoryHRMCaLamViec } from 'src/app/shared/ERP/CategoryHRMCaLamViec.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { Company } from 'src/app/shared/ERP/Company.model';


@Component({
  selector: 'app-category-hrm-ca-lam-viec',
  templateUrl: './category-hrmca-lam-viec.component.html',
  styleUrls: ['./category-hrmca-lam-viec.component.css']
})
export class CategoryHRMCaLamViecComponent implements OnInit {
@ViewChild('CategoryHRMCaLamViecSort') CategoryHRMCaLamViecSort: MatSort;
  @ViewChild('CategoryHRMCaLamViecPaginator') CategoryHRMCaLamViecPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryHRMCaLamViecService: CategoryHRMCaLamViecService,
    public CompanyService: CompanyService,
  ) {
    this.CategoryHRMCaLamViecService.BaseParameter.CompanyID = 16;
    this.CompanyService.ComponentGetByActiveToListAsync(this.CategoryHRMCaLamViecService);
  }

  OfficeTab: boolean = false;

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryHRMCaLamViecSearch();
  }

  CategoryHRMCaLamViecSearch() {
    this.CategoryHRMCaLamViecService.SearchAllByOffice(
      this.CategoryHRMCaLamViecSort,
      this.CategoryHRMCaLamViecPaginator,
      this.OfficeTab,
    );
  }

  CompanyChange() {
    this.CategoryHRMCaLamViecSearch();
  }

  SetOfficeTab(isOffice: boolean) {
    this.OfficeTab = isOffice;
    this.CategoryHRMCaLamViecService.BaseParameter.SearchString = '';
    this.CategoryHRMCaLamViecSearch();
  }

  CategoryHRMCaLamViecSave(element: CategoryHRMCaLamViec) {
    this.CategoryHRMCaLamViecService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryHRMCaLamViecService.ComponentSaveAll(this.CategoryHRMCaLamViecSort, this.CategoryHRMCaLamViecPaginator));
  }
  CategoryHRMCaLamViecDelete(element: CategoryHRMCaLamViec) {
    this.CategoryHRMCaLamViecService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryHRMCaLamViecService.ComponentDeleteAll(this.CategoryHRMCaLamViecSort, this.CategoryHRMCaLamViecPaginator));
  }
}
