import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { InvoiceInputHistory } from 'src/app/shared/ERP/InvoiceInputHistory.model';
import { InvoiceInputHistoryService } from 'src/app/shared/ERP/InvoiceInputHistory.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
@Component({
  selector: 'app-invoice-input-history',
  templateUrl: './invoice-input-history.component.html',
  styleUrls: ['./invoice-input-history.component.css']
})
export class InvoiceInputHistoryComponent implements OnInit {

  @ViewChild('InvoiceInputHistorySort') InvoiceInputHistorySort: MatSort;
  @ViewChild('InvoiceInputHistoryPaginator') InvoiceInputHistoryPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public InvoiceInputHistoryService: InvoiceInputHistoryService,
    public CompanyService: CompanyService,

  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  DateDateATA(element: InvoiceInputHistory, value) {
    element.DateATA = new Date(value);
  }
  DateDateETD(element: InvoiceInputHistory, value) {
    element.DateETD = new Date(value);
  }
  DateDateETA(element: InvoiceInputHistory, value) {
    element.DateETA = new Date(value);
  }
  DateDateFT(element: InvoiceInputHistory, value) {
    element.DateFT = new Date(value);
  }
  DateDateReal(element: InvoiceInputHistory, value) {
    element.DateReal = new Date(value);
  }
  CompanySearch() {
    this.InvoiceInputHistoryService.IsShowLoading = true;
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.GetByActiveToListAsync().subscribe(
      res => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter) {
          if (this.CompanyService.ListFilter.length > 0) {
            this.InvoiceInputHistoryService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
            this.InvoiceInputHistorySearch();
          }
        }
      },
      err => {
      },
      () => {
        this.InvoiceInputHistoryService.IsShowLoading = false;
      }
    );
  }
  InvoiceInputHistorySearch() {
    if (this.InvoiceInputHistoryService.BaseParameter.SearchString.length > 0) {
      this.InvoiceInputHistoryService.BaseParameter.SearchString = this.InvoiceInputHistoryService.BaseParameter.SearchString.trim();
      if (this.InvoiceInputHistoryService.DataSource) {
        this.InvoiceInputHistoryService.DataSource.filter = this.InvoiceInputHistoryService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.InvoiceInputHistoryService.IsShowLoading = true;
      this.InvoiceInputHistoryService.GetByCompanyIDAndEmptyToListAsync().subscribe(
        res => {
          this.InvoiceInputHistoryService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.InvoiceInputHistoryService.DataSource = new MatTableDataSource(this.InvoiceInputHistoryService.List);
          this.InvoiceInputHistoryService.DataSource.sort = this.InvoiceInputHistorySort;
          this.InvoiceInputHistoryService.DataSource.paginator = this.InvoiceInputHistoryPaginator;
        },
        err => {
        },
        () => {
          this.InvoiceInputHistoryService.IsShowLoading = false;
        }
      );
    }
  }
  InvoiceInputHistorySave(element: InvoiceInputHistory) {
    this.InvoiceInputHistoryService.IsShowLoading = true;
    element.CompanyID = this.InvoiceInputHistoryService.BaseParameter.CompanyID;
    this.InvoiceInputHistoryService.BaseParameter.BaseModel = element;
    this.InvoiceInputHistoryService.SaveAsync().subscribe(
      res => {
        this.InvoiceInputHistorySearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InvoiceInputHistoryService.IsShowLoading = false;
      }
    );
  }
  InvoiceInputHistoryDelete(element: InvoiceInputHistory) {
    if (confirm(environment.DeleteConfirm)) {
      this.InvoiceInputHistoryService.IsShowLoading = true;
      this.InvoiceInputHistoryService.BaseParameter.ID = element.ID;
      this.InvoiceInputHistoryService.RemoveAsync().subscribe(
        res => {
          this.InvoiceInputHistorySearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.InvoiceInputHistoryService.IsShowLoading = false;
        }
      );
      return environment.DeleteSuccess;
    }
  }
}
