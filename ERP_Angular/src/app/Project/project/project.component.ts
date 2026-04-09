import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Project } from 'src/app/shared/ERP/Project.model';
import { ProjectService } from 'src/app/shared/ERP/Project.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {

  @ViewChild('ProjectSort') ProjectSort: MatSort;
  @ViewChild('ProjectPaginator') ProjectPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ProjectService: ProjectService,
    public CompanyService: CompanyService,

  ) { }

  ngOnInit(): void {
    this.CompanySearch();
    this.ComponentGetYearList();
    this.ComponentGetMonthList();
  }
  ngAfterViewInit() {
    this.ProjectSearch();
  }
  DateBegin(value) {
    this.ProjectService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.ProjectService.BaseParameter.DateEnd = new Date(value);
  }
  ComponentGetYearList() {
    this.ProjectService.ComponentGetYearList();
  }
  ComponentGetMonthList() {
    this.ProjectService.ComponentGetMonthList();
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.ProjectService);
  }
  ProjectSearch() {
    this.ProjectService.IsShowLoading = true;    
    this.ProjectService.GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync().subscribe(
      res => {
        this.ProjectService.List = (res as BaseResult).List.sort((a, b) => (a.DateBegin < b.DateBegin ? 1 : -1));
        this.ProjectService.DataSource = new MatTableDataSource(this.ProjectService.List);
        this.ProjectService.DataSource.sort = this.ProjectSort;
        this.ProjectService.DataSource.paginator = this.ProjectPaginator;
      },
      err => {
      },
      () => {
        this.ProjectService.IsShowLoading = false;
      }
    );
  }
  ProjectDelete(element: Project) {
    this.ProjectService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.ProjectService.ComponentDelete(this.ProjectSort, this.ProjectPaginator));
  }

}