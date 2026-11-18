import { Base } from "./Base.model";
import { HRMChamCongLichSuSummary } from "./HRMChamCongLichSuSummary.model";


export class BaseResult extends Base {
    StatusCode?: number;
    Message?: string;
    Note?: string;
    BaseModel?: Base;
    List?: Base[];
    ListFilter?: Base[];
    ListParent?: Base[];
    ListChild?: Base[];
    ListSummary?: HRMChamCongLichSuSummary[];
    List02?: Base[];
    List03?: Base[];
    SearchString?: string;
    Count?: number;
    Sum?: number;
    IsCheck?: boolean;
}
