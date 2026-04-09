import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { BOMCompare } from 'src/app/shared/ERP/BOMCompare.model';
import { BOMCompareService } from 'src/app/shared/ERP/BOMCompare.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-bomcompare',
  templateUrl: './bomcompare.component.html',
  styleUrls: ['./bomcompare.component.css']
})
export class BOMCompareComponent {

  @ViewChild('BOMCompareSort') BOMCompareSort: MatSort;
  @ViewChild('BOMComparePaginator') BOMComparePaginator: MatPaginator;


  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public BOMCompareService: BOMCompareService,
    public MaterialService: MaterialService,
    public CompanyService: CompanyService,
  ) {
    this.BOMCompareService.BaseParameter.Month = this.BOMCompareService.BaseParameter.Year;
    this.BOMCompareService.BaseParameter.Active = true;
    this.CompanySearch();
    this.ComponentGetYearList();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  ComponentGetYearList() {
    this.BOMCompareService.ComponentGetYearList();
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.BOMCompareService);
  }
  MaterialModalOpen(element: Material) {
    this.MaterialService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(MaterialModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {

    });
  }
  MaterialModal(ID: number) {
    this.BOMCompareService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = ID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.BOMCompareService.IsShowLoading = false;
      }
    );
  }
  BOMCompareSearch() {
    if (this.BOMCompareService.BaseParameter.SearchString.length > 0) {
      this.BOMCompareService.BaseParameter.SearchString = this.BOMCompareService.BaseParameter.SearchString.trim();
      if (this.BOMCompareService.DataSource) {
        this.BOMCompareService.DataSource.filter = this.BOMCompareService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.BOMCompareService.IsShowLoading = true;
      this.BOMCompareService.GetCompanyID_YearBegin_YearEndToListAsync().subscribe(
        res => {
          this.BOMCompareService.List = (res as BaseResult).List;
          this.BOMCompareService.DataSource = new MatTableDataSource(this.BOMCompareService.List);
          this.BOMCompareService.DataSource.sort = this.BOMCompareSort;
          this.BOMCompareService.DataSource.paginator = this.BOMComparePaginator;
        },
        err => {
        },
        () => {
          this.BOMCompareService.IsShowLoading = false;
        }
      );
    }
  }
  CreateAutoAsync() {
    this.BOMCompareService.IsShowLoading = true;
    this.BOMCompareService.SyncByCompanyID_YearBegin_YearEndToListAsync().subscribe(
      res => {
      },
      err => {
      },
      () => {
        this.BOMCompareService.IsShowLoading = false;
      }
    );
  }
}
