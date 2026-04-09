import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { InvoiceInput } from 'src/app/shared/ERP/InvoiceInput.model';
import { InvoiceInputService } from 'src/app/shared/ERP/InvoiceInput.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { InvoiceInputModalComponent } from '../invoice-input-modal/invoice-input-modal.component';

@Component({
  selector: 'app-invoice-input-admin',
  templateUrl: './invoice-input-admin.component.html',
  styleUrls: ['./invoice-input-admin.component.css']
})
export class InvoiceInputAdminComponent {

  @ViewChild('InvoiceInputSort') InvoiceInputSort: MatSort;
  @ViewChild('InvoiceInputPaginator') InvoiceInputPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public InvoiceInputService: InvoiceInputService,
    public CompanyService: CompanyService,
  ) { }

  ngOnInit(): void {
    this.CompanySearch();
  }
  ngAfterViewInit() {
    this.InvoiceInputSearch();
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.InvoiceInputService);
  }
  DateBegin(value) {
    this.InvoiceInputService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.InvoiceInputService.BaseParameter.DateEnd = new Date(value);
  }
  InvoiceInputSearch() {
    this.InvoiceInputService.IsShowLoading = true;
    this.InvoiceInputService.BaseParameter.Total = environment.InitializationNumber;
    this.InvoiceInputService.BaseParameter.Sum = environment.InitializationNumber;
    this.InvoiceInputService.GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync().subscribe(
      res => {
        this.InvoiceInputService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder < b.SortOrder ? 1 : -1));
        this.InvoiceInputService.DataSource = new MatTableDataSource(this.InvoiceInputService.List);
        this.InvoiceInputService.DataSource.sort = this.InvoiceInputSort;
        this.InvoiceInputService.DataSource.paginator = this.InvoiceInputPaginator;

        for (let i = 0; i < this.InvoiceInputService.List.length; i++) {
          this.InvoiceInputService.BaseParameter.Total = this.InvoiceInputService.BaseParameter.Total + this.InvoiceInputService.List[i].Total;
          this.InvoiceInputService.BaseParameter.Sum = this.InvoiceInputService.BaseParameter.Sum + this.InvoiceInputService.List[i].TotalQuantity;
        }
      },
      err => {
      },
      () => {
        this.InvoiceInputService.IsShowLoading = false;
      }
    );
  }
  InvoiceInputDelete(element: InvoiceInput) {
    if (confirm(environment.DeleteConfirm)) {
      this.InvoiceInputService.BaseParameter.ID = element.ID;
      this.InvoiceInputService.IsShowLoading = true;
      this.InvoiceInputService.RemoveAsync().subscribe(
        res => {
          this.InvoiceInputSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);

        },
        () => {
          this.InvoiceInputService.IsShowLoading = false;
        }
      );
    }
  }
}

