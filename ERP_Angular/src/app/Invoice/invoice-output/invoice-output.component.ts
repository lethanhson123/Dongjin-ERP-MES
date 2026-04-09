import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { InvoiceOutput } from 'src/app/shared/ERP/InvoiceOutput.model';
import { InvoiceOutputService } from 'src/app/shared/ERP/InvoiceOutput.service';
import { InvoiceOutputModalComponent } from '../invoice-output-modal/invoice-output-modal.component';

@Component({
  selector: 'app-invoice-output',
  templateUrl: './invoice-output.component.html',
  styleUrls: ['./invoice-output.component.css']
})
export class InvoiceOutputComponent {

@ViewChild('InvoiceOutputSort') InvoiceOutputSort: MatSort;
  @ViewChild('InvoiceOutputPaginator') InvoiceOutputPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public InvoiceOutputService: InvoiceOutputService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.InvoiceOutputSearch();
  }
  InvoiceOutputSearch() {
    this.InvoiceOutputService.IsShowLoading = true;
    this.InvoiceOutputService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.InvoiceOutputService.BaseParameter.Active = true;
    this.InvoiceOutputService.BaseParameter.IsComplete = false;
    this.InvoiceOutputService.GetByMembershipIDToListAsync().subscribe(
      res => {
        this.InvoiceOutputService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder < b.SortOrder ? 1 : -1));
        this.InvoiceOutputService.DataSource = new MatTableDataSource(this.InvoiceOutputService.List);
        this.InvoiceOutputService.DataSource.sort = this.InvoiceOutputSort;
        this.InvoiceOutputService.DataSource.paginator = this.InvoiceOutputPaginator;
      },
      err => {
      },
      () => {
        this.InvoiceOutputService.IsShowLoading = false;
      }
    );    
  }
  InvoiceOutputSave(element: InvoiceOutput) {
    this.InvoiceOutputService.FormData = element;
    this.NotificationService.warn(this.InvoiceOutputService.ComponentSave(this.InvoiceOutputSort, this.InvoiceOutputPaginator));
  }
  InvoiceOutputDelete(element: InvoiceOutput) {
    this.InvoiceOutputService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.InvoiceOutputService.ComponentDelete(this.InvoiceOutputSort, this.InvoiceOutputPaginator));
  }
  InvoiceOutputModal(element: InvoiceOutput) {
    this.InvoiceOutputService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    dialogConfig.data = { ID: 0 };
    const dialog = this.Dialog.open(InvoiceOutputModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
      this.InvoiceOutputSearch();
    });
  }
  InvoiceOutputAdd() {
    this.InvoiceOutputService.IsShowLoading = true;
    this.InvoiceOutputService.BaseParameter.ID = environment.InitializationNumber;
    this.InvoiceOutputService.GetByIDAsync().subscribe(
      res => {
        this.InvoiceOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.InvoiceOutputModal(this.InvoiceOutputService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.InvoiceOutputService.IsShowLoading = false;
      }
    );
  }
}
