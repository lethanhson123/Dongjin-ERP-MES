export class AnnualLeaveBalanceFilter {
  CompanyID: number = 0;
  Year: number = new Date().getFullYear();
  DepartmentID?: number;
  IsOffice?: boolean;
  SearchString: string = '';
  PageIndex: number = 0;
  PageSize: number = 20;
  SortBy: string = 'EmployeeCode';
  SortDirection: string = 'asc';
}

export class AnnualLeaveBalance {
  MembershipId: number = 0;
  EmployeeCode: string = '';
  EmployeeName?: string;
  DepartmentName?: string;
  OpeningBalance: number = 0;
  OpeningDate?: string;
  CurrentBalance: number = 0;
  UpdatedDate?: string;
  UpdatedBy?: string;
}

export class AnnualLeaveBalanceAdjust {
  EmployeeCode: string = '';
  Year: number = new Date().getFullYear();
  AdjustmentType: 'ADD' | 'SUB' = 'ADD';
  Amount: number = 0;
  Reason?: string;
}

export class AnnualLeaveBalanceEditModalData {
  EmployeeCode: string = '';
  EmployeeName?: string;
  DepartmentName?: string;
  OpeningBalance: number = 0;
  OpeningDate?: string;
  CurrentBalance: number = 0;
  Year: number = new Date().getFullYear();
}

export class AnnualLeaveBalanceImportModalData {
  Year: number = new Date().getFullYear();
  Filter?: AnnualLeaveBalanceFilter;
}

export class AnnualLeaveBalanceListResult {
  StatusCode?: number;
  Message?: string;
  List: AnnualLeaveBalance[] = [];
  Count: number = 0;
}

export class AnnualLeaveBalanceImportResult {
  SuccessCount: number = 0;
  Errors: string[] = [];
}

export class AnnualLeaveBalanceSyncFilter {
  Year: number = new Date().getFullYear();
  CompanyID: number = 0;
  EmployeeCode?: string;
}

export class AnnualLeaveBalanceSyncResult {
  TotalEmployeesSynced: number = 0;
  MonthlyAccrualsAdded: number = 0;
  SeniorityAccrualsAdded: number = 0;
  Errors: string[] = [];
}

export class AnnualLeaveBalanceSyncApiResult {
  StatusCode?: number;
  Message?: string;
  BaseModel?: AnnualLeaveBalanceSyncResult;
}

export class AnnualLeaveBalanceExcelDownload {
  Blob: Blob = new Blob();
  FileName: string = '';
}
