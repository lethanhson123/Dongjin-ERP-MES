import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { MembershipListService } from 'src/app/shared/ERP/MembershipList.service';

@Component({
  selector: 'app-member-ship-list-modal01',
  templateUrl: './member-ship-list-modal01.component.html',
  styleUrls: ['./member-ship-list-modal01.component.css']
})
export class MemberShipListModal01Component implements OnInit {

  @ViewChild('MembershipSort') MembershipSort: MatSort;
  @ViewChild('MembershipPaginator') MembershipPaginator: MatPaginator;
  @ViewChild('MessageSort') MessageSort: MatSort;
  @ViewChild('MessagePaginator') MessagePaginator: MatPaginator;

  FileName: string = environment.InitializationString;
  URLTemplate: string;

  List: any[] = [];
  DataSource: MatTableDataSource<any> | undefined;
  Message: string = environment.InitializationString;
  MessageList: any[] = [];
  DataSourceMessage: MatTableDataSource<any> | undefined;
  MessageColumns: string[] = ['No', 'Content'];
  DisplayedColumns: string[] = [
    'No',
    'Active',
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

  get ExistsList(): string[] {
    if (!this.Message) {
      return [];
    }
    return this.Message.split('\n').filter((o) => o && o.length > 0);
  }




  constructor(
    public DialogRef: MatDialogRef<MemberShipListModal01Component>,
    public NotificationService: NotificationService,
    public MembershipService: MembershipListService,
  ) {
    this.URLTemplate = this.MembershipService.APIRootURL + 'Download/Employee.xlsx';
  }

  ngOnInit(): void {
  }

  MembershipChange(event, files: FileList) {
    if (files && files.length > 0) {
      this.MembershipService.FileToUpload = files;
      this.MembershipService.BaseParameter.Event = event;
      this.FileName = files[0].name;
      this.MembershipRead();
    }
  }

  MembershipRead() {
    if (!this.MembershipService.FileToUpload || this.MembershipService.FileToUpload.length == 0) {
      this.NotificationService.warn('Vui lòng chọn file Excel | 엑셀 파일을 선택하세요');
      return;
    }
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.SaveAndUploadFile20261006Async().subscribe(
      (res) => {
        console.log('[DEBUG] res:', res);
        this.MembershipService.BaseResult = res as BaseResult;
        this.List = (this.MembershipService.BaseResult.List as any[]) || [];
        this.Message = this.MembershipService.BaseResult.Message || '';
        this.MessageList = this.ExistsList.map((o) => ({ Content: o }));
        this.DataSource = new MatTableDataSource(this.List);
        this.DataSourceMessage = new MatTableDataSource(this.MessageList);
        this.MembershipService.IsShowLoading = false;
        setTimeout(() => {
          if (this.DataSource) {
            this.DataSource.sort = this.MembershipSort;
            this.DataSource.paginator = this.MembershipPaginator;
          }
          if (this.DataSourceMessage) {
            this.DataSourceMessage.sort = this.MessageSort;
            this.DataSourceMessage.paginator = this.MessagePaginator;
          }
        });
        if (!this.List || this.List.length == 0) {
          this.NotificationService.warn('Không có dữ liệu | 데이터가 없습니다');
        }
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      },
    );
  }

  MembershipUpload() {
    if (!this.MembershipService.FileToUpload || this.MembershipService.FileToUpload.length == 0) {
      this.NotificationService.warn('Vui lòng chọn file Excel | 엑셀 파일을 선택하세요');
      return;
    }
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.SaveAndUploadFileAsync().subscribe(
      (res) => {
        this.MembershipService.FileToUpload = null;
        this.MembershipService.BaseParameter.Event = null;
        this.NotificationService.warn(environment.SaveSuccess);
        this.DialogRef.close(true);
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      },
    );
  }

  MembershipExcel() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.GetByCompanyIDToExcel20260905Async().subscribe(
      (res) => {
        let url = (res as BaseResult).Message;
        window.open(url, '_blank');
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      },
    );
  }

  MembershipListModalClose() {
    this.DialogRef.close();
  }

  MembershipTemplate() {
    window.open(this.URLTemplate, '_blank');
  }

}
