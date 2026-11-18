import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnnualLeaveRequest001Component } from './annual-leave-request001.component';

describe('AnnualLeaveRequest001Component', () => {
  let component: AnnualLeaveRequest001Component;
  let fixture: ComponentFixture<AnnualLeaveRequest001Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnnualLeaveRequest001Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnnualLeaveRequest001Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
