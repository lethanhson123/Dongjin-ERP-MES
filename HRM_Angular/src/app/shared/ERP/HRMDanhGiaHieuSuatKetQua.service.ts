import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HRMDanhGiaHieuSuatKetQua } from './HRMDanhGiaHieuSuatKetQua.model';
import { BaseService } from './Base.service';

@Injectable({
    providedIn: 'root'
})
export class HRMDanhGiaHieuSuatKetQuaService extends BaseService {
    DisplayColumns001: string[] = ['No', 'MembershipName', 'CategoryDepartmentName', 'CategoryPositionName', 'ScoreTotal', 'FinalScore', 'Rank', 'Save'];
    DisplayColumns002: string[] = ['No', 'Membership', 'HieuSuat', 'NangLuc', 'ThaiDo', 'HopTac', 'QuanLy', 'DiemChuyenDoi', 'Save'];

    List: HRMDanhGiaHieuSuatKetQua[] | undefined;
    ListFilter: HRMDanhGiaHieuSuatKetQua[] | undefined;

    ListHoSo: HRMDanhGiaHieuSuatKetQua[] | undefined;
    ListPhieu: any[] | undefined;

    DataSource: any;
    DataSourceFilter: any;

    FormData!: HRMDanhGiaHieuSuatKetQua;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "HRMDanhGiaHieuSuatKetQua";
    }


}
