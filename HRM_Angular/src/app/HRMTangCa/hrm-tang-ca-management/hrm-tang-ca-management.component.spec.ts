import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMTangCaManagementComponent } from './hrm-tang-ca-management.component';

describe('HRMTangCaManagementComponent', () => {
  let component: HRMTangCaManagementComponent;
  let fixture: ComponentFixture<HRMTangCaManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HRMTangCaManagementComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMTangCaManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
