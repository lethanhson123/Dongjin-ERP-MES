import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { HRMChamCongLichSu } from 'src/app/shared/ERP/HRMChamCongLichSu.model';
import { HRMChamCongLichSuService } from 'src/app/shared/ERP/HRMChamCongLichSu.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { CategoryProcess } from 'src/app/shared/ERP/CategoryProcess.model';
import { CategoryProcessService } from 'src/app/shared/ERP/CategoryProcess.service';
import { CategoryHRMCaLamViec } from 'src/app/shared/ERP/CategoryHRMCaLamViec.model';
import { CategoryHRMCaLamViecService } from 'src/app/shared/ERP/CategoryHRMCaLamViec.service';


@Component({
  selector: 'app-hrmcham-cong-lich-su-modal',
  templateUrl: './hrmcham-cong-lich-su-modal.component.html',
  styleUrls: ['./hrmcham-cong-lich-su-modal.component.css']
})
export class HRMChamCongLichSuModalComponent implements OnInit {

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<HRMChamCongLichSuModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,


    public HRMChamCongLichSuService: HRMChamCongLichSuService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public CompanyService: CompanyService,
    public CategoryProcessService: CategoryProcessService,
    public CategoryHRMCaLamViecService: CategoryHRMCaLamViecService,
  ) {
    if (!this.HRMChamCongLichSuService.BaseParameter.BaseModel.ID) {
      this.HRMChamCongLichSuService.BaseParameter.BaseModel.CreateUserID = Number(localStorage.getItem(environment.UserID));
      this.HRMChamCongLichSuService.BaseParameter.BaseModel.CreateUserCode = localStorage.getItem(environment.UserName);
      this.HRMChamCongLichSuService.BaseParameter.BaseModel.CreateUserName = localStorage.getItem(environment.FullName);
    }
    this.CompanySearch();
    this.CategoryHRMCaLamViecSearch();
    this.CategoryProcessSearch();
  }

  ngOnInit(): void {
  }
  ngAfterViewInit() {

  }
  Close() {
    this.DialogRef.close();
  }
  CompanySearch() {
    this.CompanyChange();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryHRMCaLamViecSearch() {
    this.CategoryHRMCaLamViecService.ComponentGetByActiveToListAsync(this.HRMChamCongLichSuService);
  }
  CategoryProcessSearch() {
    this.CategoryProcessService.ComponentGetByActiveToListAsync(this.HRMChamCongLichSuService);
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.HRMChamCongLichSuService.BaseParameter.BaseModel.CompanyID;
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.HRMChamCongLichSuService);
  }
  Date(value) {
    this.HRMChamCongLichSuService.BaseParameter.BaseModel.Date = new Date(value);

  }



  Save() {
    this.HRMChamCongLichSuService.IsShowLoading = true;
    if (this.HRMChamCongLichSuService.BaseParameter.BaseModel.Date != null) {
      this.HRMChamCongLichSuService.BaseParameter.BaseModel.Display = this.HRMChamCongLichSuService.BaseParameter.BaseModel.Date.toLocaleString();
    }
    this.HRMChamCongLichSuService.SaveAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
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
