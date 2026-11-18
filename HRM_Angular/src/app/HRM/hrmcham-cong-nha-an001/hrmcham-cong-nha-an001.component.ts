import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { HRMChamCongNhaAn } from 'src/app/shared/ERP/HRMChamCongNhaAn.model';
import { HRMChamCongNhaAnService } from 'src/app/shared/ERP/HRMChamCongNhaAn.service';

import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-hrmcham-cong-nha-an001',
  templateUrl: './hrmcham-cong-nha-an001.component.html',
  styleUrls: ['./hrmcham-cong-nha-an001.component.css']
})
export class HRMChamCongNhaAn001Component implements OnInit {

   @ViewChild('HRMChamCongNhaAnSort') HRMChamCongNhaAnSort: MatSort;
  @ViewChild('HRMChamCongNhaAnPaginator') HRMChamCongNhaAnPaginator: MatPaginator;

  @ViewChild('HRMChamCongNhaAnSortFilter') HRMChamCongNhaAnSortFilter: MatSort;
  @ViewChild('HRMChamCongNhaAnPaginatorFilter') HRMChamCongNhaAnPaginatorFilter: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,
    public HRMChamCongNhaAnService: HRMChamCongNhaAnService,
    public CompanyService: CompanyService,
  ) {
    this.HRMChamCongNhaAnService.BaseParameter.CompanyID = 16;
    this.CompanySearch();
  }

  ngOnInit(): void {}

  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.HRMChamCongNhaAnService);
  }

  ngAfterViewInit() {
    this.HRMChamCongNhaAnSearch();
  }
  DateBegin(value) {
    this.HRMChamCongNhaAnService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.HRMChamCongNhaAnService.BaseParameter.DateEnd = new Date(value);
  }
  HRMChamCongNhaAnSearch() {
    this.HRMChamCongNhaAnService.IsShowLoading = true;
    this.HRMChamCongNhaAnService.GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync().subscribe(
      (res) => {
        this.HRMChamCongNhaAnService.List = (res as BaseResult).List;
        this.HRMChamCongNhaAnService.DataSource = new MatTableDataSource(this.HRMChamCongNhaAnService.List);
        this.HRMChamCongNhaAnService.DataSource.sort = this.HRMChamCongNhaAnSort;
        this.HRMChamCongNhaAnService.DataSource.paginator = this.HRMChamCongNhaAnPaginator;

        this.HRMChamCongNhaAnService.ListFilter = (res as BaseResult).ListFilter;
        this.HRMChamCongNhaAnService.DataSourceFilter = new MatTableDataSource(this.HRMChamCongNhaAnService.ListFilter);
        this.HRMChamCongNhaAnService.DataSourceFilter.sort = this.HRMChamCongNhaAnSortFilter;
        this.HRMChamCongNhaAnService.DataSourceFilter.paginator = this.HRMChamCongNhaAnPaginatorFilter;
      },
      (err) => {},
      () => {
        this.HRMChamCongNhaAnService.IsShowLoading = false;
      },
    );
  }
  HRMChamCongNhaAnExcel() {
    this.HRMChamCongNhaAnService.IsShowLoading = true;
    this.HRMChamCongNhaAnService.GetByCompanyID_DateBegin_DateEnd_SearchStringToExcelAsync().subscribe(
      (res) => {
        let url = (res as BaseResult).Message;
        window.open(url, '_blank');
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMChamCongNhaAnService.IsShowLoading = false;
      },
    );
  }
}