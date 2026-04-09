import { Base } from "./Base.model";

export class BaseResult extends Base {
    StatusCode?: number;
    Message?: string;
    Note?: string;
    BaseModel?: Base;
    List?: Base[];
    SearchString?: string;
    Count?: number;
    IsCheck?: boolean;
}
