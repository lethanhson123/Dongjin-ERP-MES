import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';


import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { HRMCaLamViecKeHoach } from 'src/app/shared/ERP/HRMCaLamViecKeHoach.model';
import { HRMCaLamViecKeHoachService } from 'src/app/shared/ERP/HRMCaLamViecKeHoach.service';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';
import { CategoryHRMCaLamViecService } from 'src/app/shared/ERP/CategoryHRMCaLamViec.service';
import { HrmCaLamViecKeHoachModalComponent } from '../hrm-ca-lam-viec-ke-hoach-modal/hrm-ca-lam-viec-ke-hoach-modal.component';
import { HrmCaLamViecKeHoachModal02Component } from '../hrm-ca-lam-viec-ke-hoach-modal02/hrm-ca-lam-viec-ke-hoach-modal02.component';

@Component({
  selector: 'app-HRMCaLamViecKeHoach',
  encapsulation: ViewEncapsulation.None,
  templateUrl: './hrm-ca-lam-viec-ke-hoach.component.html',
  styleUrls: ['./hrm-ca-lam-viec-ke-hoach.component.css']
})
export class HRMCaLamViecKeHoachComponent implements OnInit {
  DateStart: Date | null = new Date();
  DateEnd: Date | null = new Date();

  private CreateLocalDate(value: string): Date | null {
    return value ? new Date(`${value}T12:00:00`) : null;
  }

  @ViewChild('HRMCaLamViecKeHoachSort') HRMCaLamViecKeHoachSort: MatSort;
  @ViewChild('HRMCaLamViecKeHoachPaginator') HRMCaLamViecKeHoachPaginator: MatPaginator;

