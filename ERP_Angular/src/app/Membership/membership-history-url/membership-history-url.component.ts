import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { MembershipHistoryURL } from 'src/app/shared/ERP/MembershipHistoryURL.model';
import { MembershipHistoryURLService } from 'src/app/shared/ERP/MembershipHistoryURL.service';

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';

import { MembershipModalComponent } from '../membership-modal/membership-modal.component';

@Component({
  selector: 'app-membership-history-url',
  templateUrl: './membership-history-url.component.html',
  styleUrls: ['./membership-history-url.component.css']
})
export class MembershipHistoryURLComponent {

  @ViewChild('MembershipHistoryURLSort') MembershipHistoryURLSort: MatSort;
  @ViewChild('MembershipHistoryURLPaginator') MembershipHistoryURLPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public MembershipHistoryURLService: MembershipHistoryURLService,
    public MembershipService: MembershipService,

  ) {

  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.MembershipHistoryURLSearch();
  }
  ngOnDestroy(): void {

  }
  DateBegin(value) {
    this.MembershipHistoryURLService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.MembershipHistoryURLService.BaseParameter.DateEnd = new Date(value);
  }
  Date(value) {
    this.MembershipHistoryURLService.BaseParameter.Date = new Date(value);
  }
  MembershipHistoryURLSearch() {
    if (this.MembershipHistoryURLService.BaseParameter.SearchString.length > 0) {
      this.MembershipHistoryURLService.BaseParameter.SearchString = this.MembershipHistoryURLService.BaseParameter.SearchString.trim();
      if (this.MembershipHistoryURLService.DataSource) {
        this.MembershipHistoryURLService.DataSource.filter = this.MembershipHistoryURLService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.MembershipHistoryURLService.IsShowLoading = true;
      this.MembershipHistoryURLService.GetByParentName_DateBegin_DateEndToListAsync().subscribe(
        res => {
          this.MembershipHistoryURLService.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));;
          this.MembershipHistoryURLService.DataSource = new MatTableDataSource(this.MembershipHistoryURLService.List);
          this.MembershipHistoryURLService.DataSource.sort = this.MembershipHistoryURLSort;
          this.MembershipHistoryURLService.DataSource.paginator = this.MembershipHistoryURLPaginator;
        },
        err => {
        },
        () => {
          this.MembershipHistoryURLService.IsShowLoading = false;
        }
      );
    }
  }
  MembershipModal(element: MembershipHistoryURL) {
    this.MembershipService.BaseParameter.ID = element.ParentID;
    if (this.MembershipService.BaseParameter.ID > 0) {
      this.MembershipService.GetByIDAsync().subscribe(
        res => {
          this.MembershipService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
          const dialogConfig = new MatDialogConfig();
          dialogConfig.disableClose = true;
          dialogConfig.autoFocus = true;
          dialogConfig.width = environment.DialogConfigWidth;
          const dialog = this.Dialog.open(MembershipModalComponent, dialogConfig);
          dialog.afterClosed().subscribe(() => {
          });
        },
        err => {
        },
        () => {
        }
      );
    }
  }
}
