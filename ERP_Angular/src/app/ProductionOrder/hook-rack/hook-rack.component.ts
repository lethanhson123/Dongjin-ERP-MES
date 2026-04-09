import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Report } from 'src/app/shared/ERP/Report.model';
import { ReportService } from 'src/app/shared/ERP/Report.service';

import { ReportDetail } from 'src/app/shared/ERP/ReportDetail.model';
import { ReportDetailService } from 'src/app/shared/ERP/ReportDetail.service';

import { trackmtim } from 'src/app/shared/MES/trackmtim.model';
import { trackmtimService } from 'src/app/shared/MES/trackmtim.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { HookRackDetailComponent } from '../hook-rack-detail/hook-rack-detail.component';

@Component({
  selector: 'app-hook-rack',
  templateUrl: './hook-rack.component.html',
  styleUrls: ['./hook-rack.component.css']
})
export class HookRackComponent implements OnInit {

  @ViewChild('ReportSort') ReportSort: MatSort;
  @ViewChild('ReportPaginator') ReportPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ReportService: ReportService,
    public ReportDetailService: ReportDetailService,
    public trackmtimService: trackmtimService,
    public CompanyService: CompanyService,

  ) { }

  ngOnInit(): void {
    this.CompanySearch();
  }
  ngAfterViewInit() {
    this.ReportSearch();
  }
  DateBegin(value) {
    this.ReportService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.ReportService.BaseParameter.DateEnd = new Date(value);
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.ReportService);
  }
  ReportSearch() {
    this.ReportService.IsShowLoading = true;
    this.ReportService.HookRackGetByCompanyID_Begin_End_SearchStringAsync().subscribe(
      res => {
        this.ReportService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        if (this.ReportService.BaseParameter.BaseModel) {
          if (this.ReportService.BaseParameter.BaseModel.ID > 0) {
            this.ReportDetailService.BaseParameter.ParentID = this.ReportService.BaseParameter.BaseModel.ID;
            this.ReportService.IsShowLoading = true;
            this.ReportDetailService.GetByParentIDToListAsync().subscribe(
              res => {
                this.ReportDetailService.List = (res as BaseResult).List.sort((a, b) => (a.Quantity00 < b.Quantity00 ? 1 : -1));
                this.ReportService.BaseParameter.Count = 0;
                for (let i = 0; i < this.ReportDetailService.List.length; i++) {
                  this.ReportService.BaseParameter.Count = this.ReportService.BaseParameter.Count + this.ReportDetailService.List[i].Quantity00;
                }
                this.ReportDetailService.DataSource = new MatTableDataSource(this.ReportDetailService.List);
                this.ReportDetailService.DataSource.sort = this.ReportSort;
                this.ReportDetailService.DataSource.paginator = this.ReportPaginator;
              },
              err => {
              },
              () => {
                this.ReportService.IsShowLoading = false;
              }
            );
          }
        }
      },
      err => {
      },
      () => {
        this.ReportService.IsShowLoading = false;
      }
    );
  }
  ReportDownload() {
    this.ReportService.IsShowLoading = true;
    this.ReportDetailService.HookRackByParentIDExportToExcelAsync().subscribe(
      res => {
        let url = (res as BaseResult).Message;
        window.open(url, "_blank");
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.ReportService.IsShowLoading = false;
      }
    );

  }
  HookRackDetailModalOpen(element: ReportDetail) {
    this.trackmtimService.BaseParameter.Code = element.Code;
    this.trackmtimService.BaseParameter.CompanyID = this.ReportService.BaseParameter.CompanyID;
    this.trackmtimService.BaseParameter.DateBegin = this.ReportService.BaseParameter.DateBegin;
    this.trackmtimService.BaseParameter.DateEnd = this.ReportService.BaseParameter.DateEnd;
    this.trackmtimService.BaseParameter.Active = this.ReportService.BaseParameter.Active;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(HookRackDetailComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {

    });
  }
}