import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnnualLeaveBalanceComponent } from './annual-leave-balance.component';

describe('AnnualLeaveBalanceComponent', () => {
  let component: AnnualLeaveBalanceComponent;
  let fixture: ComponentFixture<AnnualLeaveBalanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AnnualLeaveBalanceComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnnualLeaveBalanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
