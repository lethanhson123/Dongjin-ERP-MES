import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { AnnualLeaveRequestService } from 'src/app/shared/RD/AnnualLeaveRequest.service';

import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { LanguageService } from 'src/app/shared/Language.service';

@Component({
  selector: 'app-annual-leave-request',
  templateUrl: './annual-leave-request.component.html',
  styleUrls: ['./annual-leave-request.component.css'],
})
export class AnnualLeaveRequestComponent implements OnInit {

  @ViewChild('AnnualLeaveRequestSort') AnnualLeaveRequestSort: MatSort;
  @ViewChild('AnnualLeaveRequestPaginator') AnnualLeaveRequestPaginator: MatPaginator;

  constructor(
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,
    public AnnualLeaveRequestService: AnnualLeaveRequestService,
    public CompanyService: CompanyService,
    public LanguageService: LanguageService,
  ) {
    this.AnnualLeaveRequestService.BaseParameter.IsOffice = undefined;
    const currentDate = new Date();
    this.AnnualLeaveRequestService.BaseParameter.DateBegin = new Date(
      currentDate.getFullYear(),
      currentDate.getMonth(),
      1,
    );
    this.CompanySearch();
  }

  ngOnInit(): void {}
  ngAfterViewInit() {}
  DateBegin(value) {
    this.AnnualLeaveRequestService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.AnnualLeaveRequestService.BaseParameter.DateEnd = new Date(value);
  }
  CompanySearch() {
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      (res) => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.AnnualLeaveRequestService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          this.AnnualLeaveRequestSearch();
        }
      },
      (err) => {},
      () => {},
    );
  }
  AnnualLeaveRequestSearch() {
    this.AnnualLeaveRequestService.IsShowLoading = true;
    this.AnnualLeaveRequestService.GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync().subscribe(
      (res) => {
        this.AnnualLeaveRequestService.List = (res as BaseResult).List;
        this.AnnualLeaveRequestService.DataSource = new MatTableDataSource(this.AnnualLeaveRequestService.List);
        this.AnnualLeaveRequestService.DataSource.sort = this.AnnualLeaveRequestSort;
        this.AnnualLeaveRequestService.DataSource.paginator = this.AnnualLeaveRequestPaginator;
      },
      (err) => {},
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
      excelDownload => this.DownloadExcelFile(excelDownload.Blob, excelDownload.FileName),
      () => {
        this.NotificationService.warn(
          this.LanguageService.TranslateService.instant('hrm-nghi-phep.requests.messages.exportFailed'),
        );
        this.AnnualLeaveRequestService.IsShowLoading = false;
      },
      () => {
        this.AnnualLeaveRequestService.IsShowLoading = false;
      },
    );
  }

  private DownloadExcelFile(fileBlob: Blob, fileName: string) {
    const downloadUrl = URL.createObjectURL(fileBlob);
    const downloadLink = document.createElement('a');
    downloadLink.href = downloadUrl;
    downloadLink.download = fileName;
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
    window.setTimeout(() => URL.revokeObjectURL(downloadUrl), 0);
  }
}
