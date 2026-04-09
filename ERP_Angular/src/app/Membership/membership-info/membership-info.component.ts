import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';

@Component({
  selector: 'app-membership-info',
  templateUrl: './membership-info.component.html',
  styleUrls: ['./membership-info.component.css']
})
export class MembershipInfoComponent {

  constructor(
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public MembershipService: MembershipService,

  ) {

  }
  ngOnInit(): void {
  }
  ngAfterViewInit() {
    this.MembershipSearch();
  }
  MembershipSearch() {
    this.MembershipService.BaseParameter.ID = Number(localStorage.getItem(environment.UserID));
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.GetByIDAsync().subscribe(
      res => {
        this.MembershipService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
      },
      err => {
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }

  Save() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.BaseResult.Message = environment.InitializationString;
    this.MembershipService.IsPasswordValidWithRegex().subscribe(
      res => {
        this.MembershipService.BaseResult = (res as BaseResult);
        let IsCheck = false;
        if (this.MembershipService.BaseResult) {
          if (this.MembershipService.BaseResult.Count > 0) {
            IsCheck = true;
            this.SaveSub();
          }
        }
        if (IsCheck == false) {
          this.MembershipService.BaseResult.Message = environment.IsPasswordValidWithRegex;
          this.NotificationService.warn(this.MembershipService.BaseResult.Message);
        }
      },
      err => {
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }
  SaveSub() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.SaveAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }
}
