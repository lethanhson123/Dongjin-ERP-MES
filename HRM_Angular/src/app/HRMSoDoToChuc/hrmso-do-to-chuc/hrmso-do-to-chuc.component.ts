import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { HRMSoDoToChuc } from 'src/app/shared/ERP/HRMSoDoToChuc.model';
import { HRMSoDoToChucService } from 'src/app/shared/ERP/HRMSoDoToChuc.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { HRMSoDoToChucModalComponent } from '../hrmso-do-to-chuc-modal/hrmso-do-to-chuc-modal.component';

@Component({
  selector: 'app-hrmso-do-to-chuc',
  templateUrl: './hrmso-do-to-chuc.component.html',
  styleUrls: ['./hrmso-do-to-chuc.component.css'],
})
export class HRMSoDoToChucComponent implements OnInit, OnDestroy {
  @ViewChild('HRMSoDoToChucSort') HRMSoDoToChucSort: MatSort;
  @ViewChild('HRMSoDoToChucPaginator') HRMSoDoToChucPaginator: MatPaginator;

  @ViewChild('KhungOverflowParent') KhungOverflowParentRef: ElementRef;
  @ViewChild('KhungOverflowChild') KhungOverflowChildRef: ElementRef;
  @ViewChild('KhungOverflow1') KhungOverflow1Ref: ElementRef;
  @ViewChild('ChartContent') ChartContentRef: ElementRef;
  private isFirstFit: boolean = true;

  //  Ẩn/hiện ảnh trong node bằng phím R
  isPhotoVisible: boolean = true;

