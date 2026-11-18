import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { MembershipListService } from 'src/app/shared/ERP/MembershipList.service';
import { MemberShipListModalComponent } from '../member-ship-list-modal/member-ship-list-modal.component';
import { MemberShipListModal01Component } from '../member-ship-list-modal01/member-ship-list-modal01.component';
import { MemberShipListModal02Component } from '../member-ship-list-modal02/member-ship-list-modal02.component';


import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';
import { CategoryPositionService } from 'src/app/shared/ERP/CategoryPosition.service';
import { CategoryHRMChucDanhService } from 'src/app/shared/ERP/CategoryHRMChucDanh.service';
import { CategoryHRMTrinhDoService } from 'src/app/shared/ERP/CategoryHRMTrinhDo.service';
import { CategoryHRMChuyenNganhService } from 'src/app/shared/ERP/CategoryHRMChuyenNganh.service';
import { CategoryHRMDanTocService } from 'src/app/shared/ERP/CategoryHRMDanToc.service';
import { CategoryHRMQuocGiaService } from 'src/app/shared/ERP/CategoryHRMQuocGia.service';
import { CategoryHRMGioiTinhService } from 'src/app/shared/ERP/CategoryHRMGioiTinh.service';
import { CategoryHRMTinhTrangLamViecService } from 'src/app/shared/ERP/CategoryHRMTinhTrangLamViec.service';

@Component({
  selector: 'app-membership-list',
  templateUrl: './membership-list.component.html',
  styleUrls: ['./membership-list.component.css']
})
export class MembershipListComponent implements OnInit {
  @ViewChild('MembershipSort') MembershipSort: MatSort;
  @ViewChild('MembershipPaginator') MembershipPaginator: MatPaginator;

  URLTemplate: string;

  constructor(
    public Router: Router,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public MembershipService: MembershipListService,
    public CompanyService: CompanyService,

    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
    public CategoryPositionService: CategoryPositionService,
    public CategoryHRMChucDanhService: CategoryHRMChucDanhService,
    public CategoryHRMTrinhDoService: CategoryHRMTrinhDoService,
    public CategoryHRMChuyenNganhService: CategoryHRMChuyenNganhService,
    public CategoryHRMDanTocService: CategoryHRMDanTocService,
    public CategoryHRMQuocGiaService: CategoryHRMQuocGiaService,
    public CategoryHRMGioiTinhService: CategoryHRMGioiTinhService,
    public CategoryHRMTinhTrangLamViecService: CategoryHRMTinhTrangLamViecService,
  ) {

    this.MembershipService.BaseParameter.Active = false;
    this.MembershipService.BaseParameter.Active001 = true;
    this.MembershipService.BaseParameter.Active003 = true;

    this.URLTemplate = this.MembershipService.APIRootURL + 'Download/Employee.xlsx';
    this.MembershipService.BaseParameter.Action = 0;
    this.CompanySearch();

    this.CategoryHRMPhongBanService.ComponentGetByActiveToListAsync(this.MembershipService);
    this.CategoryPositionService.ComponentGetByActiveToListAsync(this.MembershipService);
    this.CategoryHRMChucDanhService.ComponentGetByActiveToListAsync(this.MembershipService);
    this.CategoryHRMTrinhDoService.ComponentGetByActiveToListAsync(this.MembershipService);
    this.CategoryHRMChuyenNganhService.ComponentGetByActiveToListAsync(this.MembershipService);
    this.CategoryHRMDanTocService.ComponentGetByActiveToListAsync(this.MembershipService);
    this.CategoryHRMQuocGiaService.ComponentGetByActiveToListAsync(this.MembershipService);
    this.CategoryHRMGioiTinhService.ComponentGetByActiveToListAsync(this.MembershipService);
    this.CategoryHRMTinhTrangLamViecService.ComponentGetByActiveToListAsync(this.MembershipService);

  }
  ngOnInit(): void {

  }

  ngAfterViewInit() {
    this.MembershipSearch();
  }

