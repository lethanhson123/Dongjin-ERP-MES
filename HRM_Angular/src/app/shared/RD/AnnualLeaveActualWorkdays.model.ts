export class AnnualLeaveActualWorkdaysEmployeeFilter {
  SearchString?: string;
  PageIndex: number = 0;
  PageSize: number = 20;
}

export class AnnualLeaveActualWorkdaysCalendarFilter {
  MembershipId: number = 0;
  Year: number = new Date().getFullYear();
  Month: number = new Date().getMonth() + 1;
}

export class AnnualLeaveActualWorkdaysEmployee {
  MembershipId: number = 0;
  EmployeeCode?: string;
  EmployeeName?: string;
  DepartmentName?: string;
}

export class AnnualLeaveActualWorkdayLeave {
  LeaveTypeCode?: string;
  LeaveTypeNameVN?: string;
  LeaveTypeNameEN?: string;
  LeaveTypeNameKR?: string;
  IsDeduct: boolean = false;
  LeaveDays: number = 0;
  FromTime?: string;
  ToTime?: string;
}

export class AnnualLeaveActualWorkday {
  WorkDate: string = '';
  IsWorked: boolean = false;
  Leaves: AnnualLeaveActualWorkdayLeave[] = [];
}

export class AnnualLeaveActualWorkdaysListResult {
  StatusCode?: number;
  Message?: string;
  List: AnnualLeaveActualWorkdaysEmployee[] = [];
  Count: number = 0;
}

export class AnnualLeaveActualWorkdaysCalendarResult {
  StatusCode?: number;
  Message?: string;
  List: AnnualLeaveActualWorkday[] = [];
  Count: number = 0;
}

export interface AnnualLeaveActualWorkdaysCalendarCell {
  WorkDate: Date | null;
  Workday?: AnnualLeaveActualWorkday;
}
