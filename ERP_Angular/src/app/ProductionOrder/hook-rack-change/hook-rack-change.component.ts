import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

import { trackmtim } from 'src/app/shared/MES/trackmtim.model';
import { trackmtimService } from 'src/app/shared/MES/trackmtim.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-hook-rack-change',
  templateUrl: './hook-rack-change.component.html',
  styleUrls: ['./hook-rack-change.component.css']
})
export class HookRackChangeComponent implements OnInit {

  @ViewChild('trackmtimDetailSort') trackmtimDetailSort: MatSort;
  @ViewChild('trackmtimDetailPaginator') trackmtimDetailPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public Sanitizer: DomSanitizer,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public trackmtimService: trackmtimService,
    public CompanyService: CompanyService,

  ) {
    this.trackmtimService.BaseParameter.Active = false;
    this.trackmtimService.BaseParameter.Quantity = 0;
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {

  }
  CompanySearch() {
    this.trackmtimService.IsShowLoading = true;
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      res => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter) {
          if (this.CompanyService.ListFilter.length > 0) {
            this.trackmtimService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          }
        }
      },
      err => {
      },
      () => {
        this.trackmtimService.IsShowLoading = false;
      }
    );
  }
  trackmtimSave() {
    this.trackmtimService.BaseParameter.ListID = [];
    for (let i = 0; i < this.trackmtimService.List.length; i++) {
      if (this.trackmtimService.List[i].Active == true) {
        if (this.trackmtimService.BaseParameter.UpdateUserCode && this.trackmtimService.BaseParameter.UpdateUserCode.length > 0) {
          this.trackmtimService.List[i].POCode = this.trackmtimService.BaseParameter.UpdateUserCode;
        }
        if (this.trackmtimService.BaseParameter.Display && this.trackmtimService.BaseParameter.Display.length > 0) {
          this.trackmtimService.List[i].FinishGoodsCode = this.trackmtimService.BaseParameter.Display;
        }
        if (this.trackmtimService.BaseParameter.UpdateUserName && this.trackmtimService.BaseParameter.UpdateUserName.length > 0) {
          this.trackmtimService.List[i].ECN = this.trackmtimService.BaseParameter.UpdateUserName;
        }
        this.trackmtimService.List[i].UpdateDate = new Date();
        this.trackmtimService.BaseParameter.ListID.push(this.trackmtimService.List[i].TRACK_IDX);
      }
    }
    this.trackmtimService.IsShowLoading = true;
    this.trackmtimService.SaveByListID_PO_FinishGoods_ECNAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);

      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.trackmtimService.IsShowLoading = false;
      }
    );
  }
  trackmtimSearch() {
    this.trackmtimService.IsShowLoading = true;
    this.trackmtimService.GetByCompanyID_LeadNo_FinishGoodsToListAsync().subscribe(
      res => {
        this.trackmtimService.List = (res as BaseResult).List.sort((a, b) => (a.RACKDTM > b.RACKDTM ? 1 : -1));
        this.trackmtimService.BaseParameter.Count = 0;
        this.trackmtimService.BaseParameter.Sum = 0;
        for (let i = 0; i < this.trackmtimService.List.length; i++) {
          this.trackmtimService.BaseParameter.Sum = this.trackmtimService.BaseParameter.Sum + this.trackmtimService.List[i].QTY;
        }
        if (this.trackmtimService.BaseParameter.Quantity > 0) {
          this.trackmtimService.BaseParameter.Count = this.trackmtimService.BaseParameter.Quantity;
          for (let i = 0; i < this.trackmtimService.List.length; i++) {
            if (this.trackmtimService.BaseParameter.Count > 0) {
              this.trackmtimService.List[i].Active = true;
              this.trackmtimService.BaseParameter.Count = this.trackmtimService.BaseParameter.Count - this.trackmtimService.List[i].QTY;
              this.trackmtimService.List[i].QuantityGAP = this.trackmtimService.BaseParameter.Count;
            }
          }
        }
        this.trackmtimService.DataSource = new MatTableDataSource(this.trackmtimService.List);
        this.trackmtimService.DataSource.sort = this.trackmtimDetailSort;
        this.trackmtimService.DataSource.paginator = this.trackmtimDetailPaginator;

      },
      err => {
      },
      () => {
        this.trackmtimService.IsShowLoading = false;
      }
    );
  }
  @ViewChild("TABLE") table: ElementRef;
  trackmtimExcel() {

    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");

    let ListCompany = this.CompanyService.ListFilter.filter(o => o.ID == this.trackmtimService.BaseParameter.CompanyID);
    if (ListCompany && ListCompany.length > 0) {
      this.CompanyService.BaseParameter.BaseModel = ListCompany[0];
    }
    let filename = this.CompanyService.BaseParameter.BaseModel.Code + "-" + this.trackmtimService.BaseParameter.Name + "-HOOKRACKCheck.xlsx";
    XLSX.writeFile(wb, filename);
  }
}
