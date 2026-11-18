import { Base } from "../ERP/Base.model";

export class AnnualLeaveDetails extends Base{
Id?: number;
AnnualLeaveRequestId?: number;
FromDate?: Date;
FromTime?: string;
ToDate?: Date;
ToTime?: string;
LeaveType?: string;
LeaveDays?: number;
LeaveReason?: string;
Sequence?: number;
CreatedDate?: Date;

}


