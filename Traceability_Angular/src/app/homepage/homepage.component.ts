import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Traceability } from 'src/app/shared/ERP/Traceability.model';
import { TraceabilityService } from 'src/app/shared/ERP/Traceability.service';


@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css']
})
export class HomepageComponent implements OnInit {

  @ViewChild('TraceabilitySort') TraceabilitySort: MatSort;
  @ViewChild('TraceabilityPaginator') TraceabilityPaginator: MatPaginator;

  @ViewChild('TraceabilitySort02') TraceabilitySort02: MatSort;
  @ViewChild('TraceabilityPaginator02') TraceabilityPaginator02: MatPaginator;

  @ViewChild('TraceabilitySort03') TraceabilitySort03: MatSort;
  @ViewChild('TraceabilityPaginator03') TraceabilityPaginator03: MatPaginator;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public TraceabilityService: TraceabilityService,


  ) {
    this.TraceabilityService.BaseParameter.CompanyID = 16;
  }

  ngOnInit(): void {    
    this.TraceabilitySearch();
  }
  StartTimer() {
    setInterval(() => {
      this.TraceabilitySearch();
    }, 60000)
  }
  DateBegin(value) {
    this.TraceabilityService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.TraceabilityService.BaseParameter.DateEnd = new Date(value);
  }
  TraceabilitySearch() {
    this.TraceabilityService.IsShowLoading = true;
    this.TraceabilityService.BaseParameter.Action = 1;
    this.TraceabilityService.GetByCompanyID_Begin_End_SearchString_ActionToListAsync().subscribe(
      res => {
        this.TraceabilityService.DataSource = new MatTableDataSource((res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1)));
        this.TraceabilityService.DataSource.sort = this.TraceabilitySort;
        this.TraceabilityService.DataSource.paginator = this.TraceabilityPaginator;
      },
      err => {
      },
      () => {
        this.TraceabilityService.IsShowLoading = false;
      }
    );

    this.TraceabilityService.BaseParameter.Action = 4;
    this.TraceabilityService.GetByCompanyID_Begin_End_SearchString_ActionToListAsync().subscribe(
      res => {
        this.TraceabilityService.DataSource02 = new MatTableDataSource((res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1)));
        this.TraceabilityService.DataSource02.sort = this.TraceabilitySort02;
        this.TraceabilityService.DataSource02.paginator = this.TraceabilityPaginator02;
      },
      err => {
      },
      () => {
        this.TraceabilityService.IsShowLoading = false;
      }
    );

    this.TraceabilityService.BaseParameter.SearchString = environment.InitializationString;
  }

}
