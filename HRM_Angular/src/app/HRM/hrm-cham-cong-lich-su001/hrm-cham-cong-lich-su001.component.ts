import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { HRMChamCongLichSu } from 'src/app/shared/ERP/HRMChamCongLichSu.model';
import { HRMChamCongLichSuService } from 'src/app/shared/ERP/HRMChamCongLichSu.service';

import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';
import { HRMChamCongLichSuModalComponent } from '../hrmcham-cong-lich-su-modal/hrmcham-cong-lich-su-modal.component';

@Component({
  selector: 'app-hrm-cham-cong-lich-su001',
  templateUrl: './hrm-cham-cong-lich-su001.component.html',
  styleUrls: ['./hrm-cham-cong-lich-su001.component.css']
})
export class HrmChamCongLichSu001Component implements OnInit {

  @ViewChild('HRMChamCongLichSu001Sort') HRMChamCongLichSu001Sort: MatSort;
  @ViewChild('HRMChamCongLichSu001Paginator') HRMChamCongLichSu001Paginator: MatPaginator; 

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,
    public HRMChamCongLichSuService: HRMChamCongLichSuService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
    public CompanyService: CompanyService,
  ) {    
    this.HRMChamCongLichSuService.BaseParameter.SearchString = localStorage.getItem(environment.UserName);
    this.HRMChamCongLichSuService.BaseParameter.CompanyID = Number(localStorage.getItem(environment.CompanyID));
    this.HRMChamCongLichSu001Search();
  }

  ngOnInit(): void {
    this.HRMChamCongLichSuService.BaseParameter.CategoryHRMPhongBanID = 0;
  }
  ngAfterViewInit() {
  }
  DateBegin(value) {
    this.HRMChamCongLichSuService.BaseParameter.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.HRMChamCongLichSuService.BaseParameter.DateEnd = new Date(value);
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.HRMChamCongLichSuService);
    this.CategoryHRMPhongBanSearch();
  }
  CompanyChange() {
    this.CategoryHRMPhongBanSearch();
  }
  CategoryHRMPhongBanSearch() {
    this.CategoryHRMPhongBanService.BaseParameter.CompanyID = this.HRMChamCongLichSuService.BaseParameter.CompanyID;
    this.CategoryHRMPhongBanService.BaseParameter.Active = true;
    this.CategoryHRMPhongBanService.ComponentGetByCompanyIDAndActiveToList(this.HRMChamCongLichSuService);
  }
  HRMChamCongLichSu001Add(ID: number) {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.BaseParameter.ID = ID;
    this.HRMChamCongLichSuService.GetByIDAsync().subscribe(
      res => {
        this.HRMChamCongLichSuService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.HRMChamCongLichSuService.BaseParameter.BaseModel.CompanyID = this.HRMChamCongLichSuService.BaseParameter.CompanyID;
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        const dialog = this.Dialog.open(HRMChamCongLichSuModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => {
          this.HRMChamCongLichSu001Search();
        });
      },
      err => {
      },
      () => {
        this.HRMChamCongLichSuService.IsShowLoading = false;
      }
    );
  }
  HRMChamCongLichSu001Search() {
    this.HRMChamCongLichSuService.IsShowLoading = true;    
    this.HRMChamCongLichSuService.GetByDateBegin_DateEnd_SearchStringToListAsync().subscribe(
      res => {
        this.HRMChamCongLichSuService.List = (res as BaseResult).List;
        this.HRMChamCongLichSuService.DataSource = new MatTableDataSource(this.HRMChamCongLichSuService.List);
        this.HRMChamCongLichSuService.DataSource.sort = this.HRMChamCongLichSu001Sort;
        this.HRMChamCongLichSuService.DataSource.paginator = this.HRMChamCongLichSu001Paginator;
      },
      err => {
      },
      () => {
        this.HRMChamCongLichSuService.IsShowLoading = false;
      }
    );
  }
  HRMChamCongLichSu001Delete(element: HRMChamCongLichSu) {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.BaseParameter.ID = element.ID;
    this.HRMChamCongLichSuService.RemoveAsync().subscribe(
      res => {
        this.HRMChamCongLichSu001Search();
        this.NotificationService.warn(environment.DeleteSuccess);
      },
      err => {
        this.NotificationService.warn(environment.DeleteNotSuccess);
      },
      () => {
        this.HRMChamCongLichSuService.IsShowLoading = false;
      }
    );
  }
  HRMChamCongLichSu001Excel() {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    this.HRMChamCongLichSuService.GetByCompanyID_CategoryHRMPhongBanID_DateBegin_DateEnd_SearchStringToExcelAsync().subscribe(
      res => {
        let url = (res as BaseResult).Message;
        window.open(url, "_blank");
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMChamCongLichSuService.IsShowLoading = false;
      }
    );
  }

}
