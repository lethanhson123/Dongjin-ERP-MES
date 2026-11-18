import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { HRMCaLamViecKeHoachService } from 'src/app/shared/ERP/HRMCaLamViecKeHoach.service';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';
import { CategoryHRMCaLamViecService } from 'src/app/shared/ERP/CategoryHRMCaLamViec.service';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';

@Component({
  selector: 'app-hrm-ca-lam-viec-ke-hoach-modal',
  templateUrl: './hrm-ca-lam-viec-ke-hoach-modal.component.html',
  styleUrls: ['./hrm-ca-lam-viec-ke-hoach-modal.component.css'],
})
export class HrmCaLamViecKeHoachModalComponent implements OnInit {
  DateBegin: Date | null = null;
  DateEnd: Date | null = null;
  SelectedMembershipIDs: number[] = [];
  DepartmentSearchString = '';
  DepartmentListFiltered: any[] = [];

  get DepartmentOptions(): any[] {
    return [{ ID: 0, Name: 'All', Code: 'All' }, ...(this.DepartmentListFiltered || [])];
  }

  FilterDepartments(value: string = '') {
    this.DepartmentSearchString = (value || '').trim().toLowerCase();
    const list = this.CategoryHRMPhongBanService.ListFilter || [];
    this.DepartmentListFiltered = list.filter((item) => {
      const text = `${item.Name || ''} ${item.Code || ''} ${item.Display || ''}`.toLowerCase();
      return !this.DepartmentSearchString || text.includes(this.DepartmentSearchString);
    });
  }

  ClearDepartmentSearch() {
    this.DepartmentSearchString = '';
    this.FilterDepartments('');
  }

