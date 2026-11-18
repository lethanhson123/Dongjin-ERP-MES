import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnnualLeaveManagementComponent } from './annual-leave-management.component';

describe('AnnualLeaveManagementComponent', () => {
  let component: AnnualLeaveManagementComponent;
  let fixture: ComponentFixture<AnnualLeaveManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AnnualLeaveManagementComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnnualLeaveManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
