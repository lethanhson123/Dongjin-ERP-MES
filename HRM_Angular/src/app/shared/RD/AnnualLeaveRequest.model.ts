import { Base } from "../ERP/Base.model";
import { AnnualLeaveDetails } from "./AnnualLeaveDetails.model";

export class AnnualLeaveRequest extends Base {
    Id?: number;
    Details?: AnnualLeaveDetails[];
    RequestCode?: string;
    EmployeeCode?: string;
    EmployeeName?: string;
    DepartmentName?: string;
    DepartmentId?: number;
    SupervisorName?: string;
    CreatedBy?: number;
    Status?: string;
    Year?: number;
    FromDate?: Date;
    FromTime?: string;
    ToDate?: Date;
    ToTime?: string;
    LeaveType?: string;
    LeaveReason?: string;
    LeaveDays?: number;
    Notes?: string;
    TotalLeaveDays?: number;
    UsedLeaveDays?: number;
    RemainingLeaveDays?: number;
    CreatedDate?: Date;
    UpdatedDate?: Date;
    SubmitDate?: Date;
    CompletedDate?: Date;
    IsDeleted?: number;
    
}

export class AnnualLeaveRequestExcelDownload {
    Blob: Blob;
    FileName: string;
}


