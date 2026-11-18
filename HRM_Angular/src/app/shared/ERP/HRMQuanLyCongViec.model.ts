import { Base } from "./Base.model";
import { HRMQuanLyCongViecFile } from "./HRMQuanLyCongViecFile.model";

export class HRMQuanLyCongViec extends Base {
    Complete?: number;
    Point?: number;
    Files?: HRMQuanLyCongViecFile[];
}
