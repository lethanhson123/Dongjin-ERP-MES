import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { AnnualLeaveRequest } from 'src/app/shared/RD/AnnualLeaveRequest.model';
import { AnnualLeaveRequestService } from 'src/app/shared/RD/AnnualLeaveRequest.service';
import { AnnualLeaveDetails } from 'src/app/shared/RD/AnnualLeaveDetails.model';
import { AnnualLeaveDetailsService } from 'src/app/shared/RD/AnnualLeaveDetails.service';
import { AnnualLeaveRequestModal001Component } from '../annual-leave-request-modal001/annual-leave-request-modal001.component';

import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-annual-leave-request001',
  templateUrl: './annual-leave-request001.component.html',
  styleUrls: ['./annual-leave-request001.component.css'],
})
export class AnnualLeaveRequest001Component implements OnInit {
  @ViewChild('AnnualLeaveRequestSort') AnnualLeaveRequestSort: MatSort;
  @ViewChild('AnnualLeaveRequestPaginator') AnnualLeaveRequestPaginator: MatPaginator;
  @ViewChild('AnnualLeaveRequestSummarySort') AnnualLeaveRequestSummarySort: MatSort;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public AnnualLeaveRequestService: AnnualLeaveRequestService,
    public AnnualLeaveDetailsService: AnnualLeaveDetailsService,
    public CompanyService: CompanyService,
  ) {
    this.AnnualLeaveRequestService.BaseParameter.SearchString = localStorage.getItem(environment.UserName);
    this.AnnualLeaveRequestService.BaseParameter.CompanyID = Number(localStorage.getItem(environment.CompanyID));
    this.AnnualLeaveRequestSearch();
  }

  ngOnInit(): void { }
  ngAfterViewInit() { }
  DateBegin(value) {
    this.AnnualLeaveDetailsService.BaseParameter.UserName = localStorage.getItem('username')
    this.AnnualLeaveRequestService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.AnnualLeaveRequestService.BaseParameter.DateEnd = new Date(value);
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.AnnualLeaveRequestService);
  }
  AnnualLeaveRequestSearch() {
    this.AnnualLeaveRequestService.IsShowLoading = true;
    this.AnnualLeaveRequestService.GetByDateBegin_DateEnd_SearchStringToListAsync().subscribe(
      (res) => {
        this.AnnualLeaveRequestService.List = (res as BaseResult).List;
        this.AnnualLeaveRequestService.ListSummary = (res as BaseResult).ListFilter as AnnualLeaveRequest[];
        this.AnnualLeaveRequestService.DataSourceSummary = new MatTableDataSource(this.AnnualLeaveRequestService.ListSummary);
        this.AnnualLeaveRequestService.DataSourceSummary.sort = this.AnnualLeaveRequestSummarySort;
        this.AnnualLeaveDetailsService.GetAllToListAsync().subscribe(
          (res) => {
            const details = (res as BaseResult).List as AnnualLeaveDetails[];
            this.AnnualLeaveRequestService.List.forEach((item) => {
              item.Details = details.filter((d) => d.AnnualLeaveRequestId == item.Id);
            });
            this.AnnualLeaveRequestService.DataSource = new MatTableDataSource(this.AnnualLeaveRequestService.List);
            this.AnnualLeaveRequestService.DataSource.sort = this.AnnualLeaveRequestSort;
            this.AnnualLeaveRequestService.DataSource.paginator = this.AnnualLeaveRequestPaginator;
          },
          (err) => {
            this.AnnualLeaveRequestService.DataSource = new MatTableDataSource(this.AnnualLeaveRequestService.List);
            this.AnnualLeaveRequestService.DataSource.sort = this.AnnualLeaveRequestSort;
            this.AnnualLeaveRequestService.DataSource.paginator = this.AnnualLeaveRequestPaginator;
          },
        );
      },
      (err) => { },
      () => {
        this.AnnualLeaveRequestService.IsShowLoading = false;
      },
    );
  }
  GetRadAnnualLeaveUrl(annualLeaveRequestId: number): string {
    return `${environment.RADWebURL}AnnualLeave/AnnualLeaveDetail/${annualLeaveRequestId}`;
  }

  AnnualLeaveRequestExcel() {
    this.AnnualLeaveRequestService.IsShowLoading = true;
    this.AnnualLeaveRequestService.GetByCompanyID_DateBegin_DateEnd_SearchStringToExcelAsync().subscribe(
      (res) => {
        let url = (res as BaseResult).Message;
        if (url) {
          window.open(url, '_blank');
        } else {
          this.NotificationService.warn(environment.SaveNotSuccess);
        }
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.AnnualLeaveRequestService.IsShowLoading = false;
      },
    );
  }

  AnnualLeaveRequest001Modal() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(AnnualLeaveRequestModal001Component, dialogConfig);
    dialog.afterClosed().subscribe(() => { });
  }


}
