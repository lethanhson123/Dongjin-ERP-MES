import { MembershipFileService } from './../../shared/ERP/MembershipFile.service';
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
import { MembershipService } from 'src/app/shared/ERP/Membership.service';
import { MembershipContact } from 'src/app/shared/ERP/MembershipContact.model';
import { MembershipContactService } from 'src/app/shared/ERP/MembershipContact.service';
import { MembershipFile } from 'src/app/shared/ERP/MembershipFile.model';

import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';
import { CategoryHRMTinhTrangLamViecService } from 'src/app/shared/ERP/CategoryHRMTinhTrangLamViec.service';
import { CategoryHRMChucDanhService } from 'src/app/shared/ERP/CategoryHRMChucDanh.service';
import { CategoryHRMDanTocService } from 'src/app/shared/ERP/CategoryHRMDanToc.service';
import { CategoryHRMHonNhanService } from 'src/app/shared/ERP/CategoryHRMHonNhan.service';
import { CategoryHRMTrinhDoService } from 'src/app/shared/ERP/CategoryHRMTrinhDo.service';
import { CategoryHRMChuyenNganhService } from 'src/app/shared/ERP/CategoryHRMChuyenNganh.service';
import { CategoryHRMQuocGiaService } from 'src/app/shared/ERP/CategoryHRMQuocGia.service';
import { CategoryHRMTinhThanhService } from 'src/app/shared/ERP/CategoryHRMTinhThanh.service';
import { CategoryHRMXaPhuongService } from 'src/app/shared/ERP/CategoryHRMXaPhuong.service';
import { CategoryHRMGioiTinhService } from 'src/app/shared/ERP/CategoryHRMGioiTinh.service';
import { CategoryHRMTaiLieuService } from 'src/app/shared/ERP/CategoryHRMTaiLieu.service';
import { CategoryHRMCaLamViecService } from 'src/app/shared/ERP/CategoryHRMCaLamViec.service';
import { CategoryProcessService } from 'src/app/shared/ERP/CategoryProcess.service';

import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { CategoryPosition } from 'src/app/shared/ERP/CategoryPosition.model';
import { CategoryPositionService } from 'src/app/shared/ERP/CategoryPosition.service';

@Component({
  selector: 'app-membership',
  templateUrl: './membership.component.html',
  styleUrls: ['./membership.component.css'],
})
export class MembershipComponent implements OnInit {
  @ViewChild('MembershipSort') MembershipSort: MatSort;
  @ViewChild('MembershipPaginator') MembershipPaginator: MatPaginator;

  @ViewChild('MembershipFileSort') MembershipFileSort: MatSort;
  @ViewChild('MembershipFilePaginator') MembershipFilePaginator: MatPaginator;

  @ViewChild('MembershipContactSort') MembershipContactSort: MatSort;
  @ViewChild('MembershipContactPaginator') MembershipContactPaginator: MatPaginator;

