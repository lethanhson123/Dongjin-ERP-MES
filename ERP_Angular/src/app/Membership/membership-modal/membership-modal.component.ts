import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';

import { MembershipCompany } from 'src/app/shared/ERP/MembershipCompany.model';
import { MembershipCompanyService } from 'src/app/shared/ERP/MembershipCompany.service';

import { MembershipMenu } from 'src/app/shared/ERP/MembershipMenu.model';
import { MembershipMenuService } from 'src/app/shared/ERP/MembershipMenu.service';

import { MembershipDepartment } from 'src/app/shared/ERP/MembershipDepartment.model';
import { MembershipDepartmentService } from 'src/app/shared/ERP/MembershipDepartment.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';
import { CategoryPosition } from 'src/app/shared/ERP/CategoryPosition.model';
import { CategoryPositionService } from 'src/app/shared/ERP/CategoryPosition.service';
import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';



@Component({
  selector: 'app-membership-modal',
  templateUrl: './membership-modal.component.html',
  styleUrls: ['./membership-modal.component.css']
})
export class MembershipModalComponent {

  @ViewChild('MembershipCompanySort') MembershipCompanySort: MatSort;
  @ViewChild('MembershipCompanyPaginator') MembershipCompanyPaginator: MatPaginator;

  @ViewChild('MembershipMenuSort') MembershipMenuSort: MatSort;
  @ViewChild('MembershipMenuPaginator') MembershipMenuPaginator: MatPaginator;

  @ViewChild('MembershipDepartmentSort') MembershipDepartmentSort: MatSort;
  @ViewChild('MembershipDepartmentPaginator') MembershipDepartmentPaginator: MatPaginator;

  IsMembershipMenuActiveAll: boolean = false;

  IsMembershipCompanyActiveAll: boolean = false;

  IsMembershipDepartmentActiveAll: boolean = false;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<MembershipModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public MembershipService: MembershipService,
    public MembershipCompanyService: MembershipCompanyService,
    public MembershipMenuService: MembershipMenuService,
    public MembershipDepartmentService: MembershipDepartmentService,

