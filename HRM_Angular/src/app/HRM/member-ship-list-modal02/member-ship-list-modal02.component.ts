import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipListService } from 'src/app/shared/ERP/MembershipList.service';

@Component({
  selector: 'app-member-ship-list-modal02',
  templateUrl: './member-ship-list-modal02.component.html',
  styleUrls: ['./member-ship-list-modal02.component.css']
})
export class MemberShipListModal02Component implements OnInit {

  FileName: string = environment.InitializationString;
  List: Membership[] | undefined;
  URLTemplate: string;

  constructor(
    public DialogRef: MatDialogRef<MemberShipListModal02Component>,
    public NotificationService: NotificationService,
    public MembershipService: MembershipListService,
  ) {
    this.URLTemplate = this.MembershipService.APIRootURL + 'Download/HRMTemplateNghiViec.xlsx';
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
    this.MembershipService.ReadNghiViecExcelToListAsync().subscribe(
      (res) => {
        this.MembershipService.BaseResult = res as BaseResult;
        this.List = this.MembershipService.BaseResult.List as Membership[];
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

  MembershipListModal02Save() {
    if (!this.List || this.List.length == 0) {
      this.NotificationService.warn('Vui lòng đọc file Excel trước | 먼저 엑셀 파일을 읽어주세요');
      return;
    }
    for (let i = 0; i < this.List.length; i++) {
      this.List[i].Active = false;
    }
    this.MembershipService.BaseParameter.List = this.List;
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.SaveNghiViecListAsync().subscribe(
      (res) => {
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

  MembershipListModal02Close() {
    this.DialogRef.close();
  }

}
