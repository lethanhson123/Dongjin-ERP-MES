import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMTangCaReportsComponent } from './hrm-tang-ca-reports.component';

describe('HRMTangCaReportsComponent', () => {
  let component: HRMTangCaReportsComponent;
  let fixture: ComponentFixture<HRMTangCaReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HRMTangCaReportsComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMTangCaReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