    public CategoryDepartmentService: CategoryDepartmentService,
    public CategoryPositionService: CategoryPositionService,
    public CompanyService: CompanyService,

  ) {
    this.CompanySearch();
    this.CategoryDepartmentSearch();
    this.CategoryPositionSearch();
  }
  ngOnInit(): void {
  }
  ngAfterViewInit() {
    this.MembershipMenuSearch();
    this.MembershipCompanySearch();
    this.MembershipDepartmentSearch();
  }
  CompanySearch() {
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.MembershipService.BaseParameter.BaseModel.CompanyID;
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.MembershipService);
  }
  CategoryPositionSearch() {
    this.CategoryPositionService.BaseParameter.Active = true;
    this.CategoryPositionService.ComponentGetByActiveToListAsync(this.MembershipService);
  }
  Close() {
    this.DialogRef.close();
  }
  Save() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.BaseResult.Message = environment.InitializationString;
    this.MembershipService.IsPasswordValidWithRegex().subscribe(
      res => {
        this.MembershipService.BaseResult = (res as BaseResult);
        let IsCheck = false;
        if (this.MembershipService.BaseResult) {
          if (this.MembershipService.BaseResult.Count > 0) {
            IsCheck = true;
            this.SaveSub();
          }
        }
        if (IsCheck == false) {
          this.MembershipService.BaseResult.Message = environment.IsPasswordValidWithRegex;
          this.NotificationService.warn(this.MembershipService.BaseResult.Message);
        }
      },
      err => {
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }
  SaveSub() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.SaveAsync().subscribe(
      res => {
        this.MembershipService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }
  SavePasswordReset() {
    this.MembershipService.BaseParameter.IsComplete = true;
    this.SaveSub();
  }

  MembershipMenuSearch() {
    if (this.MembershipMenuService.BaseParameter.SearchString.length > 0) {
      this.MembershipMenuService.BaseParameter.SearchString = this.MembershipMenuService.BaseParameter.SearchString.trim();
      if (this.MembershipCompanyService.DataSource) {
        this.MembershipMenuService.DataSource.filter = this.MembershipMenuService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.MembershipService.IsShowLoading = true;
      this.MembershipMenuService.BaseParameter.ParentID = this.MembershipService.BaseParameter.BaseModel.ID;
      this.MembershipMenuService.GetByParentIDToListAsync().subscribe(
        res => {
          this.MembershipMenuService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));;
          this.MembershipMenuService.DataSource = new MatTableDataSource(this.MembershipMenuService.List);
          this.MembershipMenuService.DataSource.sort = this.MembershipMenuSort;
          this.MembershipMenuService.DataSource.paginator = this.MembershipMenuPaginator;
        },
        err => {
        },
        () => {
          this.MembershipService.IsShowLoading = false;
        }
      );
    }
  }
  MembershipCompanySearch() {
    if (this.MembershipCompanyService.BaseParameter.SearchString.length > 0) {
      this.MembershipCompanyService.BaseParameter.SearchString = this.MembershipCompanyService.BaseParameter.SearchString.trim();
      if (this.MembershipCompanyService.DataSource) {
        this.MembershipCompanyService.DataSource.filter = this.MembershipCompanyService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.MembershipService.IsShowLoading = true;
      this.MembershipCompanyService.BaseParameter.ParentID = this.MembershipService.BaseParameter.BaseModel.ID;
      this.MembershipCompanyService.GetByParentIDToListAsync().subscribe(
        res => {
          this.MembershipCompanyService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));;
          this.MembershipCompanyService.DataSource = new MatTableDataSource(this.MembershipCompanyService.List);
          this.MembershipCompanyService.DataSource.sort = this.MembershipCompanySort;
          this.MembershipCompanyService.DataSource.paginator = this.MembershipCompanyPaginator;
        },
        err => {
        },
        () => {
          this.MembershipService.IsShowLoading = false;
        }
      );
    }
  }
  MembershipDepartmentSearch() {
    if (this.MembershipDepartmentService.BaseParameter.SearchString.length > 0) {
      this.MembershipDepartmentService.BaseParameter.SearchString = this.MembershipDepartmentService.BaseParameter.SearchString.trim();
      if (this.MembershipDepartmentService.DataSource) {
        this.MembershipDepartmentService.DataSource.filter = this.MembershipDepartmentService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.MembershipService.IsShowLoading = true;
      this.MembershipDepartmentService.BaseParameter.ParentID = this.MembershipService.BaseParameter.BaseModel.ID;
      this.MembershipDepartmentService.GetByParentIDToListAsync().subscribe(
        res => {
          this.MembershipDepartmentService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));;
          this.MembershipDepartmentService.DataSource = new MatTableDataSource(this.MembershipDepartmentService.List);
          this.MembershipDepartmentService.DataSource.sort = this.MembershipDepartmentSort;
          this.MembershipDepartmentService.DataSource.paginator = this.MembershipDepartmentPaginator;
        },
        err => {
        },
        () => {
          this.MembershipService.IsShowLoading = false;
        }
      );
    }
  }
  MembershipMenuActiveChange(element: MembershipMenu) {
    this.MembershipService.IsShowLoading = true;
    this.MembershipMenuService.BaseParameter.BaseModel = element;
    this.MembershipMenuService.SaveAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }
  MembershipMenuActiveAllChange() {
    this.MembershipService.IsShowLoading = true;
    for (let i = 0; i < this.MembershipMenuService.List.length; i++) {
      this.MembershipMenuService.List[i].Active = this.IsMembershipMenuActiveAll;
    }
    this.MembershipMenuService.BaseParameter.List = this.MembershipMenuService.List;
    this.MembershipMenuService.SaveListAsync().subscribe(
      res => {
        //this.MembershipMenuSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }
  MembershipCompanyActiveChange(element: MembershipCompany) {
    this.MembershipService.IsShowLoading = true;
    this.MembershipCompanyService.BaseParameter.BaseModel = element;
    this.MembershipCompanyService.SaveAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }
  MembershipCompanyActiveAllChange() {
    this.MembershipService.IsShowLoading = true;
    for (let i = 0; i < this.MembershipCompanyService.List.length; i++) {
      this.MembershipCompanyService.List[i].Active = this.IsMembershipCompanyActiveAll;
    }
    this.MembershipCompanyService.BaseParameter.List = this.MembershipCompanyService.List;
    this.MembershipCompanyService.SaveListAsync().subscribe(
      res => {
        //this.MembershipCompanySearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }
  MembershipDepartmentActiveChange(element: MembershipDepartment) {
    this.MembershipService.IsShowLoading = true;
    this.MembershipDepartmentService.BaseParameter.BaseModel = element;
    this.MembershipDepartmentService.SaveAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }
  MembershipDepartmentActiveAllChange() {
    this.MembershipService.IsShowLoading = true;
    for (let i = 0; i < this.MembershipDepartmentService.List.length; i++) {
      this.MembershipDepartmentService.List[i].Active = this.IsMembershipDepartmentActiveAll;
    }
    this.MembershipDepartmentService.BaseParameter.List = this.MembershipDepartmentService.List;
    this.MembershipDepartmentService.SaveListAsync().subscribe(
      res => {
        //this.MembershipCompanySearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }
}
