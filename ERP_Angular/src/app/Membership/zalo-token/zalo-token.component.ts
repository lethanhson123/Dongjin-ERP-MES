import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { ZaloToken } from 'src/app/shared/ERP/ZaloToken.model';
import { ZaloTokenService } from 'src/app/shared/ERP/ZaloToken.service';


@Component({
  selector: 'app-zalo-token',
  templateUrl: './zalo-token.component.html',
  styleUrls: ['./zalo-token.component.css']
})
export class ZaloTokenComponent {

  @ViewChild('ZaloTokenSort') ZaloTokenSort: MatSort;
  @ViewChild('ZaloTokenPaginator') ZaloTokenPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,


    public ZaloTokenService: ZaloTokenService,

  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.ZaloTokenSearch();
  }
  ngOnDestroy(): void {

  }
  DateNgayGhiNhan(element: ZaloToken, value) {
    element.Date = new Date(value);
  }
  ZaloTokenSearch() {
    this.ZaloTokenService.SearchAll(this.ZaloTokenSort, this.ZaloTokenPaginator);
  }
  ZaloTokenSave(element: ZaloToken) {
    this.ZaloTokenService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.ZaloTokenService.ComponentSaveAll(this.ZaloTokenSort, this.ZaloTokenPaginator));
  }
  ZaloTokenDelete(element: ZaloToken) {
    this.ZaloTokenService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.ZaloTokenService.ComponentDeleteAll(this.ZaloTokenSort, this.ZaloTokenPaginator));
  }
}
