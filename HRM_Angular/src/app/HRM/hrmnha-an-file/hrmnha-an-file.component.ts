import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { DownloadService } from 'src/app/shared/Download.service';

import { HRMNhaAnFile } from 'src/app/shared/ERP/HRMNhaAnFile.model';
import { HRMNhaAnFileService } from 'src/app/shared/ERP/HRMNhaAnFile.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-hrmnha-an-file',
  templateUrl: './hrmnha-an-file.component.html',
  styleUrls: ['./hrmnha-an-file.component.css'],
})
export class HRMNhaAnFileComponent implements OnInit {
  @ViewChild('HRMNhaAnFileSort') HRMNhaAnFileSort: MatSort;
  @ViewChild('HRMNhaAnFilePaginator') HRMNhaAnFilePaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public HRMNhaAnFileService: HRMNhaAnFileService,
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
          this.HRMNhaAnFileService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          this.HRMNhaAnFileSearch();
        }
      },
      (err) => {},
      () => {},
    );
  }
  HRMNhaAnFileUpdateDate(value, element: HRMNhaAnFile) {
    element.UpdateDate = new Date(value);
  }
  HRMNhaAnFileFileChange(event, files: FileList) {
    if (files) {
      this.HRMNhaAnFileService.FileToUpload = files;
      this.HRMNhaAnFileService.BaseParameter.Event = event;
    }
  }
  HRMNhaAnFileSearch() {
    this.HRMNhaAnFileService.IsShowLoading = true;
    this.HRMNhaAnFileService.GetByCompanyIDAndEmptyToListAsync().subscribe(
      (res) => {
        this.HRMNhaAnFileService.List = (res as BaseResult).List.sort((a, b) => (a.UpdateDate < b.UpdateDate ? 1 : -1));
        this.HRMNhaAnFileService.DataSource = new MatTableDataSource(this.HRMNhaAnFileService.List);
        this.HRMNhaAnFileService.DataSource.sort = this.HRMNhaAnFileSort;
        this.HRMNhaAnFileService.DataSource.paginator = this.HRMNhaAnFilePaginator;
      },
      (err) => {},
      () => {
        this.HRMNhaAnFileService.IsShowLoading = false;
      },
    );
  }
  HRMNhaAnFileSave(element: HRMNhaAnFile) {
    this.HRMNhaAnFileService.IsShowLoading = true;
    element.CompanyID = this.HRMNhaAnFileService.BaseParameter.CompanyID;
    this.HRMNhaAnFileService.BaseParameter.BaseModel = element;
    this.HRMNhaAnFileService.SaveAndUploadFilesAsync().subscribe(
      (res) => {
        this.HRMNhaAnFileSearch();
        this.NotificationService.warn(environment.DeleteSuccess);
      },
      (err) => {
        this.NotificationService.warn(environment.DeleteNotSuccess);
      },
      () => {
        this.HRMNhaAnFileService.IsShowLoading = false;
      },
    );
  }
  HRMNhaAnFileDelete(element: HRMNhaAnFile) {
    if (confirm(environment.DeleteConfirm)) {
      this.HRMNhaAnFileService.IsShowLoading = true;
      this.HRMNhaAnFileService.BaseParameter.ID = element.ID;
      this.HRMNhaAnFileService.RemoveAsync().subscribe(
        (res) => {
          this.HRMNhaAnFileSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        (err) => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.HRMNhaAnFileService.IsShowLoading = false;
        },
      );
    }
  }
}
