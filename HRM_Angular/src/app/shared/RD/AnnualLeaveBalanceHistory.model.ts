export class AnnualLeaveBalanceHistoryFilter {
  CompanyID: number = 0;
  FromDate?: string;
  ToDate?: string;
  DepartmentID?: number;
  IsOffice?: boolean;
  SearchString: string = '';
  PageIndex: number = 0;
  PageSize: number = 20;
}

export class AnnualLeaveBalanceHistory {
  EmployeeCode: string = '';
  EmployeeName?: string;
  DepartmentName?: string;
  ChangeType?: string;
  OldBalance: number = 0;
  ChangeAmount: number = 0;
  NewBalance: number = 0;
  Reason?: string;
  CreatedDate?: string;
  CreatedBy?: string;
}

export class AnnualLeaveBalanceHistoryListResult {
  StatusCode?: number;
  Message?: string;
  List: AnnualLeaveBalanceHistory[] = [];
  Count: number = 0;
}

export class AnnualLeaveBalanceHistoryExcelDownload {
  Blob: Blob = new Blob();
  FileName: string = '';
}
