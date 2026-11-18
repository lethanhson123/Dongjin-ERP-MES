import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NgForm } from '@angular/forms';
import { NotificationService } from 'src/app/shared/Notification.service';
import { LanguageService } from 'src/app/shared/Language.service';
import {
  AnnualLeaveBalanceEditModalData,
  AnnualLeaveBalanceAdjust,
} from 'src/app/shared/RD/AnnualLeaveBalance.model';
import { AnnualLeaveBalanceService } from 'src/app/shared/RD/AnnualLeaveBalance.service';

@Component({
  selector: 'app-annual-leave-balance-edit-modal',
  templateUrl: './annual-leave-balance-edit-modal.component.html',
  styleUrls: ['./annual-leave-balance-edit-modal.component.css'],
})
export class AnnualLeaveBalanceEditModalComponent implements OnInit {
  private readonly AmountStep = 0.5;
  Amount: number | null = null;
  Reason: string = '';
  IsSaving: boolean = false;

  constructor(
    public DialogRef: MatDialogRef<AnnualLeaveBalanceEditModalComponent>,
    @Inject(MAT_DIALOG_DATA) public Data: AnnualLeaveBalanceEditModalData,
    public AnnualLeaveBalanceService: AnnualLeaveBalanceService,
    public NotificationService: NotificationService,
    public LanguageService: LanguageService,
  ) {}

  ngOnInit(): void {
  }

  get NewCurrentBalance(): number {
    const amount = Number(this.Amount);
    if (!Number.isFinite(amount)) {
      return this.Data.CurrentBalance;
    }
    return this.Data.CurrentBalance + amount;
  }

  IncreaseAmount(): void {
    this.Amount = this.GetNextAmount(this.AmountStep);
  }

  DecreaseAmount(): void {
    this.Amount = this.GetNextAmount(-this.AmountStep);
  }

  private GetNextAmount(stepChange: number): number {
    const currentAmount = Number(this.Amount);
    const currentAmountOrZero = Number.isFinite(currentAmount)
      ? currentAmount
      : 0;
    const nextAmount = currentAmountOrZero + stepChange;
    return Math.round(nextAmount * 10) / 10;
  }

  Save(form: NgForm): void {
    const amount = Number(this.Amount);
    if (form.invalid || !Number.isFinite(amount) || amount === 0 || this.IsSaving) {
      return;
    }

    const request: AnnualLeaveBalanceAdjust = {
      EmployeeCode: this.Data.EmployeeCode,
      Year: this.Data.Year,
      AdjustmentType: amount > 0 ? 'ADD' : 'SUB',
      Amount: Math.abs(amount),
      Reason: this.Reason.trim() || undefined,
    };

    this.IsSaving = true;
    this.AnnualLeaveBalanceService.AdjustSingleAsync(request).subscribe(
      result => {
        if (result.StatusCode != null && result.StatusCode !== 200) {
          this.NotificationService.warn(result.Message || this.LanguageService.TranslateService.instant('hrm-nghi-phep.balance.messages.saveFailed'));
          this.IsSaving = false;
          return;
        }

        this.NotificationService.success(
          this.LanguageService.TranslateService.instant('hrm-nghi-phep.balance.messages.saveSuccess'),
        );
        this.DialogRef.close(true);
      },
      error => {
        this.NotificationService.warn(
          error?.error?.Message || this.LanguageService.TranslateService.instant('hrm-nghi-phep.balance.messages.saveFailed'),
        );
        this.IsSaving = false;
      },
      () => this.IsSaving = false,
    );
  }

  Close(): void {
    if (!this.IsSaving) {
      this.DialogRef.close(false);
    }
  }
}
