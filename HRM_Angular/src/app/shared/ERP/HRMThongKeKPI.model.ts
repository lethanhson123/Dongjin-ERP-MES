import { Base } from "./Base.model";
import { HRMThongKeKPIFile } from "./HRMThongKeKPIFile.model";

export class HRMThongKeKPI extends Base {
    CategoryDepartmentID?: number;
    CategoryDepartmentName?: string;
    CategoryStatusID?: number;
    CategoryStatusName?: string;
    Target?: number;
    CurrentResult?: number;
    Weight?: number;
    RateAchievement?: number;
    Files?: HRMThongKeKPIFile[];
}
