import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';

import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { HRMDanhGiaHieuSuatService } from 'src/app/shared/ERP/HRMDanhGiaHieuSuat.service';

import { HRMDanhGiaHieuSuatKetQuaService } from 'src/app/shared/ERP/HRMDanhGiaHieuSuatKetQua.service';

import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { MembershipService } from 'src/app/shared/ERP/Membership.service';

import { HrmDanhGiaTongTheHieuSuatModalComponent } from '../hrm-danh-gia-tong-the-hieu-suat-modal/hrm-danh-gia-tong-the-hieu-suat-modal.component';

declare var M: any;

@Component({
  selector: 'app-hrm-danh-gia-tong-the-hieu-suat',
  templateUrl: './hrm-danh-gia-tong-the-hieu-suat.component.html',
  styleUrls: ['./hrm-danh-gia-tong-the-hieu-suat.component.css'],
})
export class HrmDanhGiaTongTheHieuSuatComponent implements OnInit {
  @ViewChild('HRMDanhGiaHieuSuatSort') HRMDanhGiaHieuSuatSort: MatSort;
  @ViewChild('HRMDanhGiaHieuSuatPaginator') HRMDanhGiaHieuSuatPaginator: MatPaginator;
  @ViewChild('HRMDanhGiaHieuSuatKetQuaSort') HRMDanhGiaHieuSuatKetQuaSort: MatSort;
  @ViewChild('HRMDanhGiaHieuSuatKetQuaPaginator') HRMDanhGiaHieuSuatKetQuaPaginator: MatPaginator;
  @ViewChild('MembershipSort') MembershipSort: MatSort;
  @ViewChild('MembershipPaginator') MembershipPaginator: MatPaginator;

  constructor(
    public ElementRef: ElementRef,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public HRMDanhGiaHieuSuatService: HRMDanhGiaHieuSuatService,
    public HRMDanhGiaHieuSuatKetQuaService: HRMDanhGiaHieuSuatKetQuaService,
    public CompanyService: CompanyService,
    public MembershipService: MembershipService,
  ) {
    this.HRMDanhGiaHieuSuatService.BaseParameter.CompanyID = 16;
    this.CompanySearch();
    this.MembershipSearch();
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.HRMDanhGiaHieuSuatSearch();
    this.HRMDanhGiaHieuSuatKetQuaSearch();
    this.InitTabs();
  }

  //fix bug chuyen tab meterial
  InitTabs() {
    setTimeout(() => {
      if (typeof M !== 'undefined' && M && M.Tabs) {
        var ListTabs = this.ElementRef.nativeElement.querySelectorAll('.tabs');
        ListTabs.forEach((element) => {
          var instance = M.Tabs.getInstance(element);
          if (instance) {
            instance.destroy();
          }
          M.Tabs.init(element, {});
        });
      }
    }, 100);
  }

  HRMDanhGiaHieuSuatDateBegin(value) {
    this.HRMDanhGiaHieuSuatService.BaseParameter.DateBegin = new Date(value);
  }

  HRMDanhGiaHieuSuatDateEnd(value) {
    this.HRMDanhGiaHieuSuatService.BaseParameter.DateEnd = new Date(value);
  }

  HRMDanhGiaHieuSuatKetQuaDateBegin(value) {
    this.MembershipService.BaseParameter.DateBegin = new Date(value);
  }

  HRMDanhGiaHieuSuatKetQuaDateEnd(value) {
    this.MembershipService.BaseParameter.DateEnd = new Date(value);
  }

  HRMDanhGiaHieuSuatAdd(ID: number) {
    this.HRMDanhGiaHieuSuatService.IsShowLoading = true;
    this.HRMDanhGiaHieuSuatService.BaseParameter.ID = ID;
    this.HRMDanhGiaHieuSuatService.GetByIDAsync().subscribe(
      (res) => {
        this.HRMDanhGiaHieuSuatService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.HRMDanhGiaHieuSuatService.BaseParameter.BaseModel.CompanyID = this.HRMDanhGiaHieuSuatService.BaseParameter.CompanyID;
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        const dialog = this.Dialog.open(HrmDanhGiaTongTheHieuSuatModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => {
          this.HRMDanhGiaHieuSuatSearch();
        });
      },
      (err) => {},
      () => {
        this.HRMDanhGiaHieuSuatService.IsShowLoading = false;
      },
    );
  }

