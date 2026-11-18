import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { CategoryHRMPhongBan } from 'src/app/shared/ERP/CategoryHRMPhongBan.model';
import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-member-ship-modal03',
  templateUrl: './member-ship-modal03.component.html',
  styleUrls: ['./member-ship-modal03.component.css']
})
export class MemberShipModal03Component implements OnInit {
  @ViewChild('CategoryHRMPhongBanSort') CategoryHRMPhongBanSort: MatSort;
  @ViewChild('CategoryHRMPhongBanPaginator') CategoryHRMPhongBanPaginator: MatPaginator;

  constructor(
    public DialogRef: MatDialogRef<MemberShipModal03Component>,
    public NotificationService: NotificationService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
    public CompanyService: CompanyService,
  ) {
    this.CompanySearch();
  }

  ngOnInit(): void {
  }

  CompanySearch() {
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      (res) => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.CategoryHRMPhongBanService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          this.CategoryHRMPhongBanSearch();
        }
      },
      (err) => {},
      () => {},
    );
  }

  CategoryHRMPhongBanSearch() {
    this.CategoryHRMPhongBanService.IsShowLoading = true;
    this.CategoryHRMPhongBanService.GetByCompanyIDAndEmptyToListAsync().subscribe(
      (res) => {
        this.CategoryHRMPhongBanService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.CategoryHRMPhongBanService.ListFilter = this.CategoryHRMPhongBanService.List;
        this.CategoryHRMPhongBanFilter();
      },
      (err) => {
      },
      () => {
        this.CategoryHRMPhongBanService.IsShowLoading = false;
      },
    );
  }

  CategoryHRMPhongBanFilter() {
    this.CategoryHRMPhongBanService.CategoryHRMPhongBanSearchString = this.CategoryHRMPhongBanService.BaseParameter.SearchString;
    this.CategoryHRMPhongBanService.CategoryHRMPhongBanFilter();
    this.CategoryHRMPhongBanService.DataSource = new MatTableDataSource(this.CategoryHRMPhongBanService.ListCategoryHRMPhongBanFiltered);
    this.CategoryHRMPhongBanService.DataSource.sort = this.CategoryHRMPhongBanSort;
    this.CategoryHRMPhongBanService.DataSource.paginator = this.CategoryHRMPhongBanPaginator;
  }

  CategoryHRMPhongBanSave(element: CategoryHRMPhongBan) {
    this.CategoryHRMPhongBanService.IsShowLoading = true;
    element.CompanyID = this.CategoryHRMPhongBanService.BaseParameter.CompanyID;
    this.CategoryHRMPhongBanService.BaseParameter.BaseModel = element;
    this.CategoryHRMPhongBanService.SaveAsync().subscribe(
      (res) => {
        this.CategoryHRMPhongBanService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.NotificationService.warn(environment.SaveSuccess);
        this.CategoryHRMPhongBanSearch();
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategoryHRMPhongBanService.IsShowLoading = false;
      },
    );
  }

  MembershipListModalClose() {
    this.DialogRef.close(true);
  }
}
