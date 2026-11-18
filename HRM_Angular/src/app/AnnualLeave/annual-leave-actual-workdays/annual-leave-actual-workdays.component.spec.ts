import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TranslateModule } from '@ngx-translate/core';
import { of } from 'rxjs';
import { LanguageService } from 'src/app/shared/Language.service';
import { NotificationService } from 'src/app/shared/Notification.service';
import { AnnualLeaveActualWorkdaysService } from 'src/app/shared/RD/AnnualLeaveActualWorkdays.service';
import { AnnualLeaveActualWorkdaysComponent } from './annual-leave-actual-workdays.component';

describe('AnnualLeaveActualWorkdaysComponent', () => {
  let component: AnnualLeaveActualWorkdaysComponent;
  let fixture: ComponentFixture<AnnualLeaveActualWorkdaysComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AnnualLeaveActualWorkdaysComponent],
      imports: [TranslateModule.forRoot()],
      providers: [
        {
          provide: AnnualLeaveActualWorkdaysService,
          useValue: {
            GetEmployeesAsync: () => of({ StatusCode: 200, List: [], Count: 0 }),
            GetCalendarAsync: () => of({ StatusCode: 200, List: [], Count: 0 }),
          },
        },
        {
          provide: LanguageService,
          useValue: {
            Language: 'en',
            TranslateService: { instant: (key: string) => key },
          },
        },
        {
          provide: NotificationService,
          useValue: { warn: () => undefined },
        },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnnualLeaveActualWorkdaysComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
