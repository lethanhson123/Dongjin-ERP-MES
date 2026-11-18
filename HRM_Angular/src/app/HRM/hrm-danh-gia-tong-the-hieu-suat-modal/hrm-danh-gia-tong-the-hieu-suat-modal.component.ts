import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { HRMDanhGiaHieuSuatService } from 'src/app/shared/ERP/HRMDanhGiaHieuSuat.service';

import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-hrm-danh-gia-tong-the-hieu-suat-modal',
  templateUrl: './hrm-danh-gia-tong-the-hieu-suat-modal.component.html',
  styleUrls: ['./hrm-danh-gia-tong-the-hieu-suat-modal.component.css'],
})
export class HrmDanhGiaTongTheHieuSuatModalComponent implements OnInit {
  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<HrmDanhGiaTongTheHieuSuatModalComponent>,
    public NotificationService: NotificationService,
    public HRMDanhGiaHieuSuatService: HRMDanhGiaHieuSuatService,
    public CompanyService: CompanyService,
  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {}

  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.HRMDanhGiaHieuSuatService);
  }

  HRMDanhGiaHieuSuatSave() {
    this.HRMDanhGiaHieuSuatService.IsShowLoading = true;
    this.HRMDanhGiaHieuSuatService.SaveAsync().subscribe(
      (res) => {
        this.HRMDanhGiaHieuSuatService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.NotificationService.success(environment.SaveSuccess);
        this.HRMDanhGiaHieuSuatClose();
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMDanhGiaHieuSuatService.IsShowLoading = false;
      },
    );
  }

  HRMDanhGiaHieuSuatDateBegin(value) {
    this.HRMDanhGiaHieuSuatService.BaseParameter.BaseModel.DateBegin = new Date(value);
  }

  HRMDanhGiaHieuSuatDateEnd(value) {
    this.HRMDanhGiaHieuSuatService.BaseParameter.BaseModel.DateEnd = new Date(value);
  }

  HRMDanhGiaHieuSuatClose() {
    this.DialogRef.close();
  }
}
