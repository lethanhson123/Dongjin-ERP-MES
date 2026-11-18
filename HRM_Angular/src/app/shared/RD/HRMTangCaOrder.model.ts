export class HRMTangCaOrder {
  Id: number;
  ApprovalRequestId: number;
  RequestCode: string;
  OvertimeDate: string;
  CompanyId?: number;
  CompanyName?: string;
  IsOffice: boolean;
  RequestDepartmentId?: number;
  RequestDepartmentName: string;
  DepartmentNames: string;
  EmployeeCount: number;
  TotalHours: number;
  Status: string;
  CreatorName: string;
  CreatedDate: string;
  CompletedDate?: string;
}

export class HRMTangCaFilter {
  CompanyId?: number;
  FromDate?: string;
  ToDate?: string;
  DepartmentId?: number;
  RequestDepartmentId?: number;
  IsOffice?: boolean;
  Keyword?: string;
  Page: number = 1;
  PageSize: number = 10;
  SortBy: string = 'OvertimeDate';
  SortDirection: string = 'desc';
}

export class HRMTangCaOrderPagedResult {
  Items: HRMTangCaOrder[] = [];
  TotalCount: number = 0;
  Page: number = 1;
  PageSize: number = 10;
}

export class HRMTangCaEmployee {
  Id: number;
  ApprovalOvertimeId: number;
  ApprovalRequestId: number;
  RequestCode: string;
  EmployeeId: number;
  EmployeeCode: string;
  EmployeeName: string;
  DepartmentName: string;
  IsOffice: boolean;
  RequestDepartmentName: string;
  OvertimeDate: string;
  StartTime: string;
  EndTime: string;
  TotalMinutes: number;
  TotalHours: number;
  CompletedDate?: string;
  SignatureOrNote?: string;
}

export class HRMTangCaEmployeeFilter {
  CompanyId?: number;
  FromDate?: string;
  ToDate?: string;
  DepartmentId?: number;
  IsOffice?: boolean;
  Keyword?: string;
  Page: number = 1;
  PageSize: number = 10;
  SortBy: string = 'EmployeeCode';
  SortDirection: string = 'asc';
}

export class HRMTangCaEmployeePagedResult {
  Items: HRMTangCaEmployee[] = [];
  TotalCount: number = 0;
  Page: number = 1;
  PageSize: number = 10;
}

export class HRMTangCaReportFilter {
  CompanyId?: number;
  FromDate?: string;
  ToDate?: string;
}

export class HRMTangCaDepartmentChart {
  DepartmentId?: number;
  DepartmentName: string;
  TotalHours: number;
}

export class HRMTangCaRequestDepartmentChart {
  RequestDepartmentId?: number;
  RequestDepartmentName: string;
  TotalHours: number;
}

export class HRMTangCaDivisionChart {
  IsOffice: boolean;
  TotalHours: number;
  Percentage: number;
}

export class HRMTangCaTrendChart {
  Date: string;
  TotalHours: number;
}

export class HRMTangCaReport {
  TotalHours: number;
  EmployeeCount: number;
  AverageHoursPerEmployee: number;
  TopDepartment: string;
  TopDepartmentHours: number;
  DepartmentHours: HRMTangCaDepartmentChart[];
  RequestDepartmentHours: HRMTangCaRequestDepartmentChart[];
  DivisionHours: HRMTangCaDivisionChart[];
  DailyTrends: HRMTangCaTrendChart[];
}

export class HRMTangCaExcelDownload {
  Blob: Blob;
  FileName: string;
}
