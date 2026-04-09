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

import { BOM } from 'src/app/shared/ERP/BOM.model';
import { BOMService } from 'src/app/shared/ERP/BOM.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-hook-rack-check',
  templateUrl: './hook-rack-check.component.html',
  styleUrls: ['./hook-rack-check.component.css']
})
export class HookRackCheckComponent implements OnInit {

  @ViewChild('BOMDetailSort') BOMDetailSort: MatSort;
  @ViewChild('BOMDetailPaginator') BOMDetailPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public Sanitizer: DomSanitizer,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public BOMService: BOMService,
    public CompanyService: CompanyService,

  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {

  }
  CompanySearch() {
    this.BOMService.IsShowLoading = true;
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      res => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));

      },
      err => {
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
  BOMSync() {
    this.BOMService.IsShowLoading = true;
    this.BOMService.SyncFinishGoodsListOftrackmtimAsync().subscribe(
      res => {
      },
      err => {
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
  BOMSearch() {
    if (this.BOMService.BaseParameter.SearchString.length > 0) {
      this.BOMService.BaseParameter.SearchString = this.BOMService.BaseParameter.SearchString.trim();
      if (this.BOMService.DataSource) {
        this.BOMService.DataSource.filter = this.BOMService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.BOMService.IsShowLoading = true;
      this.BOMService.GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync().subscribe(
        res => {
          this.BOMService.List = (res as BaseResult).List;
          for (let i = 0; i < this.BOMService.List.length; i++) {
            this.BOMService.List[i].UpdateDate = new Date();
          }
          this.BOMService.DataSource = new MatTableDataSource(this.BOMService.List);
          this.BOMService.DataSource.sort = this.BOMDetailSort;
          this.BOMService.DataSource.paginator = this.BOMDetailPaginator;
        },
        err => {
        },
        () => {
          this.BOMService.IsShowLoading = false;
        }
      );
    }
  }
  @ViewChild("TABLE") table: ElementRef;
  BOMExcel() {

    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");

    let ListCompany = this.CompanyService.ListFilter.filter(o => o.ID == this.BOMService.BaseParameter.CompanyID);
    if (ListCompany && ListCompany.length > 0) {
      this.CompanyService.BaseParameter.BaseModel = ListCompany[0];
    }
    let filename = this.CompanyService.BaseParameter.BaseModel.Code + "-" + this.BOMService.BaseParameter.Name + "-HOOKRACKCheck.xlsx";
    XLSX.writeFile(wb, filename);
  }
}
