import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {
  AnnualLeaveBalanceFilter,
  AnnualLeaveBalanceImportModalData,
  AnnualLeaveBalanceImportResult,
} from 'src/app/shared/RD/AnnualLeaveBalance.model';
import { AnnualLeaveBalanceService } from 'src/app/shared/RD/AnnualLeaveBalance.service';
import { NotificationService } from 'src/app/shared/Notification.service';
import { LanguageService } from 'src/app/shared/Language.service';

@Component({
  selector: 'app-annual-leave-balance-import-modal',
  templateUrl: './annual-leave-balance-import-modal.component.html',
  styleUrls: ['./annual-leave-balance-import-modal.component.css'],
})
export class AnnualLeaveBalanceImportModalComponent {
  Year: number;
  Reason: string = '';
  OpeningDate: string = this.GetTodayDateString();
  SelectedFile: File | null = null;
  Errors: string[] = [];
  IsImporting: boolean = false;
  IsDownloadingTemplate: boolean = false;

  constructor(
    public DialogRef: MatDialogRef<AnnualLeaveBalanceImportModalComponent>,
    @Inject(MAT_DIALOG_DATA) public Data: AnnualLeaveBalanceImportModalData,
    public AnnualLeaveBalanceService: AnnualLeaveBalanceService,
    public NotificationService: NotificationService,
    public LanguageService: LanguageService,
  ) {
    this.Year = Data.Year;
    this.OpeningDate = this.GetTodayDateString();
  }

  OnFileSelected(event: Event): void {
    const fileInput = event.target as HTMLInputElement;
    const selectedFile = fileInput.files && fileInput.files.length > 0 ? fileInput.files[0] : null;
    this.Errors = [];

    if (selectedFile && !selectedFile.name.toLowerCase().endsWith('.xlsx')) {
      this.SelectedFile = null;
      fileInput.value = '';
      this.Errors = [this.Translate('invalidFile')];
      return;
    }

    this.SelectedFile = selectedFile;
  }

  Import(): void {
    if (this.IsImporting) {
      return;
    }
    if (!this.SelectedFile) {
      this.Errors = [this.Translate('fileRequired')];
      return;
    }
    if (!Number.isInteger(this.Year) || this.Year < 1) {
      this.Errors = [this.Translate('invalidYear')];
      return;
    }
    if (!this.OpeningDate || Number.isNaN(new Date(`${this.OpeningDate}T00:00:00`).getTime())) {
      this.Errors = [this.Translate('invalidOpeningDate')];
      return;
    }

    this.Errors = [];
    this.IsImporting = true;
    this.DialogRef.disableClose = true;
    this.AnnualLeaveBalanceService.ImportExcelAsync(
      this.SelectedFile,
      this.Year,
      this.OpeningDate,
      this.Reason.trim() || undefined,
    ).subscribe(
      result => this.HandleImportResult(result),
      error => {
        const serverErrors = error?.error?.Errors as string[] | undefined;
        this.Errors = serverErrors && serverErrors.length > 0
          ? serverErrors
          : [error?.error?.Message || this.Translate('importFailed')];
        this.NotificationService.warn(this.Errors[0]);
        this.EndImport();
      },
      () => this.EndImport(),
    );
  }

  Close(): void {
    if (!this.IsImporting) {
      this.DialogRef.close(false);
    }
  }

  DownloadTemplate(): void {
    if (this.IsImporting || this.IsDownloadingTemplate || !this.Data.Filter) {
      return;
    }

    const filter: AnnualLeaveBalanceFilter = {
      ...this.Data.Filter,
      Year: this.Year,
    };
    this.IsDownloadingTemplate = true;
    this.AnnualLeaveBalanceService.DownloadTemplateAsync(filter).subscribe(
      file => this.DownloadFile(file.Blob, file.FileName),
      () => {
        this.NotificationService.warn(
          this.LanguageService.TranslateService.instant('hrm-nghi-phep.requests.messages.exportFailed'),
        );
        this.IsDownloadingTemplate = false;
      },
      () => this.IsDownloadingTemplate = false,
    );
  }

  private HandleImportResult(result: AnnualLeaveBalanceImportResult): void {
    this.Errors = result.Errors || [];
    if (this.Errors.length > 0) {
      this.NotificationService.warn(this.Translate('correctErrors'));
      return;
    }
    if (result.SuccessCount < 1) {
      this.Errors = [this.Translate('noRecords')];
      return;
    }

    this.NotificationService.success(
      this.LanguageService.TranslateService.instant('hrm-nghi-phep.balance.messages.importSuccess', {
        count: result.SuccessCount,
      }),
    );
    this.DialogRef.close(true);
  }

  private EndImport(): void {
    this.IsImporting = false;
    this.DialogRef.disableClose = false;
  }

  private DownloadFile(fileBlob: Blob, fileName: string): void {
    const downloadUrl = window.URL.createObjectURL(fileBlob);
    const downloadLink = document.createElement('a');
    downloadLink.href = downloadUrl;
    downloadLink.download = fileName;
    downloadLink.click();
    setTimeout(() => window.URL.revokeObjectURL(downloadUrl), 100);
  }

  private GetTodayDateString(): string {
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  private Translate(messageKey: string): string {
    return this.LanguageService.TranslateService.instant(`hrm-nghi-phep.balance.messages.${messageKey}`);
  }
}
