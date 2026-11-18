import { Injectable } from "@angular/core";
import { MembershipService } from "./Membership.service";

@Injectable({ providedIn: 'root' })
export class MembershipListService extends MembershipService {
  DisplayColumns001: string[] = [
    'No',
    'UserName',
    'Name',
    'CategoryHRMPhongBanName',
    'CategoryHRMChucDanhName',
    'Phone',
    'Email',
    'Active',
  ];
  DisplayColumns004: string[] = [
    'Edit',
    'Active',
    'No',
    'UserName',
    'FileName',
    'Name',
    'Display',
    'CategoryHRMKhoiName',
    'DepartmentKorean',
    'CategoryHRMPhongBanName',
    'PositionKorean',
    'CategoryHRMChucDanhName',
    'Rank',
    'RankKorean',
    'NgayVaoLam',
    'CategoryHRMGioiTinhName',
    'CategoryHRMTrinhDoName',
    'CategoryHRMChuyenNganhName',
    'EmploymentType',
    'CCCDNoiSinh',
    'NgaySinh',
    'NguyenQuanName',
    'CategoryHRMQuocGiaName',
    'CategoryHRMDanTocName',
    'CCCD',
    'CCCDNgayCap',
    'CCCDNgayHetHan',
    'CCCDNoiCap',
    'CCCDiaChi',
    'DiaChi',
    'Phone',
    'Email',
    'CategoryHRMTinhTrangLamViecName',
    'NgayNghiViec',
    'LyDoNghiViec',
    'Certificate',
    'OverseasExperience',
    'ForeignLanguageLevel',
    'ContractCondition',
    'RewardsDiscipline',
    'PromotionHistory',
    'TrainingDate',
    'WorkExperience',
    'Other',
    'Note',
  ];
  ReadNghiViecExcelToListAsync() {
    this.Initialization();
    let url = this.APIURL + this.Controller + '/ReadNghiViecExcelToListAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    if (this.FileToUpload) {
      if (this.FileToUpload.length > 0) {
        for (var i = 0; i < this.FileToUpload.length; i++) {
          formUpload.append('file[]', this.FileToUpload[i]);
        }
      }
    }
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  SaveNghiViecListAsync() {
    this.Initialization();
    let url = this.APIURL + this.Controller + '/SaveNghiViecListAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  SaveAndUploadFile20261006Async() {
    this.Initialization();
    let url = this.APIURL + this.Controller + '/SaveAndUploadFile20261006Async';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    if (this.FileToUpload) {
      if (this.FileToUpload.length > 0) {
        for (var i = 0; i < this.FileToUpload.length; i++) {
          formUpload.append('file[]', this.FileToUpload[i]);
        }
      }
    }
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
}
