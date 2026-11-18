import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';
import { MemberShipModal03Component } from '../member-ship-modal03/member-ship-modal03.component';

import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';
import { CategoryHRMChucDanhService } from 'src/app/shared/ERP/CategoryHRMChucDanh.service';
import { CategoryHRMTrinhDoService } from 'src/app/shared/ERP/CategoryHRMTrinhDo.service';
import { CategoryHRMChuyenNganhService } from 'src/app/shared/ERP/CategoryHRMChuyenNganh.service';
import { CategoryHRMDanTocService } from 'src/app/shared/ERP/CategoryHRMDanToc.service';
import { CategoryHRMQuocGiaService } from 'src/app/shared/ERP/CategoryHRMQuocGia.service';
import { CategoryHRMGioiTinhService } from 'src/app/shared/ERP/CategoryHRMGioiTinh.service';
import { CategoryHRMTinhTrangLamViecService } from 'src/app/shared/ERP/CategoryHRMTinhTrangLamViec.service';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';
import { CategoryPositionService } from 'src/app/shared/ERP/CategoryPosition.service';

@Component({
  selector: 'app-member-ship-list-modal',
  templateUrl: './member-ship-list-modal.component.html',
  styleUrls: ['./member-ship-list-modal.component.css'],
})
export class MemberShipListModalComponent implements OnInit {

  constructor(
    public DialogRef: MatDialogRef<MemberShipListModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Membership,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public MembershipService: MembershipService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
    public CategoryHRMChucDanhService: CategoryHRMChucDanhService,
    public CategoryHRMTrinhDoService: CategoryHRMTrinhDoService,
    public CategoryHRMChuyenNganhService: CategoryHRMChuyenNganhService,
    public CategoryHRMDanTocService: CategoryHRMDanTocService,
    public CategoryHRMQuocGiaService: CategoryHRMQuocGiaService,
    public CategoryHRMGioiTinhService: CategoryHRMGioiTinhService,
    public CategoryHRMTinhTrangLamViecService: CategoryHRMTinhTrangLamViecService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public CategoryPositionService: CategoryPositionService,
  ) {

    this.MembershipService.FileToUpload = null;
    this.MembershipService.BaseParameter.Event = null;

    if (this.data && this.data.ID > 0) {
      this.MembershipService.BaseParameter.BaseModel = this.data;
    } else {
      this.MembershipService.BaseParameter.BaseModel = { ID: 0, Active: true };
    }
    this.CategoryHRMPhongBanSearch();
    this.CategoryHRMChucDanhSearch();
    this.CategoryHRMTrinhDoSearch();
    this.CategoryHRMChuyenNganhSearch();
    this.CategoryHRMDanTocSearch();
    this.CategoryHRMQuocGiaSearch();
    this.CategoryHRMGioiTinhSearch();
    this.CategoryHRMTinhTrangLamViecSearch();
    this.CategoryDepartmentSearch();
    this.CategoryPositionSearch();
  }

  ngOnInit(): void {}

  CategoryHRMPhongBanSearch() {
    this.CategoryHRMPhongBanService.IsShowLoading = true;
    this.CategoryHRMPhongBanService.BaseParameter.Active = true;
    this.CategoryHRMPhongBanService.GetByActiveToListAsync().subscribe(
      (res) => {
        this.CategoryHRMPhongBanService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.CategoryHRMPhongBanService.ListFilter = this.CategoryHRMPhongBanService.List;
        this.CategoryHRMPhongBanService.CategoryHRMPhongBanFilter();
      },
      (err) => {},
      () => {
        this.CategoryHRMPhongBanService.IsShowLoading = false;
      },
    );
  }
  CategoryHRMChucDanhSearch() {
    this.CategoryHRMChucDanhService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMTrinhDoSearch() {
    this.CategoryHRMTrinhDoService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMChuyenNganhSearch() {
    this.CategoryHRMChuyenNganhService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMDanTocSearch() {
    this.CategoryHRMDanTocService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMQuocGiaSearch() {
    this.CategoryHRMQuocGiaService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMGioiTinhSearch() {
    this.CategoryHRMGioiTinhService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMTinhTrangLamViecSearch() {
    this.CategoryHRMTinhTrangLamViecService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.MembershipService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.GetByCompanyIDAndActiveToListAsync().subscribe(
      (res) => {
        this.CategoryDepartmentService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      },
      (err) => {},
      () => {},
    );
  }
  CategoryPositionSearch() {
    this.CategoryPositionService.ComponentGetByActiveToListAsync(this.MembershipService);
  }

  DateNgaySinh(value) {
    this.MembershipService.BaseParameter.BaseModel.NgaySinh = new Date(value);
  }
  DateNgayVaoLam(value) {
    this.MembershipService.BaseParameter.BaseModel.NgayVaoLam = new Date(value);
  }
  DateCCCDNgayCap(value) {
    this.MembershipService.BaseParameter.BaseModel.CCCDNgayCap = new Date(value);
  }
  DateCCCDNgayHetHan(value) {
    this.MembershipService.BaseParameter.BaseModel.CCCDNgayHetHan = new Date(value);
  }
  DateNgayNghiViec(value) {
    this.MembershipService.BaseParameter.BaseModel.NgayNghiViec = new Date(value);
  }
  DateTrainingDate(value) {
    this.MembershipService.BaseParameter.BaseModel.TrainingDate = new Date(value);
  }

  MembershipChange(event, files: FileList) {
    if (files && files.length > 0) {
      this.MembershipService.FileToUpload = files;
      this.MembershipService.BaseParameter.Event = event;
    } else {
      this.MembershipService.FileToUpload = null;
      this.MembershipService.BaseParameter.Event = null;
    }
  }

  MembershipListModalSave() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.Save2026Async().subscribe(
      (res) => {
        this.MembershipService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      },
    );
  }

  MembershipListModalSave20260928() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.Save20260928Async().subscribe(
      (res) => {
        this.MembershipService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.NotificationService.warn(environment.SaveSuccess);
        this.MembershipService.FileToUpload = null;
        this.MembershipService.BaseParameter.Event = null;
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      },
    );
  }

  MembershipListModalDelete() {
    if (confirm(environment.DeleteConfirm)) {
      this.MembershipService.IsShowLoading = true;
      this.MembershipService.BaseParameter.ID = this.MembershipService.BaseParameter.BaseModel.ID;
      this.MembershipService.RemoveAsync().subscribe(
        (res) => {
          this.NotificationService.warn(environment.DeleteSuccess);
          this.MembershipService.FileToUpload = null;
          this.MembershipService.BaseParameter.Event = null;
          this.DialogRef.close(true);
        },
        (err) => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.MembershipService.IsShowLoading = false;
        },
      );
    }
  }

  MembershipListModalClose() {
    this.MembershipService.FileToUpload = null;
    this.MembershipService.BaseParameter.Event = null;
    this.DialogRef.close();
  }

  CategoryHRMPhongBanModal03() {
    this.Dialog.open(MemberShipModal03Component, {
      width: '90vw',
      maxWidth: '95vw',
    }).afterClosed().subscribe((result) => {
      this.CategoryHRMPhongBanSearch();
    });
  }
}
