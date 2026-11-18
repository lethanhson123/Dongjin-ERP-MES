import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';


import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';
import { AnnualLeaveRequestService } from 'src/app/shared/RD/AnnualLeaveRequest.service';
import { AnnualLeaveType } from 'src/app/shared/RD/AnnualLeaveType.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-annual-leave-request-modal001',
  templateUrl: './annual-leave-request-modal001.component.html',
  styleUrls: ['./annual-leave-request-modal001.component.css']
})
export class AnnualLeaveRequestModal001Component implements OnInit, AfterViewInit {
  @ViewChild('ForemanSort') ForemanSort: MatSort;
  @ViewChild('ForemanPaginator') ForemanPaginator: MatPaginator;
  @ViewChild('HrSort') HrSort: MatSort;
  @ViewChild('HrPaginator') HrPaginator: MatPaginator;

  ForemanList: Membership[] = [];
  ForemanDataSource = new MatTableDataSource<Membership>();
  HrList: Membership[] = [];
  HrDataSource = new MatTableDataSource<Membership>();

  ForemanDisplayColumns: string[] = ['No', 'UserName', 'Name', 'Department'];
  CurrentMembership: Membership | undefined;
  LeaveTypeList: AnnualLeaveType[] = [];
  SelectedLeaveType = '';
  FromDate = '2026-10-28';
  FromTime = '07:30';
  ToDate = '2026-10-28';
  ToTime = '16:30';
  LeaveDays = 1;
  LeaveReason = '';
  IsSaving = false;

  constructor(
    private dialogRef: MatDialogRef<AnnualLeaveRequestModal001Component>,
    public MembershipService: MembershipService,
    public AnnualLeaveRequestService: AnnualLeaveRequestService,

  ) {
    this.MembershipService.BaseParameter.CompanyID = Number(localStorage.getItem(environment.CompanyID));
    this.MembershipService.BaseParameter.UserName = localStorage.getItem(environment.UserName) || '';
  }

  ngOnInit(): void {
    this.GetAnnualLeaveTypes();
    this.GetMembershipByUserName();
  }

  GetAnnualLeaveTypes(): void {
    this.AnnualLeaveRequestService.GetAnnualLeaveTypesAsync().subscribe(
      (res: any) => {
        const list = this.parseLeaveTypeList(res);

        this.LeaveTypeList = list
          .map((leaveType: any) => ({
            Id: leaveType.Id ?? leaveType.id,
            ParentId: leaveType.ParentId ?? leaveType.parentId,
            Code: leaveType.Code ?? leaveType.code,
            NameVN: leaveType.NameVN ?? leaveType.nameVN,
            NameEN: leaveType.NameEN ?? leaveType.nameEN,
            NameKR: leaveType.NameKR ?? leaveType.nameKR,
            Description: leaveType.Description ?? leaveType.description,
            SortOrder: leaveType.SortOrder ?? leaveType.sortOrder,
            Properties: leaveType.Properties ?? leaveType.properties,
            Icon: leaveType.Icon ?? leaveType.icon,
            IsActive: leaveType.IsActive ?? leaveType.isActive,
          }))
          .filter((leaveType) => !!leaveType.Code);

        this.SelectedLeaveType = this.LeaveTypeList[0]?.Code || '';
      },
      () => {
        this.LeaveTypeList = [];
        this.SelectedLeaveType = '';
      },
    );
  }

  private parseLeaveTypeList(response: any): any[] {
    const list = response?.List
      ?? response?.list
      ?? response?.Data?.List
      ?? response?.data?.list
      ?? response?.data
      ?? response;

    return Array.isArray(list) ? list : [];
  }

  ngAfterViewInit(): void {
    this.ForemanDataSource.sort = this.ForemanSort;
    this.ForemanDataSource.paginator = this.ForemanPaginator;
  }