  URLTemplate: string;
  ChupAnhURL: string;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,
    public MembershipService: MembershipService,
    public MembershipContactService: MembershipContactService,
    public MembershipFileService: MembershipFileService,
    public CompanyService: CompanyService,
    public CategoryHRMTinhTrangLamViecService: CategoryHRMTinhTrangLamViecService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
    public CategoryHRMChucDanhService: CategoryHRMChucDanhService,
    public CategoryHRMDanTocService: CategoryHRMDanTocService,
    public CategoryHRMHonNhanService: CategoryHRMHonNhanService,
    public CategoryHRMTrinhDoService: CategoryHRMTrinhDoService,
    public CategoryHRMChuyenNganhService: CategoryHRMChuyenNganhService,
    public CategoryHRMQuocGiaService: CategoryHRMQuocGiaService,
    public CategoryHRMTinhThanhService: CategoryHRMTinhThanhService,
    public CategoryHRMXaPhuongService: CategoryHRMXaPhuongService,
    public CategoryHRMGioiTinhService: CategoryHRMGioiTinhService,
    public CategoryHRMTaiLieuService: CategoryHRMTaiLieuService,
    public CategoryProcessService: CategoryProcessService,
    public CategoryHRMCaLamViecService: CategoryHRMCaLamViecService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public CategoryPositionService: CategoryPositionService,
  ) {
    this.URLTemplate = this.MembershipService.APIRootURL + 'Download/Employee.xlsx';
    this.MembershipService.BaseParameter.Action = 1;
    this.MembershipService.BaseParameter.IsShow = false;
    this.MembershipService.BaseParameter.List = [];
    this.MembershipService.BaseParameter.BaseModel.FileName = 'image/noimage.jpg';
    this.ChupAnhURL = environment.ChupAnhURL + '/' + this.MembershipService.BaseParameter.BaseModel.UserName;
    this.CompanySearch();
    this.CategoryHRMTinhTrangLamViecSearch();
    this.CategoryHRMPhongBanSearch();
    this.CategoryHRMChucDanhSearch();
    this.CategoryHRMTrinhDoSearch();
    this.CategoryHRMChuyenNganhSearch();
    this.CategoryHRMHonNhanSearch();
    this.CategoryHRMDanTocSearch();
    this.CategoryHRMQuocGiaSearch();
    this.CategoryHRMTinhThanhSearch();
    this.CategoryHRMXaPhuongSearch();
    this.CategoryHRMGioiTinhSearch();
    this.CategoryHRMTaiLieuSearch();
    this.CategoryHRMCaLamViecSearch();
    this.CategoryProcessSearch();
    this.CategoryPositionSearch();
  }

  ngOnInit(): void {}
  ngAfterViewInit() {
    this.MembershipSearch();
  }
  ngOnDestroy(): void {}

  CompanySearch() {
    this.MembershipService.BaseParameter.Active = true;
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));

    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      (res) => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.MembershipService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          this.MembershipActionChange(1);
          this.CategoryDepartmentSearch();
        }
      },
      (err) => {},
      () => {},
    );
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.MembershipService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.GetByCompanyIDAndActiveToListAsync().subscribe(
      (res) => {
        this.CategoryDepartmentService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      },
      (err) => {},
      () => {
        this.MembershipService.IsShowLoading = false;
      },
    );
  }
  CategoryPositionSearch() {
    this.CategoryPositionService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  MembershipModal(element: Membership) {
    this.MembershipService.BaseParameter.BaseModel = element;
    this.ChupAnhURL = environment.ChupAnhURL + '/' + this.MembershipService.BaseParameter.BaseModel.UserName;
    this.MembershipContactSearch();
    this.MembershipFileSearch();
  }

  MembershipContactSearch() {
    this.MembershipContactService.BaseParameter.ParentID = this.MembershipService.BaseParameter.BaseModel.ID;
    this.MembershipContactService.GetByParentIDAndEmptyToListAsync().subscribe((res) => {
      this.MembershipContactService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      this.MembershipContactService.ListFilter = this.MembershipContactService.List;
      this.MembershipContactService.DataSource = new MatTableDataSource(this.MembershipContactService.List);
      this.MembershipContactService.DataSource.sort = this.MembershipContactSort;
      this.MembershipContactService.DataSource.paginator = this.MembershipContactPaginator;
    });
  }
  MembershipContactSave(element: MembershipContact) {
    element.ParentID = this.MembershipService.BaseParameter.BaseModel.ID;
    this.MembershipContactService.BaseParameter.BaseModel = element;
    this.MembershipContactService.IsShowLoading = true;
    this.MembershipContactService.SaveAsync().subscribe(
      () => {
        this.NotificationService.warn(environment.SaveSuccess);
        this.MembershipContactSearch();
      },
      () => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipContactService.IsShowLoading = false;
      },
    );
  }
  MembershipContactDelete(element: MembershipContact) {
    this.MembershipContactService.BaseParameter.ID = element.ID;
    this.MembershipContactService.IsShowLoading = true;
    this.MembershipContactService.RemoveAsync().subscribe(
      () => {
        this.NotificationService.warn(environment.DeleteSuccess);
        this.MembershipContactSearch();
      },
      () => {
        this.NotificationService.warn(environment.DeleteNotSuccess);
      },
      () => {
        this.MembershipContactService.IsShowLoading = false;
      },
    );
  }
  CategoryHRMCaLamViecSearch() {
    this.CategoryHRMCaLamViecService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryProcessSearch() {
    this.CategoryProcessService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMTinhTrangLamViecSearch() {
    this.CategoryHRMTinhTrangLamViecService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMPhongBanSearch() {
    this.CategoryHRMPhongBanService.ComponentGetByActiveToListAsync(this.MembershipService);
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
  CategoryHRMHonNhanSearch() {
    this.CategoryHRMHonNhanService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMDanTocSearch() {
    this.CategoryHRMDanTocService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMQuocGiaSearch() {
    this.CategoryHRMQuocGiaService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMTinhThanhSearch() {
    this.CategoryHRMTinhThanhService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMXaPhuongSearch() {
    this.CategoryHRMXaPhuongService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMGioiTinhSearch() {
    this.CategoryHRMGioiTinhService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CategoryHRMTaiLieuSearch() {
    this.CategoryHRMTaiLieuService.ComponentGetByActiveToListAsync(this.MembershipService);
  }

  DateCCCDNgayCap(value) {
    this.MembershipService.BaseParameter.BaseModel.CCCDNgayCap = new Date(value);
  }
  DateCCCDNgayHetHan(value) {
    this.MembershipService.BaseParameter.BaseModel.CCCDNgayHetHan = new Date(value);
  }
  DateNgaySinh(value) {
    this.MembershipService.BaseParameter.BaseModel.NgaySinh = new Date(value);
  }
  DateNgayVaoLam(value) {
    this.MembershipService.BaseParameter.BaseModel.NgayVaoLam = new Date(value);
  }
  DateNgayNghiViec(value) {
    this.MembershipService.BaseParameter.BaseModel.NgayNghiViec = new Date(value);
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

  MembershipChange(event, files: FileList) {
    if (files) {
      this.MembershipService.FileToUpload = files;
      this.MembershipService.BaseParameter.Event = event;
    }
  }

  MembershipFileChange(event, files: FileList) {
    if (files) {
      this.MembershipFileService.FileToUpload = files;
      this.MembershipFileService.BaseParameter.Event = event;
    }
  }
  MembershipFileSearch() {
    if (this.MembershipService.BaseParameter.BaseModel.ID > 0) {
      if (this.MembershipFileService.BaseParameter.SearchString.length > 0) {
        this.MembershipFileService.BaseParameter.SearchString = this.MembershipFileService.BaseParameter.SearchString.trim();
        if (this.MembershipFileService.DataSource) {
          this.MembershipFileService.DataSource.filter = this.MembershipFileService.BaseParameter.SearchString.toLowerCase();
        }
      } else {
        this.MembershipFileService.BaseParameter.ParentID = this.MembershipService.BaseParameter.BaseModel.ID;
        this.MembershipService.IsShowLoading = true;
        this.MembershipFileService.GetByParentIDAndEmptyToListAsync().subscribe(
          (res) => {
            this.MembershipFileService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.MembershipFileService.DataSource = new MatTableDataSource(this.MembershipFileService.List);
            this.MembershipFileService.DataSource.sort = this.MembershipFileSort;
            this.MembershipFileService.DataSource.paginator = this.MembershipFilePaginator;
          },
          (err) => {},
          () => {
            this.MembershipService.IsShowLoading = false;
          },
        );
      }
    } else {
      this.MembershipFileService.List = [];
      this.MembershipFileService.DataSource = new MatTableDataSource(this.MembershipFileService.List);
      this.MembershipFileService.DataSource.sort = this.MembershipFileSort;
      this.MembershipFileService.DataSource.paginator = this.MembershipFilePaginator;
    }
  }
  MembershipFileDelete(element: MembershipFile) {
    this.MembershipFileService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.MembershipService.IsShowLoading = true;
      this.MembershipFileService.RemoveAsync().subscribe(
        (res) => {
          this.MembershipFileSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
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
  MembershipFileSave(element: MembershipFile) {
    this.MembershipService.IsShowLoading = true;
    element.ParentID = this.MembershipService.BaseParameter.BaseModel.ID;
    this.MembershipFileService.BaseParameter.BaseModel = element;
    this.MembershipFileService.SaveAndUploadFilesAsync().subscribe(
      (res) => {
        this.MembershipFileSearch();
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
  MembershipActionChange(Action: number) {
    this.MembershipService.BaseParameter.Action = Action;
    this.MembershipService.BaseParameter.SearchString = environment.InitializationString;
    this.MembershipSearch();
  }
  MembershipSearch() {
    if (this.MembershipService.BaseParameter.SearchString.length > 0) {
      this.MembershipService.BaseParameter.SearchString = this.MembershipService.BaseParameter.SearchString.trim();
      this.MembershipService.BaseParameter.SearchString = this.MembershipService.BaseParameter.SearchString.toLowerCase();
      this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.UserName && o.UserName.toLowerCase().includes(this.MembershipService.BaseParameter.SearchString));
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.Search && o.Search.toLowerCase().includes(this.MembershipService.BaseParameter.SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.Code && o.Code.toLowerCase().includes(this.MembershipService.BaseParameter.SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.Phone && o.Phone.toLowerCase().includes(this.MembershipService.BaseParameter.SearchString));
      }
      if (this.MembershipService.List.length == 0) {
        this.MembershipService.List = this.MembershipService.ListFilter.filter((o) => o.Name && o.Name.toLowerCase().includes(this.MembershipService.BaseParameter.SearchString));
      }
      this.MembershipService.DataSource = new MatTableDataSource(this.MembershipService.List);
      this.MembershipService.DataSource.sort = this.MembershipSort;
      this.MembershipService.DataSource.paginator = this.MembershipPaginator;
    } else {
      this.MembershipService.IsShowLoading = true;
      this.MembershipService.GetByCompanyIDToListAsync().subscribe(
        (res) => {
          this.MembershipService.BaseResult = res as BaseResult;
          this.MembershipService.ListFilter = this.MembershipService.BaseResult.List.sort((a, b) => (a.UserName > b.UserName ? 1 : -1));
          this.MembershipService.List = this.MembershipService.ListFilter;
          this.MembershipService.DataSource = new MatTableDataSource(this.MembershipService.List);
          this.MembershipService.DataSource.sort = this.MembershipSort;
          this.MembershipService.DataSource.paginator = this.MembershipPaginator;
        },
        (err) => {},
        () => {
          this.MembershipService.IsShowLoading = false;
        },
      );
    }
  }
  MembershipActiveAllChange() {
    for (let i = 0; i < this.MembershipService.List.length; i++) {
      this.MembershipService.List[i].IsNew = this.MembershipService.BaseParameter.IsShow;
      if (this.MembershipService.List[i].IsNew == true) {
        this.MembershipService.BaseParameter.List.push(this.MembershipService.List[i]);
      }
    }
  }
  MembershipActiveChange(element: Membership) {
    if (element.IsNew == true) {
      this.MembershipService.BaseParameter.List.push(element);
    }
  }
  MembershipPrint() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.PrintByListAsync().subscribe(
      (res) => {
        this.MembershipService.BaseResult = res as BaseResult;
        if (this.MembershipService.BaseResult.Message && this.MembershipService.BaseResult.Message.length > 0) {
          this.NotificationService.OpenWindowByURL(this.MembershipService.BaseResult.Message, 200, 400);
        }
      },
      (err) => {},
      () => {
        this.MembershipService.IsShowLoading = false;
      },
    );
    this.MembershipService.BaseParameter.List = [];
    for (let i = 0; i < this.MembershipService.List.length; i++) {
      this.MembershipService.List[i].IsNew = false;
    }
  }
  
  MembershipExcel() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.GetByCompanyIDToExcelAsync().subscribe(
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
  MembershipAdd() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.BaseParameter.ID = 0;
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
  MembershipSave() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.BaseParameter.BaseModel.CompanyID = this.MembershipService.BaseParameter.CompanyID;
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
}
