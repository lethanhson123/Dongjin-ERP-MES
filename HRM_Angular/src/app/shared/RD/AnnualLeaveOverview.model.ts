export class AnnualLeaveOverviewFilter {
  CompanyID: number = 0;
  Year: number = new Date().getFullYear();
  DepartmentID?: number;
  IsOffice?: boolean;
  SearchString?: string;
  PageIndex: number = 0;
  PageSize: number = 20;
  SortBy: string = 'EmployeeCode';
  SortDirection: string = 'asc';
}

export class AnnualLeaveOverview {
  MembershipId: number = 0;
  EmployeeCode?: string;
  EmployeeName?: string;
  DepartmentName?: string;
  OpeningDate?: string;
  OpeningBalance: number = 0;
  MonthlyAccruedDays: number = 0;
  SeniorityLeaveDays: number = 0;
  UsedLeaveDays: number = 0;
  ManualAdjustmentDays: number = 0;
  CurrentBalance: number = 0;
}

export class AnnualLeaveOverviewListResult {
  StatusCode?: number;
  Message?: string;
  List: AnnualLeaveOverview[] = [];
  Count: number = 0;
}

export class AnnualLeaveOverviewExcelDownload {
  Blob: Blob = new Blob();
  FileName: string = '';
}
