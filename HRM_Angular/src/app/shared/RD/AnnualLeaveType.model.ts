export interface AnnualLeaveType {
  Id: number;
  ParentId?: number | null;
  Code: string;
  NameVN?: string;
  NameEN?: string;
  NameKR?: string;
  Description?: string;
  SortOrder?: number;
  Properties?: string;
  Icon?: string;
  IsActive?: boolean;
  IsDeduct?: boolean;
}
