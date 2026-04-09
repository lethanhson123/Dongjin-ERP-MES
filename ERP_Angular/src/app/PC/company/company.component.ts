import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { CategoryCompany } from 'src/app/shared/ERP/CategoryCompany.model';
import { CategoryCompanyService } from 'src/app/shared/ERP/CategoryCompany.service';

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.css']
})
export class CompanyComponent {

@ViewChild('CompanySort') CompanySort: MatSort;
  @ViewChild('CompanyPaginator') CompanyPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CompanyService: CompanyService,
    public CategoryCompanyService: CategoryCompanyService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryCompanySearch();
    this.CompanySearch();
  }
   CategoryCompanySearch() {
    this.CategoryCompanyService.ComponentGetAllToListAsync(this.CompanyService);
  }
  CompanySearch() {
    this.CompanyService.SearchAll(this.CompanySort, this.CompanyPaginator);
  }
  CompanySave(element: Company) {
    this.CompanyService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CompanyService.ComponentSaveAll(this.CompanySort, this.CompanyPaginator));
  }
  CompanyDelete(element: Company) {
    this.CompanyService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CompanyService.ComponentDeleteAll(this.CompanySort, this.CompanyPaginator));
  }
}
