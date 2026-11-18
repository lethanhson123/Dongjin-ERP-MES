import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { NotificationService } from 'src/app/shared/Notification.service';
import { LanguageService } from 'src/app/shared/Language.service';
import { AnnualLeaveType } from 'src/app/shared/RD/AnnualLeaveType.model';
import { LookupItemService } from 'src/app/shared/RD/LookupItem.service';

@Component({
  selector: 'app-annual-leave-type',
  templateUrl: './annual-leave-type.component.html',
  styleUrls: ['./annual-leave-type.component.css'],
})
export class AnnualLeaveTypeComponent implements AfterViewInit {
  @ViewChild('AnnualLeaveTypeSort') AnnualLeaveTypeSort: MatSort;
  @ViewChild('AnnualLeaveTypePaginator') AnnualLeaveTypePaginator: MatPaginator;

  DisplayColumns: string[] = ['No', 'Code', 'NameVN', 'NameEN', 'NameKR', 'Description', 'IsDeduct', 'IsActive', 'Save'];
  DataSource: MatTableDataSource<AnnualLeaveType> = new MatTableDataSource<AnnualLeaveType>([]);

  constructor(
    public LookupItemService: LookupItemService,
    public NotificationService: NotificationService,
    public LanguageService: LanguageService,
  ) {}

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.AnnualLeaveTypeSearch();
    });
  }

  AnnualLeaveTypeSearch(): void {
    this.LookupItemService.IsShowLoading = true;
    this.LookupItemService.GetAllAndEmptyToListAsync().subscribe(
      (response) => {
        const result = response as BaseResult;
        if (result.StatusCode != null && result.StatusCode !== 200) {
          this.LookupItemService.List = [];
          this.DataSource = new MatTableDataSource<AnnualLeaveType>([]);
          this.NotificationService.warn(
            result.Message || this.LanguageService.TranslateService.instant('hrm-nghi-phep.leaveType.messages.loadFailed'),
          );
          this.LookupItemService.IsShowLoading = false;
          return;
        }

        this.LookupItemService.List = (result.List || []) as AnnualLeaveType[];
        this.DataSource = new MatTableDataSource<AnnualLeaveType>(this.LookupItemService.List);
        this.DataSource.sort = this.AnnualLeaveTypeSort;
        this.DataSource.paginator = this.AnnualLeaveTypePaginator;
      },
      () => {
        this.LookupItemService.List = [];
        this.DataSource = new MatTableDataSource<AnnualLeaveType>([]);
        this.LookupItemService.IsShowLoading = false;
        this.NotificationService.warn(
          this.LanguageService.TranslateService.instant('hrm-nghi-phep.leaveType.messages.loadFailed'),
        );
      },
      () => {
        this.LookupItemService.IsShowLoading = false;
      },
    );
  }

  AnnualLeaveTypeSave(annualLeaveType: AnnualLeaveType): void {
    annualLeaveType.Code = (annualLeaveType.Code || '').trim();
    annualLeaveType.NameVN = (annualLeaveType.NameVN || '').trim();

    if (!annualLeaveType.Code) {
      this.NotificationService.warn(
        this.LanguageService.TranslateService.instant('hrm-nghi-phep.leaveType.messages.codeRequired'),
      );
      return;
    }

    if (!annualLeaveType.NameVN) {
      this.NotificationService.warn(
        this.LanguageService.TranslateService.instant('hrm-nghi-phep.leaveType.messages.nameVnRequired'),
      );
      return;
    }

    this.LookupItemService.IsShowLoading = true;
    this.LookupItemService.SaveAnnualLeaveTypeAsync(annualLeaveType).subscribe(
      (response) => {
        const result = response as BaseResult;
        if (result.StatusCode != null && result.StatusCode !== 200) {
          this.NotificationService.warn(
            result.Message || this.LanguageService.TranslateService.instant('hrm-nghi-phep.leaveType.messages.saveFailed'),
          );
          this.LookupItemService.IsShowLoading = false;
          return;
        }

        this.NotificationService.success(
          this.LanguageService.TranslateService.instant('hrm-nghi-phep.leaveType.messages.saveSuccess'),
        );
        this.AnnualLeaveTypeSearch();
      },
      () => {
        this.LookupItemService.IsShowLoading = false;
        this.NotificationService.warn(
          this.LanguageService.TranslateService.instant('hrm-nghi-phep.leaveType.messages.saveFailed'),
        );
      },
    );
  }
}
