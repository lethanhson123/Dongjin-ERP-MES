import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnnualLeaveHistoryComponent } from './annual-leave-history.component';

describe('AnnualLeaveHistoryComponent', () => {
  let component: AnnualLeaveHistoryComponent;
  let fixture: ComponentFixture<AnnualLeaveHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AnnualLeaveHistoryComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnnualLeaveHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