  CompanySearch() {
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      (res) => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.HRMDanhGiaHieuSuatService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          this.CompanyChange();
          this.HRMDanhGiaHieuSuatSearch();
        }
      },
      (err) => {},
      () => {},
    );
  }

  CompanyChange() {
    this.HRMDanhGiaHieuSuatSearch();
    this.HRMDanhGiaHieuSuatKetQuaService.List = null;
  }

  MembershipSearch() {
    this.MembershipService.BaseParameter.CompanyID = this.HRMDanhGiaHieuSuatService.BaseParameter.CompanyID;
    this.MembershipService.BaseParameter.CategoryDepartmentID = Number(localStorage.getItem(environment.CategoryDepartmentID));
    this.MembershipService.BaseParameter.Active = true;
    this.MembershipService.GetByCategoryDepartmentID_ActiveToListAsync().subscribe(
      (res) => {
        this.MembershipService.ListFilter = (res as BaseResult).List
          .filter((o) => o.ID !=  Number(localStorage.getItem(environment.UserID)));
        this.MembershipService.DataSource01 = new MatTableDataSource(this.MembershipService.ListFilter);
        this.MembershipService.DataSource01.sort = this.MembershipSort;
        this.MembershipService.DataSource01.paginator = this.MembershipPaginator;
      },
      (err) => {},
      () => {},
    );
  }

  HRMDanhGiaHieuSuatSearch() {
    this.HRMDanhGiaHieuSuatService.IsShowLoading = true;
    this.HRMDanhGiaHieuSuatService.GetByCompanyIDToListAsync().subscribe(
      (res) => {
        this.HRMDanhGiaHieuSuatService.List = (res as BaseResult).List;
        this.HRMDanhGiaHieuSuatService.BuildTree();
        this.HRMDanhGiaHieuSuatService.DataSource = new MatTableDataSource(this.HRMDanhGiaHieuSuatService.List);
        this.HRMDanhGiaHieuSuatService.DataSource.sort = this.HRMDanhGiaHieuSuatSort;
        this.HRMDanhGiaHieuSuatService.DataSource.paginator = this.HRMDanhGiaHieuSuatPaginator;
      },
      (err) => {},
      () => {
        this.HRMDanhGiaHieuSuatService.IsShowLoading = false;
      },
    );
  }

  HRMDanhGiaHieuSuatDelete(element) {
    this.HRMDanhGiaHieuSuatService.IsShowLoading = true;
    this.HRMDanhGiaHieuSuatService.BaseParameter.ID = element.ID;
    this.HRMDanhGiaHieuSuatService.RemoveAsync().subscribe(
      (res) => {
        this.HRMDanhGiaHieuSuatSearch();
        this.NotificationService.warn(environment.DeleteSuccess);
      },
      (err) => {
        this.NotificationService.warn(environment.DeleteNotSuccess);
      },
      () => {
        this.HRMDanhGiaHieuSuatService.IsShowLoading = false;
      },
    );
  }

  HRMDanhGiaHieuSuatKetQuaSearch() {
    this.HRMDanhGiaHieuSuatKetQuaService.IsShowLoading = true;
    this.HRMDanhGiaHieuSuatKetQuaService.GetByCompanyIDToListAsync().subscribe(
      (res) => {
        this.HRMDanhGiaHieuSuatKetQuaService.List = (res as BaseResult).List;
        this.HRMDanhGiaHieuSuatKetQuaService.DataSource = new MatTableDataSource(this.HRMDanhGiaHieuSuatKetQuaService.List);
        this.HRMDanhGiaHieuSuatKetQuaService.DataSource.sort = this.HRMDanhGiaHieuSuatKetQuaSort;
        this.HRMDanhGiaHieuSuatKetQuaService.DataSource.paginator = this.HRMDanhGiaHieuSuatKetQuaPaginator;
      },
      (err) => {},
      () => {
        this.HRMDanhGiaHieuSuatKetQuaService.IsShowLoading = false;
      },
    );
  }

  HRMDanhGiaHieuSuatKetQuaSave(element) {
    this.HRMDanhGiaHieuSuatKetQuaService.IsShowLoading = true;
    this.HRMDanhGiaHieuSuatKetQuaService.BaseParameter.BaseModel = element;
    this.HRMDanhGiaHieuSuatKetQuaService.SaveAsync().subscribe(
      (res) => {
        this.HRMDanhGiaHieuSuatKetQuaSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMDanhGiaHieuSuatKetQuaService.IsShowLoading = false;
      },
    );
  }

  HRMDanhGiaHieuSuatKetQuaDelete(element) {
    this.HRMDanhGiaHieuSuatKetQuaService.IsShowLoading = true;
    this.HRMDanhGiaHieuSuatKetQuaService.BaseParameter.ID = element.ID;
    this.HRMDanhGiaHieuSuatKetQuaService.RemoveAsync().subscribe(
      (res) => {
        this.HRMDanhGiaHieuSuatKetQuaSearch();
        this.NotificationService.warn(environment.DeleteSuccess);
      },
      (err) => {
        this.NotificationService.warn(environment.DeleteNotSuccess);
      },
      () => {
        this.HRMDanhGiaHieuSuatKetQuaService.IsShowLoading = false;
      },
    );
  }


}