  CompanySearch() {
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      (res) => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.MembershipService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          this.MembershipActionChange(0);
        }
      },
      (err) => { },
      () => { },
    );
  }

  MembershipActionChange(Action: number) {
    this.MembershipService.BaseParameter.Action = Action;
    this.MembershipService.BaseParameter.SearchString = environment.InitializationString;
    this.MembershipSearch();
  }

  MembershipPrint() {
    this.MembershipService.BaseParameter.List = this.MembershipService.List.filter(o => o.IsEdit == true);
    if (this.MembershipService.BaseParameter.List.length == 0) {
      this.NotificationService.warn('Vui lòng chọn dòng cần in  (Edit)');
      return;
    }
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.PrintByListAsync().subscribe(
      (res) => {
        this.MembershipService.BaseResult = res as BaseResult;
        if (this.MembershipService.BaseResult.Message && this.MembershipService.BaseResult.Message.length > 0) {
          this.NotificationService.OpenWindowByURL(this.MembershipService.BaseResult.Message, 200, 400);
        }
      },
      (err) => { },
      () => {
        this.MembershipService.IsShowLoading = false;
      },
    );
    this.MembershipService.BaseParameter.List = [];
  }

  MembershipSearch() {
    if (this.MembershipService.BaseParameter.SearchString.length > 0) {
      const SearchString = this.MembershipService.BaseParameter.SearchString.trim().toLowerCase();
      this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.UserName && o.UserName.toLowerCase().includes(SearchString));
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.Name && o.Name.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.Display && o.Display.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.FileName && o.FileName.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.CategoryHRMKhoiName && o.CategoryHRMKhoiName.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.CategoryHRMPhongBanName && o.CategoryHRMPhongBanName.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => this.GetDepartmentKoreanDisplay(o) && this.GetDepartmentKoreanDisplay(o).toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.CategoryHRMChucDanhName && o.CategoryHRMChucDanhName.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => this.GetPositionDisplay(o) && this.GetPositionDisplay(o).toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.Rank && o.Rank.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.RankKorean && o.RankKorean.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.CategoryHRMGioiTinhName && o.CategoryHRMGioiTinhName.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.CategoryHRMTrinhDoName && o.CategoryHRMTrinhDoName.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.CategoryHRMChuyenNganhName && o.CategoryHRMChuyenNganhName.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.EmploymentType && o.EmploymentType.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.CCCDNoiSinh && o.CCCDNoiSinh.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.NguyenQuanName && o.NguyenQuanName.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.CategoryHRMQuocGiaName && o.CategoryHRMQuocGiaName.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.CategoryHRMDanTocName && o.CategoryHRMDanTocName.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.CCCD && o.CCCD.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.CCCDNoiCap && o.CCCDNoiCap.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.CCCDiaChi && o.CCCDiaChi.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.DiaChi && o.DiaChi.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.Phone && o.Phone.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.Email && o.Email.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.CategoryHRMTinhTrangLamViecName && o.CategoryHRMTinhTrangLamViecName.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.LyDoNghiViec && o.LyDoNghiViec.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.Certificate && o.Certificate.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.OverseasExperience && o.OverseasExperience.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.ForeignLanguageLevel && o.ForeignLanguageLevel.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.ContractCondition && o.ContractCondition.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.RewardsDiscipline && o.RewardsDiscipline.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.PromotionHistory && o.PromotionHistory.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.Other && o.Other.toLowerCase().includes(SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.Note && o.Note.toLowerCase().includes(SearchString));
      }
      this.MembershipService.DataSource = new MatTableDataSource(this.MembershipService.List);
      this.MembershipService.DataSource.sort = this.MembershipSort;
      this.MembershipService.DataSource.paginator = this.MembershipPaginator;
    } else {
      this.MembershipService.IsShowLoading = true;
      this.MembershipService.GetByCompanyIDToList20260915Async().subscribe(
        (res) => {
          this.MembershipService.BaseResult = res as BaseResult;
          this.MembershipService.ListFilter = this.MembershipService.BaseResult.List.sort((a, b) => (a.UserName > b.UserName ? 1 : -1));
          this.MembershipService.List = this.MembershipService.ListFilter;
          this.MembershipService.DataSource = new MatTableDataSource(this.MembershipService.List);
          this.MembershipService.DataSource.sort = this.MembershipSort;
          this.MembershipService.DataSource.paginator = this.MembershipPaginator;
        },
        (err) => { },
        () => {
          this.MembershipService.IsShowLoading = false;
        },
      );
    }
  }

  MembershipExcel() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.GetByCompanyIDToExcel20260905Async().subscribe(
      (res) => {
        let url = (res as BaseResult).Message;
        window.open(url, '_blank');
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      },
    );
  }

  MembershipTranslate() {
    if (!confirm('Dịch tiếng Hàn cho toàn bộ nhân viên ? | 한국어로 번역하시겠습니까?')) {
      return;
    }
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.TranslateKoreanAsync().subscribe(
      (res) => {
        this.MembershipSearch();
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

  


  SyncFileName() {
    if (!confirm('Dịch tiếng Hàn cho toàn bộ nhân viên ? | 한국어로 번역하시겠습니까?')) {
      return;
    }
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.SyncFileName().subscribe(
      (res) => {
        this.MembershipSearch();
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

  MembershipListSave() {
    this.MembershipService.BaseParameter.List = this.MembershipService.List.filter(o => o.IsEdit == true);
    if (this.MembershipService.BaseParameter.List.length == 0) {
      this.NotificationService.warn('Vui lòng chọn dòng cần lưu  (Edit)');
      return;
    }
    this.MembershipService.BaseParameter.BaseModel = null;
    this.MembershipService.BaseParameter.Event = null;
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.SaveList2026Async().subscribe(
      (res) => {
        this.MembershipService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MembershipService.List.filter(o => o.IsEdit == true).forEach(o => o.IsEdit = false);
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

  MembershipListModal(element: Membership) {
    this.Dialog.open(MemberShipListModalComponent, {
      data: element,
    }).afterClosed().subscribe(result => {
      this.MembershipSearch();
    });
  }

  MembershipListModalAdd() {
    this.Dialog.open(MemberShipListModalComponent, {
    }).afterClosed().subscribe(result => {
      this.MembershipSearch();
    });
  }

  MembershipListModal01() {
    this.Dialog.open(MemberShipListModal01Component, {
      width: '90vw',
      maxWidth: '95vw',
      panelClass: 'membership-excel-dialog',
    }).afterClosed().subscribe(result => {
      this.MembershipSearch();
    });
  }

  MembershipListModal02() {
    this.Dialog.open(MemberShipListModal02Component, {
      width: '700px',
    }).afterClosed().subscribe(result => {
      this.MembershipSearch();
    });
  }

  GetDepartmentKoreanDisplay(element: Membership): string {
    if (element.CategoryHRMPhongBanID && this.CategoryHRMPhongBanService.ListFilter) {
      if (this.CategoryHRMPhongBanService.ListFilter.find(o => o.ID == element.CategoryHRMPhongBanID)) {
        return this.CategoryHRMPhongBanService.ListFilter.find(o => o.ID == element.CategoryHRMPhongBanID).Display;
      }
    }
    return element.DepartmentKorean;
  }

  GetPositionDisplay(element: Membership): string {
    if (element.CategoryPositionID && this.CategoryPositionService.ListFilter) {
      if (this.CategoryPositionService.ListFilter.find(o => o.ID == element.CategoryPositionID)) {
        return this.CategoryPositionService.ListFilter.find(o => o.ID == element.CategoryPositionID).Display;
      }
    }
    return element.RankKorean;
  }

  MembershipChange(event, files: FileList) {
    if (files) {
      this.MembershipService.FileToUpload = files;
      this.MembershipService.BaseParameter.Event = event;
    }
  }

  MembershipUpload() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.SaveAndUploadFileAsync().subscribe(
      (res) => {
        this.MembershipSearch();
        this.MembershipService.FileToUpload = null;
        this.MembershipService.BaseParameter.Event = null;
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
}
