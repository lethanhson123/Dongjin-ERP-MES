import { Base } from "./Base.model";

export class Company extends Base{
ID?: number;
ParentID?: number;
ParentName?: string;
CreateDate?: Date;
CreateUserID?: number;
CreateUserCode?: string;
CreateUserName?: string;
UpdateDate?: Date;
UpdateUserID?: number;
UpdateUserCode?: string;
UpdateUserName?: string;
RowVersion?: number;
SortOrder?: number;
Active?: boolean;
Code?: string;
Name?: string;
Display?: string;
Description?: string;
Note?: string;
FileName?: string;
CompanyID?: number;
CompanyName?: string;

}


