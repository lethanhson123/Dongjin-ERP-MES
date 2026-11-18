import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnnualLeaveRequestModal001Component } from './annual-leave-request-modal001.component';

describe('AnnualLeaveRequestModal001Component', () => {
  let component: AnnualLeaveRequestModal001Component;
  let fixture: ComponentFixture<AnnualLeaveRequestModal001Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnnualLeaveRequestModal001Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnnualLeaveRequestModal001Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