  SelectDepartment(id: any) {
    this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CategoryHRMPhongBanID = Number(id) || 0;
    this.CategoryHRMPhongBanChange();
  }

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<HrmCaLamViecKeHoachModalComponent>,
    public NotificationService: NotificationService,
    public HRMCaLamViecKeHoachService: HRMCaLamViecKeHoachService,
    public CompanyService: CompanyService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
    public CategoryHRMCaLamViecService: CategoryHRMCaLamViecService,
    public MembershipService: MembershipService,
  ) {
    if (!this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel) {
      this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel = { ID: 0, Active: true };
    }
    if (this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.ID == 0) {
      this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.Active = true;
      this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CreateUserID = Number(localStorage.getItem(environment.UserID));
      this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CreateUserCode = localStorage.getItem(environment.UserName);
      this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CreateUserName = localStorage.getItem(environment.FullName);
    }
    const currentDate = this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.Date
      ? new Date(this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.Date)
      : new Date();
    this.DateBegin = new Date(currentDate);
    this.DateEnd = new Date(currentDate);

    if (this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.ID === 0 && this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.MembershipID > 0) {
      this.SelectedMembershipIDs = [this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.MembershipID];
    }
    this.CompanySearch();
    this.CategoryHRMCaLamViecSearch();
    if (this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.ID === 0
      && !this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CategoryHRMCaLamViecID) {
      this.SetFirstWorkShiftWhenLoaded();
    }
    this.MembershipService.ListChild = this.MembershipService.ListParent || [];
    if (this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CategoryHRMCaLamViecID > 0) {
      this.CategoryHRMCaLamViecChange();
    }
  }

  ngOnInit(): void {}

  Close() {
    this.DialogRef.close();
  }

  DateBeginChange(value: string) {
    this.DateBegin = value ? new Date(value) : null;
    this.ValidateDateRange();
  }

  DateEndChange(value: string) {
    this.DateEnd = value ? new Date(value) : null;
    this.ValidateDateRange();
  }

  private ValidateDateRange() {
    if (this.DateBegin && this.DateEnd && this.DateEnd < this.DateBegin) {
      this.NotificationService.warn('Ngày kết thúc không được nhỏ hơn ngày bắt đầu.');
      this.DateEnd = new Date(this.DateBegin);
    }
  }

  CompanySearch() {
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      (res) => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if ((!this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CompanyID || !this.CompanyService.ListFilter.some((o) => o.ID == this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CompanyID)) && this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CompanyID = this.CompanyService.ListFilter[0].ID;
        }
        this.CategoryHRMPhongBanSearch();
        this.MembershipSearch();
        this.FilterDepartments('');
      },
      (err) => {},
      () => {},
    );
  }

  CompanyChange() {
    this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CategoryHRMPhongBanID = 0;
    this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CategoryHRMPhongBanName = null;
    this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.MembershipID = 0;
    this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.MembershipCode = null;
    this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.MembershipName = null;
    this.CategoryHRMPhongBanSearch();
    this.MembershipSearch();
  }

  CategoryHRMCaLamViecSearch() {
    const shiftService = this.CategoryHRMCaLamViecService;
    if (shiftService.ListFilter && shiftService.ListFilter.length > 0) {
      this.SetFirstWorkShiftWhenLoaded();
      return;
    }

    shiftService.GetByActiveToListAsync().subscribe(
      (res) => {
        shiftService.ListFilter = ((res as BaseResult).List || [])
          .sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        shiftService.ListFilter01 = shiftService.ListFilter;
        shiftService.ListFilter02 = shiftService.ListFilter;
        shiftService.ListFilter03 = shiftService.ListFilter;
        this.SetFirstWorkShiftWhenLoaded();
      },
      () => {},
    );
  }

  private SetFirstWorkShiftWhenLoaded() {
    const baseModel = this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel;
    const shifts = this.CategoryHRMCaLamViecService.ListFilter || [];
    if (baseModel.ID !== 0 || baseModel.CategoryHRMCaLamViecID || shifts.length === 0) {
      return;
    }

    const defaultShift = shifts.find((shift) =>
      (shift.Name || '').trim().toLowerCase() === 'ca hành chính'
    ) || shifts[0];

    baseModel.CategoryHRMCaLamViecID = defaultShift.ID;
    this.CategoryHRMCaLamViecChange();
  }

  FormatTime(hour: any, minute: any): string {
    const formattedHour = String(hour ?? 0).padStart(2, '0');
    const formattedMinute = String(minute ?? 0).padStart(2, '0');
    return `${formattedHour}:${formattedMinute}`;
  }

  CategoryHRMCaLamViecChange() {
    this.CategoryHRMCaLamViecService.BaseParameter.ID = this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CategoryHRMCaLamViecID;
    this.CategoryHRMCaLamViecService.GetByIDAsync().subscribe(
      (res) => {
        this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CategoryHRMCaLamViecName = (res as BaseResult).BaseModel.Name;
        this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.Quantity01 = (res as BaseResult).BaseModel.Quantity01;
        this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.Quantity02 = (res as BaseResult).BaseModel.Quantity02;
        this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.Quantity03 = (res as BaseResult).BaseModel.Quantity03;
        this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.Quantity04 = (res as BaseResult).BaseModel.Quantity04;
      },
      (err) => {},
      () => {},
    );
  }

  CategoryHRMPhongBanSearch() {
    this.CategoryHRMPhongBanService.ListFilter = this.CategoryHRMPhongBanService.ListFilter || [];
    this.HRMCaLamViecKeHoachService.IsShowLoading = true;
    this.CategoryHRMPhongBanService.BaseParameter.Active = true;
    this.CategoryHRMPhongBanService.BaseParameter.CompanyID = this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CompanyID;
    this.CategoryHRMPhongBanService.GetByCompanyIDAndActiveToListAsync().subscribe(
      (res) => {
        this.CategoryHRMPhongBanService.ListFilter = ((res as BaseResult).List || []).sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.FilterDepartments(this.DepartmentSearchString);
        const BaseModel = this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel;
        if (BaseModel.CategoryHRMPhongBanID > 0 && !(this.CategoryHRMPhongBanService.ListFilter || []).some((o) => o.ID == BaseModel.CategoryHRMPhongBanID)) {
          this.CategoryHRMPhongBanService.ListFilter.unshift({
            ID: BaseModel.CategoryHRMPhongBanID,
            Name: BaseModel.CategoryHRMPhongBanName,
            Code: BaseModel.CategoryHRMPhongBanName,
            Active: true,
          });
        }
      },
      (err) => {},
      () => {
        this.HRMCaLamViecKeHoachService.IsShowLoading = false;
      },
    );
  }

  CategoryHRMPhongBanChange() {
    this.CategoryHRMPhongBanService.ListFilter = this.CategoryHRMPhongBanService.ListFilter || [];
    const BaseModel = this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel;
    let item = (this.CategoryHRMPhongBanService.ListFilter || []).find((o) => o.ID == BaseModel.CategoryHRMPhongBanID);
    if (!item && BaseModel.CategoryHRMPhongBanID > 0) {
      item = { ID: BaseModel.CategoryHRMPhongBanID, Name: BaseModel.CategoryHRMPhongBanName, Code: BaseModel.CategoryHRMPhongBanName, Active: true };
      this.CategoryHRMPhongBanService.ListFilter.unshift(item);
    }
    BaseModel.CategoryHRMPhongBanID = (item || {}).ID || BaseModel.CategoryHRMPhongBanID || 0;
    BaseModel.CategoryHRMPhongBanName = (item || {}).Name || (item || {}).Code || BaseModel.CategoryHRMPhongBanName || null;

    this.MembershipFilter(this.HRMCaLamViecKeHoachService.BaseParameter.SearchString || '');
  }

  SelectAllMemberships() {
    this.SelectedMembershipIDs = (this.MembershipService.ListChild || []).map((item) => Number(item.ID));
    this.MembershipFilter(this.HRMCaLamViecKeHoachService.BaseParameter.SearchString || '');
  }

  ClearMemberships() {
    this.SelectedMembershipIDs = [];
    this.MembershipFilter(this.HRMCaLamViecKeHoachService.BaseParameter.SearchString || '');
  }

  IsMembershipSelected(id: any): boolean {
    return this.SelectedMembershipIDs.indexOf(Number(id)) >= 0;
  }

  MembershipToggle(id: any) {
    const membershipID = Number(id);
    if (this.IsMembershipSelected(membershipID)) {
      this.SelectedMembershipIDs = this.SelectedMembershipIDs.filter((selectedID) => selectedID !== membershipID);
    } else {
      this.SelectedMembershipIDs = [...this.SelectedMembershipIDs, membershipID];
    }
    this.MembershipFilter(this.HRMCaLamViecKeHoachService.BaseParameter.SearchString || '');
  }

  MembershipChange(MembershipID: any) {
    const MembershipIDs = Array.isArray(MembershipID) ? MembershipID : [MembershipID];
    this.SelectedMembershipIDs = MembershipIDs.filter((id) => Number(id) > 0).map((id) => Number(id));
    this.MembershipFilter(this.HRMCaLamViecKeHoachService.BaseParameter.SearchString || '');

    if (this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.ID > 0) {
      const item = (this.MembershipService.ListParent || []).find((o) => o.ID == this.SelectedMembershipIDs[0]);
      if (item) {
        this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.MembershipID = item.ID;
        this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.MembershipCode = item.UserName;
        this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.MembershipName = item.Name;
        this.CategoryHRMPhongBanChange();
      }
    }
  }

  MembershipSearch() {
    this.MembershipService.ListChild = this.MembershipService.ListParent;
    const CompanyID = this.HRMCaLamViecKeHoachService.BaseParameter.CompanyID;
    const MembershipID = this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.MembershipID;
    const HasMembershipInList = (this.MembershipService.ListParent || []).some((o) => o.ID == MembershipID);
    if (!this.MembershipService.ListParent || this.MembershipService.ListParent.length == 0 || (MembershipID > 0 && !HasMembershipInList)) {
      this.MembershipService.IsShowLoading = true;
      this.MembershipService.BaseParameter.CompanyID = CompanyID;
      this.MembershipService.GetByCompanyIDAndActiveToListAsync().subscribe(
        (res) => {
          this.MembershipService.BaseResult = res as BaseResult;
          this.MembershipService.ListParent = (this.MembershipService.BaseResult.List || []).sort((a, b) => (a.UserName > b.UserName ? 1 : -1));
          this.MembershipService.ListChild = this.MembershipService.ListParent;
          if (MembershipID > 0) {
            this.MembershipChange(MembershipID);
          }
        },
        (err) => {},
        () => {
          this.MembershipService.IsShowLoading = false;
        },
      );
    } else if (MembershipID > 0) {
      this.MembershipChange(MembershipID);
    }
  }

  MembershipFilter(SearchString: string) {
    const search = (SearchString || '').trim().toLowerCase();
    const selectedIDs = new Set((this.SelectedMembershipIDs || []).map((id) => Number(id)));
    const list = this.MembershipService.ListParent || [];

    const departmentID = Number(this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.CategoryHRMPhongBanID || 0);
    this.MembershipService.ListChild = list
      .filter((o) => {
        const departmentMatch = departmentID === 0 || Number(o.CategoryHRMPhongBanID || 0) === departmentID;
        const text = `${o.UserName || ''} ${o.Name || ''} ${o.Search || ''} ${o.Display || ''} ${o.CategoryHRMPhongBanName || ''}`.toLowerCase();
        return departmentMatch && (!search || text.includes(search));
      })
      .sort((a, b) => {
        const aSelected = selectedIDs.has(Number(a.ID));
        const bSelected = selectedIDs.has(Number(b.ID));
        if (aSelected !== bSelected) {
          return aSelected ? -1 : 1;
        }
        return (a.UserName || '').localeCompare(b.UserName || '');
      });
  }

  Save() {
    const BaseModel = this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel;
    if (!this.DateBegin || !this.DateEnd) {
      this.NotificationService.warn('Vui lòng chọn khoảng thời gian.');
      return;
    }
    if (this.DateEnd < this.DateBegin) {
      this.NotificationService.warn('Ngày kết thúc không được nhỏ hơn ngày bắt đầu.');
      return;
    }

    BaseModel.Date = new Date(this.DateBegin);
    this.HRMCaLamViecKeHoachService.BaseParameter.DateBegin = new Date(this.DateBegin);
    this.HRMCaLamViecKeHoachService.BaseParameter.DateEnd = new Date(this.DateEnd);
    (BaseModel as any).DateBegin = new Date(this.DateBegin);
    (BaseModel as any).DateEnd = new Date(this.DateEnd);

    const MembershipIDs = this.SelectedMembershipIDs.length > 0
      ? this.SelectedMembershipIDs
      : (BaseModel.MembershipID > 0 ? [BaseModel.MembershipID] : []);

    if (MembershipIDs.length === 0) {
      this.NotificationService.warn('Vui lòng chọn nhân viên.');
      return;
    }

    const List: any[] = [];
    const firstDate = new Date(this.DateBegin);
    const lastDate = new Date(this.DateEnd);
    for (const MembershipID of MembershipIDs) {
      const item = (this.MembershipService.ListParent || []).find((o) => o.ID == MembershipID);
      for (const date = new Date(firstDate); date <= lastDate; date.setUTCDate(date.getUTCDate() + 1)) {
        List.push({
          ...BaseModel,
          ID: 0,
          Date: new Date(date),
          MembershipID,
          MembershipCode: item ? item.UserName : BaseModel.MembershipCode,
          MembershipName: item ? item.Name : BaseModel.MembershipName,
          CategoryHRMPhongBanID: item && item.CategoryHRMPhongBanID
            ? item.CategoryHRMPhongBanID
            : BaseModel.CategoryHRMPhongBanID,
          CategoryHRMPhongBanName: item && item.CategoryHRMPhongBanName
            ? item.CategoryHRMPhongBanName
            : BaseModel.CategoryHRMPhongBanName,
        });
      }
    }

    this.HRMCaLamViecKeHoachService.IsShowLoading = true;
    const request = this.HRMCaLamViecKeHoachService.SaveManyAsync(List);

    request.subscribe(
      (res) => {
        if (MembershipIDs.length === 1) {
          this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        }
        this.NotificationService.warn((res as BaseResult).Message || environment.SaveSuccess);
        this.DialogRef.close(true);
      },
      () => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.HRMCaLamViecKeHoachService.IsShowLoading = false;
      },
    );
  }

  Delete() {
    if (confirm(environment.DeleteConfirm)) {
      this.HRMCaLamViecKeHoachService.IsShowLoading = true;
      this.HRMCaLamViecKeHoachService.BaseParameter.ID = this.HRMCaLamViecKeHoachService.BaseParameter.BaseModel.ID;
      this.HRMCaLamViecKeHoachService.RemoveAsync().subscribe(
        (res) => {
          this.NotificationService.warn(environment.DeleteSuccess);
          this.DialogRef.close(true);
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
}
