import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnnualLeaveTypeComponent } from './annual-leave-type.component';

describe('AnnualLeaveTypeComponent', () => {
  let component: AnnualLeaveTypeComponent;
  let fixture: ComponentFixture<AnnualLeaveTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AnnualLeaveTypeComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnnualLeaveTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
