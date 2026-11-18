import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { DownloadService } from 'src/app/shared/Download.service';

import { HRMNhaAnThucDon } from 'src/app/shared/ERP/HRMNhaAnThucDon.model';
import { HRMNhaAnThucDonService } from 'src/app/shared/ERP/HRMNhaAnThucDon.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-hrmnha-an-thuc-don',
  templateUrl: './hrmnha-an-thuc-don.component.html',
  styleUrls: ['./hrmnha-an-thuc-don.component.css'],
})
export class HRMNhaAnThucDonComponent implements OnInit {
  @ViewChild('HRMNhaAnThucDonSort') HRMNhaAnThucDonSort: MatSort;
  @ViewChild('HRMNhaAnThucDonPaginator') HRMNhaAnThucDonPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public HRMNhaAnThucDonService: HRMNhaAnThucDonService,
    public CompanyService: CompanyService,
  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {}
  ngAfterViewInit() {}

  CompanySearch() {
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      (res) => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.HRMNhaAnThucDonService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          this.HRMNhaAnThucDonSearch();
        }
      },
      (err) => {},
      () => {},
    );
  }
  HRMNhaAnThucDonUpdateDate(value, element: HRMNhaAnThucDon) {
    element.UpdateDate = new Date(value);
  }
  HRMNhaAnThucDonFileChange(event, files: FileList) {
    if (files) {
      this.HRMNhaAnThucDonService.FileToUpload = files;
      this.HRMNhaAnThucDonService.BaseParameter.Event = event;
    }
  }
  HRMNhaAnThucDonSearch() {
    this.HRMNhaAnThucDonService.IsShowLoading = true;
    this.HRMNhaAnThucDonService.GetByCompanyIDAndEmptyToListAsync().subscribe(
      (res) => {
        this.HRMNhaAnThucDonService.List = (res as BaseResult).List.sort((a, b) => (a.UpdateDate < b.UpdateDate ? 1 : -1));
        this.HRMNhaAnThucDonService.DataSource = new MatTableDataSource(this.HRMNhaAnThucDonService.List);
        this.HRMNhaAnThucDonService.DataSource.sort = this.HRMNhaAnThucDonSort;
        this.HRMNhaAnThucDonService.DataSource.paginator = this.HRMNhaAnThucDonPaginator;
      },
      (err) => {},
      () => {
        this.HRMNhaAnThucDonService.IsShowLoading = false;
      },
    );
  }
  HRMNhaAnThucDonSave(element: HRMNhaAnThucDon) {
    this.HRMNhaAnThucDonService.IsShowLoading = true;
    element.CompanyID = this.HRMNhaAnThucDonService.BaseParameter.CompanyID;
    this.HRMNhaAnThucDonService.BaseParameter.BaseModel = element;
    this.HRMNhaAnThucDonService.SaveAndUploadFilesAsync().subscribe(
      (res) => {
        this.HRMNhaAnThucDonSearch();
        this.NotificationService.warn(environment.DeleteSuccess);
      },
      (err) => {
        this.NotificationService.warn(environment.DeleteNotSuccess);
      },
      () => {
        this.HRMNhaAnThucDonService.IsShowLoading = false;
      },
    );
  }
  HRMNhaAnThucDonDelete(element: HRMNhaAnThucDon) {
    if (confirm(environment.DeleteConfirm)) {
      this.HRMNhaAnThucDonService.IsShowLoading = true;
      this.HRMNhaAnThucDonService.BaseParameter.ID = element.ID;
      this.HRMNhaAnThucDonService.RemoveAsync().subscribe(
        (res) => {
          this.HRMNhaAnThucDonSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        (err) => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.HRMNhaAnThucDonService.IsShowLoading = false;
        },
      );
    }
  }
}
