import { element } from 'protractor';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from '../shared/ERP/Membership.service';

import { CategoryHRMSuatAn } from 'src/app/shared/ERP/CategoryHRMSuatAn.model';
import { CategoryHRMSuatAnService } from 'src/app/shared/ERP/CategoryHRMSuatAn.service';

import { HRMChamCongNhaAn } from 'src/app/shared/ERP/HRMChamCongNhaAn.model';
import { HRMChamCongNhaAnService } from 'src/app/shared/ERP/HRMChamCongNhaAn.service';

import { HRMNhaAnThucDon } from 'src/app/shared/ERP/HRMNhaAnThucDon.model';
import { HRMNhaAnThucDonService } from 'src/app/shared/ERP/HRMNhaAnThucDon.service';

import { HRMChamCongBarcode } from 'src/app/shared/ERP/HRMChamCongBarcode.model';
import { HRMChamCongBarcodeService } from 'src/app/shared/ERP/HRMChamCongBarcode.service';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css'],
})
export class HomepageComponent implements OnInit {
  @ViewChild('MembershipSort') MembershipSort: MatSort;
  @ViewChild('MembershipPaginator') MembershipPaginator: MatPaginator;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,
    public MembershipService: MembershipService,

    public CategoryHRMSuatAnService: CategoryHRMSuatAnService,
    public HRMChamCongNhaAnService: HRMChamCongNhaAnService,
    public HRMNhaAnThucDonService: HRMNhaAnThucDonService,
    public HRMChamCongBarcodeService: HRMChamCongBarcodeService,
  ) {}

  ngOnInit(): void {}
  ngAfterViewInit() {
    let token = localStorage.getItem(environment.Token);
    this.MembershipService.Headers = new HttpHeaders();
    this.MembershipService.Headers = this.MembershipService.Headers.append('Authorization', 'Bearer ' + token);
    this.HRMNhaAnThucDonSearch();
    this.CategoryHRMSuatAnSearch();
    this.HRMChamCongBarcodeSearch();
    this.StartTimer();
  }
  ngOnDestroy(): void {}
  StartTimer() {
    setInterval(() => {
      this.HRMChamCongBarcodeSearch();
    }, 60000);
  }
  HRMNhaAnThucDonSearch() {
    this.HRMNhaAnThucDonService.GetSortOrderAsync().subscribe(
      (res) => {
        this.HRMNhaAnThucDonService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
      },
      (err) => {},
      () => {},
    );
  }
  CategoryHRMSuatAnSearch() {
    this.CategoryHRMSuatAnService.BaseParameter.Active = true;
    this.CategoryHRMSuatAnService.GetByActiveToListAsync().subscribe(
      (res) => {
        this.CategoryHRMSuatAnService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CategoryHRMSuatAnService.ListFilter && this.CategoryHRMSuatAnService.ListFilter.length > 0) {
          this.CategoryHRMSuatAnChange(this.CategoryHRMSuatAnService.ListFilter[0]);
        }
      },
      (err) => {},
      () => {},
    );
  }
  CategoryHRMSuatAnChange(CategoryHRMSuatAn: CategoryHRMSuatAn) {
    this.HRMChamCongNhaAnService.BaseParameter.UserName = this.MembershipService.BaseParameter.BaseModel.UserName;
    if (this.HRMChamCongNhaAnService.BaseParameter.UserName != null) {
      this.HRMChamCongNhaAnService.BaseParameter.ID = CategoryHRMSuatAn.ID;
      this.HRMChamCongNhaAnService.BaseParameter.Active = false;
      this.HRMChamCongNhaAnService.SaveByUserName20261116Async().subscribe(
        (res) => {
          this.HRMChamCongNhaAnService.BaseResult = res as BaseResult;
          this.HRMChamCongNhaAnService.BaseParameter.BaseModel = this.HRMChamCongNhaAnService.BaseResult.BaseModel;
          this.NotificationService.warn(environment.SaveSuccess);
        },
        (err) => {
          this.NotificationService.warn(environment.SaveNotSuccess);
        },
        () => {
          this.HRMChamCongNhaAnService.IsShowLoading = false;
        },
      );
    }
  }
  MembershipSearch() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.BaseParameter.ID = Number(localStorage.getItem(environment.UserID));
    this.MembershipService.GetByIDAsync().subscribe(
      (res) => {
        this.MembershipService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
      },
      (err) => {},
      () => {
        this.MembershipService.IsShowLoading = false;
      },
    );
  }
  HRMChamCongBarcodeSearch() {
    this.HRMChamCongBarcodeService.IsShowLoading = true;
    this.HRMChamCongBarcodeService.GetBySearchString_RandomToListAsync().subscribe(
      (res) => {
        this.HRMChamCongBarcodeService.List = (res as BaseResult).List;
        if (this.HRMChamCongBarcodeService.List != null && this.HRMChamCongBarcodeService.List.length > 0) {
          this.HRMChamCongBarcodeService.BaseParameter.BaseModel = this.HRMChamCongBarcodeService.List[0];
        }
      },
      (err) => {},
      () => {
        this.HRMChamCongBarcodeService.IsShowLoading = false;
      },
    );
  }
  
}
