import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnnualLeaveRequestComponent } from './annual-leave-request.component';

describe('AnnualLeaveRequestComponent', () => {
  let component: AnnualLeaveRequestComponent;
  let fixture: ComponentFixture<AnnualLeaveRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnnualLeaveRequestComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnnualLeaveRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
