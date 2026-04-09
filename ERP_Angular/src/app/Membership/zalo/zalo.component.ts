import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { ZaloZNS } from 'src/app/shared/ERP/ZaloZNS.model';
import { ZaloZNSService } from 'src/app/shared/ERP/ZaloZNS.service';

@Component({
  selector: 'app-zalo',
  templateUrl: './zalo.component.html',
  styleUrls: ['./zalo.component.css']
})
export class ZaloComponent implements OnInit {

  @ViewChild('ZaloZNSSort') ZaloZNSSort: MatSort;
  @ViewChild('ZaloZNSPaginator') ZaloZNSPaginator: MatPaginator;

  URLTemplate: string;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,


    public ZaloZNSService: ZaloZNSService,

  ) {
    this.URLTemplate = this.ZaloZNSService.APIRootURL + "Download/Zalo.xlsx";
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  ngOnDestroy(): void {

  }
  ZaloZNSSend() {
    this.ZaloZNSService.IsShowLoading = true;
    this.ZaloZNSService.SendZaloTuyenDungCongNhan2026Async().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
      },
      () => {
        this.ZaloZNSService.IsShowLoading = false;
      }
    );
  }
  ZaloZNSChange(event, files: FileList) {
    if (files) {
      this.ZaloZNSService.FileToUpload = files;
      this.ZaloZNSService.BaseParameter.Active = false;
      this.ZaloZNSService.BaseParameter.Event = event;
    }
  }
  Save() {
    this.ZaloZNSService.IsShowLoading = true;
    this.ZaloZNSService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.ZaloZNSService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.ZaloZNSSearch();
        this.ZaloZNSService.FileToUpload = null;
        this.ZaloZNSService.BaseParameter.Event = null;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.ZaloZNSService.IsShowLoading = false;
      }
    );
  }
  ZaloZNSSearch() {
    this.ZaloZNSService.IsShowLoading = true;
    if (this.ZaloZNSService.BaseParameter.SearchString.length > 0) {
      this.ZaloZNSService.BaseParameter.SearchString = this.ZaloZNSService.BaseParameter.SearchString.trim();
      if (this.ZaloZNSService.DataSource) {
        this.ZaloZNSService.DataSource.filter = this.ZaloZNSService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.ZaloZNSService.GetByCodeToListAsync().subscribe(
        res => {
          this.ZaloZNSService.List = (res as BaseResult).List.sort((a, b) => (a.UpdateDate < b.UpdateDate ? 1 : -1));
          this.ZaloZNSService.DataSource = new MatTableDataSource(this.ZaloZNSService.List);
          this.ZaloZNSService.DataSource.sort = this.ZaloZNSSort;
          this.ZaloZNSService.DataSource.paginator = this.ZaloZNSPaginator;
        },
        err => {
        },
        () => {
          this.ZaloZNSService.IsShowLoading = false;
        }
      );
    }
  }
}
