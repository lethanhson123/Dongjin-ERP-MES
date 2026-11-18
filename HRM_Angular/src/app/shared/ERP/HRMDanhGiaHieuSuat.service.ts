import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HRMDanhGiaHieuSuat } from './HRMDanhGiaHieuSuat.model';
import { BaseService } from './Base.service';

@Injectable({
    providedIn: 'root'
})
export class HRMDanhGiaHieuSuatService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'ParentID', 'Code', 'Name', 'Year', 'DateBegin', 'DateEnd', 'MaxScore', 'Weight', 'IsOffice', 'Active', 'Save'];
    DisplayColumns002: string[] = ['No', 'Membership', 'HieuSuat', 'NangLuc', 'ThaiDo', 'HopTac', 'QuanLy', 'DiemChuyenDoi'];

    List: HRMDanhGiaHieuSuat[] | undefined;
    ListFilter: HRMDanhGiaHieuSuat[] | undefined;
    ListCapBac: HRMDanhGiaHieuSuat[] | undefined;
    ListTieuChi: HRMDanhGiaHieuSuat[] | undefined;
    ListVaiTro: HRMDanhGiaHieuSuat[] | undefined;


    DataSource: any;
    DataSourceFilter: any;

    FormData!: HRMDanhGiaHieuSuat;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "HRMDanhGiaHieuSuat";
    }

    BuildTree() {
        this.ListCapBac = [];
        this.ListTieuChi = [];
        this.ListVaiTro = [];
        if (this.List && this.List.length > 0) {
            this.ListCapBac = this.List.filter((o) => o.Code == 'CAPBAC');
            this.ListTieuChi = this.List.filter((o) => o.Code == 'TIEUCHI');
            this.ListVaiTro = this.List.filter((o) => o.Code == 'VAITRO');
        }
    }

}