  GetMembershipByUserName(): void {
    this.AnnualLeaveRequestService.IsShowLoading = true;
    this.MembershipService.GetByUserNameAsync().subscribe(
      (res) => {
        const membership = (res as BaseResult).BaseModel as Membership | undefined;
        this.MembershipService.BaseParameter.BaseModel = membership;
        this.CurrentMembership = membership;
        this.GetViewerByUserName();
        this.GetManagerByCategoryPhongBan();
      },
      () => {
        this.MembershipService.BaseParameter.BaseModel = undefined;
        this.CurrentMembership = undefined;
      },
      () => {
        this.AnnualLeaveRequestService.IsShowLoading = false;
      },
    );
  }

  GetViewerByUserName(): void {
    this.AnnualLeaveRequestService.IsShowLoading = true;
    this.MembershipService.BaseParameter.UserName = '22040405';
    this.MembershipService.GetByUserNameAsync().subscribe(
      (res) => {
        const viewer = (res as BaseResult).BaseModel as Membership | undefined;
        this.HrList = viewer ? [viewer] : [];
        this.HrDataSource = new MatTableDataSource(this.HrList);
      },
      () => {
        this.HrList = [];
      },
      () => {
        this.AnnualLeaveRequestService.IsShowLoading = false;
      },
    );
  }

  GetManagerByCategoryPhongBan(): void {
    this.AnnualLeaveRequestService.IsShowLoading = true;
    this.MembershipService.BaseParameter.UserName = localStorage.getItem(environment.UserName) || '';
    this.MembershipService.GetByParentIDCategoryPhongBanIDAsync().subscribe(
      (res) => {
        const manager = (res as BaseResult).BaseModel as Membership | undefined;
        this.ForemanList = manager ? [manager] : [];
        this.ForemanDataSource = new MatTableDataSource(this.ForemanList);
        this.ForemanDataSource.sort = this.ForemanSort;
      },
      () => {
        this.ForemanList = [];
      },
      () => {
        this.AnnualLeaveRequestService.IsShowLoading = false;
      },
    );
  }

  SaveFullAsync(): void {
    const membership = this.CurrentMembership;
    if (!membership?.UserName) {
      return;
    }

    if (!this.SelectedLeaveType || !this.FromDate || !this.ToDate || this.LeaveDays <= 0) {
      return;
    }

    this.IsSaving = true;
    this.AnnualLeaveRequestService.IsShowLoading = true;

    const model = {
      EmployeeCode: membership.UserName,
      EmployeeName: membership.Name,
      DepartmentId: membership.CategoryHRMPhongBanID || membership.CategoryDepartmentID,
      SupervisorName: this.ForemanList[0]?.Name,
      CreatedBy: Number(localStorage.getItem(environment.UserID)) || undefined,
      Year: new Date(this.FromDate).getFullYear(),
      LeaveType: this.SelectedLeaveType,
      LeaveReason: this.LeaveReason,
      TotalLeaveDays: this.LeaveDays,
      Details: [{
        FromDate: this.FromDate,
        FromTime: this.FromTime,
        ToDate: this.ToDate,
        ToTime: this.ToTime,
        LeaveType: this.SelectedLeaveType,
        LeaveDays: this.LeaveDays,
        LeaveReason: this.LeaveReason,
      }],
      Approvers: this.ForemanList
        .filter((approver) => !!approver.UserName)
        .map((approver, index) => ({
          UserName: approver.UserName,
          Role: 'Manager',
          Order: index + 1,
        })),
      Viewers: this.HrList
        .filter((viewer) => !!viewer.UserName)
        .map((viewer) => ({ UserName: viewer.UserName })),
    };

    const baseParameter = {
      ...this.AnnualLeaveRequestService.BaseParameter,
      BaseModel: model,
    };

    this.AnnualLeaveRequestService.SaveFullAsync(baseParameter).subscribe(
      (res: any) => {
        if (res?.StatusCode && res.StatusCode >= 400) {
          return;
        }
        this.dialogRef.close(res);
      },
      () => {
        this.IsSaving = false;
      },
      () => {
        this.IsSaving = false;
        this.AnnualLeaveRequestService.IsShowLoading = false;
      },
    );
  }

  closeModal(): void {
    this.dialogRef.close();
  }


}
