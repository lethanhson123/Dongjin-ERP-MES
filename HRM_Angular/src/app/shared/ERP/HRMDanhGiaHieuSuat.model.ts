import { Base } from "./Base.model";

export class HRMDanhGiaHieuSuat extends Base {
    Year?: number;
    DateBegin?: Date;
    DateEnd?: Date;
    MaxScore?: number;
    Weight?: number;
    IsOffice?: boolean;
}