  @ViewChild('HRMCaLamViecKeHoachSortCa') HRMCaLamViecKeHoachSortCa: MatSort;
  @ViewChild('HRMCaLamViecKeHoachSortDepartment') HRMCaLamViecKeHoachSortDepartment: MatSort;
  @ViewChild('HRMCaLamViecKeHoachSortSum') HRMCaLamViecKeHoachSortSum: MatSort;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,
    public HRMCaLamViecKeHoachService: HRMCaLamViecKeHoachService,
    public CompanyService: CompanyService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
    public CategoryHRMCaLamViecService: CategoryHRMCaLamViecService,
  ) {
    this.HRMCaLamViecKeHoachService.BaseParameter.CompanyID = 16;
    this.HRMCaLamViecKeHoachService.BaseParameter.CategoryHRMPhongBanID = 0;
    this.HRMCaLamViecKeHoachService.BaseParameter.Date = new Date();
    this.HRMCaLamViecKeHoachService.BaseParameter.DateBegin = new Date(this.HRMCaLamViecKeHoachService.BaseParameter.Date);
    this.HRMCaLamViecKeHoachService.BaseParameter.DateEnd = new Date(this.HRMCaLamViecKeHoachService.BaseParameter.Date);
    this.DateStart = new Date(this.HRMCaLamViecKeHoachService.BaseParameter.DateBegin);
    this.DateEnd = new Date(this.HRMCaLamViecKeHoachService.BaseParameter.DateEnd);
  }

  ngOnInit(): void {
    this.CategoryHRMCaLamViecService.ComponentGetByActiveToListAsync(this.HRMCaLamViecKeHoachService);
    this.CompanySearch();
  }
  ngAfterViewInit() {
    this.HRMCaLamViecKeHoachSearch();
  }
  CompanySearch() {
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      (res) => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.CategoryHRMPhongBanSearch();
      },
      (err) => { },
      () => { },
    );
  }
  CompanyChange() {
    this.HRMCaLamViecKeHoachService.BaseParameter.CategoryHRMPhongBanID = 0;
    this.CategoryHRMPhongBanSearch();
    this.HRMCaLamViecKeHoachSearch();
  }

  CategoryHRMCaLamViecChange(element: HRMCaLamViecKeHoach) {
    const shift = (this.CategoryHRMCaLamViecService.ListFilter || []).find((item) => item.ID == element.CategoryHRMCaLamViecID);
    if (!shift) {
      return;
    }

    element.CategoryHRMCaLamViecName = shift.Name;
    element.Quantity01 = shift.Quantity01;
    element.Quantity02 = shift.Quantity02;
    element.Quantity03 = shift.Quantity03;
    element.Quantity04 = shift.Quantity04;
    element.IsEdit = true;
  }

  Save(element: HRMCaLamViecKeHoach) {
    this.HRMCaLamViecKeHoachService.IsShowLoading = true;
    this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel = element;
    this.HRMCaLamViecKeHoachService.Save20260930Async().subscribe(
      () => {
        element.IsEdit = false;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      () => this.NotificationService.warn(environment.SaveNotSuccess),
      () => this.HRMCaLamViecKeHoachService.IsShowLoading = false,
    );
  }
  CategoryHRMPhongBanSearch() {
    this.CategoryHRMPhongBanService.BaseParameter.CompanyID = this.HRMCaLamViecKeHoachService.BaseParameter.CompanyID;
    this.CategoryHRMPhongBanService.ComponentGetByCompanyIDAndActiveToList(this.HRMCaLamViecKeHoachService);
  }
  DateStartChange(value: string) {
    this.DateStart = value ? new Date(`${value}`) : null;
    this.ValidateDateRange();
    this.UpdateDateParameters();
  }

  DateEndChange(value: string) {
    this.DateEnd = value ? new Date(`${value}`) : null;
    this.ValidateDateRange();
    this.UpdateDateParameters();
  }

  private UpdateDateParameters() {
    this.HRMCaLamViecKeHoachService.BaseParameter.Date = this.DateStart;
    this.HRMCaLamViecKeHoachService.BaseParameter.DateBegin = this.DateStart;
    this.HRMCaLamViecKeHoachService.BaseParameter.DateEnd = this.DateEnd;
  }


 

  DateChange(value) {
    this.DateStart = value ? new Date(`${value}`) : null;
    this.DateEnd = this.DateStart ? new Date(this.DateStart) : null;
    this.UpdateDateParameters();
  } 

  HRMCaLamViecKeHoachSearch() {
    this.ValidateDateRange();
    this.UpdateDateParameters();
    this.HRMCaLamViecKeHoachService.IsShowLoading = true;
    this.HRMCaLamViecKeHoachService.GetByCompanyIDAndCategoryPhongBanIDAndDateToListAsync().subscribe(
      (res) => {
        this.HRMCaLamViecKeHoachService.ListFilter = ((res as BaseResult).List || []).sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.HRMCaLamViecKeHoachService.Sum = (res as BaseResult).Sum || 0;
        let ListSum = ((res as BaseResult).ListChild || []) as HRMCaLamViecKeHoach[];
        this.HRMCaLamViecKeHoachService.ListSum = ListSum;

        this.HRMCaLamViecKeHoachService.ListCa = [];
        let ListCategoryHRMCaLamViecName = [...new Set(ListSum.map((o) => o.CategoryHRMCaLamViecName))];
        for (let CategoryHRMCaLamViecName of ListCategoryHRMCaLamViecName) {
          let item: HRMCaLamViecKeHoach = {
            ID: 0,
            CategoryHRMCaLamViecName: CategoryHRMCaLamViecName,
            Quantity01: ListSum.filter((o) => o.CategoryHRMCaLamViecName == CategoryHRMCaLamViecName).reduce((sum, o) => sum + (o.Quantity01 || 0), 0),
          };
          this.HRMCaLamViecKeHoachService.ListCa.push(item);
        }
        
        this.HRMCaLamViecKeHoachService.DataSource01 = new MatTableDataSource(this.HRMCaLamViecKeHoachService.ListCa);
        this.HRMCaLamViecKeHoachService.DataSource01.sort = this.HRMCaLamViecKeHoachSortCa;

        this.HRMCaLamViecKeHoachService.ListDepartment = [];
        let ListCategoryHRMPhongBanName = [...new Set(ListSum.map((o) => o.CategoryHRMPhongBanName))];

        for (let CategoryHRMPhongBanName of ListCategoryHRMPhongBanName) {
          let item: HRMCaLamViecKeHoach = {
            ID: 0,
            CategoryHRMPhongBanName: CategoryHRMPhongBanName,
            Quantity01: ListSum.filter((o) => o.CategoryHRMPhongBanName == CategoryHRMPhongBanName).reduce((sum, o) => sum + (o.Quantity01 || 0), 0),
          };
          this.HRMCaLamViecKeHoachService.ListDepartment.push(item);
        }
        this.HRMCaLamViecKeHoachService.DataSource02 = new MatTableDataSource(this.HRMCaLamViecKeHoachService.ListDepartment);
        this.HRMCaLamViecKeHoachService.DataSource02.sort = this.HRMCaLamViecKeHoachSortDepartment;

        this.HRMCaLamViecKeHoachService.DataSource03 = new MatTableDataSource(ListSum);
        this.HRMCaLamViecKeHoachService.DataSource03.sort = this.HRMCaLamViecKeHoachSortSum;

        this.HRMCaLamViecKeHoachService.List = this.HRMCaLamViecKeHoachService.ListFilter.filter((item) =>
          ((item.MembershipCode || '') + ' ' + (item.MembershipName || '') + ' ' + (item.CompanyName || '') + ' ' + (item.CategoryHRMPhongBanName || '') + ' ' + (item.CategoryHRMCaLamViecName || '')).toLowerCase().includes((this.HRMCaLamViecKeHoachService.BaseParameter.SearchString || '').trim().toLowerCase())
        );
        this.HRMCaLamViecKeHoachService.DataSource = new MatTableDataSource(this.HRMCaLamViecKeHoachService.List);
        this.HRMCaLamViecKeHoachService.DataSource.sort = this.HRMCaLamViecKeHoachSort;
        this.HRMCaLamViecKeHoachService.DataSource.paginator = this.HRMCaLamViecKeHoachPaginator;
      },
      (err) => { },
      () => {
        this.HRMCaLamViecKeHoachService.IsShowLoading = false;
      },
    );
  }

  private ValidateDateRange() {
    if (this.DateStart && this.DateEnd && this.DateEnd < this.DateStart) {
      this.NotificationService.warn('Ngày kết thúc không được nhỏ hơn ngày bắt đầu.');
      this.DateEnd = new Date(this.DateStart);
    }
  }

  HRMCaLamViecKeHoachDelete(element: HRMCaLamViecKeHoach) {
    if (confirm(environment.DeleteConfirm)) {
      this.HRMCaLamViecKeHoachService.IsShowLoading = true;
      this.HRMCaLamViecKeHoachService.BaseParameter.ID = element.ID;
      this.HRMCaLamViecKeHoachService.RemoveAsync().subscribe(
        (res) => {
          this.NotificationService.warn(environment.DeleteSuccess);
          this.HRMCaLamViecKeHoachSearch();
        },
        (err) => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.HRMCaLamViecKeHoachService.IsShowLoading = false;
        },
      );
    }
  }
  HRMCaLamViecKeHoachAdd(ID: number) {
    this.HRMCaLamViecKeHoachService.IsShowLoading = true;
    this.HRMCaLamViecKeHoachService.BaseParameter.ID = ID;
    this.HRMCaLamViecKeHoachService.GetByIDAsync().subscribe(
      (res) => {
        this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CompanyID = this.HRMCaLamViecKeHoachService.BaseParameter.CompanyID;
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        dialogConfig.maxWidth = '96vw';
        dialogConfig.maxHeight = 'calc(100vh - 32px)';
        dialogConfig.panelClass = 'hrm-ca-lam-viec-ke-hoach-dialog';
        const dialog = this.Dialog.open(HrmCaLamViecKeHoachModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => {
          this.CompanySearch();
        });
      },
      (err) => { },
      () => {
        this.HRMCaLamViecKeHoachService.IsShowLoading = false;
      },
    );
  }
  
  HRMCaLamViecKeHoachUpload() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(HrmCaLamViecKeHoachModal02Component, dialogConfig);
    dialog.afterClosed().subscribe(() => {
      this.HRMCaLamViecKeHoachSearch();
    });
  }
  HRMCaLamViecKeHoachExcel() {
    this.HRMCaLamViecKeHoachService.IsShowLoading = true;
    this.HRMCaLamViecKeHoachService.GetByCompanyIDAndCategoryPhongBanIDAndDateToExcelAsync().subscribe(
      (res) => {
        let url = (res as BaseResult).Message;
        window.open(url, '_blank');
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMCaLamViecKeHoachService.IsShowLoading = false;
      },
    );
  }
}
