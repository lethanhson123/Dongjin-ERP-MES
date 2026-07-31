import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
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
  selector: 'app-employee-info',
  templateUrl: './employee-info.component.html',
  styleUrls: ['./employee-info.component.css']
})
export class EmployeeInfoComponent implements OnInit {

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public MembershipService: MembershipService,


  ) {


    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this.MembershipService.BaseParameter.SearchString = this.ActiveRouter.snapshot.params.SearchString;
        this.MembershipSearch();
      }
    });
  }
  ngOnInit(): void {
  }
  ngAfterViewInit() {
  }
  MembershipSearch() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.GetByUserNameAsync().subscribe(
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
}
