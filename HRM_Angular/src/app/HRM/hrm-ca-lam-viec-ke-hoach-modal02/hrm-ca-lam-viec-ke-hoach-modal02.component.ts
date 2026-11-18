import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { HRMCaLamViecKeHoach } from 'src/app/shared/ERP/HRMCaLamViecKeHoach.model';
import { HRMCaLamViecKeHoachService } from 'src/app/shared/ERP/HRMCaLamViecKeHoach.service';
import { CategoryHRMCaLamViecService } from 'src/app/shared/ERP/CategoryHRMCaLamViec.service';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-hrm-ca-lam-viec-ke-hoach-modal02',
  templateUrl: './hrm-ca-lam-viec-ke-hoach-modal02.component.html',
  styleUrls: ['./hrm-ca-lam-viec-ke-hoach-modal02.component.css']
})
export class HrmCaLamViecKeHoachModal02Component implements OnInit {

  @ViewChild('HRMCaLamViecKeHoachModal02Sort') HRMCaLamViecKeHoachModal02Sort: MatSort;
  @ViewChild('HRMCaLamViecKeHoachModal02Paginator') HRMCaLamViecKeHoachModal02Paginator: MatPaginator;

  get List(): HRMCaLamViecKeHoach[] | undefined {
    return this.HRMCaLamViecKeHoachService.BaseParameter.List as HRMCaLamViecKeHoach[];
  }

  constructor(
    public DialogRef: MatDialogRef<HrmCaLamViecKeHoachModal02Component>,
    public NotificationService: NotificationService,
    public HRMCaLamViecKeHoachService: HRMCaLamViecKeHoachService,
    public CategoryHRMCaLamViecService: CategoryHRMCaLamViecService,
    public CompanyService: CompanyService,
  ) {
  }

  ngOnInit(): void {
    this.CompanySearch();
  }

  CompanySearch() {
    if (this.HRMCaLamViecKeHoachService.BaseParameter.CompanyID > 0) {
      return;
    }
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      (res) => {
        this.CompanyService.ListFilter = ((res as BaseResult).List || []).sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if ((!this.HRMCaLamViecKeHoachService.BaseParameter.CompanyID || this.HRMCaLamViecKeHoachService.BaseParameter.CompanyID == 0) && this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.HRMCaLamViecKeHoachService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
        }
      },
      (err) => { },
      () => { },
    );
  }

  DateChange(value) {
    this.HRMCaLamViecKeHoachService.BaseParameter.Date = value ? new Date(value) : null;
  }

  HRMCaLamViecKeHoachChange(event, files: FileList) {
    if (files && files.length > 0) {
      this.HRMCaLamViecKeHoachService.FileToUpload = files;
      this.HRMCaLamViecKeHoachService.BaseParameter.Event = event;
      this.HRMCaLamViecKeHoachService.FileName = files[0].name;
      this.HRMCaLamViecKeHoachRead();
    }
  }

  HRMCaLamViecKeHoachRead() {
    if (!this.HRMCaLamViecKeHoachService.FileToUpload || this.HRMCaLamViecKeHoachService.FileToUpload.length == 0) {
      this.NotificationService.warn('Vui lòng chọn file Excel | 엑셀 파일을 선택하세요');
      return;
    }
    this.HRMCaLamViecKeHoachService.IsShowLoading = true;
    this.HRMCaLamViecKeHoachService.ReadExcelToListAsync().subscribe(
      (res) => {
        this.HRMCaLamViecKeHoachService.BaseResult = res as BaseResult;
        this.HRMCaLamViecKeHoachService.BaseParameter.List = this.HRMCaLamViecKeHoachService.BaseResult.List as HRMCaLamViecKeHoach[];
        this.HRMCaLamViecKeHoachService.DataSource = new MatTableDataSource(this.List || []);
        this.HRMCaLamViecKeHoachService.DataSource.sort = this.HRMCaLamViecKeHoachModal02Sort;
        this.HRMCaLamViecKeHoachService.DataSource.paginator = this.HRMCaLamViecKeHoachModal02Paginator;
        if (!this.List || this.List.length == 0) {
          this.NotificationService.warn('Không có dữ liệu | 데이터가 없습니다');
        }
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMCaLamViecKeHoachService.IsShowLoading = false;
      },
    );
  }

  NormalizeName(value: string | null | undefined): string {
    return (value || '').replace(/\s+/g, ' ').trim().toLowerCase();
  }

  HRMCaLamViecKeHoachSave() {
    const list = this.HRMCaLamViecKeHoachService.BaseParameter.List as HRMCaLamViecKeHoach[];
    if (!list || list.length == 0) {
      this.NotificationService.warn('Vui lòng đọc file Excel trước | 먼저 엑셀 파일을 읽어주세요');
      return;
    }
    if (!this.HRMCaLamViecKeHoachService.BaseParameter.CompanyID || this.HRMCaLamViecKeHoachService.BaseParameter.CompanyID == 0) {
      this.NotificationService.warn('Vui lòng chọn công ty | 회사를 선택하세요');
      return;
    }
    const now = new Date();
    const userID = Number(localStorage.getItem(environment.UserID));
    const userCode = localStorage.getItem(environment.UserName);
    const fullName = localStorage.getItem(environment.FullName);
    const companyID = this.HRMCaLamViecKeHoachService.BaseParameter.CompanyID;
    for (let i = 0; i < list.length; i++) {
      list[i].Active = true;
      list[i].CompanyID = companyID;
      if (!list[i].ID || list[i].ID == 0) {
        list[i].CreateDate = now;
        list[i].CreateUserID = userID;
        list[i].CreateUserCode = userCode;
        list[i].CreateUserName = fullName;
      }
      list[i].UpdateDate = now;
      list[i].UpdateUserID = userID;
      list[i].UpdateUserCode = userCode;
      list[i].UpdateUserName = fullName;
    }
    this.HRMCaLamViecKeHoachService.IsShowLoading = true;
    this.HRMCaLamViecKeHoachService.SaveManyAsync(list).subscribe(
      (res) => {
        this.NotificationService.warn((res as BaseResult).Message || environment.SaveSuccess);
        this.DialogRef.close(true);
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMCaLamViecKeHoachService.IsShowLoading = false;
      },
    );
  }

  HRMCaLamViecKeHoachClose() {
    this.DialogRef.close();
  }

}