  //  Drag-to-pan: giữ Space + kéo chuột để di chuyển sơ đồ
  isSpaceDown: boolean = false;
  isPanning: boolean = false;
  private panStartX: number = 0;
  private panStartY: number = 0;
  private panStartScrollLeft: number = 0;
  private panStartScrollTop: number = 0;
  private boundKeyDown = (e: KeyboardEvent) => this.onKeyDown(e);
  private boundKeyUp = (e: KeyboardEvent) => this.onKeyUp(e);
  private boundMouseMove = (e: MouseEvent) => this.onPanMove(e);
  private boundMouseUp = () => this.onPanEnd();

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public HRMSoDoToChucService: HRMSoDoToChucService,
    public CompanyService: CompanyService,
  ) {
    this.HRMSoDoToChucService.BaseParameter.Count = 1000;
    this.CompanySearch();
  }

  ngOnInit(): void {
    window.addEventListener('keydown', this.boundKeyDown);
    window.addEventListener('keyup', this.boundKeyUp);
  }

  ngOnDestroy(): void {
    window.removeEventListener('keydown', this.boundKeyDown);
    window.removeEventListener('keyup', this.boundKeyUp);
    window.removeEventListener('mousemove', this.boundMouseMove);
    window.removeEventListener('mouseup', this.boundMouseUp);
  }

  onKeyDown(e: KeyboardEvent) {
    // Chỉ kích hoạt khi đang ở khung sơ đồ, tránh bắt Space khi đang gõ input/select
    const target = e.target as HTMLElement;
    if (target && (target.tagName === 'INPUT' || target.tagName === 'SELECT' || target.tagName === 'TEXTAREA' || target.isContentEditable)) {
      return;
    }
    if (e.code === 'Space') {
      this.isSpaceDown = true;
      e.preventDefault(); // chặn Space làm cuộn trang
    }
    if (e.code === 'KeyR') {
      this.isPhotoVisible = !this.isPhotoVisible; // toggle ẩn/hiện ảnh
    }
  }

  onKeyUp(e: KeyboardEvent) {
    if (e.code === 'Space') {
      this.isSpaceDown = false;
      if (this.isPanning) {
        this.onPanEnd();
      }
    }
  }

  onPanStart(e: MouseEvent) {
    // Chỉ pan khi đang giữ Space và bấm chuột trái
    if (!this.isSpaceDown || e.button !== 0) {
      return;
    }
    const parentEl = this.KhungOverflowParentRef.nativeElement as HTMLElement;
    this.isPanning = true;
    this.panStartX = e.clientX;
    this.panStartY = e.clientY;
    this.panStartScrollLeft = parentEl.scrollLeft;
    this.panStartScrollTop = parentEl.scrollTop;
    e.preventDefault();
    window.addEventListener('mousemove', this.boundMouseMove);
    window.addEventListener('mouseup', this.boundMouseUp);
  }

  onPanMove(e: MouseEvent) {
    if (!this.isPanning) {
      return;
    }
    const parentEl = this.KhungOverflowParentRef.nativeElement as HTMLElement;
    // Kéo chuột sang phải => nội dung dịch sang phải => giảm scrollLeft
    parentEl.scrollLeft = this.panStartScrollLeft - (e.clientX - this.panStartX);
    parentEl.scrollTop = this.panStartScrollTop - (e.clientY - this.panStartY);
    e.preventDefault();
  }

  onPanEnd() {
    this.isPanning = false;
    window.removeEventListener('mousemove', this.boundMouseMove);
    window.removeEventListener('mouseup', this.boundMouseUp);
  }

  // Giữ Space + con lăn chuột => zoom (thay vì cuộn)
  onWheel(e: WheelEvent) {
    if (!this.isSpaceDown) {
      return; // không giữ Space => cuộn bình thường
    }
    e.preventDefault();

    const parentEl = this.KhungOverflowParentRef.nativeElement as HTMLElement;
    const contentEl = this.KhungOverflow1Ref.nativeElement as HTMLElement;

    const oldZoom = this.zoomLevel;
    const step = e.deltaY < 0 ? 0.1 : -0.1; // lăn lên => phóng to
    const newZoom = Math.max(0.1, Math.min(oldZoom + step, 2));
    if (newZoom === oldZoom) {
      return;
    }

    // Vị trí con trỏ trong khung nhìn
    const rect = parentEl.getBoundingClientRect();
    const pointerX = e.clientX - rect.left + parentEl.scrollLeft;
    const pointerY = e.clientY - rect.top + parentEl.scrollTop;

    // Điểm dưới con trỏ theo hệ tọa độ nội dung gốc (bù margin + scale)
    const marginLeft = parseFloat(contentEl.style.marginLeft) || 0;
    const marginTop = parseFloat(contentEl.style.marginTop) || 0;
    const contentX = (pointerX - marginLeft * oldZoom) / oldZoom;
    const contentY = (pointerY - marginTop * oldZoom) / oldZoom;

    this.zoomLevel = newZoom;

    // Giữ điểm dưới con trỏ đứng yên sau khi zoom
    setTimeout(() => {
      parentEl.scrollLeft = contentX * newZoom + marginLeft * newZoom - (e.clientX - rect.left);
      parentEl.scrollTop = contentY * newZoom + marginTop * newZoom - (e.clientY - rect.top);
    });
  }
  ngAfterViewInit() {}
  zoomLevel: number = 1;
  zoomIn() {
    this.zoomLevel = Math.min(this.zoomLevel + 0.1, 2);
  }

  zoomOut() {
    this.zoomLevel = Math.max(this.zoomLevel - 0.1, 0.1);
  }

  resetZoom() {
    this.zoomLevel = 1;
  }
  CompanySearch() {
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      (res) => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.HRMSoDoToChucService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          this.HRMSoDoToChucSearch();
        }
      },
      (err) => {},
      () => {},
    );
  }

  HRMSoDoToChucSearch() {
    this.HRMSoDoToChucService.IsShowLoading = true;
    this.HRMSoDoToChucService.BaseParameter.Active = true;
    this.HRMSoDoToChucService.GetByCompanyIDAndActiveToListAsync().subscribe(
      (res) => {
        this.HRMSoDoToChucService.BaseResult = res as BaseResult;
        this.HRMSoDoToChucService.List = this.HRMSoDoToChucService.BaseResult.List;
        this.HRMSoDoToChucService.BaseParameter.LanguageID = Number(localStorage.getItem(environment.LanguageID));
        for (let i = 0; i < this.HRMSoDoToChucService.List.length; i++) {
          switch (this.HRMSoDoToChucService.BaseParameter.LanguageID) {
            case 1:
              this.HRMSoDoToChucService.List[i].CategoryHRMPhongBanName = this.HRMSoDoToChucService.List[i].CategoryHRMPhongBanName01;
              this.HRMSoDoToChucService.List[i].CategoryPositionName = this.HRMSoDoToChucService.List[i].CategoryPositionName01;
              this.HRMSoDoToChucService.List[i].Mission = this.HRMSoDoToChucService.List[i].Mission01;
              break;
            case 2:
              this.HRMSoDoToChucService.List[i].CategoryHRMPhongBanName = this.HRMSoDoToChucService.List[i].CategoryHRMPhongBanName03;
              this.HRMSoDoToChucService.List[i].CategoryPositionName = this.HRMSoDoToChucService.List[i].CategoryPositionName02;
              this.HRMSoDoToChucService.List[i].Mission = this.HRMSoDoToChucService.List[i].Mission02;
              break;
            case 3:
              this.HRMSoDoToChucService.List[i].CategoryHRMPhongBanName = this.HRMSoDoToChucService.List[i].CategoryHRMPhongBanName02;
              this.HRMSoDoToChucService.List[i].CategoryPositionName = this.HRMSoDoToChucService.List[i].CategoryPositionName03;
              this.HRMSoDoToChucService.List[i].Mission = this.HRMSoDoToChucService.List[i].Mission03;
              break;
          }
        }
      },
      (err) => {},
      () => {
        this.HRMSoDoToChucService.IsShowLoading = false;
        if (this.isFirstFit) {
          this.isFirstFit = false;
          setTimeout(() => this.autoFit());
        }
      },
    );
  }
  autoFit() {
    if (!this.KhungOverflowParentRef || !this.KhungOverflow1Ref) {
      return;
    }
    const parentEl = this.KhungOverflowParentRef.nativeElement as HTMLElement;
    const childEl = this.KhungOverflowChildRef.nativeElement as HTMLElement;
    const contentEl = this.KhungOverflow1Ref.nativeElement as HTMLElement;

    const parentWidth = parentEl.clientWidth;
    const parentHeight = parentEl.clientHeight;

    // Bỏ scale tạm để đo kích thước NỘI DUNG THẬT.
    // #ChartContent có width cố định (1000px) nhưng các node con có thể rộng hơn,
    // nên phải lấy max giữa scrollWidth của #ChartContent và child wrapper.
    const prevTransform = childEl.style.transform;
    childEl.style.transform = 'none';
    const contentElNode = this.ChartContentRef?.nativeElement as HTMLElement;
    const contentWidth = Math.max(childEl.scrollWidth, contentElNode?.scrollWidth || 0);
    const contentHeight = Math.max(childEl.scrollHeight, contentElNode?.scrollHeight || 0);
    childEl.style.transform = prevTransform;

    if (!contentWidth || !contentHeight || !parentWidth || !parentHeight) {
      return;
    }

    // Tính tỉ lệ để toàn bộ sơ đồ (ngang + dọc) vừa trong khung nhìn, chừa lề 4%
    const scaleX = parentWidth / contentWidth;
    const scaleY = parentHeight / contentHeight;
    const newZoom = Math.max(0.1, Math.min(Math.min(scaleX, scaleY) * 0.96, 2));
    this.zoomLevel = newZoom;

    // Canh giữa: child có transform-origin: top left => mép trên-trái giữ nguyên.
    // margin tính theo pixel gốc (chia zoomLevel để bù scale) rồi đẩy vào giữa khung.
    const scaledWidth = contentWidth * newZoom;
    const scaledHeight = contentHeight * newZoom;
    contentEl.style.marginLeft = scaledWidth < parentWidth ? (parentWidth - scaledWidth) / 2 / newZoom + 'px' : '0px';
    contentEl.style.marginRight = contentEl.style.marginLeft;
    contentEl.style.marginTop = scaledHeight < parentHeight ? (parentHeight - scaledHeight) / 2 / newZoom + 'px' : '0px';
    contentEl.style.marginBottom = '0px';

    // Cuộn về đầu (gốc sơ đồ ở trên).
    setTimeout(() => {
      parentEl.scrollLeft = 0;
      parentEl.scrollTop = 0;
    });
  }
  HRMSoDoToChucSearch001(ID: number) {
    let List = this.HRMSoDoToChucService.List.filter((o) => o.ParentID == ID).sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
    if (List.length > 1) {
      this.HRMSoDoToChucService.BaseParameter.Sum = this.HRMSoDoToChucService.BaseParameter.Sum - this.HRMSoDoToChucService.BaseParameter.Count / 2;
    }
    return List;
  }
  HRMSoDoToChucAdd(ID: number, HRMSoDoToChuc: HRMSoDoToChuc, Action: number) {    
    this.HRMSoDoToChucService.IsShowLoading = true;
    this.HRMSoDoToChucService.BaseParameter.ID = ID;
    this.HRMSoDoToChucService.GetByIDAsync().subscribe(
      (res) => {
        this.HRMSoDoToChucService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.HRMSoDoToChucService.BaseParameter.BaseModel.ID = ID;
        this.HRMSoDoToChucService.BaseParameter.BaseModel.CompanyID = this.HRMSoDoToChucService.BaseParameter.CompanyID;
        if (HRMSoDoToChuc != null && HRMSoDoToChuc.ID > 0) {
          this.HRMSoDoToChucService.BaseParameter.BaseModel.CompanyID = this.HRMSoDoToChucService.BaseParameter.CompanyID;
          this.HRMSoDoToChucService.BaseParameter.BaseModel.CategoryHRMPhongBanID = HRMSoDoToChuc.CategoryHRMPhongBanID;
          this.HRMSoDoToChucService.BaseParameter.BaseModel.CategoryPositionID = HRMSoDoToChuc.CategoryPositionID;
          this.HRMSoDoToChucService.BaseParameter.BaseModel.Note = HRMSoDoToChuc.Note;
          this.HRMSoDoToChucService.BaseParameter.BaseModel.Mission = HRMSoDoToChuc.Mission;
          this.HRMSoDoToChucService.BaseParameter.BaseModel.Mission01 = HRMSoDoToChuc.Mission01;
          this.HRMSoDoToChucService.BaseParameter.BaseModel.Mission02 = HRMSoDoToChuc.Mission02;
          this.HRMSoDoToChucService.BaseParameter.BaseModel.Mission03 = HRMSoDoToChuc.Mission03;

          this.HRMSoDoToChucService.BaseParameter.SearchString = environment.InitializationString;
          this.HRMSoDoToChucService.BaseParameter.BaseModel.MembershipID = environment.InitializationNumber;
          this.HRMSoDoToChucService.BaseParameter.BaseModel.MembershipName = environment.InitializationString;
          if (Action == 1) {
            this.HRMSoDoToChucService.BaseParameter.BaseModel.ParentID = HRMSoDoToChuc.ID;
          }
          if (Action == 2) {
            this.HRMSoDoToChucService.BaseParameter.BaseModel.ParentID = HRMSoDoToChuc.ParentID;
            this.HRMSoDoToChucService.BaseParameter.BaseModel.SortOrder = HRMSoDoToChuc.SortOrder - 1;
          }
          if (Action == 3) {
            this.HRMSoDoToChucService.BaseParameter.BaseModel.ParentID = HRMSoDoToChuc.ParentID;
            this.HRMSoDoToChucService.BaseParameter.BaseModel.SortOrder = HRMSoDoToChuc.SortOrder + 1;
          }
        }        
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        const dialog = this.Dialog.open(HRMSoDoToChucModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => {
          this.HRMSoDoToChucSearch();
        });
      },
      (err) => {},
      () => {
        this.HRMSoDoToChucService.IsShowLoading = false;
      },
    );
  }
}
