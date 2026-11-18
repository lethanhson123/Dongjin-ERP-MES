import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { HRMSoDoToChuc } from 'src/app/shared/ERP/HRMSoDoToChuc.model';
import { HRMSoDoToChucService } from 'src/app/shared/ERP/HRMSoDoToChuc.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { CategoryHRMPhongBan } from 'src/app/shared/ERP/CategoryHRMPhongBan.model';
import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';

import { CategoryPosition } from 'src/app/shared/ERP/CategoryPosition.model';
import { CategoryPositionService } from 'src/app/shared/ERP/CategoryPosition.service';

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';

@Component({
  selector: 'app-hrmso-do-to-chuc-modal',
  templateUrl: './hrmso-do-to-chuc-modal.component.html',
  styleUrls: ['./hrmso-do-to-chuc-modal.component.css'],
})
export class HRMSoDoToChucModalComponent implements OnInit {
  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<HRMSoDoToChucModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public HRMSoDoToChucService: HRMSoDoToChucService,
    public MembershipService: MembershipService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,

    public CategoryPositionService: CategoryPositionService,
  ) {
    this.MembershipSearch();
    this.CategoryPositionSearch();
    this.CategoryHRMPhongBanSearch();
  }

  ngOnInit(): void { }

  ngAfterViewInit() { }


  Close() {
    this.DialogRef.close();
  }


  MembershipSearch() {
    this.MembershipService.ListChild = this.MembershipService.ListParent;
    if (this.MembershipService.ListParent) {
      if (this.MembershipService.ListParent.length == 0) {
        this.MembershipService.IsShowLoading = true;
        this.MembershipService.BaseParameter.CompanyID = this.HRMSoDoToChucService.BaseParameter.CompanyID;
        this.MembershipService.GetByCompanyIDAndActiveToListAsync().subscribe(
          (res) => {
            this.MembershipService.BaseResult = res as BaseResult;
            this.MembershipService.ListParent = this.MembershipService.BaseResult.List.sort((a, b) => (a.UserName > b.UserName ? 1 : -1));
            this.MembershipService.ListChild = this.MembershipService.ListParent;
          },
          (err) => { },
          () => {
            this.MembershipService.IsShowLoading = false;
          },
        );
      }
    }
  }


  MembershipFilter(SearchString: string) {
    this.MembershipService.ListChild = this.MembershipService.ListParent;
    if (this.HRMSoDoToChucService.BaseParameter.SearchString.length > 0) {
      this.HRMSoDoToChucService.BaseParameter.SearchString = this.HRMSoDoToChucService.BaseParameter.SearchString.trim();
      this.HRMSoDoToChucService.BaseParameter.SearchString = this.HRMSoDoToChucService.BaseParameter.SearchString.toLowerCase();
      this.MembershipService.ListChild = this.MembershipService.ListParent.filter(
        (o) => o.CategoryDepartmentName && o.CategoryDepartmentName.toLowerCase().includes(this.HRMSoDoToChucService.BaseParameter.SearchString),
      );
      if (this.MembershipService.ListChild.length == 0) {
        this.MembershipService.ListChild = this.MembershipService.ListParent.filter((o) => o.UserName && o.UserName.toLowerCase().includes(this.HRMSoDoToChucService.BaseParameter.SearchString));
      }
      if (this.MembershipService.ListChild.length == 0) {
        this.MembershipService.ListChild = this.MembershipService.ListParent.filter((o) => o.Search && o.Search.toLowerCase().includes(this.HRMSoDoToChucService.BaseParameter.SearchString));
      }
      if (this.MembershipService.ListChild.length == 0) {
        this.MembershipService.ListChild = this.MembershipService.ListParent.filter((o) => o.Name && o.Name.toLowerCase().includes(this.HRMSoDoToChucService.BaseParameter.SearchString));
      }
      if (this.MembershipService.ListChild.length == 0) {
        this.MembershipService.ListChild = this.MembershipService.ListParent.filter((o) => o.Display && o.Display.toLowerCase().includes(this.HRMSoDoToChucService.BaseParameter.SearchString));
      }
    }
  }


  CategoryPositionSearch() {
    this.CategoryPositionService.ComponentGetByActiveToListAsync(this.HRMSoDoToChucService);
  }


  CategoryDepartmentSearch() {
    if (this.CategoryDepartmentService.ListFilter) {
      if (this.CategoryDepartmentService.ListFilter.length == 0) {
        this.HRMSoDoToChucService.IsShowLoading = true;
        this.CategoryDepartmentService.BaseParameter.CompanyID = this.HRMSoDoToChucService.BaseParameter.BaseModel.CompanyID;
        this.CategoryDepartmentService.GetByCompanyID_Active_IsOfficeToListAsync().subscribe(
          (res) => {
            this.CategoryDepartmentService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          },
          (err) => { },
          () => {
            this.HRMSoDoToChucService.IsShowLoading = false;
          },
        );
      }
    }
  }

  CategoryHRMPhongBanSearch() {
    this.HRMSoDoToChucService.IsShowLoading = true;
    this.CategoryHRMPhongBanService.CategoryHRMPhongBanSearchString = environment.InitializationString;
    this.CategoryHRMPhongBanService.BaseParameter.CompanyID = this.HRMSoDoToChucService.BaseParameter.BaseModel.CompanyID;
    this.CategoryHRMPhongBanService.GetByCompanyIDAndActiveToListAsync().subscribe(
      (res) => {
        this.CategoryHRMPhongBanService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.CategoryHRMPhongBanService.ListFilter = this.CategoryHRMPhongBanService.List;
        this.CategoryHRMPhongBanService.CategoryHRMPhongBanFilter();
      },
      (err) => { },
      () => {
        this.HRMSoDoToChucService.IsShowLoading = false;
      },
    );
  }


  HRMSoDoToChucAdd() {
    this.HRMSoDoToChucService.IsShowLoading = true;
    this.HRMSoDoToChucService.BaseParameter.ID = 0;
    this.HRMSoDoToChucService.GetByIDAsync().subscribe(
      (res) => {
        this.HRMSoDoToChucService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
      },
      (err) => { },
      () => {
        this.HRMSoDoToChucService.IsShowLoading = false;
      },
    );
  }
  HRMSoDoToChucSave() {    
    this.HRMSoDoToChucService.IsShowLoading = true;
    this.HRMSoDoToChucService.SaveAsync().subscribe(
      (res) => {
        this.HRMSoDoToChucService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMSoDoToChucService.IsShowLoading = false;
      },
    );
  }
}
