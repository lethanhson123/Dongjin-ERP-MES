import { AfterViewInit, Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { LanguageService } from 'src/app/shared/Language.service';
import { NotificationService } from 'src/app/shared/Notification.service';
import {
  AnnualLeaveActualWorkday,
  AnnualLeaveActualWorkdayLeave,
  AnnualLeaveActualWorkdaysCalendarCell,
  AnnualLeaveActualWorkdaysCalendarFilter,
  AnnualLeaveActualWorkdaysEmployee,
  AnnualLeaveActualWorkdaysEmployeeFilter,
} from 'src/app/shared/RD/AnnualLeaveActualWorkdays.model';
import { AnnualLeaveActualWorkdaysService } from 'src/app/shared/RD/AnnualLeaveActualWorkdays.service';

@Component({
  selector: 'app-annual-leave-actual-workdays',
  templateUrl: './annual-leave-actual-workdays.component.html',
  styleUrls: ['./annual-leave-actual-workdays.component.css'],
})
export class AnnualLeaveActualWorkdaysComponent implements AfterViewInit {
  Employees: AnnualLeaveActualWorkdaysEmployee[] = [];
  SelectedEmployee?: AnnualLeaveActualWorkdaysEmployee;
  SearchString = '';
  PageIndex = 0;
  PageSize = 20;
  TotalEmployeeCount = 0;
  IsLoadingEmployees = false;
  IsLoadingCalendar = false;
  EmployeesErrorKey = '';
  CalendarErrorMessage = '';
  SelectedMonth = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
  CalendarCells: AnnualLeaveActualWorkdaysCalendarCell[] = [];

  constructor(
    public AnnualLeaveActualWorkdaysService: AnnualLeaveActualWorkdaysService,
    public LanguageService: LanguageService,
    public NotificationService: NotificationService,
  ) {}

  ngAfterViewInit(): void {
    setTimeout(() => this.SearchEmployees());
  }

  get CurrentMonthLabel(): string {
    return new Intl.DateTimeFormat(this.GetLocale(), { month: 'long', year: 'numeric' })
      .format(this.SelectedMonth);
  }

  get WorkedDaysCount(): number {
    return this.CalendarCells.filter(cell => cell.WorkDate != null && cell.Workday?.IsWorked).length;
  }

  get LeaveDeductDaysCount(): number {
    return this.CalendarCells.filter(cell => cell.WorkDate != null
      && !cell.Workday?.IsWorked
      && cell.Workday?.Leaves.some(leave => leave.IsDeduct)).length;
  }

  get LeaveNonDeductDaysCount(): number {
    return this.CalendarCells.filter(cell => cell.WorkDate != null
      && !cell.Workday?.IsWorked
      && cell.Workday?.Leaves.some(leave => !leave.IsDeduct)).length;
  }

  SearchEmployees(resetPage: boolean = true): void {
    if (resetPage) {
      this.PageIndex = 0;
    }

    this.IsLoadingEmployees = true;
    this.EmployeesErrorKey = '';
    this.CalendarErrorMessage = '';
    this.Employees = [];
    this.SelectedEmployee = undefined;
    this.CalendarCells = [];

    const filter: AnnualLeaveActualWorkdaysEmployeeFilter = {
      SearchString: this.SearchString.trim() || undefined,
      PageIndex: this.PageIndex,
      PageSize: this.PageSize,
    };

    this.AnnualLeaveActualWorkdaysService.GetEmployeesAsync(filter).subscribe(
      response => {
        if (response.StatusCode && response.StatusCode >= 400) {
          this.HandleEmployeeLoadFailure();
          return;
        }

        this.Employees = response.List || [];
        this.TotalEmployeeCount = response.Count || 0;
        if (this.Employees.length > 0) {
          this.SelectEmployee(this.Employees[0]);
        }
      },
      () => this.HandleEmployeeLoadFailure(),
      () => this.IsLoadingEmployees = false,
    );
  }

  EmployeePageChange(pageEvent: PageEvent): void {
    this.PageIndex = pageEvent.pageIndex;
    this.PageSize = pageEvent.pageSize;
    this.SearchEmployees(false);
  }

  SelectEmployee(employee: AnnualLeaveActualWorkdaysEmployee): void {
    this.SelectedEmployee = employee;
    this.LoadCalendar();
  }

  PreviousMonth(): void {
    this.SelectedMonth = new Date(this.SelectedMonth.getFullYear(), this.SelectedMonth.getMonth() - 1, 1);
    this.LoadCalendar();
  }

  NextMonth(): void {
    this.SelectedMonth = new Date(this.SelectedMonth.getFullYear(), this.SelectedMonth.getMonth() + 1, 1);
    this.LoadCalendar();
  }

  LoadCalendar(): void {
    if (!this.SelectedEmployee) {
      this.CalendarCells = [];
      return;
    }

    this.IsLoadingCalendar = true;
    this.CalendarErrorMessage = '';
    this.CalendarCells = [];
    const filter: AnnualLeaveActualWorkdaysCalendarFilter = {
      MembershipId: this.SelectedEmployee.MembershipId,
      Year: this.SelectedMonth.getFullYear(),
      Month: this.SelectedMonth.getMonth() + 1,
    };

    this.AnnualLeaveActualWorkdaysService.GetCalendarAsync(filter).subscribe(
      response => {
        if (response.StatusCode && response.StatusCode >= 400) {
          this.HandleCalendarLoadFailure(response.Message);
          return;
        }

        this.CalendarCells = this.BuildCalendarCells(response.List || []);
      },
      error => this.HandleCalendarLoadFailure(error?.error?.Message),
      () => this.IsLoadingCalendar = false,
    );
  }

  GetLeaveTypeName(leave: AnnualLeaveActualWorkdayLeave): string {
    if (this.LanguageService.Language === 'vi') {
      return leave.LeaveTypeNameVN || leave.LeaveTypeCode || '';
    }
    if (this.LanguageService.Language === 'kr') {
      return leave.LeaveTypeNameKR || leave.LeaveTypeNameEN || leave.LeaveTypeCode || '';
    }
    return leave.LeaveTypeNameEN || leave.LeaveTypeNameVN || leave.LeaveTypeCode || '';
  }

  IsWeekend(date: Date | null): boolean {
    return date != null && (date.getDay() === 0 || date.getDay() === 6);
  }

  private BuildCalendarCells(workdays: AnnualLeaveActualWorkday[]): AnnualLeaveActualWorkdaysCalendarCell[] {
    const calendarCells: AnnualLeaveActualWorkdaysCalendarCell[] = [];
    const workdayByDate = new Map<string, AnnualLeaveActualWorkday>();
    for (const workday of workdays) {
      workdayByDate.set(this.GetDateKey(workday.WorkDate), workday);
    }

    const monthBegin = new Date(this.SelectedMonth.getFullYear(), this.SelectedMonth.getMonth(), 1);
    const leadingEmptyCellCount = (monthBegin.getDay() + 6) % 7;
    for (let emptyCellIndex = 0; emptyCellIndex < leadingEmptyCellCount; emptyCellIndex++) {
      calendarCells.push({ WorkDate: null });
    }

    const daysInMonth = new Date(
      this.SelectedMonth.getFullYear(),
      this.SelectedMonth.getMonth() + 1,
      0,
    ).getDate();
    for (let dayNumber = 1; dayNumber <= daysInMonth; dayNumber++) {
      const workDate = new Date(this.SelectedMonth.getFullYear(), this.SelectedMonth.getMonth(), dayNumber);
      const workday = workdayByDate.get(this.GetDateKey(workDate));
      calendarCells.push({ WorkDate: workDate, Workday: workday });
    }

    while (calendarCells.length < 35 || calendarCells.length % 7 !== 0) {
      calendarCells.push({ WorkDate: null });
    }

    return calendarCells;
  }

  private GetDateKey(dateValue: string | Date): string {
    if (dateValue instanceof Date) {
      const year = dateValue.getFullYear();
      const month = (dateValue.getMonth() + 1).toString().padStart(2, '0');
      const day = dateValue.getDate().toString().padStart(2, '0');
      return `${year}-${month}-${day}`;
    }

    return dateValue.substring(0, 10);
  }

  private GetLocale(): string {
    if (this.LanguageService.Language === 'vi') {
      return 'vi-VN';
    }
    if (this.LanguageService.Language === 'kr') {
      return 'ko-KR';
    }
    return 'en-US';
  }

  private HandleEmployeeLoadFailure(): void {
    this.Employees = [];
    this.SelectedEmployee = undefined;
    this.TotalEmployeeCount = 0;
    this.CalendarCells = [];
    this.EmployeesErrorKey = 'hrm-nghi-phep.actualWorkdays.messages.loadEmployeesFailed';
    this.IsLoadingEmployees = false;
    this.NotificationService.warn(
      this.LanguageService.TranslateService.instant(this.EmployeesErrorKey),
    );
  }

  private HandleCalendarLoadFailure(message?: string): void {
    this.CalendarCells = [];
    this.CalendarErrorMessage = message && message.trim()
      ? message
      : this.LanguageService.TranslateService.instant(
        'hrm-nghi-phep.actualWorkdays.messages.loadCalendarFailed',
      );
    this.IsLoadingCalendar = false;
    this.NotificationService.warn(this.CalendarErrorMessage);
  }
}
